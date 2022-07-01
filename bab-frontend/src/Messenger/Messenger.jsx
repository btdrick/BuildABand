import "./Messenger.css"
import Navbar from "../components/header/Navbar"
import Conversation from "../components/conversations/conversation"
import Message from "../components/message/Message"
import { variables } from '../Variables.js';
import ChatOnline from "../components/chatOnline/ChatOnline"
import UserProfile from "../components/UserProfile";
import { useEffect, useState, useRef } from "react"


export default function Messenger(){
    const [currentChat, setCurrentChat] = useState(null);
    const [conversations, setConversations] = useState([]);
    const [messages, setMessages] = useState([]);
    const MusicianID =  UserProfile.getMusicianID();
    const [newMessage, setNewMessage] = useState("");
    const scrollRef = useRef();

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
            
        }

        getMessages();

    },[currentChat])

    const handleSubmit = async(e) => {
        e.preventDefault();
       
        const res = await fetch (variables.API_URL+'message/', {
                            method: 'POST',
                            headers:{
                                'Accept':'application/json',
                                'Content-Type': 'application/json'
                            }, 
                            body:JSON.stringify({
                                SenderID:MusicianID,
                                Text: newMessage,
                                ConversationID:currentChat.ConversationID, 
                            })
                         })
         const data = await res.json();
        console.log(data);
        setMessages([...messages, data]);
        setNewMessage("");

    }

    
    useEffect(() => {
    scrollRef.current?.scrollIntoView({ behavior: "smooth" });
    }, [messages]);
 
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
                    <div ref={scrollRef}>
                       <Message message={m} own={m.SenderID === MusicianID} /> 
                    </div>
                    
                  ))
                  } 
                </div>
                <div className="chatBoxBottom">
                    <textarea className="chatMessageInput" 
                    placeholder="Write something"
                    onChange={(e)=>setNewMessage(e.target.value)}
                    value={newMessage}>
                    </textarea>
                    <button className="chatSubmitButton" onClick={handleSubmit}>Send</button>
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