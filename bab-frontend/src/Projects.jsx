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
    /* Project owner's projects */
    const [projects, setProjects] = useState([]);
    /* Current user's collaborations */
    const [collaborativeProjectIDs, setCollaborativeProjectIDs] = useState([]);
    /* Projects are loading? */
    const [loading, setLoading] = useState(true);

    /* Makes api call to backend to get the user's projects */
    const getProjectOwnersProjects = useCallback(async () => {
        const response = await fetch(variables.API_URL+'project/'+id);
        const data = await response.json();
        return data.reverse();
    }, [id]);

    /* Makes api call to backend to get projectID for user's project collaborations */
    const getUserProjectCollaborationsProjectIDs = useCallback(async () => {
        const response = await fetch(variables.API_URL+'collaboration/collaborator/'+UserProfile.getMusicianID());
        const data = await response.json();
        const projectIDs = data.map(function(row) { return row.ProjectID; });
        return projectIDs;
    }, []);

    /* Hook is called after each refresh */
    useEffect(() => {
        setLoading(true);
        getProjectOwnersProjects().then((data) => {
            setProjects(data);
        });
        getUserProjectCollaborationsProjectIDs().then((data) => {
            setCollaborativeProjectIDs(data);
        });
        setLoading(false);
    }, [getProjectOwnersProjects, getUserProjectCollaborationsProjectIDs]);

    /* Renders visible projects */
    const renderProjects = () => {
        return projects.map(project => {
            console.log(collaborativeProjectIDs)
            if (collaborativeProjectIDs.some((projectID) => projectID === project.ProjectID)) {
                return <div className='row' key={project.ProjectID}>
                    <Project 
                    ProjectID={project.ProjectID}
                    OwnerID={project.OwnerID}
                    Name={project.Name}
                    Description={project.Description}
                    FileName={project.FileName}
                    AzureFileName={project.AzureFileName}
                    Private={project.IsPrivate === 1} 
                    ToggleIsPrivate={toggleProjectIsPrivate} />
                </div>
            }
            else {
                return <></>
            }});
    }

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
            getProjectOwnersProjects().then((data) => {
                setProjects(data);
            });
        },(_error)=>{
            alert("Unable to start project");
        });
    }

    /* Toggles a project as either private or public */
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
        .then((result) => { 
            /* Update projects with toggled project */
            const updatedProjects = projects.map(project => {
                var toggle;
                if(project.ProjectID === projectID) {
                    project.IsPrivate === 1 ? toggle = 0 : toggle = 1;
                    return {...project, IsPrivate: toggle}
                }
                return project;
            });
            setProjects(updatedProjects);
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
                    <>{renderProjects()}</>
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