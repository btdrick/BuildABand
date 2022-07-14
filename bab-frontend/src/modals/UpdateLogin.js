import {React, useState} from 'react';
import { variables } from '../Variables.js';
import { useNavigate } from 'react-router-dom';
import UserProfile from '../components/UserProfile.js';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

const UpdateLogin = () => {
    /* Update login modal attributes */
    const [modalShow, setModalShow] = useState(false);
    const [input, setInput] = useState([]);
    let navigate = useNavigate();

    /* Handles form input */
    const handleInput = (e) => {
        const fieldName = e.target.name;
        const inputValue = e.target.value;
        var newInput = input;
        newInput[fieldName] = inputValue;
        setInput(newInput);
    }

    /* Validate input */
    function validInput() {
        if (input['New Password'] === input['New Password 2']) {
            return true;
        }
        return false;
    }

    /* Handle submit */
    const handleSubmit = () => {
        if (validInput && window.confirm('Are you sure you want to update your login?')) {
            fetch(variables.API_URL+'accounts/'+UserProfile.getMusicianID(),{
                method:'PATCH',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json'
                },
                body:JSON.stringify({   
                    AccountID:      UserProfile.getMusicianID(),
                    Username:       "",
                    Password:       ""
                })
            })
            .then(res=>res.json())
            .then((result)=>{ 
                alert(result);   
                navigate('/profile/'+UserProfile.getMusicianID());           
            },(_error)=>{
                alert('An error has occurred with updating your information');
            });
        }
    }

    return(
        <div style={{display: 'flex', justifyContent: 'center', marginBottom: '1em'}}>
            <Button
            className=""     
            variant="primary" 
            onClick={() => setModalShow(true)}>
            Update Login</Button>
            {/* Update login modal */}
            <Modal
            size="lg"
            show={modalShow}
            onHide={() => setModalShow(false)}
            aria-labelledby="update-login-modal"
            centered>
                <Modal.Header closeButton>
                    <Modal.Title id="update-login-modal">Update Login</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3">
                            <Form.Label>New username</Form.Label>
                            <Form.Control type="text" name="New Username" onChange={(e) => handleInput(e)}/>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>New password</Form.Label>
                            <Form.Control type="password" name="New Password" onChange={(e) => handleInput(e)}/>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Re-enter new password</Form.Label>
                            <Form.Control type="password" name="New Password 2" onChange={(e) => handleInput(e)}/>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Old password</Form.Label>
                            <Form.Control type="password" name="Old Password" onChange={(e) => handleInput(e)}/>
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button 
                    type="button" 
                    className="btn btn-danger" 
                    onClick={() => setModalShow(false)}>
                    Close</Button>
                    <Button 
                    type="button"
                    className="btn btn-primary"
                    onClick={ handleSubmit }>
                    Submit</Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}

export default UpdateLogin;