import { React, useCallback } from 'react';
import {variables} from './Variables.js';
import { useParams } from "react-router-dom";
import Feed from './components/feed/Feed.jsx';
import Navbar from './components/header/Navbar';
import AddConnection from './components/connection/AddConnection.js';
import UserProfile from './components/UserProfile.js';
import './style/home.css';

function Profile() {
    /* Profile's owner */
    const { id } = useParams();

    /* Makes api call to backend to get the user's posts */
    const getUsersPosts = useCallback(async () => {
        const response = await fetch(variables.API_URL+'post/'+ id);
        const data = await response.json();
        return data.reverse();
    }, [id]);

    /* Renders the profile page's html. You can't pass entire object to child component */
    return ( 
        <div id="container">
            <Navbar/>
            <h3 className="title"> This is the Profile page </h3> 
            <div className="container-lg">
                <AddConnection followerID={ id } />
                <Feed getPosts={ getUsersPosts } 
                canCreatePost={ parseInt(id) === UserProfile.getMusicianID() }/>
            </div>
        </div>
    );
}

export default Profile;