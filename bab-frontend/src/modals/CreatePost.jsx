import {React, useState, useRef, useEffect} from 'react';
import InputGroup from 'react-bootstrap/InputGroup';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import UserProfile from '../components/UserProfile';
import { variables } from '../Variables';

const CreatePost= ({ canCreatePost, handleSubmit }) => {
    /* Create post modal attributes */
    const [modalTitle, setModalTitle] = useState("");
    const [content, setContent] = useState("");
    const [audioID, setAudioID] = useState(0);
    const [postID, setPostID] = useState(0);
    const [fileInfo, setFileInfo] = useState({});
    const [resetFile, setResetFile] = useState('')
    const file = useRef();

    /* Only submits post after audio file is uploaded */
    useEffect(() => { 
        if(audioID !== 0) {
            handleSubmit(content, audioID);
            setModalTitle("");
            setContent("");
            setFileInfo({});
            var randomString = Math.random().toString(36)
            setResetFile(randomString);
        }
        setAudioID(0);
    }, [content, audioID, handleSubmit])

    /* Handles event of text entry for Post content */
    const changePostContent =(e)=>{
        setContent(e.target.value)
    };

    const changeFile = (e) => {
        setFileInfo(e.target.files[0]);
    }

    /* Handles onClick event for Add button */
    const addClick = () => {
        setModalTitle("Create Post");
        setPostID(0);
        setContent("");
        setFileInfo({});
        var randomString = Math.random().toString(36)
        setResetFile(randomString);
    };

    const onClick = (event) => {
        event.preventDefault();
        if(content !== "") {
            submitFileInfo();
        } else {
            alert('Post content cannot be blank');
        }
    }

    const submitFileInfo = async () => {
        const response = await fetch(variables.API_URL+'audio?filename=' + fileInfo.name + '&musicianID=' + UserProfile.getMusicianID(),{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/octet-stream'
            },
            body: fileInfo
        })
        if (!response.ok) {  
            alert("Invalid file upload")
            return;
        } 
        const result = await response.json();
        setAudioID(result)
    }

    return (
        <div>
            {/* Create post button*/}
            {canCreatePost
                && <Button 
                type="button"
                className="btn m-3"
                data-bs-toggle="modal"
                data-bs-target="#create-post-modal"
                onClick={addClick}>Create Post</Button>}
            {/* Create post modal */}
            <div className="modal fade" id="create-post-modal" tabIndex="-1" aria-hidden="true">
                <div className="modal-dialog modal-lg modal-dialog-centered">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">{ modalTitle }</h5>
                            <Button 
                            type="button" 
                            className="btn-close" 
                            data-bs-dismiss="modal" 
                            aria-label="Close"/>
                        </div> 
                        <div className="modal-body">
                            {/* Modal input */}
                            <InputGroup className="mb-3">
                                <InputGroup.Text 
                                id="inputPostContent">
                                Content</InputGroup.Text>
                                <Form.Control 
                                type="text"
                                aria-describedby="inputPostContent"
                                aria-label="Enter post content..."
                                placeholder="Enter post content..."
                                value={ content } 
                                onChange={ changePostContent }/>
                            </InputGroup>
                            <Form.Group controlId="formFile" className="mb-3">
                                <Form.Label>(Optional) Add a Music Sample (.wav or .mp3)</Form.Label>
                                <Form.Control 
                                    type="file" 
                                    accept='.wav, .mp3' 
                                    ref={file}
                                    key={resetFile || ''}
                                    onChange={ changeFile } />
                            </Form.Group>                                  
                            {postID===0?
                            <Button type="button"
                            className="btn btn-primary float-start"
                            onClick={ onClick }
                            data-bs-dismiss="modal" 
                            >Create</Button>
                            :null}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default CreatePost;