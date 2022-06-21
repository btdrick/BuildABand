import { useState, useEffect } from "react";
import { variables } from '../../Variables.js';
import CommentForm from "./CommentForm";
import '../../index.css';

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
    deleteComment }) => {
    
    /* Likes for comment */
    const [likes, setLikes] = useState([]);
    /* Amount of likes a comment has */
    const [likesCount, setLikesCount] = useState(0);
    
    /* Makes api call to backend to get all comment likes */
    const getLikes = async() => {
        const response = await fetch(variables.API_URL+'comment/'+comment.CommentID+'/like');
        const data = await response.json(); 
        return data;
    };

    useEffect(() => {
        getLikes().then((data) => {
            setLikes(data);
            setLikesCount(data.length);
        });
    }, [comment.MusicianID]);

    /* Returns true if user has liked a comment before */
    function hasLiked() {
        const hasRow = likes.map(index => index.MusicianID === 1 && index.CommentID === comment.CommentID);
        console.log("hasRow", hasRow);
        return !hasRow.includes(true);
    };

    /* User cannot stack likes */
    const canLike = hasLiked();

    const likeComment = async(type) => {
        switch(type) {
            case 'Like':
                /* Add a like */
                fetch(variables.API_URL+'comment',{
                    method:'POST',
                    headers:{
                        'Accept':'application/json',
                        'Content-Type':'application/json'
                    },
                    body:JSON.stringify({   
                        CommentID: comment.CommentID,
                        MusicianID: 1,
                        CreatedTime: new Date()
                    })
                })
                .then(res=>res.json())
                .then(()=>{ 
                    /* Refresh likes */
                    getLikes().then((data) => {
                        setLikes(data);
                        setLikesCount(data.length);
                    });       
                },(_error)=>{
                    alert('An error has occurred with liking your comment');
                })
                break;
            case 'Unlike':
                /* Remove a like */
                fetch(variables.API_URL+'comment/'+commentID,{
                    method:'DELETE',
                    headers:{
                        'Accept':'application/json',
                        'Content-Type':'application/json'
                    },
                    body:JSON.stringify({   
                        CommentLikeID: commentLikeID
                    })
                })
                .then(res=>res.json())
                .then(()=>{ 
                    /* Refresh likes */
                    getLikes().then((data) => {
                        setLikes(data);
                        setLikesCount(data.length);
                    });         
                },(_error)=>{
                    alert('An error has occurred with unliking your comment');
                })
                break;
            default:
                break;
        }
    };

    /* Only a logged-in user can reply to a comment */
    const canReply = Boolean(currentUserID);
    const isReplying = activeComment 
        && activeComment.type === "replying" 
        && activeComment.CommentID === comment.CommentID;
    /* Ensures only one layer of replies */
    const replyID = parentID ? parentID : comment.CommentID;

    /* User can only delete their own comments (if it has no replies) */
    const canDelete = currentUserID === comment.MusicianID 
    && replies.length === 0;

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
                {(likesCount >= 1) 
                && <div className="comment-content">{ likesCount } Likes</div>}
                <div className="comment-actions">
                    {/* Like section */}
                    {canLike ? (
                        <div 
                        className="comment-action" 
                        onClick={() => likeComment({type: "like"})}>Like</div>
                    ) : (
                        <div 
                        className="comment-action"
                        onClick={() => likeComment({type: "unlike"})}>Unlike</div>
                    )}
                    {/* Reply section */}
                    {canReply 
                    && (<div 
                    className="comment-action" 
                    onClick={() => setActiveComment({CommentID: comment.CommentID, type: "replying"})}>
                    Reply
                    </div>)}
                    {/* Delete section */}
                    {canDelete 
                    && (<div
                    className="comment-action"
                    onClick={() => deleteComment(comment.CommentID)}>
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
                            deleteComment={ deleteComment }/>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default Comment;