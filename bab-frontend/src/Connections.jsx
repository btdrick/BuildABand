import React, {Component} from "react";
import { variables } from "./Variables";

class Connections extends Component{
    
    constructor(){
        super();
        this.state = {
            connections: []
        };
    }

    //Get connections
    componentDidMount(){
        fetch(variables.API_URL + "musicianconnections/" + this.props.musicianID)
            .then(res => res.json())
            .then(result => {
                this.setState({
                    connections:result
                });
            })
    }


}

export default Connections;