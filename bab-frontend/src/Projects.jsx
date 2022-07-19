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

    const canSeeAllProjects = true;

    /* Makes api call to backend to get the user's projects */
    const getUserProjects = useCallback(async () => {
        const response = await fetch(variables.API_URL+'project/'+id);
        const data = await response.json();
        if (data !== undefined) {
            return data.reverse();
        }
    }, [id]);

    /* Hook is called after each refresh */
    useEffect(() => {
        setLoading(true);
        getUserProjects().then((data) => {
            setProjects(data);
            console.log("New")
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
            console.log(_error);
            alert("Unable to start project");
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
                            name={project.Name}
                            FileName={project.FileName}
                            AzureFileName={project.AzureFileName} /> 
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