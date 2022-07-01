import {React, useState} from 'react';
import Button from 'react-bootstrap/Button';

const UpdateLogin = () => {
    /* Update login modal attributes */
    const [modalTitle, setModalTitle] = useState("");
    const [content, setContent] = useState("");

    /* Is user validated to update account login? */
    const [validated, setValidated] = useState(false);
    /* Only validated users can update their login */
    const canUpdateLogin = validated === true;

    /* Handles event for update click */
    const updateClick = () => {
        setValidated(true);
        setModalTitle("Update Login");
        setContent("");
    }

    return(
        <div>
            <Button 
                type="button"
                className="btn m-3"
                data-bs-toggle="modal"
                data-bs-target="#updateLoginModal"
                onClick={updateClick}>Update Login</Button>
        </div>
    );
}

export default UpdateLogin;