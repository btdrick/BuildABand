import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import Home from './Home';
import Profile from './Profile';
import NewUser from  './NewUser';
import reportWebVitals from './reportWebVitals';
import { HashRouter, Routes, Route } from "react-router-dom";

/* All of the routing is defined here */
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <HashRouter>
      <Routes>
        <Route path="/" element={<App/>} />
        <Route path="/home" element={<Home/>}/>
        <Route path="/newUser" element={<NewUser/>}/>
        <Route path="/profile/1" element={<Profile musicianID = '1' />}/>
      </Routes>
    </HashRouter>
  </React.StrictMode>
);

reportWebVitals();
