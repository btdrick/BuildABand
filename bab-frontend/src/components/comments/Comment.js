import '../../index.css';
import CommentForm from "./CommentForm";

/* Attributes */
const Comment = ({ 
    comment, 
    addComment, 
    replies, 
    parentID = 
    null, 
    currentUserID, 
    activeComment, 
    setActiveComment, 
    updateComment,
    deleteComment }) => {
    
    /* Only a logged-in user can reply to a comment */
    const canReply = Boolean(currentUserID);
    const isReplying = activeComment 
        && activeComment.type === "replying" 
        && activeComment.CommentID === comment.CommentID;
    /* Ensures only one layer of replies */
    const replyID = parentID ? parentID : comment.CommentID;
    
    /* User can only edit their own comments */
    const canEdit = currentUserID === comment.MusicianID;
    const isEditing = activeComment 
    && activeComment.type === "editing"
    && activeComment.CommentID === comment.CommentID;
    
    /* User can only delete their own comments (if it has no replies) */
    const canDelete = currentUserID === comment.MusicianID 
    && replies.length === 0;

    /* Format comment time displayed */
    const createdTime = 
        new Date(comment.CreatedTime).toLocaleDateString() + " " 
        + new Date(comment.CreatedTime).toLocaleTimeString();
    
    return(
        <div key={ comment.ComentID } className="comment">
            <div className="comment-image-container">
                <img src={ require("./user-icon.png") } alt="user icon" />
            </div>
            <div className="comment-right-part">
                <div className="comment-content">
                    <div className="comment-author">
                        UserID { comment.MusicianID }</div>
                    <div>{ createdTime }</div>
                </div>
                {!isEditing && <div className="comment-text">{comment.Content}</div>}
                {/* Edit comment form */}
                {isEditing && (
                <CommentForm
                    submitLabel="Update"
                    hasCancelButton
                    initialText={comment.Content}
                    handleSubmit={(text) => updateComment(text, comment)}
                    handleCancel={() => {
                    setActiveComment(null);
                    }}
                />)}
                <div className="comment-actions">
                    {/* Reply section */}
                    {canReply 
                    && (<div 
                    className="comment-action" 
                    onClick={() => setActiveComment({CommentID: comment.CommentID, type: "replying"})}>
                    Reply
                    </div>)}
                    {/* Edit section */}
                    {canEdit
                    && (<div
                    className="comment-action"
                    onClick={() => setActiveComment({ CommentID: comment.CommentID, type: "editing"})}>
                    Edit 
                    </div>)}
                    {/* Delete section */}
                    {canDelete 
                    && (<div
                    className="comment-action"
                    onClick={() => deleteComment(comment)}>
                    Delete
                    </div>)}
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
                            setActiveComment={ setActiveComment }
                            updateComment={ updateComment }
                            deleteComment={ deleteComment }/>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default Comment;