import React, { Component } from "react";
import { variables } from "./Variables";
import UserProfile from "./components/UserProfile";
import Navbar from './components/header/Navbar';

class Connections extends Component {

    constructor(props) {
        super(props);
        this.state = {
            pendingConn: [],
            connectedConn: [],
        };
    }

    //Get connections
    componentDidMount() {

        this.getConnection();
    }

    componentDidUpdate() {
        this.getConnection();
    }

    acceptConnection = (event) => {
        const connectionID = event.target.value;
        fetch(variables.API_URL + "musicianconnections/accept/" + connectionID, {
            method: "POST"
        })
            .then(res => (res.json()))
            .then(result => alert(result));
      
    }

    getConnection() {
        fetch(variables.API_URL + "musicianconnections/" + UserProfile.getMusicianID())
            .then(res => res.json())
            .then(result => {
                this.setState({
                    pendingConn: result.filter(conn => !conn.Connected && conn.FollowerID === UserProfile.getMusicianID()),
                    connectedConn: result
                });
            })

    }

    rejectConnection = (event) => {
        const connectionID = event.target.value;
        fetch(variables.API_URL + "musicianconnections/reject/" + connectionID, {
            method: "POST"
        })
            .then(res => (res.json()))
            .then(result => alert(result));
    }


    render() {
       

        return (
            <div>
                <Navbar />
                <div>
                    <h2>Connection Status</h2>
                    <table style={{ width: "20%" }}>
                        <thead>
                            <tr>
                                <th>
                                    Musician Name
                                </th>
                                <th>
                                    Status
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                this.state.connectedConn.map(conn =>
                                    <tr key={conn.ConnectionID}>
                                        <td> {conn.FollowerID === UserProfile.getMusicianID()?
                                            conn.InitiatorNames : conn.FollowerNames}</td>
                                        <td> {conn.Connected ? "connected" : "pending"}</td>
                                        <td>{conn.Connected? 
                                            (
                                             <button value={conn.ConnectionID} 
                                                    onClick={this.rejectConnection}>
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
                                <th>
                                    Musician Name
                                </th>
                                <th>
                                    Status
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                this.state.pendingConn.map(conn =>
                                    <tr key={conn.ConnectionID}>
                                         <td> {conn.FollowerID === UserProfile.getMusicianID()?
                                            conn.InitiatorNames : conn.FollowerNames}</td>
                                        <td> {conn.Connected ? "connected" : "pending"}</td>
                                        <td>
                                            <button value={conn.ConnectionID}
                                                onClick={this.acceptConnection}>
                                                Accept
                                            </button>
                                        </td>
                                        <td>
                                            <button value={conn.ConnectionID}
                                                onClick={this.rejectConnection}>
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

}



export default Connections;