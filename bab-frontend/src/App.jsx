import './App.css';
import { Login } from './Login';
import { Link } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
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
