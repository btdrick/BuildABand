import {React} from "react";
import { useParams } from "react-router-dom";
import Navbar from './components/header/Navbar';
import UpdateProfileForm from "./components/updateProfile/UpdateProfileForm";
import UpdateLogin from "./modals/UpdateLogin";
import './style/home.css';

function AccountSettings() {
    const { id } = useParams();

    return(
        <div id="container">
            <Navbar/>
            <h3 className="title"> Account Settings </h3>
            <div className="container-lg">
                <UpdateLogin />
                <UpdateProfileForm musicianID={id} />
            </div>
        </div>
    );
    
}

export default AccountSettings;