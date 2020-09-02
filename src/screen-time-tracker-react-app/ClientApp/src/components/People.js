import React, { Component } from 'react';
import DataSheet from 'react-datasheet';

export class People extends Component {
    constructor(props) {
        super(props);
        this.state = {
            peopleGrid: [
                [{ value: "Name", readOnly: true }, { value: "Date of Birth", readOnly: true }],
                [{ value: "Evalan" }, { value: "1988/09/02" }],
                [{ value: "" }, { value: "" }]
            ]
        };
    }

    onContextMenu = (e, cell, i, j) => cell.readOnly ? e.preventDefault() : null;

    render() {
        return (
            <div>
                <h1>People</h1>
                <div className="sheet-container">
                    <DataSheet
                        data={this.state.peopleGrid}
                        valueRenderer={cell => cell.value}
                        onContextMenu={this.onContextMenu}
                    />
                </div>
            </div>
        );
    }
}