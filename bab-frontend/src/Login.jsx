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

  /* API call to authenticate user. Passes username and password via query strings (https://en.wikipedia.org/wiki/Query_string) */
  async getUserLogin() {
    let errors = {};

      const response = await fetch(variables.API_URL+'accounts/login?username='+ this.state.input.username + '&password=' + this.state.input.password);
      if (!response.ok) {  
        errors["badResponse"] = await response.text();
        this.setState({
          errors: errors
        });
        return;
      }  
      const data = await response.json();
      this.setState({
        musician: data,
        isAuthenticated: true
    });
  }

  /* If the isAuthenticated property is true, it redirects users to their homepage. Otherwise, the
  login form appears */
  render() {
    if (this.state.isAuthenticated) {
      return <Navigate to='/home' />
    }
    return (
      <div>
        <h1>Login</h1>
        <form onSubmit={this.handleSubmit}>
          <div className="form-group">
            <label htmlFor="username">Username:</label>
            <input 
              type="text" 
              name="username" 
              value={this.state.input.username}
              onChange={this.handleChange}
              className="form-control" 
              placeholder="Enter username" 
              id="username" />
              <div className="text-danger">{this.state.errors.username}</div>
          </div>

          <div className="form-group">
            <label htmlFor="password">Password:</label>
            <input 
              type="password" 
              name="password" 
              value={this.state.input.password}
              onChange={this.handleChange}
              className="form-control" 
              placeholder="Enter password" 
              id="password" />
              <div className="text-danger">{this.state.errors.password}</div>
          </div>

          <input type="submit" value="Submit" className="btn btn-success" />
        </form>

        <div className="text-danger">{this.state.errors.badResponse}</div>
      </div>
    );
  }
}

export default Login;