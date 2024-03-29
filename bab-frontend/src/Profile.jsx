import { React, useCallback, useState, useEffect } from 'react';
import { variables } from './Variables.js';
import { useParams } from "react-router-dom";
import UserProfile from './components/UserProfile.js';
import Feed from './components/feed/Feed.jsx';
import Navbar from './components/header/Navbar';
import AddConnection from './components/connection/AddConnection.js';
import DeactivateAccount from './components/account/DeactivateAccount.js';
import Button from 'react-bootstrap/Button'
import './style/home.css';

function Profile() {
    /* Profile's owner */
    const { id } = useParams();
    const isMyProfile = (id === UserProfile.getMusicianID().toString()) ? true : false;
    const [profileInfo, setProfileInfo] = useState([]);
    const [connection, setConnection] = useState([]);
    const [isActive, setIsActive] = useState(false);

    /* Sets profile and connection information */
    useEffect(() => {
        ///Get whether user is active
        const getIsActive = async() => {
            fetch(variables.API_URL+"accounts/"+id+"/is_active")
            .then(res=>res.json())
            .then((data) => {
                setIsActive(data);
            })
        }
        getIsActive();

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

    const canDeactivate = isMyProfile || UserProfile.getIsAdmin();

    /* Makes api call to backend to get the user's posts */
    const getUsersPosts = useCallback(async () => {
        const response = await fetch(variables.API_URL+'post/'+ id);
        const data = await response.json();
        return data.reverse();
    }, [id]);

    /* Renders the profile page's html. You can't pass entire object to child component */
    if (isActive) {
        return ( 
            <div id="container">
                <Navbar/>
                {/* Profile header */}
                <h3 className="title"> Profile: {profileInfo.Fname + " " + profileInfo.Lname} </h3>
                {profileInfo.Instrument ? (
                <h4 className="text-center text-muted">Instrument: {profileInfo.Instrument} </h4>
                ) : (
                <h4 className="text-center text-muted">No instrument chosen{profileInfo.Instrument} </h4>)}
                <h6 className="text-center">{profileInfo.City + " ," + profileInfo.StateCode}</h6>
                {/* Profile buttons */}
                <div className="container-lg">
                    {/* Deactivate account section */}
                    {canDeactivate && 
                        <DeactivateAccount accountID={ id } />}
                    {/* Add connection section */}
                    {!isMyProfile &&
                        <AddConnection 
                        followerID={ parseInt(id) }
                        connection={ connection } />}
                    {/* Projects */}
                        <Button 
                        variant="secondary"
                        href={`#/profile/projects/${id}`}>
                        Projects</Button>
                    {/* Profile feed */}
                        <Feed getPosts={ getUsersPosts } 
                        canCreatePost={ parseInt(id) === UserProfile.getMusicianID() }/>
                </div>
            </div>
        );
    }
    if (!isActive) {
        return(<div>This user is currently inactive</div>);
    }
}

export default Profile;