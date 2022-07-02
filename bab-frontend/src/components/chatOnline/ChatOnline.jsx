import "./ChatOnline.css";
import { useEffect, useState, useRef } from "react";
import { variables } from '../../Variables.js';



export default function ChatOnline({connection, currentMusicianID}){
    const [musician, setMusician] = useState([]);
    
    useEffect(() =>{
        const ID = connection.InitiatorID === currentMusicianID? connection.FollowerID : connection.InitiatorID;
    
        const getMusician = async ()=> {
          const res = await fetch(variables.API_URL+'musician/'+ ID);
          const data = await res.json();
          setMusician(data[0]);
        };
    
        getMusician()
       
        
      }, [currentMusicianID, connection]);


    return(
        <div className="chatOnline">
            <div className="chatOnlineFriend">
                <div className="chatOnlineImgContainer">
                    <img className="chatOnlineImg" src="https://images.unsplash.com/photo-1656342696961-8716ada75c7d?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=735&q=80" alt="" />
                    <div className="chatOnlineBadge"></div>
                </div>
                <span className="chatOnlineName"> {musician.Fname + " " + musician.Lname}</span>
            </div>
         </div>
    )
       
  
}