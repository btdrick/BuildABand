import {React, useState} from 'react';
import InputGroup from 'react-bootstrap/InputGroup';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

const CreatePost= ({ canCreatePost, handleSubmit }) => {
    /* Create post modal attributes */
    const [modalTitle, setModalTitle] = useState("");
    const [content, setContent] = useState("");
    const [postID, setPostID] = useState(0);

    /* Handles event of text entry for Post content */
    const changePostContent =(e)=>{
        setContent(e.target.value)
    };

    /* Handles onClick event for Add button */
    const addClick = () => {
        setModalTitle("Create Post");
        setPostID(0);
        setContent("");
    };

    const onClick = (event) => {
        event.preventDefault();
        handleSubmit(content);
        setModalTitle("");
        setContent("");
    }

    return (
        <div>
            {/* Create post button*/}
            {canCreatePost
                && <Button 
                type="button"
                className="btn m-3"
                data-bs-toggle="modal"
                data-bs-target="#exampleModal"
                onClick={addClick}>Create Post</Button>}
            {/* Create post modal */}
            <div className="modal fade" id="exampleModal" tabIndex="-1" aria-hidden="true">
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

                                       
                            {postID===0?
                            <Button type="button"
                            className="btn btn-primary float-start"
                            onClick={ onClick }
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