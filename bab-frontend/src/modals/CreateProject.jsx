import { React, useState, useRef, useEffect, useCallback } from 'react';
import { variables } from '../Variables';
import Modal from 'react-bootstrap/Modal';
import Alert from 'react-bootstrap/Alert';
import Multiselect from 'multiselect-react-dropdown';
import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';
import Button from 'react-bootstrap/Button';
import UserProfile from '../components/UserProfile';

const CreateProject = ( { handleSubmit }) => {
    const [modalShow, setModalShow] = useState(false);
    const [connectedConnections, setConnectedConnection] = useState([]);
    const [collaborators, setCollaborators] = useState([]);
    const [projectName, setProjectName] = useState("");
    const [projectDescription, setProjectDescription] = useState("");
    const [resetFile, setResetFile] = useState("");
    const [error, setError] = useState("");
    const [audioID, setAudioID] = useState(0);
    
    const file = useRef();
    const multiselectRef = useRef();

    //Get all connections for the musician
    const getConnections = useCallback(async () => {
        const response = await fetch(variables.API_URL + "musicianconnections/" + UserProfile.getMusicianID())
        const data = await response.json();
        return data;
    }, []);

    /* Hook is called after each refresh */
    useEffect(() => {
        getConnections().then((data) => {
            var newData = [];
            data.map(x => x.FollowerID === UserProfile.getMusicianID() ? newData.push({id: x.InitiatorID, name: x.InitiatorNames}) : newData.push({id: x.FollowerID, name: x.FollowerNames}))
            setConnectedConnection(newData);
        });
        if (audioID !== 0) {
            handleSubmit(projectName, audioID);
            setProjectName("");
            setProjectDescription("");
            var randomString = Math.random().toString(36)
            setResetFile(randomString);
            setAudioID(0);
            setModalShow(false);
        }
        console.log("yes")
    }, [getConnections, audioID, projectName, handleSubmit]);

    /* Resets modal upon close */
    function closeModal() {
        setProjectName("");
        setProjectDescription("");
        multiselectRef.current.resetSelectedValues();
        setCollaborators([]);
        var randomString = Math.random().toString(36)
        setResetFile(randomString);
        setError(undefined);
        setModalShow(false);
    }

    /* Handles onClick event for creating a new project button */
    const createProjectButton = () => {
        setProjectName("");
        setProjectDescription("");
        setCollaborators([]);
        var randomString = Math.random().toString(36)
        setResetFile(randomString);
        setModalShow(true);
    };

    /* Handles change for project name */
    const handleInput = (e) => {
        if (error) {
            setError(undefined);
        }
        if (e.target.name === "input-project-name") {
            setProjectName(e.target.value)
        }
        if (e.target.name === "input-project-description") {
            setProjectDescription(e.target.value)
        }
    };

    /* Button event after form is submitted */
    const onClick = (event) => {
        event.preventDefault();
        if(projectName === "") {
            setError('Must include project name');
        } else {
            submitFileInfo(); 
        }
    }

    /* Submits file for project */
    const submitFileInfo = async () => {
        var submittedFile = file.current.files[0] !== undefined ? file.current.files[0] : null;
        if(submittedFile === null) {
            setError("File required for project creation")
            return;
        }
        if(submittedFile.name.length > 45) {
            setError("File name must be under 45 characters")
            return;
        }
        var regex = /^[A-Za-z0-9\-_.]+$/g;
        if(!submittedFile.name.match(regex)) {
            setError("File name can only contain letters, numbers, periods, hyphens, and underscores")
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
            setError("Invalid file upload")
            return;
        } 
        const result = await response.json();
        setAudioID(result)
    }

    const onSelectCollaborator = (selectedList, selectedItem) => {
        setCollaborators(multiselectRef.current.getSelectedItems());
    }

    const onRemoveCollaborator = (selectedList, selectedItem) => {
        multiselectRef.current.resetSelectedValues();
    }

    return (
        <div style={{display: 'flex', justifyContent: 'center', marginBottom: '1em'}}>
            {/* Create project button */}
            <Button 
            variant="primary"
            onClick={() => createProjectButton()}>
            Create a Project</Button>
            {/* Create project modal */}
            <Modal
            size="lg"
            show={modalShow}
            onHide={() => closeModal()}
            aria-labelledby="create-project-modal"
            centered>
                <Modal.Header closeButton>
                    <Modal.Title id="create-project-modal">Create Project</Modal.Title>
                </Modal.Header>
                {/* Validation Errors */}
                {error && 
                <Alert variant="danger">
                    <Alert.Heading>An error has occurred</Alert.Heading>
                    <p>{error}</p>
                </Alert>}
                {/* Modal input */}
                <Modal.Body>
                    {/* Project Name */}
                    <InputGroup className="mb-3">
                        <InputGroup.Text 
                        id="input-project-name">
                        Project Name</InputGroup.Text>
                        <Form.Control 
                        type="text"
                        name="input-project-name"
                        aria-describedby="input-project-name"
                        aria-label="Enter project name..."
                        placeholder="Enter project name..."
                        value={ projectName } 
                        onChange={ (e) => handleInput(e) }/>
                    </InputGroup>
                    {/* Project Description */}
                    <InputGroup className="mb-3">
                        <InputGroup.Text 
                        id="input-project-description">
                        Description</InputGroup.Text>
                        <Form.Control 
                        type="text"
                        name="input-project-description"
                        aria-describedby="input-project-description"
                        aria-label="Enter project description..."
                        placeholder="Enter project description..."
                        value={ projectDescription } 
                        onChange={ (e) => handleInput(e) }/>
                    </InputGroup>
                    {/* Project Collaborators */}
                    <Form.Group className="mb-3">
                        <Multiselect
                            displayValue="name"
                            onKeyPressFn={ function noRefCheck(){} }
                            onRemove={ onRemoveCollaborator }
                            onSearch={ function noRefCheck(){} }
                            onSelect={ onSelectCollaborator }
                            options={ connectedConnections }
                            ref={ multiselectRef }
                            showCheckbox />
                        <Form.Text muted>
                        (Optional) Select collaborators
                        </Form.Text>
                    </Form.Group>
                    {/* Project File */}
                    <Form.Group controlId="formFile" className="mb-3">
                        <Form.Control 
                            type="file" 
                            accept='.wav, .mp3' 
                            ref={file}
                            key={resetFile || ''} />
                        <Form.Text muted>
                        (Required) Add a Music Sample (.wav or .mp3)
                        </Form.Text>
                    </Form.Group>  
                </Modal.Body>
                <Modal.Footer>
                    <p className="text-muted">Project name and file are required</p>
                    <Button 
                    variant="danger"
                    onClick={() => closeModal()}>
                    Close</Button>
                    <Button type="button"
                    variant="primary"
                    onClick={(e) => onClick(e) }
                    >Create</Button>
                </Modal.Footer>
            </Modal>    
        </div>
    );
}

export default CreateProject;