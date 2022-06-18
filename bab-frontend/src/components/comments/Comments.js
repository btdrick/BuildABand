import { useState, useEffect } from "react";
import { variables } from '../../Variables.js';
import Comment from './Comment';
import CommentForm from './CommentForm';

/* User will only be able to edit, delete comments linked to their UserID */
const Comments = ({ currentUserID, currentPostID }) => {
    
    /* All comments from backend */
    const [backendComments, setBackendComments] = useState([]);
    /* Amount of comments to show at a time */
    const [visibleComments, setVisibleComments] = useState(5);
    /* Which comment is active */
    const [activeComment, setActiveComment] = useState(null);
    
    /* Parent comments (not replies) with postID referencing current post*/
    const rootComments = backendComments   
    .filter(
        (backendComment) => backendComment.ParentID === null && backendComment.PostID === currentPostID);
    
    /* Replies to comments */
    const getReplies = (commentID) =>
    backendComments
        .filter((backendComment) => backendComment.ParentID === commentID)
        .sort(
        (a, b) =>
            new Date(a.CreatedTime).getTime() - new Date(b.CreatedTime).getTime()
        );
    
    /* Add comment */
    const addComment = (text, parentID) => {
        createComment(text, parentID);
        setActiveComment(null);
    }
    
    /* Sets backend comments */
     useEffect(() => {
        getComments().then((data) => {
            setBackendComments(data);
        });
    }, []);

    /* Makes api call to backend to get all comments */
    const getComments = async() => {
        const response = await fetch(variables.API_URL+'comment');
        const data = await response.json(); 
        return data;
    };

    /* Increases amount of visible comments */
    const showMoreComments = () => {
        setVisibleComments(prevValue => prevValue + 5);
    }

    /* Decreases amoung of visible comments */
    const showFewerComments = () => {
        setVisibleComments(prevValue => prevValue - 5);
    }

    /* Posts comment to database */
    const createComment = async(text, parentID) => {
        fetch(variables.API_URL+'comment',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                CreatedTime: new Date(),
                ParentID:    parentID,                           
                MusicianID:  currentUserID,
                PostID:      currentPostID,
                Content:     text
            })
        })
        .then(res=>res.json())
        .then(()=>{ 
            /* Refresh backendComments */
            getComments().then((data) => {
                setBackendComments(data);
            })       
        },(_error)=>{
            alert('An error has occurred with submitting your comment');
        })
    }

    /* Delete comment from database */
    const deleteComment = async(commentID) => {
        if (window.confirm("Are you sure you want to remove comment?")) {
            fetch(variables.API_URL+'comment/'+commentID,{
                method:'DELETE',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json'
                },
                body:JSON.stringify({   
                    CommentID: commentID
                })
            })
            .then(res=>res.json())
            .then(()=>{ 
                /* Update backendComments */
                const updatedBackendComments = backendComments.filter(
                    (backendComment) => backendComment.CommentID !== commentID
                  );
                  setBackendComments(updatedBackendComments);         
            },(_error)=>{
                alert('An error has occurred with deleting your comment');
            })
        }
    }

    return ( 
        <div className="comments">
            <h3 className="comments-title">Comments</h3>
            <div className="comment-form-title">Write comment</div>
            <CommentForm submitLabel="Write" handleSubmit={addComment} />
            <div className="comments-container">
                {/* Map comments and their replies by parent */}
                {rootComments.slice(0, visibleComments).map(rootComment => (
                    <Comment 
                    key={ rootComment.CommentID } 
                    comment={ rootComment } 
                    addComment={ addComment }
                    replies={ getReplies(rootComment.CommentID) }
                    currentUserID={ currentUserID }
                    activeComment={ activeComment }
                    setActiveComment={ setActiveComment }
                    deleteComment={ deleteComment } />
                ))}
            </div>
            {(rootComments.length > 5) 
            && (visibleComments < rootComments.length) 
            && <button 
                className="btn btn-secondary" 
                onClick={showMoreComments}>Load next 5 comments</button>
            }
            {(visibleComments > 5)
                && <button 
                className="btn btn-secondary" 
                onClick={showFewerComments}>Show fewer comments</button>
            }
        </div>
    );
};

export default Comments;