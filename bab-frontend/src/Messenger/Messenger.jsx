import "./Messenger.css"
import Navbar from "../components/header/Navbar"
import Conversation from "../components/conversations/conversation"
import Message from "../components/message/Message"
import { variables } from '../Variables.js';
import ChatOnline from "../components/chatOnline/ChatOnline"
import UserProfile from "../components/UserProfile";
import { useEffect, useState } from "react"

export default function Messenger(){
    const [conversations, setConversations] = useState([]);
    const MusicianID =  UserProfile.getMusicianID();

    useEffect(()=> {
      const getConversations = async ()=> {
        const res = await fetch(variables.API_URL+'conversation/'+ MusicianID);
        const data = await res.json();
        setConversations(data);
      };
      getConversations(); 

    },[MusicianID]);

 
    return(
        <>
        <Navbar />
        <div className="messenger">
            <div className="chatMenu">
                <div className="chatMenuWrapper">
                    <input placeholder="Search for friends" className="chatMenuInput" />
                    {conversations.map((c) => (
                        <Conversation conversation={c}/>
                    ))}  
                </div>
            </div>
            <div className="chatBox">
               <div className="chatBoxWrapper">
                <div className="chatBoxTop">
                    <Message />
                    <Message own={true} />
                    <Message />
                    <Message />
                    <Message own={true} />
                    <Message />
                    <Message />
                    <Message />
                    <Message own={true} />
                    <Message />
                    <Message />
                    <Message own={true} />
                    <Message />
                    <Message />
                    <Message />
                    <Message own={true} />
                    <Message />
                    <Message />
                    <Message own={true} />
                    <Message />
                    <Message />
                </div>
                <div className="chatBoxBottom">
                    <textarea className="chatMessageInput" placeholder="Write something"></textarea>
                    <button className="chatSubmitButton">Send</button>
                </div>
              </div>      
            </div>
            <div className="chatOnline">
                <div className="chatOnlineWrapper">
                    <ChatOnline />
                    <ChatOnline />
                    <ChatOnline />
                    <ChatOnline />
                </div>
            </div>
        </div>
        </>
    )
}