import React, { Component } from 'react';
import { variables } from './Variables.js';
import Post from './components/post/Post.js';
import UserProfile from './components/UserProfile.js';
import Navbar from './components/header/Navbar';
import './style/home.css';

export class Home extends Component {
    /* Constructor for the component. Tracks the posts shown on the feed and whether the page's loading status */
    constructor(props) {
        super(props);
        this.handler = this.handler.bind(this);
        this.state = { 
            posts:      [], 
            musicians:  [],
            PostID:     0,          
            MusicianID: UserProfile.getMusicianID(),
            Content:    "",
            modalTitle: "",
            loading:    true
        };
    };

    /* Allows child to update the posts */
    handler() {
        this.getPosts();
    }

    /* Once the page renders, this lifecycle method takes place */
    componentDidMount(){
        this.getPosts();
        this.getMusicians();
    };
    
    /* Makes api call to backend to get all posts */
    async getPosts() {
        const response = await fetch(variables.API_URL+'post');
        const data = await response.json();
        this.setState({
            posts:  data,
            loading: false,
        });
    };

    /* Makes api call to backend to get all Musicians */
    async getMusicians() {
        const response = await fetch(variables.API_URL+'musician');
        const data = await response.json();
        this.setState({
            musicians: data
        });
    };

    /* Handles event of text entry for Post content */
    changePostContent =(e)=>{
        this.setState({Content:e.target.value});
    };

    /* Handles onClick event for Add button */
    addClick() {
        this.setState({
            modalTitle: "Create Post",
            PostID:     0,
            Content:    ""          
        });
    };

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
        },(_error)=>{
            alert('Post content cannot be blank');
        });
    };

    /* Removes post and its affiliated likes, comments from database */
    async deletePost(postID) {
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
                this.handler();
            },(_error)=>{
                alert('An error has occurred with deleting your post');
            });
        }
    }

    /* HTML for the Build-A-Band Main feed */
    render() {
        const {     
            modalTitle,
            PostID,
            Content
        }=this.state;
        return ( 
            <div id="container">   
                <Navbar/>          
                <h3 className="title"> Build-A-Band Home Feed </h3>
                {/* Create post modal*/}
                <button type="button"
                className="btn btn-primary m-3"
                data-bs-toggle="modal"
                data-bs-target="#exampleModal"
                onClick={()=>this.addClick()}>
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
                 
                 {/* List-group feed containing card-style group-items */}
                <div>
                    <ul className="list-group">
                        {this.state.posts.map((post, index) => 
                        <li key={index} className="list-group-item">
                            <Post 
                            PostID={ post.PostID }
                            CreatedTime={ post.CreatedTime }
                            Content={ post.Content }
                            MusicianID={ post.MusicianID }
                            deletePost={ this.deletePost } 
                            handler={ this.handler }/>
                            </li>)}
                    </ul>
                </div>
                </div>
            </div>
        )
    }
}

export default Home;