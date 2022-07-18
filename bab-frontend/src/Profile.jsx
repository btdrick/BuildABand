import { React, useCallback, useState, useEffect } from 'react';
import {variables} from './Variables.js';
import { useParams } from "react-router-dom";
import UserProfile from './components/UserProfile.js';
import Feed from './components/feed/Feed.jsx';
import Project from './components/project/Project.jsx';
import Navbar from './components/header/Navbar';
import AddConnection from './components/connection/AddConnection.js';
import CreateProject from './modals/CreateProject.jsx';
import DeactivateAccount from './components/account/DeactivateAccount.js';
import './style/home.css';

function Profile() {
    /* Profile's owner */
    const { id } = useParams();
    const isMyProfile = (id === UserProfile.getMusicianID().toString()) ? true : false;
    const [profileInfo, setProfileInfo] = useState([]);
    const [connection, setConnection] = useState([]);
    const [projects, setProjects] = useState([]);

    /* Updates projects in real time */
    useEffect(() => {
        console.log(projects);
    }, [projects]);

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
        getUserProjects();
    }, [id, isMyProfile]);

    const canDeactivate = isMyProfile || UserProfile.getIsAdmin();

    /* Makes api call to backend to get the user's posts */
    const getUsersPosts = useCallback(async () => {
        const response = await fetch(variables.API_URL+'post/'+ id);
        const data = await response.json();
        return data.reverse();
    }, [id]);

    /* Makes api call to backend to get the user's projects */
    const getUserProjects = useCallback(async () => {
        const response = await fetch(variables.API_URL+'project/'+id);
        const data = await response.json();
        setProjects(data.reverse());
    }, [id])

    /* Renders the profile page's html. You can't pass entire object to child component */
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
                {/* Profile feed */}
                <table width="100%">
                    <colgroup>
                        <col span="4" style={{width: "50%"}}></col>
                        <col span="4"></col>
                    </colgroup>
                    <thead style={{textAlign: "center"}}>
                        <tr>
                            <th>Post History</th>
                            <th>Open Projects</th>
                        </tr>
                    </thead>

                    <tbody>
                        <tr>
                            <td>

                            </td>
                            <td style={{textAlign: "center"}}>
                                <CreateProject />
                            </td>
                        </tr>

                        <tr>
                            <td style={{verticalAlign: "top"}}>
                                <Feed getPosts={ getUsersPosts } 
                                canCreatePost={ parseInt(id) === UserProfile.getMusicianID() }/>
                            </td>
                            <td style={{verticalAlign: "top"}}>
                                {projects.map(x => <div className='row' key={x.ProjectID}>
                                    <Project 
                                        name={x.Name}
                                        FileName={x.FileName}
                                        AzureFileName={x.AzureFileName}
                                    /> 
                                </div>)}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default Profile;