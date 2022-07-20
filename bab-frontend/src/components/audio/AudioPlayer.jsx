import React, {useEffect} from 'react';
import ReactPlayer from "react-player";
import { variables } from '../../Variables';
import { useState } from 'react';
 
function AudioPlayer(props) {
  const [file, setFile] = useState({});

  useEffect(() => { 
    getAudioFile();
  },[props.FileName])

  const getAudioFile = async () => {
    const response = await fetch(variables.API_URL+'audio/blob?azureFileName='+ props.AzureFileName);
    const data = await response;
    setFile(data.url);
  }

  return (
    <div>
      <h4>Track name: {props.FileName}</h4>
      <ReactPlayer
        url={file}
        width="400px"
        height="50px"
        playing={false}
        controls={true}
      />
    </div>
  );
}
 
export default AudioPlayer;