import "./Messenger.css"
import Navbar from "../components/header/Navbar"

export default function Messenger(){
    return(
        <>
        <Navbar />
        <div className="messenger">
            <div className="chatMenu">
                <div className="chatMenuWrapper">
                    <input placeholder="Search for friends" className="chatMenuInput" />
                </div>
            </div>
            <div className="chatBox">
               <div className="chatBoxWrapper">Chat</div> 
            </div>
            <div className="chatOnline">
                <div className="chatOnlineWrapper">Online</div>
            </div>
        </div>
        </>
    )
}