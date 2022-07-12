import React, { useState, useCallback, useEffect } from "react";
import { variables } from "./Variables";
import UserProfile from "./components/UserProfile";
import Navbar from './components/header/Navbar';
import Table from 'react-bootstrap/Table'
import Button from 'react-bootstrap/Button'

function Connections() {
    const [pendingConnections, setPendingConnections] = useState([]);
    const [connectedConnections, setConnectedConnection] = useState([]);
    const [activeConnections, setActiveConnections] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    //Get all connections for the musician
    const getConnections = useCallback(async () => {
        const response = await fetch(variables.API_URL + "musicianconnections/" + UserProfile.getMusicianID())
        const data = await response.json();
        return data;
    }, []);

    //Get all connections for the musician
    const getActiveConnections = useCallback(async () => {
        const response = await fetch(variables.API_URL + "musicianconnections/" + UserProfile.getMusicianID() + "/active")
        const data = await response.json();
        return data;
    }, []);

    //Check if a connection is active
    function isActiveConnection(connection) {
        var isActive = false;
        activeConnections.every(conn => {
            isActive = conn.ConnectionID === connection.ConnectionID;
            if (isActive === true) {
                return false;
            }
            return true;
        });

        return isActive;
    }

    /* Check connection status and return corresponding value */
    function printConnectionStatus (connection){
        if (connection === 0)
            return "Pending";
        else if (connection === 1)
            return "Connected";
        else if (connection === 2)
            return "Rejected";
    }

    function getStatusButton(conn){
        if ( conn.Connected === 1 || (!isActiveConnection(conn) && !conn.Connected))
          {
            return(
                <Button size="sm"
                    className="btn btn-danger"
                    value={conn.ConnectionID} 
                    onClick={e=> disconnect(e)}>
                    Disconnect
                </Button>
            )
          }
            
        else if ( conn.Connected === 2 || (!isActiveConnection(conn) && !conn.Connected))
         {
            return (
                <Button size="sm"
                    className="btn btn-danger"
                    value={conn.ConnectionID} 
                    onClick={e=> clearRejectedConnection(e)}>
                    Clear
                </Button>
            )       
         }
            
        else 
            return null;              
                   
    }	

    /* Sets connections */
    useEffect(() => {
        getConnections().then((data) => {
            const pendingConnectionData = 
                data.filter(conn => conn.Connected === 0 && 
                conn.FollowerID === UserProfile.getMusicianID());
            setPendingConnections(pendingConnectionData);
            setConnectedConnection(
                data.filter(conn => !(conn.Connected === 2 &&
                    conn.FollowerID === UserProfile.getMusicianID()))
            );
        });

        getActiveConnections().then((data) => {
            setActiveConnections(data);
        });

        setIsLoading(false);
    }, [getConnections, getActiveConnections]);

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
                    setConnectedConnection(
                        data.filter(conn => !(conn.Connected === 2 &&
                            conn.FollowerID === UserProfile.getMusicianID()))
                    );
                });
            });
    }

    const clearRejectedConnection =async(e) => {
       
                const connectionID = e.target.value;
                fetch(variables.API_URL + "musicianconnections/disconnect/" + connectionID, {
                    method: "POST"
                })
                    .then(res => (res.json()))
                    .then(result => { 
                        getConnections().then((data) => {
                            const pendingConnectionData = 
                                data.filter(conn => !conn.Connected && 
                                conn.FollowerID === UserProfile.getMusicianID());
                            setPendingConnections(pendingConnectionData);
                            setConnectedConnection(
                                data.filter(conn => !(conn.Connected === 2 &&
                                conn.FollowerID === UserProfile.getMusicianID()))
                                );
                        });  
                    });
            
        
    }

    //Confirms disconnect
    const disconnect = async(e) => {
        if(window.confirm("Confirm disconnection?")) {
            {
                const connectionID = e.target.value;
                fetch(variables.API_URL + "musicianconnections/disconnect/" + connectionID, {
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
                            setConnectedConnection(
                                data.filter(conn => !(conn.Connected === 2 &&
                                conn.FollowerID === UserProfile.getMusicianID()))
                            );
                        });  
                    });
            }
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
                        data.filter(conn => conn.Connected === 0 && 
                        conn.FollowerID === UserProfile.getMusicianID());
                    setPendingConnections(pendingConnectionData);
                    setConnectedConnection(
                        data.filter(conn => !(conn.Connected === 2 &&
                            conn.FollowerID === UserProfile.getMusicianID()))
                    );
                });  
            });
    }
    if (!isLoading) {
        return (
            <div className="container-lg justify-content-center text-center">
                <Navbar />
                <h1 style={{marginTop: "8%"}}>Connections Page</h1>
                <hr></hr>
                <div className="mt-4" style={{display: "inline-block", margin: "2em", minWidth: "40%"}}>
                    <h2>Connection Status</h2>
                    <Table striped bordered hover variant="dark">
                        <thead>
                            <tr>
                                <th>Musician Name</th>
                                <th>Status</th>
                                <th colSpan={2}></th>
                            </tr>
                        </thead>
                        <tbody>
                            {connectedConnections.map(conn =>
                                <tr key={conn.ConnectionID}>
                                    {isActiveConnection(conn) ? (
                                        <td> {conn.FollowerID === UserProfile.getMusicianID() ?
                                            conn.InitiatorNames : conn.FollowerNames}</td>
                                            ) : (
                                        <td className="text-warning"> {conn.FollowerID === UserProfile.getMusicianID() ?
                                            "(Inactive) " + conn.InitiatorNames : "(Inactive) " + conn.FollowerNames}</td>)}

                                    <td> {printConnectionStatus(conn.Connected) }</td>
                                    <td>
                                        {getStatusButton (conn)}
                                    </td>                
                                </tr>
                            )}
                        </tbody>
                    </Table>
                </div>
                <div className="mt-4" style={{display: "inline-block", margin: "2em", minWidth: "40%"}}>
                    <h2>Pending Requests</h2>
                    <Table striped bordered hover variant="dark">
                        <thead>
                            <tr>
                                <th>Musician Name</th>
                                <th>Status</th>
                                <th colSpan={2}></th>
                            </tr>
                        </thead>
                        <tbody>
                            {pendingConnections.map(function(conn) {
                                if(isActiveConnection(conn)) {
                                return <tr key={conn.ConnectionID}>
                                    <td> {conn.FollowerID === UserProfile.getMusicianID() ?
                                    conn.InitiatorNames : conn.FollowerNames}</td>
                                    <td> {conn.Connected ? "connected" : "pending"}</td>
                                    <td>
                                        <Button size="sm"
                                            className="btn btn-primary" 
                                            value={conn.ConnectionID}
                                            onClick={e => acceptConnection(e)}>
                                            Accept
                                        </Button>
                                    </td>
                                    <td>
                                        <Button size="sm"
                                            className="btn btn-danger"
                                            value={conn.ConnectionID}
                                            onClick={e => rejectConnection(e)}>
                                            Reject
                                        </Button>
                                    </td>
                                </tr>}
                                else {
                                    return null;
                                }}
                            )}
                        </tbody>
                    </Table>
                </div>
            </div>
        )
    }
    if (isLoading) {
        return(<div>Loading connections...</div>)
    }
}

export default Connections;