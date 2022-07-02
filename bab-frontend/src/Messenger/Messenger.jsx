import "./Messenger.css";
import Navbar from "../components/header/Navbar";
import Conversation from "../components/conversations/conversation";
import Message from "../components/message/Message";
import { variables } from '../Variables.js';
import ChatOnline from "../components/chatOnline/ChatOnline";
import UserProfile from "../components/UserProfile";
import { useEffect, useState, useRef } from "react";


export default function Messenger(){
    const [currentChat, setCurrentChat] = useState(null);
    const [conversations, setConversations] = useState([]);
    const [connections, setConnections] = useState([]);
    const [messages, setMessages] = useState([]);
    const MusicianID =  UserProfile.getMusicianID();
    const [newMessage, setNewMessage] = useState("");
    const scrollRef = useRef();

    const getConversations = async ()=> {
        const res = await fetch(variables.API_URL+'conversation/'+ MusicianID);
        const data = await res.json();
        setConversations(data);
      };

    useEffect(()=> {
     
      getConversations(); 

    },[MusicianID]);

    useEffect( ()=> {
       const getConnections = async ()=> {
          const res = await fetch(variables.API_URL + "musicianconnections/" + MusicianID)
          const data = await res.json();     
          setConnections(data);
       };
       getConnections();
    },[MusicianID]);
         

    const getMessages = async()=> {
        const res = await fetch(variables.API_URL+'message/'+ currentChat.ConversationID);
        const data = await res.json();
        setMessages(data);
        
    }

    useEffect(()=> {
        const timer = setInterval(getMessages, 60000);
        return () => clearInterval(timer);
    },[])


    useEffect(()=> {
        getMessages();

    },[currentChat])

    const addConversation = async(e) => {

        try {
     
        const SenderID = e.InitiatorID === MusicianID? e.FollowerID : e.InitiatorID;
        
        const res = await fetch (variables.API_URL+'conversation/'+ MusicianID + '/' + SenderID, {
                            method: 'POST'})
       getConversations();
            
        } catch (error) {
            
        }

    }
   

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
                         });
        const data = await res.json();
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
                        <h3> Conservation</h3>
                        {conversations.map((c) => (
                            <div onClick={()=> setCurrentChat(c)}>
                            <Conversation conversation={c}/>
                            </div>
                        ))}    
                </div>
            </div>
            <div className="chatBox">
            <h3> Chat</h3>
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
            <h3> Connections</h3>
                <div className="chatOnlineWrapper">
                      
                             { connections.map(c=> (
                                <div onClick={()=> addConversation(c)}>
                                        <ChatOnline connection={c} currentMusicianID={MusicianID} />
                                </div>                            
                            ))}            
                </div>
            </div>
        </div>
        </>
    )
}