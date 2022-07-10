import React from 'react';
import {Navigate} from 'react-router-dom';
import { variables } from '../../Variables';
import SearchBar from "../../components/searchBar/SearchBar";
import UserProfile from '../../components/UserProfile.js';
import Dropdown from 'react-bootstrap/Dropdown';
import Nav from 'react-bootstrap/Nav';
import Card from 'react-bootstrap/Card';
import LogoutIcon from '@mui/icons-material/Logout';
import "./navbar.css";

export class Navbar extends React.Component {
    constructor() {
        super();
        this.state = {
          musicianID: UserProfile.getMusicianID(),
          musicians: [],
          currentMusicianInfo: [],
          isLoggedOut: false,
        };
        this.logout = this.logout.bind(this);
    }

    componentDidMount(){
        this.getActiveMusicians();
        this.getCurrentMusicianInfo();
    }

    /* Makes api call to backend to get all Musicians */
    async getActiveMusicians() {
        const response = await fetch(variables.API_URL+'musician/active');
        const data = await response.json();
        this.setState({
            musicians: data
        });
    }

    /* Makes api call to backend to get current musician info */
    async getCurrentMusicianInfo() {
        const response = await fetch(variables.API_URL+'musician/'+this.state.musicianID);
        const data = await response.json();
        this.setState({
            currentMusicianInfo: data[0]
        });
    }

    /* Clears the session for logout */
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
                    <nav className="navbar fixed-top navbar-expand-lg bg-dark navbar-dark"> 
                        <div className="collapse navbar-collapse" id="navbarSupportedContent">                
                            <a className="navbar-brand" href="#/home">
                                <img id="logo" src={require('../../style/images/bab.png')} alt={'Build a band logo'}/>
                            </a>
                            <SearchBar 
                            placeholder={"Search for a musician..."}
                            data={this.state.musicians} 
                            />
                            {/* Search a user */}
                            {/*<Search 
                            className="nav-search"
                            placeholder="Search for a musician..." 
                            data={ this.state.musicians } />*/}
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
                                <Nav.Item>
                                  <Nav.Link href="#/messenger">
                                      Messages
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
                                        {UserProfile.getIsAdmin() && <p className="text-center" style={{color:"purple"}}>(Admin)</p>}
                                        <img id="avatar" src={require('../comments/user-icon.png')} alt={'User icon'}
                                        style={{float: 'left', marginRight: 1 + 'em'}}/>
                                        <Card.Title>
                                            {this.state.currentMusicianInfo.Fname + " " + this.state.currentMusicianInfo.Lname}
                                        </Card.Title>
                                        <Card.Subtitle className="mb-2 text-muted">
                                            {this.state.currentMusicianInfo.Instrument}
                                        </Card.Subtitle>
                                    </Card>
                                    <Dropdown.Item href={`#/profile/${UserProfile.getMusicianID()}`}>Profile</Dropdown.Item>
                                    <Dropdown.Item onClick={this.logout}>Logout<LogoutIcon style={{marginLeft: 5 + 'px'}}/></Dropdown.Item>
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