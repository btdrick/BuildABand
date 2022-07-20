import { React } from "react";
import { useNavigate } from 'react-router-dom';
import { variables } from '../../Variables.js';
import UserProfile from "../UserProfile.js";
import Button from 'react-bootstrap/Button'

const DeactivateAccount = ({ accountID }) => {
    let navigate = useNavigate();

    /* Clears the session for logout */
    function logout() {
        UserProfile.clearSession();
        navigate('/');
    };

    /* Deactivates the account */
    const deactivateAccount = async() => {
        if (window.confirm("WARNING:\n\nAre you sure you want to deactivate this account?\n" +
        "This profile, along with its posts and comments, will be inaccessible until reactivation")) {
            fetch(variables.API_URL+'accounts/'+accountID+'/deactivate',{
                method:'PATCH',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json'
                },
                body:JSON.stringify({   
                    AccountID: accountID
                })
            })
            .then(res=>res.json())
            .then((result)=>{ 
                alert(result);
                accountID = parseInt(accountID);
                if(accountID !== UserProfile.getMusicianID()) {
                    navigate('/home');
                } 
                if(accountID === UserProfile.getMusicianID()) {
                    logout();
                }           
            },(_error)=>{
                alert('An error has occurred with deactivating this account');
            });
        }
    }

    return(
        <div style={{display: 'flex', justifyContent: 'center', marginBottom: '1em'}}>
            <Button className="btn btn-danger"
                onClick={deactivateAccount}>
                Deactivate Account
            </Button>
        </div>
    )
}
export default DeactivateAccount;