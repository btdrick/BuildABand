import React, { Component } from 'react';
import { Link } from "react-router-dom";
import { Post } from './Post';

export class Profile extends Component {
    render() {
        /* make api call to properly populate posts with post objects */
        const posts = [1, 2, 3];
        return ( 
            <div>
                <Link to="/">Login</Link>
                <h3> This is the Profile page </h3> 
                <div>
                <table>
                    <tr>
                        <th>Post History</th>
                    </tr>
                    {posts.map(post => <tr><Post /*postDetails={post}*/ /></tr>)}
                </table> 
                </div>
            </div>
        )
    }
}

export default Profile;