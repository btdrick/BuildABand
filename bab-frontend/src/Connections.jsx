import React, { useState, useCallback, useEffect } from "react";
import { variables } from "./Variables";
import UserProfile from "./components/UserProfile";
import Navbar from './components/header/Navbar';

function Connections() {
    const [pendingConnections, setPendingConnections] = useState([]);
    const [connectedConnections, setConnectedConnection] = useState([]);

    //Get all connections for the musician
    const getConnections = useCallback(async () => {
        const response = await fetch(variables.API_URL + "musicianconnections/" + UserProfile.getMusicianID())
        const data = await response.json();
        return data;
    }, []);

    /* Sets connections */
    useEffect(() => {
        getConnections().then((data) => {
            const pendingConnectionData = 
                data.filter(conn => !conn.Connected && 
                conn.FollowerID === UserProfile.getMusicianID());
            setPendingConnections(pendingConnectionData);
            setConnectedConnection(data);
        });
    }, [getConnections]);

    //Accepts the connection
    const acceptConnection = async(event) => {
        const connectionID = event.target.value;
        fetch(variables.API_URL + "musicianconnections/accept/" + connectionID, {
            method: "POST"
        })
            .then(res => (res.json()))
            .then(result => { 
                alert(result)
                getConnections().then((data) => {
                    const pendingConnectionData = 
                        data.filter(conn => !conn.Connected && 
                        conn.FollowerID === UserProfile.getMusicianID());
                    setPendingConnections(pendingConnectionData);
                    setConnectedConnection(data);
                });
            });
    }

    //Confirms disconnect
    const disconnect = async(e) => {
        if(window.confirm("Confirm disconnection?")) {
            rejectConnection(e);
        }
    }

    //Removes a connection
    const rejectConnection = async(e) => {
        const connectionID = e.target.value;
        fetch(variables.API_URL + "musicianconnections/reject/" + connectionID, {
            method: "POST"
        })
            .then(res => (res.json()))
            .then(result => { 
                alert(result);
                getConnections().then((data) => {
                    const pendingConnectionData = 
                        data.filter(conn => !conn.Connected && 
                        conn.FollowerID === UserProfile.getMusicianID());
                    setPendingConnections(pendingConnectionData);
                    setConnectedConnection(data);
                });  
            });
    }

    return (
        <div>
            <Navbar />
            <div className="mt-5 pt-5" >
                <h2>Connection Status</h2>
                <table style={{ width: "40%" }}>
                    <thead>
                        <tr>
                            <th>Musician Name</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            connectedConnections.map(conn =>
                                <tr key={conn.ConnectionID}>
                                    <td> {conn.FollowerID === UserProfile.getMusicianID()?
                                        conn.InitiatorNames : conn.FollowerNames}</td>
                                    <td> {conn.Connected ? "connected" : "pending"}</td>
                                    <td>{conn.Connected? 
                                        (
                                            <button value={conn.ConnectionID} 
                                                onClick={e=> disconnect(e)}>
                                                Disconnect
                                        </button> ) : null }   
                                    </td>                
                                </tr>
                            )
                        }
                    </tbody>
                </table>
            </div>
            <div className="pt-4">
                <h2>Pending Friends Request</h2>
                <table style={{ width: "20%" }}>
                    <thead>
                        <tr>
                            <th>Musician Name</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            pendingConnections.map(conn =>
                                <tr key={conn.ConnectionID}>
                                        <td> {conn.FollowerID === UserProfile.getMusicianID()?
                                        conn.InitiatorNames : conn.FollowerNames}</td>
                                    <td> {conn.Connected ? "connected" : "pending"}</td>
                                    <td>
                                        <button value={conn.ConnectionID}
                                            onClick={e => acceptConnection(e)}>
                                            Accept
                                        </button>
                                    </td>
                                    <td>
                                        <button value={conn.ConnectionID}
                                            onClick={e => rejectConnection(e)}>
                                            Reject
                                        </button>
                                    </td>
                                </tr>
                            )
                        }
                    </tbody>
                </table>
            </div>
        </div>
    )
}

export default Connections;