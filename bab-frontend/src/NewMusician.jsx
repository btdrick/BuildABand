import React, {Component} from "react";
import { variables } from "./Variables";
import {Navigate} from 'react-router-dom';


class NewMusician extends Component {
 
  constructor() {
    super();
    this.state = {
      input: {},
      errors: {},
      states: [],
      submitResult:""
      };  

     
     this.handleChange = this.handleChange.bind(this);
     this.handleSubmit= this.handleSubmit.bind(this);
  }
   
  //Load states data by calling its api
  
  componentDidMount(){
      fetch(variables.API_URL+'states')
      .then(res=> res.json())
          .then((result) =>{
            this.setState({
              states: result
          });
        })
      
  }



  handleChange(event){
      let input = this.state.input;
      input[event.target.name] = event.target.value;
  
      this.setState({
          input
      });

  }

  //method to return option value for states to be called by select element
  getStates(){
    return this.state.states.map((item) => {
      return <option value={item.StateCode} > {item.StateName}</option>;
    });
    
  }

 
  

  handleSubmit(event){
      event.preventDefault();

      if (this.validate()){
           fetch(variables.API_URL+'musician', {
              method: 'POST',
              headers:{
                  'Accept':'application/json',
                  'Content-Type': 'application/json'
              },
              body: JSON.stringify({
                  Username:this.state.input.username,
                  Password:this.state.input.password,
                  Fname:this.state.input.fname,
                  Lname:this.state.input.lname,
                  DateOfBirth:this.state.input.dateOfBirth,
                  Phone:this.state.input.phone,
                  Email:this.state.input.email,
                  Sex:this.state.input.sex,
                  Address1:this.state.input.address1,
                  Address2:this.state.input.address2,
                  City:this.state.input.city,
                  ZipCode:this.state.input.zipCode,
                  StateCode:this.state.input.stateCode,
                  Instrument:this.state.input.instrument,   
              })
          })
          .then(res=> res.json())
          .then((result) =>{
            this.setState({ submitResult:result});
            alert(result)},
          (error)=> {
            alert('Failed');
          }); 
      }

      
  }
  validate(){
      let input = this.state.input;
      let errors = {};
      let isValid = true;
   
      if (!input["username"]) {
        isValid = false;
        errors["username"] = "Please enter your username.";
      }

      if (!input["sex"]) {
        isValid = false;
        errors["sex"] = "Please enter your sex.";
      }

      if (!input["address1"]) {
        isValid = false;
        errors["address1"] = "Please enter your address.";
      }
      if (!input["city"]) {
        isValid = false;
        errors["city"] = "Please enter your city.";
      }

      if (!input["fname"]) {
        isValid = false;
        errors["fname"] = "Please enter your First name.";
      }

      if (!input["lname"]) {
        isValid = false;
        errors["lname"] = "Please enter your Last name.";
      }
  
      if (typeof input["username"] !== "undefined") {
        const re = /^\S*$/;
        if(input["username"].length < 6 || !re.test(input["username"])){
            isValid = false;
            errors["username"] = "Please enter valid username.";
        }
      }
  
      if (!input["email"]) {
        isValid = false;
        errors["email"] = "Please enter your email Address.";
      }

      
      if (!input["zipCode"]) {
        isValid = false;
        errors["zipCode"] = "Please enter your zip code.";
      }

      if (!input["stateCode"]) {
        isValid = false;
        errors["stateCode"] = "Please select your state.";
      }


      if (!input["dateOfBirth"]) {
        isValid = false;
        errors["dateOfBirth"] = "Please enter your date of birth.";
      }

      if (typeof input["email"] !== "undefined") {
          
        var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
        if (!pattern.test(input["email"])) {
          isValid = false;
          errors["email"] = "Please enter valid email address.";
        }
      }
  
      if (!input["phone"]) {
        isValid = false;
        errors["phone"] = "Please enter valid phone number(XXX-XXX-XXXX).";
      }

      if (typeof input["phone"] !== "undefined") {
          
        var pattern = new RegExp(/^[0-9]{3}-[0-9]{3}-[0-9]{4}$/i);
        if (!pattern.test(input["phone"])) {
          isValid = false;
          errors["phone"] = "Please enter valid phone number(XXX-XXX-XXXX).";
        }
      }
  
      if (!input["password"]) {
        isValid = false;
        errors["password"] = "Please enter your password.";
      }
  
      if (!input["confirm_password"]) {
        isValid = false;
        errors["confirm_password"] = "Please enter your confirm password.";
      }
  
      if (typeof input["password"] !== "undefined") {
        if(input["password"].length < 6){
            isValid = false;
            errors["password"] = "Please add at least 6 charachter.";
        }
      }
  
      if (typeof input["password"] !== "undefined" && typeof input["confirm_password"] !== "undefined") {
          
        if (input["password"] !== input["confirm_password"]) {
          isValid = false;
          errors["password"] = "passwords don't match.";
        }
      }
  
      this.setState({
        errors: errors
      });
  
      return isValid;
  }

