import { React, useCallback, useState, useEffect } from 'react';
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

    /* Sets profile and connection information */
    useEffect(() => {
        ///Set musician information
        const getProfileInfo = async() => {
            fetch(variables.API_URL+'musician/'+ id)
            .then(res=>res.json())
            .then((data) => {
                setProfileInfo(data[0]);
            })
        }
        getProfileInfo();
        
        //Set connection information
        const getConnection = async () => {
            if (isMyProfile) {
                setConnection([]);
                return;
            }
            fetch(variables.API_URL + "musicianconnections/" + id)
            .then(res=>res.json())
            .then((data) => {
                const dataConnection = data.find((conn) => (parseInt(id) === conn.FollowerID && UserProfile.getMusicianID() === conn.InitiatorID) 
                || (parseInt(id) === conn.InitiatorID && UserProfile.getMusicianID() === conn.FollowerID));
                setConnection(dataConnection);
            })
        }
        getConnection();
    }, [id, isMyProfile]);

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
            {profileInfo.Instrument ? (
            <h4 className="text-center text-muted">Instrument: {profileInfo.Instrument} </h4>
            ) : (
            <h4 className="text-center text-muted">No instrument chosen{profileInfo.Instrument} </h4>)}
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