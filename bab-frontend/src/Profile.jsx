import { React, useState, useCallback, useEffect } from 'react';
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
    const isMyProfile = (id === UserProfile.getMusicianID().toString()) ? true : false;
    const [profileInfo, setProfileInfo] = useState([]);
    const [connection, setConnection] = useState([]);

    /* Sets backend posts */
    useEffect(() => {
        const getProfileInfo = async() => {
            const response = await fetch(variables.API_URL+'musician/'+ id);
            const data = await response.json();
            return data;
        }
        getProfileInfo().then((data) => {
            setProfileInfo(data[0]);
        });

        //Get all connections for the musician
        const getConnections = async () => {
            const response = await fetch(variables.API_URL + "musicianconnections/" + id)
            const data = await response.json();
            return data;
        }
        getConnections().then((data) => {
            const connection = data.find((connection) => connection.FollowerID === parseInt(id));
            setConnection(connection);
        });
    }, [id]);

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
            <h3 className="title"> Profile: {profileInfo.Fname + " " + profileInfo.Lname} </h3> 
            <h4 className="mg-3 text-center"> Instrument: {profileInfo.Instrument} </h4>
            <div className="container-lg">
                {!isMyProfile &&
                    <AddConnection 
                    followerID={ parseInt(id) }
                    connection={ connection } />
                }
                <Feed getPosts={ getUsersPosts } 
                canCreatePost={ parseInt(id) === UserProfile.getMusicianID() }/>
            </div>
        </div>
    );
}

export default Profile;