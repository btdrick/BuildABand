import { useState, useEffect } from "react";
import { variables } from '../../Variables.js';
import Comment from './Comment';
import CommentForm from './CommentForm';

/* User will only be able to edit, delete comments linked to their UserID */
const Comments = ({ currentUserID, currentPostID }) => {
    
    /* All comments from backend */
    const [backendComments, setBackendComments] = useState([]);
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

    return ( 
        <div className="comments">
            <h3 className="comments-title">Comments</h3>
            <div className="comment-form-title">Write comment</div>
            <CommentForm submitLabel="Write" handleSubmit={addComment} />
            <div className="comments-container">
                {/* Map comments and their replies by parent */}
                {rootComments.map(rootComment => (
                    <Comment 
                    key={ rootComment.CommentID } 
                    comment={ rootComment } 
                    addComment={ addComment }
                    replies={ getReplies(rootComment.CommentID) }
                    currentUserID={ currentUserID }
                    activeComment={ activeComment }
                    setActiveComment={ setActiveComment } />
                ))}
            </div>
        </div>
    );
};

export default Comments;