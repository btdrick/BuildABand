import "./ChatOnline.css"

export default function ChatOnline(){
    return(
        <div className="chatOnline">
            <div className="chatOnlineFriend">
                <div className="chatOnlineImgContainer">
                    <img className="chatOnlineImg" src="https://images.unsplash.com/photo-1656342696961-8716ada75c7d?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=735&q=80" alt="" />
                    <div className="chatOnlineBadge"></div>
                </div>
                <span className="chatOnlineName"> John snow</span>
            </div>
         </div>
    )
       
  
}