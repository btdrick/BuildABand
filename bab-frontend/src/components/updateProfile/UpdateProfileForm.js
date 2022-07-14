import {React, useState, useEffect} from 'react';
import { variables } from '../../Variables.js';
import { useNavigate } from 'react-router-dom';
import ValidateFields from './ValidateUpdateProfileFields.js';
import Form from 'react-bootstrap/Form';

const UpdateProfileForm = ({musicianID}) => {
    const [musician, setMusicianInfo] = useState([]);
    const [input, setInput] = useState([]);
    const [states, setStates] = useState([]);
    let navigate = useNavigate();

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
        if (e.target.type === 'date' && !Date.parse(e.target.value)) {
            return;
        }
        const fieldName = e.target.name;
        const inputValue = e.target.value;
        var newInput = input;
        newInput[fieldName] = inputValue;
        setInput(newInput);
    }

    /* Handle form submission */
    const handleSubmit = () => {
        console.log(input)
        if (!ValidateFields.validSubmit(input)) {
            alert('Invalid input detected');
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

    /* Handle form reset */
    const handleReset = () => {
        setInput(musician);
        document.getElementById("update-profile-form").reset(); 
    }

    //Method to return option value for states to be called by select element
    function getStateNames() {
        return states.map((item) => {
        return <option key={item.StateCode} value={item.StateCode} > {item.StateName}</option>;
        });
    }

    return (
        <Form id="update-profile-form">
            <Form.Group className="mb-3">
            <div className="row p-4"> 
                {/* Date of Birth */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="dob">Date of Birth:</Form.Label>
                    <Form.Control 
                    type="date" 
                    defaultValue={musician.DateOfBirth} 
                    onChange={ (e) => handleInput(e)} 
                    id="DateOfBirth"  
                    name="DateOfBirth"></Form.Control>
                </div>
                {/* Email */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="email">Email:</Form.Label>
                    <Form.Control 
                    type="email" 
                    defaultValue={musician.Email}
                    onChange={ (e) => handleInput(e) } 
                    id="Email"  
                    name="Email"
                    maxLength={200}></Form.Control>
                </div>
            </div>
            <div className="row p-4"> 
                {/* Address 1 */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="address1">Address 1:</Form.Label>
                    <Form.Control 
                    type="text" 
                    defaultValue={musician.Address1} 
                    onChange={ (e) => handleInput(e) } 
                    id="Address1"  
                    name="Address1"
                    maxLength={50}></Form.Control>
                </div>
                {/* Address 2 */}
                <div className="col-sm-6">
                    <Form.Label htmlFor="address2">Address 2:</Form.Label>
                    <Form.Control 
                    type="text" 
                    defaultValue={musician.Address2}
                    onChange={ (e) => handleInput(e) } 
                    id="Address2"  
                    name="Address2"
                    maxLength={50}></Form.Control>
                </div>
            </div>
            <div className="row p-4"> 
                {/* City */}
                <div className="col-sm-4">
                    <Form.Label htmlFor="city">City:</Form.Label>
                    <Form.Control 
                    type="text" 
                    defaultValue={musician.City} 
                    onChange={ (e) => handleInput(e) } 
                    id="City"  
                    name="City"
                    maxLength={50}></Form.Control>
                </div>
                {/* State */}
                <div className="col-sm-4">
                    <Form.Label htmlFor="stateCode">State:</Form.Label>
                    <Form.Select 
                    onChange={ (e) => handleInput(e) } 
                    id="StateCode"  
                    name="StateCode">
                        {getStateNames()}
                    </Form.Select>
                </div>
                {/* Zip */}
                <div className="col-sm-4">
                    <Form.Label htmlFor="zip">Zip Code:</Form.Label>
                    <Form.Control 
                    type="text" 
                    defaultValue={musician.ZipCode} 
                    onChange={ (e) => handleInput(e) } 
                    id="ZipCode"  
                    name="ZipCode"
                    maxLength={20}></Form.Control>
                </div>
            </div>
            <div className="row p-4"> 
                <div className="col-sm-4">
                <Form.Label htmlFor="phone">Phone:</Form.Label>
                    <Form.Control 
                    type="text" 
                    defaultValue={musician.Phone}
                    onChange={ (e) => handleInput(e) } 
                    id="Phone"  
                    name="Phone"></Form.Control>
                </div>
                {/* Instrument */}
                <div className="col-sm-8">
                    <Form.Label htmlFor="instrument">Instrument:</Form.Label>
                    <Form.Control 
                    type="text" 
                    defaultValue={musician.Instrument}
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
                onClick={() => handleReset()}>
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