  render() {
    if (this.state.submitResult === "New user created"){
      return <Navigate to='/home' />
    }
    return (
      <div class="container">
        <div>
          <h1>Create New Musician</h1>
          <form onSubmit={this.handleSubmit}>
            <div class="form-group">
              <label for="username">Username:</label>
              <input type="text"
                name="username"
                value={this.state.input.username}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter Username"
                id="username" />
              <div className="text-danger">{this.state.errors.username}</div>
            </div>

            <div class="form-group">
              <label for="First Name">First Name:</label>
              <input
                type="text"
                name="fname"
                value={this.state.input.fname}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter First Name"
                id="fname" />

              <div className="text-danger">{this.state.errors.fname}</div>
            </div>
            <div class="form-group">
              <label for="Last Name">Last Name:</label>
              <input
                type="text"
                name="lname"
                value={this.state.input.lname}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter Last Name"
                id="lname" />
              <div className="text-danger">{this.state.errors.lname}</div>
            </div>
            <div class="form-group">
              <label for="email">email Address:</label>
              <input
                type="text"
                name="email"
                value={this.state.input.email}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter email"
                id="email" />

              <div className="text-danger">{this.state.errors.email}</div>
            </div>
            <div class="form-group">
              <label for="password">password:</label>
              <input
                type="password"
                name="password"
                value={this.state.input.password}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter password"
                id="password" />

              <div className="text-danger">{this.state.errors.password}</div>
            </div>

            <div class="form-group">
              <label for="password">Confirm Password:</label>
              <input
                type="password"
                name="confirm_password"
                value={this.state.input.confirm_password}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter confirm password"
                id="confirm_password" />

              <div className="text-danger">{this.state.errors.confirm_password}</div>
            </div>
            <div class="form-group">
              <label for="dateOfBirth">Date of Birth:</label>
              <input
                type="Date"
                name="dateOfBirth"
                value={this.state.input.dateOfBirth}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter Date of Birth"
                id="dateOfBirth" />

              <div className="text-danger">{this.state.errors.dateOfBirth}</div>
            </div>
            <div class="form-group">
              <label for="phone">phone:</label>
              <input
                type="text"
                name="phone"
                value={this.state.input.phone}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter phone number"
                id="phone" />

              <div className="text-danger">{this.state.errors.phone}</div>
            </div>
            <div class="form-group">
              <label for="sex">sex:</label>
              <input
                type="text"
                name="sex"
                value={this.state.input.sex}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter sex"
                id="sex" />

              <div className="text-danger">{this.state.errors.sex}</div>
            </div>

            <div class="form-group">
              <label for="address1">address1:</label>
              <input
                type="text"
                name="address1"
                value={this.state.input.address1}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter address1"
                id="address1" />

              <div className="text-danger">{this.state.errors.address1}</div>
            </div>

            <div class="form-group">
              <label for="address2">address2:</label>
              <input
                type="text"
                name="address2"
                value={this.state.input.address2}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter address2"
                id="address2" />

              <div className="text-danger">{this.state.errors.address2}</div>
            </div>

            <div class="form-group">
              <label for="city">city:</label>
              <input
                type="text"
                name="city"
                value={this.state.input.city}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter city"
                id="city" />

              <div className="text-danger">{this.state.errors.city}</div>
            </div>
            <div class="form-group">
              <label for="zipCode">zipCode:</label>
              <input
                type="text"
                name="zipCode"
                value={this.state.input.zipCode}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter zipCode"
                id="zipCode" />

              <div className="text-danger">{this.state.errors.zipCode}</div>
            </div>
            <div className="form-group">
              <label htmlFor="stateCode">StateCode:</label>
              <select name="stateCode" id="stateCode" 
              value={this.state.input.stateCode} 
              onChange={this.handleChange} 
              className="form-select">
                <option value=""> -- Select a State -- </option>
                {this.getStates()}
              </select>
            
           

            <div className="text-danger">{this.state.errors.stateCode}</div>
            </div>
            <div class="form-group">
              <label for="instrument">instrument:</label>
              <input
                type="text"
                name="instrument"
                value={this.state.input.instrument}
                onChange={this.handleChange}
                class="form-control"
                placeholder="Enter instrument"
                id="instrument" />

              <div className="text-danger">{this.state.errors.instrument}</div>
            </div>
            <br/>
            <input type="submit" value="Submit" class="btn btn-success" />

          </form>
        
        </div>
      </div>
    );
  }
  
}


export default NewMusician;

