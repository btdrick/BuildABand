import {React, useState, useEffect} from 'react';
import { variables } from '../../Variables.js';
import Form from 'react-bootstrap/Form'

const UpdateProfileForm = ({musicianID}) => {
    const [musician, setMusician] = useState([]);

    const [dateOfBirth, setDateOfBirth] = useState('');
    const [email, setEmail] = useState('');
    const [address1, setAddress1] = useState('');
    const [address2, setAddress2] = useState('');
    const [city, setCity] = useState('');
    const [stateCode, setStateCode] = useState('');
    const [zipCode, setZipCode] = useState('');
    const [instrument, setInstrument] = useState('');
    
    /* Once the page renders, this hook takes place */
    useEffect(() => {
        /* Retrieve information related to author of post */
        const getMusician = async() => {
            const response = await fetch(variables.API_URL+'musician/'+ musicianID);
            const data = await response.json(); 
            var author = data[0];

            return author;
        };
        getMusician().then((data) => {
            setMusician(data);
            setDateOfBirth(new Date(data.DateOfBirth).toLocaleDateString());
            setEmail(data.Email);
            setAddress1(data.Address1);
            setAddress2(data.Address2);
            setCity(data.City);
            setStateCode(data.StateCode);
            setZipCode(data.ZipCode);
            setInstrument(data.Instrument);
        });
    }, [musicianID]);

    return (
        <Form>
            <Form.Group className="mb-3">
            <div className="row p-4"> 
                {/* Date of Birth */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="dob">Date of Birth:</Form.Label>
                    <Form.Control 
                    type="text" 
                    value={dateOfBirth || ''} 
                    onChange={ (e) => setDateOfBirth(e.target.value) } 
                    onFocus={ (e)=> e.target.type='date' }
                    onBlur={ (e)=> e.target.type='text' }
                    id="dob"  
                    name="dob"></Form.Control>
                </div>
                {/* Email */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="email">Email:</Form.Label>
                    <Form.Control 
                    type="email" 
                    value={email || ''}
                    onChange={ (e) => setEmail(e.target.value) } 
                    id="email"  
                    name="email"></Form.Control>
                </div>
            </div>
            <div className="row p-4"> 
                {/* Address 1 */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="address1">Address 1:</Form.Label>
                    <Form.Control 
                    type="text" 
                    value={address1 || ''} 
                    onChange={ (e) => setAddress1(e.target.value) } 
                    id="address1"  
                    name="address1"></Form.Control>
                </div>
                {/* Address 2 */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="address2">Address 2:</Form.Label>
                    <Form.Control 
                    type="text" 
                    value={address2 || ''}
                    onChange={ (e) => setAddress2(e.target.value) } 
                    id="address2"  
                    name="address2"></Form.Control>
                </div>
            </div>
            <div className="row p-4"> 
                {/* City */}
                <div className="col-sm-4">
                    <Form.Label htmlFor="city">City:</Form.Label>
                    <Form.Control 
                    type="text" 
                    value={city || ''} 
                    onChange={ (e) => setCity(e.target.value) } 
                    id="city"  
                    name="city"></Form.Control>
                </div>
                {/* State */}
                <div className="col-sm-4">
                    <Form.Label htmlFor="state">State:</Form.Label>
                    <Form.Control 
                    type="text" 
                    value={stateCode || ''}
                    onChange={ (e) => setStateCode(e.target.value) } 
                    id="state"  
                    name="state"></Form.Control>
                </div>
                {/* Zip */}
                <div className="col-sm-4">
                    <Form.Label htmlFor="zip">Zip Code:</Form.Label>
                    <Form.Control 
                    type="text" 
                    value={zipCode || ''} 
                    onChange={ (e) => setZipCode(e.target.value) } 
                    id="zip"  
                    name="zip"></Form.Control>
                </div>
            </div>
            <div className="row p-4"> 
                {/* Instrument */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="instrument">Instrument:</Form.Label>
                    <Form.Control 
                    type="text" 
                    value={instrument || ''}
                    onChange={ (e) => setInstrument(e.target.value) } 
                    id="instrument"  
                    name="instrument"></Form.Control>
                </div>
            </div>
            </Form.Group>
            <div className="pl-4 p-2 ">
            <button type="button" className="btn btn-primary text-center pl-4 pr-4">Update</button>
            </div>
        </Form>
    );
}

export default UpdateProfileForm;