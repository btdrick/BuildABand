import React from 'react';
import {variables} from './Variables.js';
import {Navigate} from 'react-router-dom';

export class Login extends React.Component {
    constructor() {
    super();
    this.state = {
      input: {},
      errors: {},
      musician: [],
      redirectToHome: false,
      isAuthenticated: false
    };
    /* Standard way to bind event handlers */
    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }    

  /* Keeps state up to date when user input changes */
  handleChange(event) {
    let input = this.state.input;
    input[event.target.name] = event.target.value;
    this.setState({
      input
    });
  }    

  /* Handles login form submission */
  handleSubmit(event) {
    event.preventDefault();
    if(this.validate()){
        console.log(this.state);
        let input = {};
        input["username"] = "";
        input["password"] = "";
        this.setState({input:input});
    }

    this.setState({redirectToHome: true});
  }

  /* Validates user input */
  validate(){
      let input = this.state.input;
      let errors = {};
      let isValid = true;
      if (!input["username"]) {
        isValid = false;
        errors["username"] = "Please enter your username.";
      }
      if (!input["password"]) {
        isValid = false;
        errors["password"] = "Please enter your password.";
      }

      this.setState({
        errors: errors
      });

      this.getUserLogin();
      if (!this.state.isAuthenticated) {
        isValid = false;
      }
      
      return isValid;
  }

  /* API call to authenticate user */
  async getUserLogin() {
    const response = await fetch(variables.API_URL+'login/'+ this.state.input.username + '/' + this.state.input.password);
        const data = await response.json();
        if(Object.entries(data).length === 0){
          this.setState({
            errors: "User does not exist."
          })
        } else {
          this.setState({
            musician: data,
            isAuthenticated: true
        });
        }
  }

  /* If the redirectToHome property is true, it redirects users to their homepage. Otherwise, the
  login form appears */
  render() {
    if (this.state.redirectToHome) {
      return <Navigate to='/home' />
    }
    return (
      <div>
        <h1>Login</h1>
        <form onSubmit={this.handleSubmit}>
          <div class="form-group">
            <label for="username">Username:</label>
            <input 
              type="text" 
              name="username" 
              value={this.state.input.username}
              onChange={this.handleChange}
              class="form-control" 
              placeholder="Enter username" 
              id="username" />
              <div className="text-danger">{this.state.errors.username}</div>
          </div>

          <div class="form-group">
            <label for="password">Password:</label>
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

          <input type="submit" value="Submit" class="btn btn-success" />
        </form>
      </div>
    );
  }
}

export default Login;