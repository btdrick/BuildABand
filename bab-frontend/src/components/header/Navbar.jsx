import React from 'react';
import { Link } from "react-router-dom";
import UserProfile from '../../components/UserProfile.js';
import Search from "../../components/search/Search";
import {Navigate} from 'react-router-dom';
import { variables } from '../../Variables';
import "./navbar.css";


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
                    {/* Navigation bar for logged- in user*/}
                    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
                    <div className="container-fluid">
                        <Link className="nav-link" to="/home">Home</Link>
                        <Link className="nav-link" to={`/profile/${UserProfile.getMusicianID()}`}>Profile</Link>
                        <Link className="nav-link" to="/connections">Connections</Link>
                        <button className="nav-link bg-dark" onClickCapture={this.logout}>Logout</button>
                        {/* Search a user */}
                        <Search 
                        className="nav-search"
                        placeholder="Search for a musician..." 
                        data={ this.state.musicians } />
                    </div>
                    </nav>
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