import '../../index.css';
import CommentForm from "./CommentForm";

const Comment = ({ comment, addComment, replies, parentID = null, currentUserID, activeComment, setActiveComment }) => {
    
    /* Only a logged-in user can reply to a comment */
    const canReply = Boolean(currentUserID);
    const isReplying = activeComment 
        && activeComment.type === "replying" 
        && activeComment.CommentID === comment.CommentID;
    /* Ensures only one layer of replies */
    const replyID = parentID ? parentID : comment.CommentID;

    /* Format comment time displayed */
    const createdTime = 
        new Date(comment.CreatedTime).toLocaleDateString() + " " 
        + new Date(comment.CreatedTime).toLocaleTimeString();
    
    return(
        <div className="comment">
            <div className="comment-image-container">
                <img src={ require("./user-icon.png") } alt="user icon" />
            </div>
            <div className="comment-right-part">
                <div className="comment-content">
                    <div className="comment-author">
                        UserID { comment.MusicianID }</div>
                    <div>{ createdTime }</div>
                </div>
                <div className="comment-text">{comment.Content}</div>
                <div className="comment-actions">
                    {/* Reply option will be unavailable if user not logged in */}
                    {canReply 
                    && <div 
                    className="comment-action" 
                    onClick={() => setActiveComment({CommentID: comment.CommentID, type: "replying"})}>
                    Reply
                    </div>}
                </div>
                {/* Replies form */}
                {isReplying && (
                    <CommentForm 
                    submitLabel="Reply" 
                    handleSubmit={(text) => addComment(text, replyID)}/>
                )}
                {/* This will not allow for nested comments in replies */}
                {replies.length > 0 && (
                    <div className="replies">
                        {replies.map(reply => (
                            <Comment 
                            key={ reply.CommentID } 
                            comment={ reply }
                            addComment={ addComment }
                            replies={[]}
                            parentID={ comment.CommentID }
                            currentUserID={ currentUserID }
                            activeComment={ activeComment }
                            setActiveComment={ setActiveComment }/>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default Comment;