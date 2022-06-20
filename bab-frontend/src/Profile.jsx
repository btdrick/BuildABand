import React, { Component } from 'react';
import { Post } from './Post';
import {variables} from './Variables.js';
import UserProfile from './components/UserProfile.js';
import Navbar from './components/header/Navbar';
import AddConnection from './components/connection/AddConnection.js'

export class Profile extends Component {
    /* Constructor for the component. Tracks the posts shown on the profile and whether the page's loading status */
    constructor(props) {
        super(props);
        this.state = { 
            posts:      [], 
            PostID:     0,          
            MusicianID: UserProfile.getMusicianID(),
            Content:    "",
            modalTitle: "",
            loading:    true
        };
    }

    /* Once the page renders, this lifecycle method takes place */
    componentDidMount(){
        this.getUsersPosts();
    }
    
    /* Makes api call to backend to get the user's posts */
    async getUsersPosts() {
        const response = await fetch(variables.API_URL+'post/'+ this.state.MusicianID);
        const data = await response.json();
        this.setState({
            posts: data,
            loading: false,
        });
    }

    /* Handles event of text entry for Post content */
    changePostContent =(e)=>{
        this.setState({Content:e.target.value});
    }

    /* Handles onClick event for Add button */
    addClick(){
        this.setState({
            modalTitle: "Create Post",
            PostID:     0,
            Content:    ""          
        });
    }

    /* Handles onClick event for Create button */
    createClick(){
        fetch(variables.API_URL+'post',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                CreatedTime: new Date(),                           
                MusicianID: this.state.MusicianID,
                Content:    this.state.Content
            })
        })
        .then(res=>res.json())
        .then((result)=>{
            alert(result);
            this.getUsersPosts();
        },(_error)=>{
            alert('Post content cannot be blank');
        })
    }

    /* Renders the profile page's html. You can't pass entire object to child component */
    render() {
        const {     
            modalTitle,
            PostID,
            Content
        }=this.state;
        if (this.state.loading) {
            return (
                <div>
                    <p>loading...</p>
                </div>
            );
        }
        else {
            return ( 
                <div>
                    <Navbar/>
                    <h3> This is the Profile page </h3> 
                    <button type="button"
                    className="btn btn-primary m-2 float-end"
                    data-bs-toggle="modal"
                    data-bs-target="#exampleModal"
                    onClick={()=>this.addClick()}>
                        Create Post
                    </button>
    
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
                                    onChange={this.changePostContent}/>
                                </div>
    
                                <div className="modal-body">               
                                    {PostID===0?
                                    <button type="button"
                                    className="btn btn-primary float-start"
                                    onClick={()=>this.createClick()}
                                    >Create</button>
                                    :null}
                                </div>
                            </div>
                        </div>
                    </div>
                    <ul>
                        {this.state.posts.map((post, index) => 
                        <li key={index}><Post content={post.Content} /></li>)}
                        <AddConnection followerID={this.state.MusicianID}/>    
                    </ul>
                               
                </div>
            )
        }
    }
}

export default Profile;