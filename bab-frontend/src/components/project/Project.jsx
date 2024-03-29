import { React } from "react";
import AudioPlayer from '../audio/AudioPlayer';
import Button from "react-bootstrap/esm/Button";
import LockIcon from '@mui/icons-material/Lock';
import UserProfile from '../UserProfile';
import image from './soundpic.jpg';
import './project.css';

const Project = (props) => {
    const isProjectOwner = props.OwnerID === UserProfile.getMusicianID();

    /* Render button to toggle if project is public */
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

    /* Render list of project collaborators if they exist */
    const renderCollaborators = () => {
        const collaborators = props.Collaborators;
        if (Object.keys(collaborators).length > 1) {
            return <div className="project-box-footer">
                <h4>Collaborators: </h4>
                <div className="list-group">
                    {collaborators.map((collaborator) => {
                        if (collaborator.musicianID !== UserProfile.getMusicianID()) {
                            return <a key={collaborator.musicianID}
                            href={`#/profile/${collaborator.musicianID}`} 
                            className="list-group-item list-group-item-action">
                            {collaborator.FirstName + " " + collaborator.LastName}</a>
                        }
                        else {
                            return null;
                        }
                    })}
                </div>
            </div>
        }
    }

    return(
        <div className="container-lg card">
            <img src={image} style={{width: "50%", margin: "0 auto"}} alt="sound pic" />
            <div style={{width: "50%", margin: "0 auto"}}>
                <table>
                    <thead>
                        <tr>
                            <th>
                                <h4>{props.Private === true && <LockIcon />}<b>Project Name: </b>{props.Name}</h4>
                                <p>created by {props.OwnerName}</p>
                            </th>
                            <th style={{verticalAlign:"top", paddingLeft:"20px"}}>
                                {isProjectOwner && renderToggleProjectIsPrivateButton()}
                            </th>
                        </tr>
                    </thead>
                </table>
                
                <hr></hr>
                
                {props.Description !== "" && <h3>About: {props.Description}</h3>}

                <hr></hr>

                <AudioPlayer FileName={props.FileName} AzureFileName={props.AzureFileName} />
                <hr></hr>
                { renderCollaborators() }
            </div>
        </div> 
    )
}

export default Project;