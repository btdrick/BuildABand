import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import Home from './Home';
import Login from './components/Login'
import Profile from './Profile';
import NewMusician from  './NewMusician';
import reportWebVitals from './reportWebVitals';
import { HashRouter, Routes, Route } from "react-router-dom";
import Connections from './Connections';

/* All of the routing is defined here */
const root = ReactDOM.createRoot(document.getElementById('root'));
//todo: pass entered musician id to home and profile path if it exists
root.render(
  <React.StrictMode>
    <HashRouter>
      <Routes>
        <Route path="/" element={<App/>} />
        <Route path="/home" element={<Home/>}/>
        <Route path="/login" element={<Login/>} />
        <Route path="/newMusician" element={<NewMusician/>}/>
        <Route path="/profile/1" element={<Profile musicianID = {1} />}/>
        <Route path="/connections" element={<Connections/>}/>
      </Routes>
    </HashRouter>
  </React.StrictMode>
);

reportWebVitals();
