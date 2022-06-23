import { React, useState, useEffect } from 'react';
import { variables } from './Variables.js';
import Comments from './components/comments/Comments';
import UserProfile from './components/UserProfile';
import './style/post.css';

/* This is a child component to serveral other components, i.e. child component in Profile component */
const Post = (props) => {
    /* Constructor for the component. Tracks the posts shown on the feed and whether the page's loading status */
    const [state, setState] = useState({
        authorInfo: [], 
        authorName: "",
        loading: true,
    })

    const getAuthorInfo = async() => {
        const response = await fetch(variables.API_URL+'musician/'+ props.musicianID);
        const data = await response.json(); 
        setState({
            authorInfo: data[0],
            authorName: data.Fname + " " + data.Lname,
        });
    };
    
    /* Once the page renders, this hook takes place */
    useEffect(() => {
        getAuthorInfo();
    })
            return ( 
                <div id="container">
                    {/* Card style post */}
                    <div className="card text-white bg-dark mb-3">
                        <div className="card-body">
                            <h5 className="card-title"> { state.authorName } said: </h5>
                            <p className="card-text"> { props.content } </p>
                            <cite title="Created Time">{ state.createdTime }</cite>
                        </div>
                    
                        <button type="button" className="btn btn-primary" style={{margin: 0.2 + 'em'}}> Like </button>
                    </div>
                    {/* Render comments for current post */}
                    {/** @todo: replace set currentUserID with session variable **/}
                    <Comments currentUserID={UserProfile.getMusicianID()} currentPostID={ props.postID } />
                </div>
                            
                    
            );
}

export default Post;