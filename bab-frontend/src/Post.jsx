import { React, useState, useEffect } from 'react';
import { variables } from './Variables.js';
import Comments from './components/comments/Comments';

/* This is a child component to serveral other components, i.e. child component in Profile component */
const Post = (props) => {
    /* Constructor for the component. Tracks the posts shown on the feed and whether the page's loading status */
    const [state, setState] = useState({
        authorInfo: [], 
        authorName: "",
        likes: [],
        likesCount: 0
    });

    /* Makes api call to backend to get all post likes */
    const getLikes = async() => {
        const response = await fetch(variables.API_URL+'post/'+props.postID+'/like');
        const data = await response.json(); 

        return data;
    };

    /* Once the page renders, this hook takes place */
    useEffect(() => {
        const getAuthorInfo = async() => {
            const response = await fetch(variables.API_URL+'musician/'+ props.musicianID);
            const data = await response.json(); 
            let author = data[0];
            setState({
                authorInfo: author, 
                authorName: author.Fname + " " + author.Lname
            });
        };
        getAuthorInfo();
        getLikes().then(data => {
            setState({
                likes: data,
                likesCount: data.length
            })
        });
    }, [props.musicianID]);

    /* Gets like by its ID */
    function getLike() {
        const like = state.likes.find(index => index.MusicianID === props.musicianID && index.PostID === props.PostID);
        return like;
    };

    /* Returns true if user has liked a post before */
    function hasLiked() {
        const hasRow = getLike();
        const hasLiked = (hasRow !== undefined);

        return hasLiked;
    };

    /* User cannot stack likes */
    const canLike = hasLiked();

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
                MusicianID: props.MusicianID,
                CreatedTime: new Date()
            })
        })
        .then(res=>res.json())
        .then(()=>{ 
            /* Refresh likes */
            getLikes().then((data) => {
                setState({
                    likes: data,
                    likesCount: data.length
                });
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
                setState({
                    likes: data,
                    likesCount: data.length
                });
            });         
        },(_error)=>{
            alert('An error has occurred with unliking your post');
        });
    };

    return ( 
        <div>
            {/* Card style post */}
            <div className="card text-white bg-dark mb-3" style={{maxWidth: 18 + 'rem'}}>
                <div className="card-header"><h3> This is what a post looks like </h3></div>
                <div className="card-body">
                    <h5 className="card-title"> Post info: </h5>
                    <p className="card-text"> {props.content} </p>
                </div>
                {/* Like section */}
                {canLike ? (
                <button type="button" 
                className="btn btn-primary" 
                style={{margin: 0.2 + 'em'}} 
                onClick={() => unlikePost()}> Unlike </button>
                ) : (
                <button type="button" 
                className="btn btn-primary" 
                style={{margin: 0.2 + 'em'}} 
                onClick={() => likePost()}> Like </button>
                )}
                {(state.likesCount >= 1)
                && <div className="card-footer">{state.likesCount} Likes</div>}
            </div>
            {/* Render comments for current post */}
            <Comments currentUserID={1} currentPostID={props.postID} />
        </div>        
    )
    
}

export default Post;