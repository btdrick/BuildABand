import { React, useState, useEffect } from 'react';
import { variables } from '../../Variables.js';
import Comments from '../comments/Comments';
import UserProfile from '../UserProfile';
import './post.css';

/* This is a child component to serveral other components, i.e. child component in Profile component */
const Post = (props) => {
    /* Current user */
    const currentUserID = UserProfile.getMusicianID();
    /* Likes for post */
    const[likes, setLikes] = useState([]);
    /* Amount of likes a post has */
    const [likesCount, setLikesCount] = useState(0);
    /* Author of post */
    const[authorInfo, setAuthorInfo] = useState([]);

    /* Makes api call to backend to get all post likes */
    const getLikes = async() => {
        const response = await fetch(variables.API_URL+'post/'+props.PostID+'/like');
        const data = await response.json(); 
        return data;
    };

    /* Once the page renders, this hook takes place */
    useEffect(() => {
        /* Retrieve information related to author of post */
        const getAuthorInfo = async() => {
            const response = await fetch(variables.API_URL+'musician/'+ props.MusicianID);
            const data = await response.json(); 
            var author = data[0];

            return author;
        };
        getAuthorInfo().then((data) => {
            setAuthorInfo(data);
        });

        getLikes().then((data) => {
            setLikes(data);
            setLikesCount(data.length);
        });
    }, [props.musicianID]);

    /* Author of post */
    const authorName = authorInfo.Fname + " " + authorInfo.Lname;

    /* Gets like by its ID */
    function getLike() {
        const like = likes.find(index => index.MusicianID === currentUserID && index.PostID === props.PostID);
        return like;
    };

    /* Returns true if user has liked a post before */
    function hasLiked() {
        const hasRow = getLike();
        const hasLiked = (hasRow !== undefined);

        return hasLiked;
    };

    /* Posts post like to database */
    const likePost = async() => {
        fetch(variables.API_URL+'post/like',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                PostID: props.PostID,
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
            alert('An error has occurred with liking your post');
        });              
    };

    /* Removes post like from database */
    const unlikePost = async() => {
        let like = getLike();
        fetch(variables.API_URL+'post/like/'+like.PostLikeID,{
            method:'DELETE',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                PostLikeID: like.PostLikeID
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
            alert('An error has occurred with unliking your post');
        });
    };

    /* User cannot stack likes */
    const canLike = hasLiked();

    return ( 
        <div id="container">
            {/* Card style post */}
            <div className="card text-white bg-dark mb-3">
                <div className="card-body">
                    <h5 className="card-title"> { authorName } said: </h5>
                    <p className="card-text"> { props.content } </p>
                    <cite title="Created Time">{ props.createdTime }</cite>
                </div>
                {/* Like section */}
                {canLike ? (
                        <button       
                        type="button"                 
                        className="btn btn-primary" 
                        style={{margin: 0.2 + 'em'}}
                        onClick={() => unlikePost()}> Unlike </button>
                    ) : (
                        <button   
                        type="button"                 
                        className="btn btn-primary" 
                        style={{margin: 0.2 + 'em'}}
                        onClick={() => likePost()}> Like </button>
                    )}
                
                {(likesCount >= 1)
                && <div className="card-footer">{likesCount} Likes</div>}
            </div>
            {/* Render comments for current post */}
            <Comments currentUserID={UserProfile.getMusicianID()} currentPostID={ props.postID } />
        </div>      
    );
}

export default Post;