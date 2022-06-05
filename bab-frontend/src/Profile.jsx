import React, { Component } from 'react';
import { Link } from "react-router-dom";

export class Profile extends Component {
    render() {
        return ( 
            <div>
                <Link to="/">Login</Link>
                <h3> This is the Profile page </h3> 
                <div>
                    <h2>Posts</h2>
                    <th></th>
                </div>
            </div>
        )
    }
}

export default Profile;