import "./Messenger.css"
import Navbar from "../components/header/Navbar"
import Conversation from "../components/conversations/conversation"
import Message from "../components/message/Message"
import ChatOnline from "../components/chatOnline/ChatOnline"

export default function Messenger(){
    return(
        <>
        <Navbar />
        <div className="messenger">
            <div className="chatMenu">
                <div className="chatMenuWrapper">
                    <input placeholder="Search for friends" className="chatMenuInput" />
                     <Conversation />
                     <Conversation />
                     <Conversation />
                     <Conversation />
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