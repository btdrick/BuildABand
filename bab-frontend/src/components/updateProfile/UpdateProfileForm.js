import {React, useState, useEffect} from 'react';
import { variables } from '../../Variables.js';
import { useNavigate } from 'react-router-dom';
import ValidateFields from './ValidateUpdateProfileFields.js';
import Form from 'react-bootstrap/Form';
import Alert from 'react-bootstrap/Alert';

const UpdateProfileForm = ({musicianID}) => {
    const [musician, setMusicianInfo] = useState([]);
    const [input, setInput] = useState([]);
    const [errors, setErrors] = useState([]);
    const [states, setStates] = useState([]);
    let navigate = useNavigate();

    /* Validates string is not undefined or empty */
    function isEmptyOrWhiteSpaces(value) { 
        if (typeof value === 'string') {
            return value === null || value.match(/^ *$/) !== null;
        }
      }

    /* Once the page renders, this hook takes place */
    useEffect(() => {
        /* Retrieve information related to author of post */
        const getOriginalMusicianInfo = async() => {
            const response = await fetch(variables.API_URL+'musician/'+ musicianID);
            const data = await response.json(); 
            var author = data[0];

            return author;
        };
        getOriginalMusicianInfo().then((data) => {
            data.DateOfBirth = new Date(data.DateOfBirth).toISOString().split('T')[0];
            setMusicianInfo(data);
            if(!Object.keys(input).length) {
                setInput(data);
            }
        });
        
        /* Get states for form */
        const getStates = async() => {
            fetch(variables.API_URL+'states')
            .then(res=> res.json())
            .then((result) => {
                setStates(result);
            });
        }
        getStates();
    }, [musicianID, input]);

    /* Handles form input */
    const handleInput = (e) => {
        if (errors) {
            setErrors([]);
        }
        if (e.target.type === 'date' && !Date.parse(e.target.value)) {
            return;
        } 
        const fieldName = e.target.name;
        const inputValue = e.target.value;
        var newInput = input;
        newInput[fieldName] = inputValue;

        /* Replace keys of empty values with original musician information */
        if (!!Object.keys(input).length) {
            Object.keys(newInput).forEach(key => {
                const value = newInput[key];
                if (isEmptyOrWhiteSpaces(value) || value === null) {
                  newInput[key] = musician[key];
                }
            });
        }

        setInput(newInput);
    }

    /* Handle form submission */
    const handleSubmit = () => {
        const errors = ValidateFields.invalidSubmit(input, musician);
        if (errors !== false) {
            setErrors(errors);
            return;
        }
        if (window.confirm("Are you sure you want to update your information?")) {
            fetch(variables.API_URL+'musician/'+musician.MusicianID,{
                method:'PATCH',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json'
                },
                body:JSON.stringify({   
                    MusicianID:     musician.MusicianID,
                    DateOfBirth:    input.DateOfBirth,
                    Phone:          input.Phone,
                    Email:          input.Email,
                    Address1:       input.Address1,
                    Address2:       input.Address2,
                    City:           input.City,
                    StateCode:      input.StateCode,
                    ZipCode:        input.ZipCode,
                    Instrument:     input.Instrument
                })
            })
            .then(res=>res.json())
            .then((result)=>{ 
                alert(result);   
                navigate('/profile/'+musician.MusicianID);           
            },(_error)=>{
                alert('An error has occurred with updating your information');
            });
        }
    }

    //Method to return option value for states to be called by select element
    function getStateNames() {
        return states.map((item) => {
        return <option key={item.StateCode} value={item.StateCode} > {item.StateName}</option>;
        });
    }

    return (
        <Form id="update-profile-form">
            {/* Validation Errors */}
            {!!Object.keys(errors).length && 
            <Alert variant="danger" onClose={() => setErrors([])} dismissible>
                <Alert.Heading>Invalid input error:</Alert.Heading>
                <ul>{Object.keys(errors).map(function(key) {return <li key={key}>{errors[key]}</li>})}</ul> 
            </Alert>}
            <Form.Group className="mb-3">
            <div className="row p-4"> 
                {/* Date of Birth */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="DateOfBirth">Date of Birth:</Form.Label>
                    <Form.Control 
                    type="date" 
                    defaultValue={musician.DateOfBirth} 
                    onChange={ (e) => handleInput(e)} 
                    id="DateOfBirth"  
                    name="DateOfBirth"></Form.Control>
                </div>
                {/* Email */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="Email">Email:</Form.Label>
                    <Form.Control 
                    type="email" 
                    placeholder={musician.Email}
                    onChange={ (e) => handleInput(e) } 
                    id="Email"  
                    name="Email"
                    maxLength={200}></Form.Control>
                </div>
            </div>
            <div className="row p-4"> 
                {/* Address 1 */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="Address1">Address 1:</Form.Label>
                    <Form.Control 
                    type="text" 
                    placeholder={musician.Address1} 
                    onChange={ (e) => handleInput(e) } 
                    id="Address1"  
                    name="Address1"
                    maxLength={50}></Form.Control>
                </div>
                {/* Address 2 */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="Address2">Address 2:</Form.Label>
                    <Form.Control 
                    type="text" 
                    placeholder={musician.Address2}
                    onChange={ (e) => handleInput(e) } 
                    id="Address2"  
                    name="Address2"
                    maxLength={50}></Form.Control>
                </div>
            </div>
            <div className="row p-4"> 
                {/* City */}
                <div className="col-sm-4">
                    <Form.Label htmlFor="City">City:</Form.Label>
                    <Form.Control 
                    type="text" 
                    placeholder={musician.City} 
                    onChange={ (e) => handleInput(e) } 
                    id="City"  
                    name="City"
                    maxLength={50}></Form.Control>
                </div>
                {/* State */}
                <div className="col-sm-4">
                    <Form.Label htmlFor="StateCode">State:</Form.Label>
                    <Form.Select 
                    onChange={ (e) => handleInput(e) } 
                    value={input["StateCode"]}
                    id="StateCode"  
                    name="StateCode">
                        {getStateNames()}
                    </Form.Select>
                </div>
                {/* Zip */}
                <div className="col-sm-4">
                    <Form.Label htmlFor="ZipCode">Zip Code:</Form.Label>
                    <Form.Control 
                    type="text" 
                    placeholder={musician.ZipCode} 
                    onChange={ (e) => handleInput(e) } 
                    id="ZipCode"  
                    name="ZipCode"
                    maxLength={20}></Form.Control>
                </div>
            </div>
            <div className="row p-4"> 
                <div className="col-sm-4">
                <Form.Label htmlFor="Phone">Phone:</Form.Label>
                    <Form.Control 
                    type="text" 
                    placeholder={musician.Phone}
                    onChange={ (e) => handleInput(e) } 
                    id="Phone"  
                    name="Phone"></Form.Control>
                </div>
                {/* Instrument */}
                <div className="col-sm-8">
                    <Form.Label htmlFor="Instrument">Instrument:</Form.Label>
                    <Form.Control 
                    type="text" 
                    placeholder={musician.Instrument}
                    onChange={ (e) => handleInput(e) } 
                    id="Instrument"  
                    name="Instrument"></Form.Control>
                </div>
            </div>
            <div style={{display: 'flex', justifyContent: 'center'}}>
                <button 
                type="button" 
                className="btn btn-warning text-center pl-4 pr-4"
                style={{margin: "0 1em"}}
                onClick={() => document.getElementById("update-profile-form").reset(setInput(musician))}>
                Reset Values
                </button>
                               
                <button 
                type="button" 
                className="btn btn-primary text-center pl-4 pr-4"
                style={{margin: "0 1em"}}
                onClick={() => handleSubmit()}>
                Update Info
                </button>
            </div>
            </Form.Group>
        </Form>
    );
}

export default UpdateProfileForm;