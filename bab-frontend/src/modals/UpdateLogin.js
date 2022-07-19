import {React, useState} from 'react';
import { variables } from '../Variables.js';
import { useNavigate } from 'react-router-dom';
import UserProfile from '../components/UserProfile.js';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Alert from 'react-bootstrap/Alert';

const UpdateLogin = () => {
    /* Update login modal attributes */
    const [modalShow, setModalShow] = useState(false);
    const [input, setInput] = useState([]);
    const [error, setError] = useState(undefined);
    let navigate = useNavigate();

    /* Resets modal upon close */
    function closeModal() {
        setModalShow(false);
        setInput([]);
        setError(undefined);
    }

    /* Handles form input */
    const handleInput = (e) => {
        if (error) {
            setError(undefined);
        }
        const fieldName = e.target.name;
        const inputValue = e.target.value;
        var newInput = input;
        newInput[fieldName] = inputValue;
        setInput(newInput);
    }

    /* Validate input */
    const validInput = async() => {
        const noNewUsername = !input['newUsername'] || input['newUsername'] === '';
        const noNewPassword = (!input['newPassword'] || input['newPassword'] === '')
            && (!input['confirmedNewPassword'] || input['confirmedNewPassword'] === '')
        if (noNewUsername && noNewPassword) {
            setError('Please enter a new username or password');
            return;
        }
        /* Validate login credentials */
        const response = await fetch(variables.API_URL+'accounts/login?username='+ input['username'] + '&password=' + input['password']);
        if (!response.ok) {  
            setError('Current username or password is incorrect');
            return;
        } 
        /* Validate new username, password format */
        const canUpdateLogin = (validNewUsername() && validNewPassword()) || 
        (validNewUsername() && noNewPassword) || (validNewPassword() && noNewUsername)
        if (canUpdateLogin) {
            setError('');
            return window.confirm('Are you sure you want to update your login?');  
        }
    }

    /* New username validation */
    function validNewUsername() {
        const usernameRegex = /^\S*$/;
        if (input['username'] === input['newUsername']) {
            setError('New username cannot be same as current username');
            return false;
        }
        if (input['newUsername'].length <= 6 || !usernameRegex.test(input["username"])) {
            setError('Please enter a valid username with 7 or more characters');
            return false;
        }

        return true;
    }

    /* New password validation */
    function validNewPassword() {
        if (input['password'] === input['newPassword']) {
            setError('New password cannot be same as current password');
            return false;
        }
        if (input['newPassword'] !== input['confirmedNewPassword']) {
            setError('New passwords do not match');
            return false;
        }
        if (input['newPassword'].length <= 5) {
            setError('Password must be at least 6 characters');
            return false;
        }   

        return true;
    }

    /* Handle submit */
    const handleSubmit = () => {
        const musicianID = UserProfile.getMusicianID();
        if (validInput() && !error) {
            fetch(variables.API_URL+'accounts/'+musicianID,{
                method:'PATCH',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json'
                },
                body:JSON.stringify({   
                    AccountID:      musicianID,
                    Username:       input['newUsername'],
                    Password:       input['newPassword']
                })
            })
            .then(res=>res.json())
            .then((result)=>{ 
                alert(result);   
                navigate('/profile/'+musicianID);           
            },(_error)=>{
                setError('An error has occurred with updating your information');
            });
        }
    }

    return(
        <div style={{display: 'flex', justifyContent: 'center', marginBottom: '1em'}}>
            <Button   
            variant="primary" 
            onClick={() => setModalShow(true)}>
            Update Login</Button>
            {/* Update login modal */}
            <Modal
            size="lg"
            show={modalShow}
            onHide={() => closeModal()}
            aria-labelledby="update-login-modal"
            centered>
                <Modal.Header closeButton>
                    <Modal.Title id="update-login-modal">Update Login</Modal.Title>
                </Modal.Header>
                {/* Validation Errors */}
                {error && 
                <Alert variant="danger">
                    <Alert.Heading>An error has occurred</Alert.Heading>
                    <p>{error}</p>
                </Alert>}
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3">
                            <Form.Label>New username</Form.Label>
                            <Form.Control type="text" name="newUsername" onChange={(e) => handleInput(e)}/>
                            <Form.Text id="newUsername" muted>
                                Your username must be at least 7 characters
                            </Form.Text>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>New password</Form.Label>
                            <Form.Control type="password" name="newPassword" onChange={(e) => handleInput(e)}/>
                            <Form.Text id="newUsername" muted>
                                Your password must be at least 6 characters
                            </Form.Text>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Re-enter new password</Form.Label>
                            <Form.Control type="password" name="confirmedNewPassword" onChange={(e) => handleInput(e)}/>
                        </Form.Group>
                        <hr></hr>
                        <div className="row">
                            <div className="col-sm-6">
                            <Form.Group className="mb-3">
                                <Form.Label>Current username</Form.Label>
                                <Form.Control type="text" name="username" onChange={(e) => handleInput(e)}/>
                                <Form.Text id="newUsername" muted>
                                Log-in credentials are required for submission
                                </Form.Text>
                            </Form.Group>
                            </div>
                            <div className="col-sm-6">
                            <Form.Group className="mb-3">
                                <Form.Label>Current password</Form.Label>
                                <Form.Control type="password" name="password" onChange={(e) => handleInput(e)}/>
                            </Form.Group>
                            </div>
                        </div>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <p className="text-muted">You may either update username, password, or both</p>
                    <Button 
                    type="button" 
                    className="btn btn-danger" 
                    onClick={() => closeModal()}>
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