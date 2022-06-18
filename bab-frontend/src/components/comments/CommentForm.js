import { useState } from "react";

/* Attributes */
const CommentForm = ({ 
    handleSubmit, 
    submitLabel, 
    hasCancelButton = false,
    handleCancel,
    initialText = "" }) => {
        
    const [text, setText] = useState(initialText)
    const isTextareaDisabled = text.length === 0;
    const onSubmit = (event) => {
        event.preventDefault();
        handleSubmit(text);
        setText("");
    }

    return(
        <form onSubmit={ onSubmit }>
            <textarea 
            className="comment-form-textarea" 
            value={ text } 
            onChange={ (e) => setText(e.target.value) }
            style={{resize: 'none'}}>
            </textarea>
            <button 
            className="btn btn-primary" 
            disabled={isTextareaDisabled}>{ submitLabel }</button>
            {hasCancelButton && (
            <button
            type="button"
            className="btn btn-danger comment-form-cancel-button"
            onClick={handleCancel}> Cancel </button>)}
        </form>
    );
};

export default CommentForm;