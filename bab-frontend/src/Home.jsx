import React, { Component } from 'react';
import { variables } from './Variables.js';
import Feed from './components/feed/Feed.js';
import Navbar from './components/header/Navbar';
import './style/home.css';

export class Home extends Component {
    /* Makes api call to backend to get all posts */
    async getAllPosts() {
        const response = await fetch(variables.API_URL+'post');
        const data = await response.json();
        return data;
    };

    /* HTML for the Build-A-Band Main feed */
    render() {
        return ( 
            <div id="container">   
                <Navbar/>          
                <h3 className="title"> Build-A-Band Home Feed </h3>
                <div className="container-lg">
                    <Feed getPosts={ this.getAllPosts } 
                    canCreatePost={ true } />
                </div>
            </div>
        )
    }
}

export default Home;