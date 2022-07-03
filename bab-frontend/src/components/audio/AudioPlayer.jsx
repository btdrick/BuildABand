import React from 'react';
import ReactPlayer from "react-player";
import track1 from '../../audioFiles/track1.wav'
 
function AudioPlayer() {
  return (
    <div>
      <h3>Song Title</h3>
      <ReactPlayer
        url={track1}
        width="400px"
        height="50px"
        playing={false}
        controls={true}
      />
    </div>
  );
}
 
export default AudioPlayer;