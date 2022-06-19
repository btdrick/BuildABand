import './style/App.css';
import { Link } from "react-router-dom";
import { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import {Home} from './Home'
import {variables} from './Variables.js';
import { useEffect } from 'react';
 
function App() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [musicianID, setMusicianID] = useState(0);
  const [isLoggedin, setIsLoggedin] = useState(false);
  const [isAuthenticated, setAuthentication] = useState(false);
  const [error, setError] = useState({username: '', password: '', badResponse: ''});
 
  const login = (event) => {
    event.preventDefault();
    validate();
  };
 
  const logout = () => {
    localStorage.removeItem('token-info');
    setIsLoggedin(false);
  };

  /* Validates user input */
  const validate = () => {
    let isValid = true;
    if (!username) {
      isValid = false;
      setError({username: 'Please enter your username'})
      return;
    }
    if (!password) {
      isValid = false;
      setError({username: 'Please enter your password'})
      return;
    }
    getUserLogin();
    if (!isAuthenticated) {
      isValid = false;
    }
    return isValid;
  }

  /* API call to authenticate user. Passes username and password via query strings (https://en.wikipedia.org/wiki/Query_string) */
  const getUserLogin = async () => {
      const response = await fetch(variables.API_URL+'accounts/login?username='+ username + '&password=' + password);
      if (!response.ok) {  
        setError({badResponse: (await response.text())})
        return;
      }  
      const data = await response.json();
      setMusicianID(data);
      setAuthentication(true);
  }
 
  useEffect(() => {
    if (isAuthenticated) {
      const userData = {
        username,
        password,
        musicianID,
        isLoggedin,
        isAuthenticated,
      };
      localStorage.setItem('token-info', JSON.stringify(userData));
        setIsLoggedin(true);
        setUsername('');
        setPassword('');
        setAuthentication(false);
        setMusicianID(0);
        setError({username: '', password: '', badResponse: ''});
    }
  }, [isAuthenticated])

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
                type="text"
                onChange={(e) => setUsername(e.target.value)}
                value={username}
                placeholder="Username"
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