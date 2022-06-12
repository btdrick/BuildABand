import './App.css';
import { Login } from './Login';
import { Link } from "react-router-dom";

function App() {
  //todo: if user tries to click on profile link, prompt them to login
  //todo: if they click on home page without being logged in, show them blank home page feed
 return (
  <div>
    <Link to="/home">Home</Link>
    <Link to="/profile/1">Profile</Link>
    <h2>This is the Login Page</h2>
    <Login />
  </div>
 );
}
export default App;
