import React, {useState} from 'react';
import Form from 'react-bootstrap/Button';
function UploadAudio() {

	return(
        <div>    
                 <Form.Group controlId="formFile" className="mb-3">
                    <Form.Label>Default file input example</Form.Label>
                    <Form.Control type="file" />
                </Form.Group>
         </div>
    )
}

export default UploadAudio;