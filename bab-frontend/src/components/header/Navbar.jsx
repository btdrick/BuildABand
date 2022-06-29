import React from 'react';
import {Navigate} from 'react-router-dom';
import { variables } from '../../Variables';
import UserProfile from '../../components/UserProfile.js';
import Search from "../../components/search/Search";
import Dropdown from 'react-bootstrap/Dropdown';
import Nav from 'react-bootstrap/Nav'
import Card from 'react-bootstrap/Card'
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
        if (window.confirm("Are you sure you want to logout?")) {
        UserProfile.clearSession();
        this.setState({isLoggedOut: true});
        }
    };

    render() {
        
            if (!this.state.isLoggedOut) {
                return (
                <header>
                    {/* Navigation bar for logged-in user*/}
                    <nav className="navbar fixed-top navbar-expand-lg navbar-dark bg-dark"> 
                        <div className="collapse navbar-collapse" id="navbarSupportedContent">                
                            <a className="navbar-brand" href="#/home">
                                <img id="logo" src={require('../../style/images/Build-A-Band.png')} alt={'Build a band logo'}/>
                            </a>
                            {/* Search a user */}
                            <Search 
                            className="nav-search"
                            placeholder="Search for a musician..." 
                            data={ this.state.musicians } />
                            {/* Nav links */}
                            <Nav className="ms-auto" defaultActiveKey="/home">
                                <Nav.Item>
                                    <Nav.Link href="#/home">
                                        Home
                                    </Nav.Link>
                                </Nav.Item>
                                <Nav.Item>
                                    <Nav.Link href="#/connections">
                                        Connections
                                    </Nav.Link>
                                </Nav.Item>
                            </Nav>
                            {/* Dropdown menu */}
                            <Dropdown className="dropdown">
                                <Dropdown.Toggle variant="dark" id="dropdown-basic">
                                    <img id="avatar" src={require('../comments/user-icon.png')} alt={'User icon'}/>
                                </Dropdown.Toggle>

                                <Dropdown.Menu className="dropdown-menu">
                                    <Card body>
                                        <img id="avatar" src={require('../comments/user-icon.png')} alt={'User icon'}
                                        style={{float: 'left', marginRight: 1 + 'em'}}/>
                                        <Card.Title>John Doe</Card.Title>
                                        <Card.Subtitle className="mb-2 text-muted">Guitar</Card.Subtitle>
                                    </Card>
                                    <Dropdown.Item href={`#/profile/${UserProfile.getMusicianID()}`}>Profile</Dropdown.Item>
                                    <Dropdown.Item onClick={this.logout}>Logout</Dropdown.Item>
                                </Dropdown.Menu>
                            </Dropdown>
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