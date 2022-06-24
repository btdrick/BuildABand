import React, {Component} from "react";
import { variables } from '../../Variables.js';
import UserProfile from "../../components/UserProfile";

class AddConnection extends Component{
    constructor(props){
      super(props);  

      this.sendConnectionRequest = this.sendConnectionRequest.bind(this);
    }

    sendConnectionRequest(){
        fetch (variables.API_URL + "musicianconnections/" + 
                UserProfile.getMusicianID()+"/" + this.props.followerID,{
                method: "POST"})
                .then(res => res.json())
                .then(result=> alert(result))         
    }


    render(){
        return(
            <div>
                <button className="btn btn-primary"
                    onClick={this.sendConnectionRequest}>
                    Add Connection
                </button>
            </div>
        )

    }
   

    
}

export default AddConnection;