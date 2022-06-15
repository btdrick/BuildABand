import { useState, useEffect } from "react";
import {variables} from '../Variables.js';
import Comment from './Comment';

/* User will only be able to edit, delete comments linked to their UserID */
const Comments = ({currentUserID}) => {
    /* All comments from backend */
    const [backendComments, setBackendComments] = useState([]);
    /* Parent comments, not replies */
    const rootComments = backendComments
    /* Introduce ParentID attribute for this filter to work
    .filter(
        (backendComment) => backendComment.parentID === null);*/

    /* Sets backend comments */
     useEffect(() => {
        getComments().then((data) => {
            setBackendComments(data);
        });
    }, []);

    /* Makes api call to backend to get all comments */
    const getComments = async() => {
        const response = await fetch(variables.API_URL+'comment');
        return await response.json(); 
    };

    return ( 
        <div className="comments">
            <h3 className="comments-title">Comments</h3>
            <div className="comments-container">
                {rootComments.map(rootComment => (
                    <Comment key={rootComment.CommentID} comment={rootComment} />
                ))}
            </div>
        </div>
    );
};

export default Comments;