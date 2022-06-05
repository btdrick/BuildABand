import React, { Component } from 'react';

export class Post extends Component {
    //keep track of each post attribute
    render() {
        return ( 
            <div>
                <h3> This is what a post looks like </h3> 
                {/*<h2>ID: {this.props.post.musicanID}</h2>
                <p>You said: {this.props.post.content} </p>*/}
            </div>
        )
    }
}

export default Post;