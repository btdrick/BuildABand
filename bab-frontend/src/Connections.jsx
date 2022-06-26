import React, {Component} from "react";
import { variables } from "./Variables";
import UserProfile from "./components/UserProfile";
import Navbar from './components/header/Navbar';

class Connections extends Component{
    
    constructor(props){
        super(props);
        this.state = {
            pendingConn: [],
            connectedConn: []
        };
    }

    //Get connections
    componentDidMount(){
        this.getConnection();
    }

    acceptConnection = (event) => {
        console.log(event.target.value);
        const connectionID = event.target.value;
        fetch( variables.API_URL +"musicianconnections/accept/" + connectionID,{
            method:"POST"})
            .then(res=> (res.json()))
            .then(result => alert(result))
            this.getConnection();
    }

    getConnection(){
        fetch(variables.API_URL + "musicianconnections/" + UserProfile.getMusicianID())
            .then(res => res.json())
            .then(result => {
                this.setState({
                    pendingConn:result.filter(conn => !conn.Connected),
                    connectedConn:result.filter(conn => conn.Connected)
                });
            })

    }
    

 

    render() {
        
      return(
          <div>
            <Navbar/>
            <div>
              <h2>Connected Friends</h2>
               <table style={{width: "20%"}}>
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
                            <td> {conn.FollowerNames}</td>
                            <td> {conn.Connected? "connected" : "pending"}</td>
                            
                        </tr> 
                        )
                    }
                </tbody>
              </table>   
            </div>
            <div className="pt-4">
            <h2>Pending Friends Request</h2>
               <table style={{width: "20%"}}>
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
                            <td> {conn.FollowerNames}</td>
                            <td> {conn.Connected? "connected" : "pending"}</td>
                            <td>
                                <button value={conn.ConnectionID} 
                                    onClick={this.acceptConnection}>
                                    Accept
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