import React from 'react';
import { HashRouter, Routes, Route } from "react-router-dom";
import ReactDOM from 'react-dom/client';
import App from './App';
import Home from './Home';
import Profile from './Profile';
import AccountSettings from './AccountSettings';
import NewMusician from  './NewMusician';
import reportWebVitals from './reportWebVitals';
import Connections from './Connections';
import Messenger  from './Messenger/Messenger';
import Projects from './Projects';

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
        <Route path="/profile/Settings/:id" element={<AccountSettings/>}/>
        <Route path="/connections" element={<Connections/>}/>
        <Route path="/messenger" element={<Messenger/>}/>
        <Route path="/profile/Projects/:id" element={<Projects />}/>
      </Routes>
    </HashRouter>
  </React.StrictMode>
);

reportWebVitals();
