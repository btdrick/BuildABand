import React, { Component } from 'react';

/* This is a child component to serveral other components, i.e. child component in Profile component */
export class Post extends Component {
    render() {
        return ( 
            <div>
                <h3> This is what a post looks like </h3> 
                <p>Post info: {this.props.content} </p>
            </div>
        )
    }
}

export default Post;