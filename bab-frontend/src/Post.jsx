import React, { Component } from 'react';
import { variables } from './Variables.js';
import Comments from './components/comments/Comments';
import UserProfile from './components/UserProfile';
import './style/post.css';

/* This is a child component to serveral other components, i.e. child component in Profile component */
export class Post extends Component {
    /* Constructor for the component. Tracks the posts shown on the feed and whether the page's loading status */
    constructor(props) {
        super(props);
        this.state = { 
            authorInfo: [], 
            authorName: ""
        };
    }
    
    /* Once the page renders, this lifecycle method takes place */
    componentDidMount(){
        const getAuthorInfo = async() => {
            const response = await fetch(variables.API_URL+'musician/'+this.props.musicianID);
            const data = await response.json(); 
            var author = data[0]
            
            return author;
        };

        getAuthorInfo().then((data) => {
            this.setState ({
                authorInfo: data,
                authorName: data.Fname + " " + data.Lname,
            });
        })
    }

    render() {
        return ( 
            <div id="container">
                {/* Card style post */}
                <div className="card text-white bg-dark mb-3">
                    <div className="card-body">
                        <h5 className="card-title"> { this.state.authorName } said: </h5>
                        <p className="card-text"> { this.props.content } </p>
                        <cite title="Created Time">{ this.state.createdTime }</cite>
                    </div>
                
                    <button type="button" className="btn btn-primary" style={{margin: 0.2 + 'em'}}> Like </button>
                </div>
                {/* Render comments for current post */}
                {/** @todo: replace set currentUserID with session variable **/}
                <Comments currentUserID={UserProfile.getMusicianID()} currentPostID={ this.props.postID } />
            </div>
                           
                  
        )
    }
}

export default Post;