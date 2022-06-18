import React, { Component } from 'react';
import Comments from './components/comments/Comments';

/* This is a child component to serveral other components, i.e. child component in Profile component */
export class Post extends Component {
    render() {
        return ( 
            <div>
                {/* Card style post */}
                <div className="card text-white bg-dark mb-3" style={{maxWidth: 18 + 'rem'}}>
                    <div className="card-header"><h3> This is what a post looks like </h3></div>
                    <div className="card-body">
                        <h5 className="card-title"> Post info: </h5>
                        <p className="card-text"> {this.props.content} </p>
                    </div>
                
                    <button type="button" className="btn btn-primary" style={{margin: 0.2 + 'em'}}> Like </button>
                    <div className="card-footer">
                        {this.props.Likes} Likes
                    </div>
                </div>
                {/* Render comments for current post */}
                <Comments currentUserID={1} currentPostID={this.props.postID} />
            </div>
                           
                  
        )
    }
}

export default Post;