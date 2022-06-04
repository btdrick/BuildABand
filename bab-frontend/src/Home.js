import React, { Component } from 'react';
import { Link } from "react-router-dom";

export class Home extends Component {
    render() {
        return ( 
            <div>
                <Link to="/">Login</Link>
                <h3> This is the Home page </h3> 
            </div>
        )
    }
}