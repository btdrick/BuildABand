import { React, useState, useEffect, useCallback } from 'react';
import {variables} from './Variables.js';
import { useParams } from "react-router-dom";
import Post from './components/post/Post.js';
import Navbar from './components/header/Navbar';
import AddConnection from './components/connection/AddConnection.js';
import UserProfile from './components/UserProfile.js';
import './style/home.css';

function Profile() {
    const [state, setState] = useState({
        posts: [],         
        MusicianID: 0,
        loading:    true
    });

    /* Profile's owner */
    const { id } = useParams();
    /* Current session user ID */
    const currentUser = UserProfile.getMusicianID();
    /* User can only create a post on their own profile */
    const canCreatePost = parseInt(id) === currentUser;

    /* Create post modal attributes */
    const [modalTitle, setModalTitle] = useState("");
    const [content, setContent] = useState("");
    const [postID, setPostID] = useState(0);
    
    /* Makes api call to backend to get the user's posts */
    const getUsersPosts = useCallback(async () => {
        const response = await fetch(variables.API_URL+'post/'+ id);
        const data = await response.json();
        setState({
            posts: data,
            loading: false,
        });
    }, [id]);

    /* Once the page renders, this lifecycle method takes place */
    useEffect(() => {
        getUsersPosts();
    }, [getUsersPosts]);

    /* Handles event of text entry for Post content */
    const changePostContent =(e)=>{
        setContent(e.target.value)
    };

    /* Handles onClick event for Add button */
    const addClick = () => {
        setModalTitle("Create Post");
        setPostID(0);
        setContent("");
    };

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
                MusicianID: currentUser,
                Content:    content
            })
        })
        .then(res=>res.json())
        .then((result)=>{
            alert(result);
            getUsersPosts();
        },(_error)=>{
            alert('Post content cannot be blank');
        });
    };

    /* Removes post and its affiliated likes, comments from database */
    const deletePost = (postID) => {
        if (window.confirm("Are you sure you want to remove this post and its affiliated comments?")) {
            fetch(variables.API_URL+'post/'+postID,{
                method:'DELETE',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json'
                },
                body:JSON.stringify({   
                    PostID: postID
                })
            })
            .then(res=>res.json())
            .then((result)=>{ 
                alert(result);  
                getUsersPosts();
            },(_error)=>{
                alert('An error has occurred with deleting your post');
            });
        }
    }

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
                                MusicianID={ post.MusicianID } 
                                getUserPosts={ getUsersPosts }
                                deletePost={ deletePost } />
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