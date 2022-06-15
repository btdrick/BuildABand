import '../../index.css';

const Comment = ({ comment, replies }) => {
    return(
        <div className="comment">
            <div className="comment-image-container">
                <img src={ require("./user-icon.png") } alt="user icon" />
            </div>
            <div className="comment-right-part">
                <div className="comment-content">
                    <div className="comment-author">
                        UserID { comment.MusicianID }</div>
                    <div>{ comment.CreatedTime }</div>
                </div>
                <div className="comment-text">{comment.Content}</div>
                {/* This will not allow for nested comments in replies */}
                {replies.length > 0 && (
                    <div className="replies">
                        {replies.map(reply => (
                            <Comment 
                            key={ reply.CommentID } 
                            comment={ reply }  
                            replies={[]}/>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default Comment;