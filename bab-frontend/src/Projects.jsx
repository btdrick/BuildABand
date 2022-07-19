import { React, useState, useEffect, useCallback } from 'react';
import { useParams } from 'react-router-dom';
import { variables } from './Variables.js';
import Navbar from './components/header/Navbar';
import Project from './components/project/Project';
import CreateProject from './modals/CreateProject.jsx';
import UserProfile from './components/UserProfile';
import './style/home.css';

function Projects() {
    const { id } = useParams();
    /* Users projects */
    const [projects, setProjects] = useState([]);
    /* Projects are loading? */
    const [loading, setLoading] = useState(true);

    /* Makes api call to backend to get the user's projects */
    const getUserProjects = useCallback(async () => {
        const response = await fetch(variables.API_URL+'project/'+id);
        const data = await response.json();
        if (data !== undefined) {
            console.log(data)
            return data.reverse();
        }
    }, [id]);

    /* Hook is called after each refresh */
    useEffect(() => {
        setLoading(true);
        getUserProjects().then((data) => {
            setProjects(data);
        });
        setLoading(false);
    }, [getUserProjects]);

    /* Creates a new project */
    const createProject = async (projectName, audioID) => {
        fetch(variables.API_URL+'project', {
            method: 'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                Name:    projectName,                           
                OwnerID: UserProfile.getMusicianID(),
                AudioID: audioID
            })
        })
        .then(res=>res.json())
        .then((result) => {
            getUserProjects().then((data) => {
                setProjects(data);
            });
        },(_error)=>{
            alert("Unable to start project");
        });
    }

    const toggleProjectIsPrivate = async (projectID) => {
        fetch(variables.API_URL+'project/'+projectID+"/private",{
            method:'PATCH',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                ProjectID:  projectID
            })
        })
        .then(res=>res.json())
        .then((result)=>{ 
            /* Refresh projects */
            getUserProjects().then((data) => {
                setProjects(data);
            });               
        },(_error)=>{
            alert('An error has occurred with toggling your project');
        });
    }

    if (!loading) {
        return(
            <div id="container">
                <Navbar/>
                <h3 className="title"> Projects </h3>
                <div className="container-lg">
                    <CreateProject handleSubmit={ createProject }/>
                    {projects.length > 0 ? (
                    <>
                        {projects.map(project => <div className='row' key={project.ProjectID}>
                            <Project 
                            ProjectID={project.ProjectID}
                            OwnerID={project.OwnerID}
                            Name={project.Name}
                            Description={project.Description}
                            FileName={project.FileName}
                            AzureFileName={project.AzureFileName}
                            Private={project.IsPrivate === 1} 
                            ToggleIsPrivate={toggleProjectIsPrivate} /> 
                        </div>)}
                    </>
                    ) : ( 
                        <h6 className="text-center text-muted">No projects found</h6>
                    )}
                </div>
            </div>
        )
    }
    if (loading) {
        return (<div>loading projects...</div>);
    }
    
}

export default Projects;