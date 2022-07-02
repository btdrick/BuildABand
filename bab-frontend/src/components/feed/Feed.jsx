import { useState, useEffect } from "react";
import { variables } from '../../Variables.js';
import Post from '../post/Post.js';
import CreatePost from '../../modals/CreatePost';
import UserProfile from '../UserProfile.js';
import FeedFilterSwitch from './FeedFilterSwitch';
import './feed.css';

const Feed = ({ getPosts, canCreatePost, canFilterPosts }) => {
    /* All comments from backend */
    const [backendPosts, setBackendPosts] = useState([]);
    /* Amount of posts to show at a time */
    const [visiblePosts, setVisiblePosts] = useState(5);
    /* Feed is loading? */
    const [loading, setLoading] = useState(true);

    /* Sets backend posts */
    useEffect(() => {
        getPosts().then((data) => {
            setBackendPosts(data);
            setLoading(false);
        });
    }, [getPosts]);

    /* Increases amount of visible posts */
    const showMorePosts = () => {
        setVisiblePosts(prevValue => prevValue + 5);
    };

    /* Decreases amoung of visible posts */
    const showFewerPosts = () => {
        setVisiblePosts(prevValue => prevValue - 5);
    };

    /* Handles onClick event for Create button */
    const createPost = (content, audioID) => {
        fetch(variables.API_URL+'post',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({   
                CreatedTime: new Date(),                           
                MusicianID: UserProfile.getMusicianID(),
                Content:    content,
                AudioID: audioID,
            })
        })
        .then(res=>res.json())
        .then((result)=>{
            alert(result);
            getPosts().then((data) => {
                setBackendPosts(data);
            });
        },(_error)=>{
            alert('Post content cannot be blank');
        });
    };

    /* Removes post and its affiliated likes, comments from database */
    const deletePost = async(postID) => {
        if (window.confirm("Are you sure you want to remove this post and its affiliated comments?")) {
            fetch(variables.API_URL+'post/'+postID,{
                method:'DELETE',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json'
                },
                body:JSON.stringify({   
                    PostID: postID
                })
            })
            .then(res=>res.json())
            .then((result)=>{ 
                alert(result);  
                /* Update backendPostss */
                const updatedBackendPosts = backendPosts.filter(
                    (backendPost) => backendPost.PostID !== postID
                );
                setBackendPosts(updatedBackendPosts);
            },(_error)=>{
                alert('An error has occurred with deleting your post');
            });
        }
    }

    /* Makes an api call to get all of the current user's connections */
    const getUserConnections = async() => {
        const response = await fetch(variables.API_URL+'musicianconnections/'+UserProfile.getMusicianID());
        const data = await response.json();
        return data;
    }
        
    if (!loading) {
        return (
            <div>
                {/* Toggle to filter feed */}
                {canFilterPosts === true &&
                <FeedFilterSwitch 
                getConnections={ getUserConnections }
                getBackendPosts={ getPosts }
                setBackendPosts={ setBackendPosts }/>}
                {/* Post creation modal */}
                <CreatePost canCreatePost={ canCreatePost } 
                handleSubmit={ createPost } />
                {/* List-group feed containing card-style group-items */}
                <ul className="list-group">
                    {backendPosts.slice(0, visiblePosts).map((post, index) => 
                        <li key={index} className="list-group-item">
                            <Post 
                            PostID={ post.PostID }
                            CreatedTime={ post.CreatedTime }
                            Content={ post.Content }
                            MusicianID={ post.MusicianID }
                            deletePost={ deletePost } />
                        </li>
                    )}
                </ul>
                {(backendPosts.length > 5) 
                && (visiblePosts < backendPosts.length) 
                && <button 
                    className="btn btn-secondary" 
                    onClick={showMorePosts}>Load next 5 posts</button>
                }
                {(visiblePosts > 5)
                    && <button 
                    className="btn btn-secondary" 
                    onClick={showFewerPosts}>Show fewer posts</button>
                }
            </div>
        );
    }
    /* Do not render feed if loading posts */
    if (loading) {
        return (<div>loading feed...</div>);
    }
}

export default Feed;