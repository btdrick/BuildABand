import './style/App.css';
import { Link } from "react-router-dom";
import { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import {Home} from './Home'
import {variables} from './Variables.js';
 
function App() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [musicianID, setMusicianID] = useState('')
  const [isLoggedin, setIsLoggedin] = useState(false);
  const [isAuthenticated, setAuthentication] = useState(false);
  const [error, setError] = useState({});
 
  const login = (event) => {
    event.preventDefault();
    console.log(username, password);
    const userData = {
      username,
      password,
    };
    if (validate()) {
      localStorage.setItem('token-info', JSON.stringify(userData));
      setIsLoggedin(true);
      setUsername('');
      setPassword('');
      setError({})
    }
  };
 
  const logout = () => {
    localStorage.removeItem('token-info');
    setIsLoggedin(false);
  };

  /* Validates user input */
  const validate = () => {
    let errors = {};
    let isValid = true;
    if (!username) {
      isValid = false;
      errors["username"] = "Please enter your username.";
    }
    if (!password) {
      isValid = false;
      errors["password"] = "Please enter your password.";
    }
    setError(errors);
    console.log('errors: ' + error.username + error.password)
    getUserLogin();

    if (!isAuthenticated) {
      isValid = false;
    }
    return isValid;
  }

  /* API call to authenticate user. Passes username and password via query strings (https://en.wikipedia.org/wiki/Query_string) */
  const getUserLogin = async () => {
    let errors = {};
      const response = await fetch(variables.API_URL+'accounts/login?username='+ username + '&password=' + password);
      if (!response.ok) {  
        errors["badResponse"] = await response.text();
        setError("Username or password incorrect");
        return;
      }  
      const data = await response.json();
      setMusicianID(data);
      console.log('musician' + musicianID);
      setAuthentication(true);
  }
 
  return (
    <>
      <div style={{ textAlign: 'center' }}>
        <h1>This is the landing page</h1>
        {!isLoggedin ? 
        (
          <>
          <Link to="/newMusician">New Musician</Link>

            <form action="">
              <input
                type="email"
                onChange={(e) => setUsername(e.target.value)}
                value={username}
                placeholder="Email"
              />
              <div className="text-danger">{error.username}</div>

              <input
                type="password"
                onChange={(e) => setPassword(e.target.value)}
                value={password}
                placeholder="Password"
              />
              <div className="text-danger">{error.password}</div>

              <div className="text-danger">{error.badResponse}</div>
              <button type="submit" onClick={login}>
                Login
              </button>
            </form>
          </>
        ) : 
        (
          <>
            <h1>User is logged in</h1>
            <button onClickCapture={logout}>logout user</button>
            <Home musicianID={musicianID} />
          </>
        )}
      </div>
    </>
  );
}
 
export default App;