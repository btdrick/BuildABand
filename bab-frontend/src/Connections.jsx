import React, {Component} from "react";
import { variables } from "./Variables";

class Connections extends Component{
    
    constructor(props){
        super(props);
        this.state = {
            connections: []
        };
    }

    //Get connections
    componentDidMount(){
        fetch(variables.API_URL + "musicianconnections/" + this.props.MusicianID)
            .then(res => res.json())
            .then(result => {
                this.setState({
                    connections:result
                });
            })
    }

    render() {
      return(
          <div>
            <h1>Friends connections</h1>
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
                    this.state.connections.map(conn =>
                        <tr key={conn.ConnectionID}>
                            <td> {conn.FollowerNames}</td>
                            <td> {conn.Connected? "Connected": "Not Connected"}</td>
                        </tr>
                        )
                    }
                </tbody>
            </table>
          </div>  
        )  
    }
        


}

export default Connections;