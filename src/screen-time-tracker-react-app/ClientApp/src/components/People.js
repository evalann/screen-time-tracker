import React, { Component } from 'react';
import DataSheet from 'react-datasheet';
import DateTimePicker from 'react-datetime-picker';
import _ from 'lodash'

const idColIndex = 0;
const nameColIndex = 1;
const dateOfBirthColIndex = 2;
export class People extends Component {
    constructor(props) {
        super(props);
        this.state = {
            peopleGrid: [
                [{ value: "Id", readOnly: true }, { value: "Name", readOnly: true }, { value: "Date of Birth", readOnly: true }]
            ],
            loading: true
        };
    }

    componentDidMount() {
        this.populatePeopleData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderPeopleGrid(this.state.peopleGrid);

        return contents;
    }

    renderPeopleGrid(peopleGrid) {
        return (
            <div>
                <h1>People</h1>
                <div className="sheet-container">
                    <DataSheet
                        data={peopleGrid}
                        valueRenderer={cell => cell.value}
                        onContextMenu={this.onContextMenu}
                        onCellsChanged={this.onCellsChanged}
                    />
                    <button className="add-button" onClick={this.addRow}>+</button>
                </div>
            </div>
        );
    }

    onContextMenu = (e, cell, i, j) => cell.readOnly ? e.preventDefault() : null;

    onCellsChanged = (changes) => {
        const peopleGrid = this.state.peopleGrid.map(row => [...row]);
        changes.forEach(({ cell, row, col, value }) => {
            // For the date changes we need to use the cell value because the DatePicker's change of value doesn't seem to get to this callback.
            if (this.isDateOfBirthChange(col)) value = new Date(cell.value);
            peopleGrid[row][col] = { ...peopleGrid[row][col], value };
            this.addOrUpdatePerson(peopleGrid[row]);
        });
        this.setState({ peopleGrid });
    }

    isDateOfBirthChange(col) {
        return col === dateOfBirthColIndex;
    }

    async addOrUpdatePerson(row) {
        let id = row[idColIndex].value;
        let name = row[nameColIndex].value;
        let dob = row[dateOfBirthColIndex].value;

        // currently we only support adding people, but will support updates in the future
        if (!this.validateRow(id, name, dob)) return;

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: name, dateOfBirth: dob })
        };
        const response = await fetch('people', requestOptions);
        await response.json();
        // todo: Error handling and replace latest row with a disabled row
        // todo: there's a bug with the date picker on change when selecting values on the default calendar pop up.
    }

    validateRow = (id, name, dob) => {
        if (id !== "") return false;
        if (name.trim() === "" || dob === null) return false;

        return true;
    }

    addRow = () => {
        const peopleGrid = this.state.peopleGrid.map(row => [...row]);
        let newRow = [{ value: "", readOnly: true }, { value: "" }, this.dateOfBirthCol(peopleGrid.length, null)];

        if (!this.lastRowIsIncomplete(peopleGrid)) peopleGrid.push(newRow);

        this.setState({ peopleGrid: peopleGrid, loading: false });
    }

    lastRowIsIncomplete(peopleGrid) {
        return peopleGrid[peopleGrid.length - 1][nameColIndex].value.trim() === "" || peopleGrid[peopleGrid.length - 1][dateOfBirthColIndex].value === null;
    }

    dateOfBirthCol = (index, dateOfBirth, readOnly) => {
        const dateOfBirthOnChange = (index, value) => {
            if(value == null) return;
            const peopleGrid = this.state.peopleGrid.map(row => [...row]);
            readOnly = this.validateRow(peopleGrid[index]);
            peopleGrid[index][dateOfBirthColIndex] = this.dateOfBirthCol(index, new Date(value).toUTCString(), true);
            this.setState({ peopleGrid: peopleGrid, loading: false });
        }

        const dateOfBirthComponent = (index, dateOfBirth, readOnly) => {
            return (
                <DateTimePicker disabled={readOnly} format="dd-MMM-y" value={new Date(dateOfBirth)} onChange={(value) => dateOfBirthOnChange(index, value)} />
            )
        }

        return { value: dateOfBirth, component: dateOfBirthComponent(index, dateOfBirth, readOnly), forceComponent: true }
    }

    async populatePeopleData() {
        const response = await fetch('people');
        const data = await response.json();

        let tempGrid = [
            [{ value: "Id", readOnly: true }, { value: "Name", readOnly: true }, { value: "Date of Birth", readOnly: true }]
        ];

        tempGrid = tempGrid.concat(_.range(1, data.length + 1).map(gridIndex => {
            const dataIndex = gridIndex - 1;
            return [{ readOnly: true, value: data[dataIndex].id }, { value: data[dataIndex].name }, this.dateOfBirthCol(gridIndex, data[dataIndex].dateOfBirth, true)];
        }));

        this.setState({ peopleGrid: tempGrid, loading: false });
    }
}