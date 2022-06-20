import { useState } from 'react';
import {Navigate} from 'react-router-dom';
import {variables} from './Variables.js';
import { useEffect } from 'react';
import UserProfile from './components/UserProfile.js';
import 'bootstrap/dist/css/bootstrap.min.css';
import './style/App.css';
 
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
    <div id="login-background">
      <div style={{backgroundColor: '#3a3a3f'}}>
          <img id="logo" src={require('./style/images/Build-A-Band.png')} alt={'Build a band logo'}/>
      </div>
      
      <div className='login-page' style={{ textAlign: 'center' }}>
        <h1>Login</h1>
        {!isLoggedin ? 
        (
          <div>

            <ul className="nav nav-pills nav-justified mb-3" id="ex1" role="tablist">
              <li className="nav-item" role="presentation">
                <a style={{backgroundColor: '#495c7c'}} className="nav-link active" id="tab-login" data-mdb-toggle="pill" href="#/" role="tab"
                  aria-controls="pills-login" aria-selected="true">Login</a>
              </li>
              <li className="nav-item" role="presentation">
                <a className="nav-link" id="tab-register" data-mdb-toggle="pill" href="#/newMusician" role="tab"
                  aria-controls="pills-register" aria-selected="false">Register</a>
              </li>
            </ul>

            <div className="tab-content">
              <div className="tab-pane fade show active" id="pills-login" role="tabpanel" aria-labelledby="tab-login">

                <form action="">
                  <div className="form-group">
                    <input
                      type="text"
                      className="form-control"
                      onChange={(e) => setUsername(e.target.value)}
                      value={username}
                      placeholder="Username"
                    />
                    <div className="text-danger">{error.username}</div>
                  </div>

                    <div className="form-group">
                      <input
                        type="password"
                        className="form-control"
                        onChange={(e) => setPassword(e.target.value)}
                        value={password}
                        placeholder="Password"
                      />
                      <div className="text-danger">{error.password}</div>
                      <div className="text-danger">{error.badResponse}</div>
                    </div>
                    <br/>
                    <button className="btn btn-dark btn-lg btn-block" type="submit" onClick={login} style={{backgroundColor: '#495c7c', border: 'none'}}>
                      Login
                    </button>
                </form>
              </div>

            </div>

          </div>
        ) : 
        (
          <>
            <Navigate to='/home' />
          </>
        )}
      </div>
    </div>
  );
}
 
export default App;