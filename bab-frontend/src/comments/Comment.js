import '../index.css';

const Comment = ({ comment }) => {
    return(
        <div className="comment">
            <div className="comment-image-container">
                <img src={require("./user-icon.png")} alt="user icon" />
            </div>
            <div className="comment-right-part">
                <div className="comment-content">
                    <div className="comment-author">
                        UserID {comment.MusicianID}</div>
                    <div>{comment.CreatedTime}</div>
                </div>
                <div className="comment-text">{comment.Content}</div>
            </div>
        </div>
    );
};

export default Comment;