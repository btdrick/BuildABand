import './style/App.css';
import { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import {Navigate} from 'react-router-dom';
import {variables} from './Variables.js';
import { useEffect } from 'react';
import UserProfile from './components/UserProfile.js';
 
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
      UserProfile.setMusicianID(data);
  }
 
  useEffect(() => {
    if (isAuthenticated) {
        setIsLoggedin(true);
        setUsername('');
        setPassword('');
        setAuthentication(false);
        setMusicianID(0);
        setError({username: '', password: '', badResponse: ''});
    }
  }, [isAuthenticated, isLoggedin, musicianID, password, username])

  return (
    <>
      <div className='login-page' style={{ textAlign: 'center' }}>
        <h1>Login</h1>
        {!isLoggedin ? 
        (
          <body>

            <ul class="nav nav-pills nav-justified mb-3" id="ex1" role="tablist">
              <li class="nav-item" role="presentation">
                <a class="nav-link active" id="tab-login" data-mdb-toggle="pill" href="#/" role="tab"
                  aria-controls="pills-login" aria-selected="true">Login</a>
              </li>
              <li class="nav-item" role="presentation">
                <a class="nav-link" id="tab-register" data-mdb-toggle="pill" href="#/newMusician" role="tab"
                  aria-controls="pills-register" aria-selected="false">Register</a>
              </li>
            </ul>

            <div class="tab-content">
              
              <div class="tab-pane fade show active" id="pills-login" role="tabpanel" aria-labelledby="tab-login">
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
                    <button className="btn btn-dark btn-lg btn-block" type="submit" onClick={login}>
                      Login
                    </button>
                </form>
              </div>

            </div>

          </body>
        ) : 
        (
          <>
            <Navigate to='/home' />
          </>
        )}
      </div>
    </>
  );
}
 
export default App;