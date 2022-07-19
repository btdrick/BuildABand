import {React, useState, useRef, useEffect} from 'react';
import { variables } from '../Variables';
import Modal from 'react-bootstrap/Modal';
import InputGroup from 'react-bootstrap/InputGroup';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Alert from 'react-bootstrap/Alert';
import UserProfile from '../components/UserProfile';

const CreatePost= ({ canCreatePost, handleSubmit }) => {
    /* Create post modal attributes */
    const [modalShow, setModalShow] = useState(false);
    const [postContent, setPostContent] = useState("");
    const [audioID, setAudioID] = useState(0);
    const [postID, setPostID] = useState(0);
    const [resetFile, setResetFile] = useState('');
    const [error, setError] = useState(undefined);
    const file = useRef();

    /* This hook is called with each render */
    useEffect(() => { 
        if(audioID !== 0 || audioID === null) {
            audioID === null ? handleSubmit(postContent, 0) : handleSubmit(postContent, audioID);
            setPostContent("");
            var randomString = Math.random().toString(36)
            setResetFile(randomString);
            setModalShow(false);
        }
        setAudioID(0);
    }, [postContent, audioID, handleSubmit])

    /* Resets modal upon close */
    function closeModal() {
        setModalShow(false);
        setPostContent("");
        setError(undefined);
    }

    /* Handles event of text entry for Post content */
    const changePostContent =(e)=>{
        if (error) {
            setError(undefined);
        }
        setPostContent(e.target.value)
    };

    /* Handles onClick event for Add button */
    const addClick = () => {
        setModalShow(true);
        setPostID(0);
        setPostContent("");
        var randomString = Math.random().toString(36)
        setResetFile(randomString);
    };

    /* Handles onClick event for modal */
    const onClick = (event) => {
        event.preventDefault();
        if(postContent !== "") {
            submitFileInfo();
        } else {
            setError('Post content cannot be blank')
        }
    }

    /* Submits audio file information */
    const submitFileInfo = async () => {
        const submittedFile = file.current.files[0] !== undefined ? file.current.files[0] : null;
        if(submittedFile === null) {
            setAudioID(null);
            return;
        }
        if(submittedFile.name.length > 45) {
            setError("File name must be under 45 characters")
            return;
        }
        const fileNameRegex = /^[A-Za-z0-9\-_.]+$/g;
        if(!submittedFile.name.match(fileNameRegex)) {
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

    return (
        <div style={{display: 'flex', justifyContent: 'center', marginBottom: '1em'}}>
            {/* Create post button*/}
            {canCreatePost
                && <Button 
                variant="primary"
                onClick={() => addClick()}>
                Create Post</Button>}
            {/* Create post modal */}
            <Modal
            size="lg"
            show={modalShow}
            onHide={() => closeModal()}
            aria-labelledby="create-post-modal"
            centered>
                <Modal.Header closeButton>
                    <Modal.Title id="create-post-modal">Create Post</Modal.Title>
                </Modal.Header>
                {/* Validation Errors */}
                {error && 
                <Alert variant="danger">
                    <Alert.Heading>An error has occurred</Alert.Heading>
                    <p>{error}</p>
                </Alert>}
                <Modal.Body>
                    {/* Modal input */}
                    <InputGroup className="mb-3">
                    <InputGroup.Text 
                    id="input-post-content">
                    Content</InputGroup.Text>
                    <Form.Control 
                    type="text"
                    aria-describedby="inputPostContent"
                    aria-label="Enter post content..."
                    placeholder="Enter post content..."
                    value={ postContent } 
                    onChange={ changePostContent }/>
                    </InputGroup>
                    <Form.Group controlId="formFile" className="mb-3">
                        <Form.Label>(Optional) Add a Music Sample (.wav or .mp3)</Form.Label>
                        <Form.Control 
                        type="file" 
                        accept='.wav, .mp3' 
                        ref={file}
                        key={resetFile || ''} />
                    </Form.Group>
                </Modal.Body>
                <Modal.Footer>
                    <Button 
                    type="button" 
                    className="btn btn-danger" 
                    onClick={() => closeModal()}>
                    Close</Button>
                    {postID===0?
                    <Button type="button"
                    variant="primary"
                    onClick={ (e) => onClick(e) }
                    >Create</Button>
                    :null}
                </Modal.Footer>
            </Modal>
        </div>
    );
}

export default CreatePost;