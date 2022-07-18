import { React, useState, useRef, useEffect, useCallback } from 'react';
import Multiselect from 'multiselect-react-dropdown';
import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';
import Button from 'react-bootstrap/Button';
import UserProfile from '../components/UserProfile';
import { variables } from '../Variables';

const CreateProject = (props) => {
    const [collaborators, setCollaborators] = useState([]);
    const [projectName, setProjectName] = useState("");
    const [projectDescription, setProjectDescription] = useState("");
    const [fileInfo, setFileInfo] = useState({});
    const [resetFile, setResetFile] = useState('');
    const [audioID, setAudioID] = useState(0);
    const file = useRef();
    const [connectedConnections, setConnectedConnection] = useState([]);
    const multiselectRef = useRef();
    const closeRef = useRef();

    //Get all connections for the musician
    const getConnections = useCallback(async () => {
        const response = await fetch(variables.API_URL + "musicianconnections/" + UserProfile.getMusicianID())
        const data = await response.json();
        return data;
    }, []);

    /* Sets connections */
    useEffect(() => {
        getConnections().then((data) => {
            var newData = [];
            data.map(x => x.FollowerID === UserProfile.getMusicianID() ? newData.push({id: x.InitiatorID, name: x.InitiatorNames}) : newData.push({id: x.FollowerID, name: x.FollowerNames}))
            setConnectedConnection(newData);
        });
        submitProject();
        multiselectRef.current.resetSelectedValues();
        closeRef.current.click();
        props.refreshEvent();
        
    }, [getConnections, audioID]);

    /* Handles onClick event for creating a new project button */
    const createProjectButton = () => {
        setProjectName("");
        setProjectDescription("");
        setCollaborators([]);
        var randomString = Math.random().toString(36)
        setResetFile(randomString);
    };

    /* Handles change for project name */
    const changeProjectName =(e)=>{
        setProjectName(e.target.value)
    };

    /* Handles change for project description */
    const changeProjectDescription =(e)=>{
        setProjectDescription(e.target.value)
    };

    /* Handles change for file entered */
    const changeFile = (e) => {
        setFileInfo(e.target.files[0]);
    }

    /* Button event after form is submitted */
    const onClick = (event) => {
        event.preventDefault();
        if(projectName === "") {
            alert('Must include project name');
        } else {
            submitFileInfo();
        }
    }

    const submitFileInfo = async () => {
        var submittedFile = file.current.files[0] !== undefined ? file.current.files[0] : null;
        if(submittedFile === null) {
            setAudioID(null);
            return;
        }
        if(submittedFile.name.length > 45) {
            alert("File name must be under 45 characters")
            return;
        }
        var regex = /^[A-Za-z0-9\-_.]+$/g;
        if(!submittedFile.name.match(regex)) {
            alert("File name can only contain letters, numbers, periods, hyphens, and underscores")
            return;
        }
        const response = await fetch(variables.API_URL+'audio?filename=' + submittedFile.name + '&musicianID=' + UserProfile.getMusicianID(),{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/octet-stream'
            },
            body: submittedFile
        })
        if (!response.ok) {  
            alert("Invalid file upload")
            return;
        } 
        const result = await response.json();
        setAudioID(result)
    }

    const submitProject = async () => {
        const response = await fetch(variables.API_URL+'project', {
            method: 'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                Name: projectName,                           
                OwnerID: UserProfile.getMusicianID(),
                MusicID: audioID
            })
        });
        if (!response.ok) {  
            alert("Unable to start project");
            return;
        }
        //TODO add collaborators to endpoint
    }

    const onSelectCollaborator = (selectedList, selectedItem) => {
        setCollaborators(multiselectRef.current.getSelectedItems());
    }

    const onRemoveCollaborator = (selectedList, selectedItem) => {
        multiselectRef.current.resetSelectedValues();
    }

    return (
        <>
            <Button 
                type="button"
                className="btn m-3"
                data-bs-toggle="modal"
                data-bs-target="#exampleModal"
                onClick={createProjectButton}>Create a Project
            </Button>
        <div className="modal fade" id="exampleModal" tabIndex="-1" aria-hidden="true">
                <div className="modal-dialog modal-lg modal-dialog-centered">
                    <div className="modal-content">

                    <div className="modal-header">
                            <h5 className="modal-title">Create a Project</h5>
                            <Button 
                            type="button" 
                            className="btn-close" 
                            data-bs-dismiss="modal" 
                            aria-label="Close"
                            ref={closeRef}/>
                        </div>

                        <div className="modal-body">
                            <InputGroup className="mb-3">
                                <InputGroup.Text 
                                id="inputProjectName">
                                Project Name</InputGroup.Text>
                                <Form.Control 
                                type="text"
                                aria-describedby="inputProjectName"
                                aria-label="Enter project name..."
                                placeholder="Enter project name..."
                                value={ projectName } 
                                onChange={ changeProjectName }/>
                            </InputGroup>

                            <InputGroup className="mb-3">
                                <InputGroup.Text 
                                id="inputProjectDescription">
                                Description</InputGroup.Text>
                                <Form.Control 
                                type="text"
                                aria-describedby="inputProjectDescription"
                                aria-label="Enter project description..."
                                placeholder="Enter project description..."
                                value={ projectDescription } 
                                onChange={ changeProjectDescription }/>
                            </InputGroup>

                            <Multiselect
                                displayValue="name"
                                onKeyPressFn={function noRefCheck(){}}
                                onRemove={onRemoveCollaborator}
                                onSearch={function noRefCheck(){}}
                                onSelect={onSelectCollaborator}
                                options={connectedConnections}
                                ref={multiselectRef}
                                showCheckbox
                            />

                            <Form.Group controlId="formFile" className="mb-3">
                                <Form.Label>(Optional) Add a Music Sample (.wav or .mp3)</Form.Label>
                                <Form.Control 
                                    type="file" 
                                    accept='.wav, .mp3' 
                                    ref={file}
                                    key={resetFile || ''}
                                    onChange={ changeFile } />
                            </Form.Group>   

                            <Button type="button"
                            className="btn btn-primary float-start"
                            onClick={ onClick }
                            >Create</Button>
                        </div>
                    </div>
                </div>
        </div>
        </>
    )
}

export default CreateProject;