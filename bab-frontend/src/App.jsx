import './App.css';
import { Link } from "react-router-dom";

function App() {
 return (
  <div>
    <Link to="/home">Home</Link>
    <Link to="/profile/1">Profile</Link>
    <br/>
    <Link to="/newUser">New User</Link>
    <h2>This is the Login Page</h2>
  </div>
 );
}
export default App;
