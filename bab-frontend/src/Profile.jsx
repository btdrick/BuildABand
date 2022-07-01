import { React, useState, useEffect, useCallback } from 'react';
import {variables} from './Variables.js';
import { useParams } from "react-router-dom";
import Feed from './components/feed/Feed.js';
import Navbar from './components/header/Navbar';
import AddConnection from './components/connection/AddConnection.js';
import UserProfile from './components/UserProfile.js';
import { useParams } from "react-router-dom";
import './style/home.css';

function Profile() {
    /* Profile's owner */
    const { id } = useParams();

    /* Makes api call to backend to get the user's posts */
    const getUsersPosts = useCallback(async () => {
        const response = await fetch(variables.API_URL+'post/'+ id);
        const data = await response.json();
        return data;
    }, [id]);

    /* Renders the profile page's html. You can't pass entire object to child component */
        if (state.loading) {
            return (
                <div>
                    <p>loading...</p>
                </div>
            );
        }
        else {
            return ( 
                <div id="container">
                    <Navbar/>
                    <h3 className="title"> This is the Profile page </h3> 
                    {/* Create post modal*/}
                    {canCreatePost
                    && <button type="button"
                    className="btn btn-primary m-3"
                    data-bs-toggle="modal"
                    data-bs-target="#exampleModal"
                    onClick={addClick}>
                        Create Post
                    </button>}
                    <div className="container-lg">
                    <div className="modal fade" id="exampleModal" tabIndex="-1" aria-hidden="true">
                        <div className="modal-dialog modal-lg modal-dialog-centered">
                            <div className="modal-content">
                                <div className="modal-header">
                                    <h5 className="modal-title">{modalTitle}</h5>
                                    <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"
                                    ></button>
                                </div> 
    
                                <div className="input-group mb-3">
                                    <span className="input-group-text">Content</span>
                                    <input type="text" className="form-control"
                                    value={content}
                                    onChange={changePostContent}/>
                                </div>
    
                                <div className="modal-body">               
                                    {postID===0?
                                    <button type="button"
                                    className="btn btn-primary float-start"
                                    onClick={createClick}
                                    >Create</button>
                                    :null}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div>
                        <ul className="list-group">
                            {state.posts.map((post, index) => 
                            <li key={index} className="list-group-item">                               
                                <Post 
                                PostID={ post.PostID }
                                CreatedTime={ post.CreatedTime }
                                Content={ post.Content }
                                MusicianID={ post.MusicianID } />
                                </li>)}
                            <AddConnection followerID={id}/>    
                        </ul>
                    </div>
                    </div>
                </div>
            )
        }
    
}

export default Profile;