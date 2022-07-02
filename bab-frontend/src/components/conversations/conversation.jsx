import { useEffect } from "react";
import { useState } from "react";
import { variables } from '../../Variables.js';
import UserProfile from "../UserProfile";
import "./conversation.css"

export default function Conversation({conversation}){
  const [musician, setMusician] = useState([]);
  const MusicianID =  UserProfile.getMusicianID();

  useEffect(() =>{
    const friendID = conversation.ReceiverID === MusicianID? conversation.SenderID : conversation.ReceiverID;
    
    const getMusician = async ()=> {
      const res = await fetch(variables.API_URL+'musician/'+ friendID);
      const data = await res.json();
      setMusician(data[0]);
    };

    getMusician();
    
  
    
  }, [MusicianID,conversation]);

 

    return(
      
        <div className="conversation">
          <img className="conversationImg" src="https://images.unsplash.com/photo-1656342696961-8716ada75c7d?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=735&q=80" alt="" />
          <span className="conversationName"> {musician.Fname + " " + musician.Lname}</span>
          {UserProfile.setCurrentConversationNames(musician.Fname + " " + musician.Lname)}
        </div>
    )
}