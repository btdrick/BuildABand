import {React} from "react";
import AudioPlayer from '../audio/AudioPlayer';
import Button from "react-bootstrap/esm/Button";
import LockIcon from '@mui/icons-material/Lock';
import UserProfile from '../UserProfile';
import image from './soundpic.jpg';
import './project.css';

const Project = (props) => {
    const isProjectOwner = props.OwnerID === UserProfile.getMusicianID();

    const renderToggleProjectIsPrivateButton = () => {
        if (props.Private === true) {
            return <Button 
            variant="primary"
            onClick={() => props.ToggleIsPrivate(props.ProjectID)}>
            Make Public</Button>
        }
        if (props.Private === false) {
            return <Button 
            variant="warning"
            onClick={() => props.ToggleIsPrivate(props.ProjectID)}>
            Make Private</Button>
        }
    }

    return(
        <div className="card">
            <img src={image} style={{width: "100%"}} alt="sound pic" />
            <AudioPlayer FileName={props.FileName} AzureFileName={props.AzureFileName} />
            
            <div className="container">
                <h4>{props.Private === true && <LockIcon />}<b>{props.Name}</b></h4>
                {props.Description !== undefined && <p>Project description: {props.Description}</p>}
                
                {isProjectOwner && renderToggleProjectIsPrivateButton()}
                
                <div className="project-box-footer">
                    <div className="participants">
                        <img src={ require("../comments/user-icon.png") } alt="user icon" style={{borderRadius: "50%", border: "1px solid black"}} />
                        <img src={ require("../comments/user-icon.png") } alt="user icon" style={{borderRadius: "50%", border: "1px solid black", marginLeft: "-30px"}} />
                        <button className="add-participant" style={{color: "#4f3ff0", border: "1px solid black"}}>
                            <svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round" className="feather feather-plus">
                                <path d="M12 5v14M5 12h14" />
                            </svg>
                        </button>
                    </div>
                </div>
            </div>
        </div> 
    )
}

export default Project;