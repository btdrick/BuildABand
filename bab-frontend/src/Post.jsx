import React, { Component } from 'react';
import { Link } from "react-router-dom";

export class Post extends Component {
    render() {
        return ( 
            <div>
                <Link to="/">Login</Link>
                <h3> This is what a post looks like </h3> 
            </div>
        )
    }
}

export default Post;