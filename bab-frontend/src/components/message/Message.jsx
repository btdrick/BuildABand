import "./Message.css"
import {format} from "timeago.js";

export default function Message({message,own}){
    return(
        <div className={own? "message own" : "message"}>
            <div className="messageTop">
                <img className="messageImg" src="https://images.unsplash.com/photo-1656342696961-8716ada75c7d?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=735&q=80" alt="" />
                 <p className="messageText"> {message.Text} </p>
            </div>
           
            <div className="messageBottom">{format(message.CreatedTime)}</div>
        </div>
    )
}