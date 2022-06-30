import {React, useState} from 'react';
import {variables} from '../Variables.js';
import UserProfile from '../components/UserProfile.js';
import InputGroup from 'react-bootstrap/InputGroup';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

const CreatePost= ({ canCreatePost, getPosts, setBackendPosts }) => {
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

    /* Handles onClick event for Create button */
    const createClick = () => {
        fetch(variables.API_URL+'post',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                CreatedTime: new Date(),                           
                MusicianID: UserProfile.getMusicianID(),
                Content:    content
            })
        })
        .then(res=>res.json())
        .then((result)=>{
            alert(result);
            getPosts().then((data) => {
                setBackendPosts(data);
            });
        },(_error)=>{
            alert('Post content cannot be blank');
        });
    };

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
                            onClick={ createClick }
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