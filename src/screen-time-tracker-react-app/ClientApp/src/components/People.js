import React, { Component } from 'react';
import DataSheet from 'react-datasheet';

export class People extends Component {
    constructor(props) {
        super(props);
        this.state = { 
            peopleGrid: [
                [{ value: "Name", readOnly: true }, { value: "Date of Birth", readOnly: true }]
            ], 
            loading: true 
        };
    }

    componentDidMount() {
      this.populatePeopleData();
    }

    onContextMenu = (e, cell, i, j) => cell.readOnly ? e.preventDefault() : null;

    onCellsChanged = (changes) => {
        const peopleGrid = this.state.peopleGrid.map(row => [...row]);
        changes.forEach(({cell, row, col, value}) => {
            peopleGrid[row][col] = { ...peopleGrid[row][col], value };
        });
        this.setState({ peopleGrid });
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
                </div>
            </div>
        );
    }

    async populatePeopleData() {
      const response = await fetch('people');
      const data = await response.json();
      
      let tempGrid = [
        [{value: "Name", readOnly: true}, {value: "Date of Birth", readOnly: true}]
      ];
      
      data.forEach(({name, dateOfBirth}) => {
        let gridRow = [{value: name}, {value: dateOfBirth}];
        tempGrid.push(gridRow);
      });

      this.setState({ peopleGrid: tempGrid, loading: false });
    }
}