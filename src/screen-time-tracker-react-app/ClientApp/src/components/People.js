import React, { Component } from 'react';
import DataSheet from 'react-datasheet';

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
        changes.forEach(({cell, row, col, value}) => {
            peopleGrid[row][col] = { ...peopleGrid[row][col], value };
            this.addOrUpdatePerson(peopleGrid[row]);
        });
        this.setState({ peopleGrid });
    }

    async addOrUpdatePerson(row) {
        let id = row[0].value;
        let name = row[1].value;
        let dob = row[2].value;
        
        // currently we only support adding people, but will support updates in the future
        if(!this.validateRow(id, name, dob)) return;
        
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: name, dateOfBirth: dob })
        };
        const response = await fetch('people', requestOptions);
        const data = await response.json();
        console.log(data);
    }

    validateRow = (id, name, dob) => {
        if(id !== "") return false;
        if(name.trim() === "" || dob.trim() === "") return false;

        return true;
    } 

    addRow = () => {
        const peopleGrid = this.state.peopleGrid.map(row => [...row]);
        let newRow = [{value: "", readOnly: true}, {value: ""}, {value: ""}];

        if(!this.lastRowIsIncomplete(peopleGrid)) peopleGrid.push(newRow);

        this.setState({peopleGrid: peopleGrid, loading: false});
    }

    lastRowIsIncomplete(peopleGrid) {
        return peopleGrid[peopleGrid.length - 1][1].value.trim() === "" || peopleGrid[peopleGrid.length - 1][2].value.trim() === "";
    }

    async populatePeopleData() {
      const response = await fetch('people');
      const data = await response.json();
      
      let tempGrid = [
        [{value: "Id", readOnly: true}, {value: "Name", readOnly: true}, {value: "Date of Birth", readOnly: true}]
      ];
      
      data.forEach(({id, name, dateOfBirth}) => {
        let gridRow = [{value: id, readOnly: true}, {value: name}, {value: dateOfBirth}];
        tempGrid.push(gridRow);
      });

      this.setState({ peopleGrid: tempGrid, loading: false });
    }
}