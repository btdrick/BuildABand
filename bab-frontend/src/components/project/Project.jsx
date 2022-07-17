import './project.css';
import image from './soundpic.jpg'
import AudioPlayer from '../audio/AudioPlayer';

const Project = (props) => {
    const hasAudio = (props.FileName === "" || props.FileName === "undefined" || props.FileName == null) ? false : true;

    return(
        <>
            <div className="card">
                <img src={image} style={{width: "100%"}} alt="sound pic" />
                {hasAudio ? 
                <AudioPlayer FileName={props.FileName} AzureFileName={props.AzureFileName} />
                :
                <></>
                }
                <div className="container">
                    <h4><b>{props.name}</b></h4>
                    <p>Project description</p>

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
        </>
    )
}

export default Project;