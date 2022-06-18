import React from 'react';
import { Link } from "react-router-dom";

export class Navbar extends React.Component {
    constructor() {
        super();
        this.state = {
          isAuthenticated: false
        };
    }

    render() {
        return (
            <header>
                <Link to="/home">Home</Link>
                <Link to="/profile/1">Profile</Link>
            </header>
        )
    }

}

export default Navbar;