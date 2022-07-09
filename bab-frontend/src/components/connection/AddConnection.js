import { React, useState, useEffect } from "react";
import { variables } from '../../Variables.js';
import UserProfile from "../../components/UserProfile";
import Button from 'react-bootstrap/Button'

const AddConnection = ({ followerID, connection }) => {
    const [isConnected, setIsConnected] = useState(false);
    const [isPending, setIsPending] = useState(false);

    /* Sets connection status */
    useEffect(() => {
        if (typeof connection === 'undefined') {
            setIsConnected(false);
            setIsPending(false);
        }
        if (typeof connection !== 'undefined') {
            setIsConnected(connection.Connected === true);
            setIsPending(connection.Connected === false);
        }
    }, [connection]);
    
    /* Send connection request to musician */
    const sendConnectionRequest = async() => {
        fetch (variables.API_URL + "musicianconnections/" + 
                UserProfile.getMusicianID()+"/" + followerID,{
                method: "POST"})
                .then(res => res.json())
                .then(result=> alert(result))         
    }

    return(
        <div style={{display: 'flex', justifyContent: 'center', marginBottom: '1em'}}>
            {!isConnected && !isPending &&
            <Button className="btn btn-primary"
                onClick={sendConnectionRequest}>
                Add Connection
            </Button>}
            {isPending && !isConnected &&
            <Button className="btn btn-secondary" disabled>
                Connection Pending . . .
            </Button>}
            {isConnected && <p className="text-center text-muted">You're connected with this musician</p>}
        </div>
    )
}

export default AddConnection;