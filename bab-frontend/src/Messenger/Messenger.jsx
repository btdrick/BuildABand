import "./Messenger.css"
import Navbar from "../components/header/Navbar"
import Conversation from "../components/conversations/conversation"
import Message from "../components/message/Message"
import { variables } from '../Variables.js';
import ChatOnline from "../components/chatOnline/ChatOnline"
import UserProfile from "../components/UserProfile";
import { useEffect, useState } from "react"

export default function Messenger(){
    const [currentChat, setCurrentChat] = useState(null);
    const [conversations, setConversations] = useState([]);
    const [messages, setMessages] = useState([]);
    const MusicianID =  UserProfile.getMusicianID();

    useEffect(()=> {
      const getConversations = async ()=> {
        const res = await fetch(variables.API_URL+'conversation/'+ MusicianID);
        const data = await res.json();
        setConversations(data);
      };
      getConversations(); 

    },[MusicianID]);

    useEffect(()=> {
        const getMessages =async()=> {
            const res = await fetch(variables.API_URL+'message/'+ currentChat.ConversationID);
            const data = await res.json();
            setMessages(data);
            console.log(data)
        }

        getMessages();

    },[currentChat])
 
    return(
        <>
        <Navbar />
        <div className="messenger">
            <div className="chatMenu">
                <div className="chatMenuWrapper">
                    <input placeholder="Search for friends" className="chatMenuInput" />
                        {conversations.map((c) => (
                            <div onClick={()=> setCurrentChat(c)}>
                            <Conversation conversation={c}/>
                            </div>
                        ))}    
                </div>
            </div>
            <div className="chatBox">
               <div className="chatBoxWrapper">
                {
                    currentChat?
                <>
                <div className="chatBoxTop">
                  {messages.map(m=>(
                    <Message message={m} own={m.SenderID=== MusicianID} />
                  ))
                  } 
                </div>
                <div className="chatBoxBottom">
                    <textarea className="chatMessageInput" placeholder="Write something"></textarea>
                    <button className="chatSubmitButton">Send</button>
                </div> </>: <span className="noConversationText"> Open a conversation to start a chart </span>}
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