import { React, useState, useEffect } from 'react';
import Post from './Post';
import AudioPlayer from './components/audio/AudioPlayer';
import {variables} from './Variables.js';
import Navbar from './components/header/Navbar';
import AddConnection from './components/connection/AddConnection.js';
import UserProfile from './components/UserProfile.js';
import { useParams } from "react-router-dom";
import './style/home.css';

function Profile() {
    const [state, setState] = useState({
        posts: [],
        PostID:     0,          
        MusicianID: UserProfile.getProfileID(),
        Content:    "",
        modalTitle: "",
        loading:    true
    })
    const { id } = useParams();

    /* Once the page renders, this lifecycle method takes place */
    useEffect(() => {
        getUsersPosts();
    })
    
    /* Makes api call to backend to get the user's posts */
    const getUsersPosts = async () => {
        const response = await fetch(variables.API_URL+'post/'+ id);
        const data = await response.json();
        setState({
            posts: data,
            loading: false,
        });
    }

    /* Handles event of text entry for Post content */
    const changePostContent =(e)=>{
        setState({Content:e.target.value});
    }

    /* Handles onClick event for Add button */
    const addClick = () => {
        setState({
            modalTitle: "Create Post",
            PostID:     0,
            Content:    ""          
        });
    }

    /* Handles onClick event for Create button */
    const createClick = () => {
        fetch(variables.API_URL+'post',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                CreatedTime: new Date(),                           
                MusicianID: state.MusicianID,
                Content:    state.Content
            })
        })
        .then(res=>res.json())
        .then((result)=>{
            alert(result);
            getUsersPosts();
        },(_error)=>{
            alert('Post content cannot be blank');
        })
    }

    /* Renders the profile page's html. You can't pass entire object to child component */
        const {     
            modalTitle,
            PostID,
            Content
        }=state;
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
                    <AudioPlayer />
                    <button type="button"
                    className="btn btn-primary m-3"
                    data-bs-toggle="modal"
                    data-bs-target="#exampleModal"
                    onClick={addClick}>
                        Create Post
                    </button>
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
                                    value={Content}
                                    onChange={changePostContent}/>
                                </div>
    
                                <div className="modal-body">               
                                    {PostID===0?
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
                                postID={ post.PostID }
                                createdTime={ post.CreatedTime }
                                content={ post.Content }
                                musicianID={ post.MusicianID } />
                                </li>)}
                            <AddConnection followerID={state.MusicianID}/>    
                        </ul>
                    </div>
                    </div>
                </div>
            )
        }
    
}

export default Profile;