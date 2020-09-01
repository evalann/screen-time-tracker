import React from 'react';
import ReactDOM from 'react-dom';
import { People } from './People';

describe('people', () => {
    test('renders without crashing', async () => {
        const div = document.createElement('div');
        ReactDOM.render(
            <People />, div);
        await new Promise(resolve => setTimeout(resolve, 1000));
    });

    test('expect true to be truthy', async () => {
        expect(true).toBe(true);
    });
});