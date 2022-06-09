import React, { Component } from 'react';
import { Link } from "react-router-dom";
import { Post } from './Post';
import {variables} from './Variables.js';

export class Profile extends Component {
    /* Constructor for the component. Tracks the posts shown on the profile and whether the page's loading status */
    constructor(props) {
        super(props);
        this.state = { 
            posts: [], 
            loading: true
        };
    }

    /* Once the page renders, this lifecycle method takes place */
    componentDidMount(){
        this.getUsersPosts();
    }
    
    /* Makes api call to backend to get the user's posts */
    async getUsersPosts() {
        const response = await fetch(variables.API_URL+'posts/'+ this.props.musicianID);
        const data = await response.json();
        this.setState({
            posts: data,
            loading: false,
        });
    }

    /* Renders the profile page's html. You can't pass entire object to child component */
    render() {
        return ( 
            <div>
                <Link to="/">Login</Link>
                <h3> This is the Profile page </h3> 
                <div>              
                <ul>
                    {this.state.posts.map((post, index) => <li key={index} style={{ outline:"thin solid black" }}> <Post content={post.Content}/></li>)}
                </ul>  
                </div>
            </div>
        )
    }
}

export default Profile;