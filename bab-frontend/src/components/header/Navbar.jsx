import React from 'react';
import { Link } from "react-router-dom";
import UserProfile from '../../components/UserProfile.js';
import Search from "../../components/search/Search";
import {Navigate} from 'react-router-dom';
import { variables } from '../../Variables';


export class Navbar extends React.Component {
    constructor() {
        super();
        this.state = {
          musicianID: UserProfile.getMusicianID(),
          musicians: [],
          isLoggedOut: false,
        };
        this.logout = this.logout.bind(this);
    }

    componentDidMount(){
        this.getMusicians();
    }

    /* Makes api call to backend to get all Musicians */
    async getMusicians() {
        const response = await fetch(variables.API_URL+'musician');
        const data = await response.json();
        this.setState({
            musicians: data
        });
    }

    logout() {
        UserProfile.clearSession();
        this.setState({isLoggedOut: true});
      };

    render() {
        
            if (!this.state.isLoggedOut) {
                return (
                <header>
                <Link to="/home">Home</Link>
                <Link to="/profile">Profile</Link>
                <Link to="/connections">Connections</Link>
                <button onClickCapture={this.logout}>Logout</button>
                <Search 
                placeholder="Search for a musician..." 
                data={ this.state.musicians } />
            </header>
                )
            }
            else {
                return(
                    <Navigate to="/"></Navigate>
                )
            }
        
    }

}

export default Navbar;