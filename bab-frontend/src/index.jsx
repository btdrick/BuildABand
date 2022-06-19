import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import Home from './Home';
import Profile from './Profile';
import NewMusician from  './NewMusician';
import reportWebVitals from './reportWebVitals';
import { HashRouter, Routes, Route } from "react-router-dom";
import Connections from './Connections';
import UserProfile from './components/UserProfile.js';

/* All of the routing is defined here. Note: Use backtics ` for dynamic paths, not double
or single quotes */
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <HashRouter>
      <Routes>
        <Route path="/" element={<App/>} />
        <Route path="/home" element={<Home/>}/>
        <Route path="/newMusician" element={<NewMusician/>}/>
        <Route path="/profile/:id" element={<Profile/>}/>
        <Route path="/connections" element={<Connections/>}/>
      </Routes>
    </HashRouter>
  </React.StrictMode>
);

reportWebVitals();
