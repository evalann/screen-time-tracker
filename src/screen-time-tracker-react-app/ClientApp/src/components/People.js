import React, { Component } from 'react';
import DataSheet from 'react-datasheet';

export class People extends Component {
    constructor(props) {
        super(props);
        // todo: Populate from the api, if no values then show an empty grid.
        this.state = {
            peopleGrid: [
                [{ value: "Name", readOnly: true }, { value: "Date of Birth", readOnly: true }],
                [{ value: "Evalan" }, { value: "1988/09/02" }],
                [{ value: "" }, { value: "" }]
            ]
        };
    }

    onContextMenu = (e, cell, i, j) => cell.readOnly ? e.preventDefault() : null;

    onCellsChanged = (changes) => {
        const peopleGrid = this.state.peopleGrid.map(row => [...row]);
        changes.forEach(({cell, row, col, value}) => {
            console.log(peopleGrid[row][col]);
            peopleGrid[row][col] = { ...peopleGrid[row][col], value };
        })
        console.log("got here");
        this.setState({ peopleGrid });
    }

    render() {
        return (
            <div>
                <h1>People</h1>
                <div className="sheet-container">
                    <DataSheet
                        data={this.state.peopleGrid}
                        valueRenderer={cell => cell.value}
                        onContextMenu={this.onContextMenu}
                        onCellsChanged={this.onCellsChanged}
                    />
                </div>
            </div>
        );
    }
}