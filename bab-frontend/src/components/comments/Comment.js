import { useState, useEffect, useCallback } from "react";
import { variables } from '../../Variables.js';
import { Link } from "react-router-dom";
import CommentForm from "./CommentForm";
import './comments.css';

/* Attributes */
const Comment = ({ 
    comment, 
    addComment, 
    replies, 
    parentID = null, 
    currentUserID, 
    activeComment, 
    setActiveComment, 
    updateComment,
    deleteComment }) => {
    
    /* Likes for comment */
    const [likes, setLikes] = useState([]);
    /* Amount of likes a comment has */
    const [likesCount, setLikesCount] = useState(0);
    
    /* Makes api call to backend to get all comment likes */
    const getLikes = useCallback(async() => {
        const response = await fetch(variables.API_URL+'comment/'+comment.CommentID+'/like');
        const data = await response.json(); 
        return data;
    }, [comment.CommentID]);

    /* Gets like by its ID */
    function getLike() {
        const like = likes.find(index => index.MusicianID === currentUserID && index.CommentID === comment.CommentID);
        return like;
    }

    /* Returns true if user has liked a comment before */
    function hasLiked() {
        const hasRow = getLike();
        const hasLiked = (hasRow !== undefined);

        return hasLiked;
    }

    /* User cannot stack likes */
    const canLike = hasLiked();

    /* Posts comment like to database */
    const likeComment = async() => {
        fetch(variables.API_URL+'comment/like',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                CommentID: comment.CommentID,
                MusicianID: currentUserID,
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
        });              
    }

    /* Removes comment like from database */
    const unlikeComment = async() => {
        let like = getLike();
        fetch(variables.API_URL+'comment/like/'+like.CommentLikeID,{
            method:'DELETE',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                CommentLikeID: comment.commentID
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
        });
    }

    /* Author of comment */
    const [authorInfo, setAuthorInfo] = useState([]);

    /* Sets state for comment author information */
    useEffect(() => {
        const getAuthorInfo = async() => {
            const response = await fetch(variables.API_URL+'musician/'+comment.MusicianID);
            const data = await response.json(); 
            var author = data[0]
            
            return author;
        };
        getAuthorInfo().then((data) => {
            setAuthorInfo(data);
        }); 
        
         getLikes().then((data) => {
            setLikes(data);
            setLikesCount(data.length);
        });
    }, [comment.MusicianID, getLikes]);

    /* Name of comment author */
    const authorName = authorInfo.Fname + " " + authorInfo.Lname;

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
                    <Link to={`/profile/${comment.MusicianID}`}>{authorName}</Link></div>
                    <div>{ createdTime }</div>
                </div>
                {!isEditing && <div className="comment-text">{comment.Content}</div>}
                {/* Comment likes section */}
                {(likesCount >= 1) 
                && <div className="comment-content">{ likesCount } Likes</div>}
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
                    {/* Like section */}
                    {canLike ? (
                        <div 
                        className="comment-action" 
                        onClick={() => unlikeComment()}>Unike</div>
                    ) : (
                        <div 
                        className="comment-action"
                        onClick={() => likeComment()}>Like</div>
                    )}
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
                            deleteComment={ deleteComment } />
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default Comment;