import React, { Component } from 'react';
import { Link } from "react-router-dom";
import { Post } from './Post';
import {variables} from './Variables.js';

export class Home extends Component {
    /* Constructor for the component. Tracks the posts shown on the feed and whether the page's loading status */
    constructor(props) {
        super(props);
        this.state = { 
            posts:      [], 
            PostID:     0,          
            MusicianID: 0,
            Content:    "",

            modalTitle: "",
            loading:    true
        };
    }

    /* Once the page renders, this lifecycle method takes place */
    componentDidMount(){
        this.getPosts();
    }
    
    /* Makes api call to backend to get all posts */
    async getPosts() {
        const response = await fetch(variables.API_URL+'post');
        const data = await response.json();
        this.setState({
            posts:  data,
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
            MusicianID: 1,
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
            this.getPosts();
        },(_error)=>{
            alert('Post content cannot be blank');
        })
    }

    /* HTML for the Build-A-Band Main feed */
    render() {
        const {     
            modalTitle,
            PostID,
            Content
        }=this.state;
        return ( 
            <div>
                <Link to="/">Login</Link>              
                <h3> This is the Home page </h3>
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
                 
                 {/* List-group feed containing card-style group-items */}
                <div>
                    <ul className="list-group" style={{listStyleType: 'none'}}>
                        {this.state.posts.map((post, index) => 
                        <li key={index} className="list-group-item"><Post content={post.Content} postID={post.PostID} /></li>)}
                    </ul>
                </div>
            </div>
        )
    }
}

export default Home;