import { React, useState, useEffect } from 'react';
import Form from 'react-bootstrap/Form';
import UserProfile from '../UserProfile';

const FeedFilterSwitch = ({ getConnections, getBackendPosts, setBackendPosts }) => {
    /* All of the users connections */
    const [connections, setConnections] = useState([]);

    /* Once the page renders, this hook takes place */
    useEffect(() => {
        getConnections().then((data) => {
            var acceptedConnections = data.filter(connection => connection.Connected);
            setConnections(acceptedConnections);
        });
    }, [getConnections]);

    /* Applies friends only filter to backend posts */
    const applyFriendsOnlyFilter = () => {
        /* Collect unique initiator, follower IDs*/
        var connectionIDs = [];
        connections.forEach(function(connection) {
            var initiator = connectionIDs.findIndex(i => i === connection.InitiatorID);
            var follower = connectionIDs.findIndex(i => i === connection.FollowerID);
            if(initiator <= -1) {
                connectionIDs.push(connection.InitiatorID);
            }
            if(follower <= -1) {
                connectionIDs.push(connection.FollowerID);
            }
        });
        if(!connectionIDs.includes(UserProfile.getMusicianID())) {
            connectionIDs.push(UserProfile.getMusicianID());
        }

        /* Set posts to posts made by friends */
        getBackendPosts().then((data) => {
            var friendsPosts = data.filter(post => connectionIDs.includes(post.MusicianID));
            setBackendPosts(friendsPosts);
        });
    }
    
    /* Handles event of toggle feed filter */
    const toggleFilter =(e)=>{
        let toggled = e.target.checked; 
        if(toggled === true) {
            applyFriendsOnlyFilter();
        }
        if(toggled === false) {
            getBackendPosts().then((data) => {
                setBackendPosts(data);
            });
        } 
    };

    return (
        <Form.Switch 
            type="switch"
            id="custom-switch"
            label="Friends' posts only"
            onChange={toggleFilter}/> 
    );
}

export default FeedFilterSwitch;