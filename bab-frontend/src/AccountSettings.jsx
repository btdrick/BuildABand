import React, {Component} from "react";
import Navbar from './components/header/Navbar';
import UpdateLogin from "./modals/UpdateLogin";
import './style/home.css';

class AccountSettings extends Component {
    render() {
        return(
            <div id="container">
                <Navbar/>
                <h3 className="title"> Account Settings </h3>
                <div className="container-lg">
                    <UpdateLogin />
                </div>
            </div>
        );
    }
}

export default AccountSettings;