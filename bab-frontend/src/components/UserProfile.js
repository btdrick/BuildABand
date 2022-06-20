var UserProfile = (function() {
   
    // pull id from sessionStorage 
   var getMusicianID = function() {
     return parseInt(sessionStorage.musicianID);   
   };
 
   //Set musician id to sessionStorage 
   var setMusicianID = function(ID) {
       sessionStorage.musicianID = ID;     
     
   };

   var clearSession = function (){
       sessionStorage.clear();
   }
 
   return {
       getMusicianID: getMusicianID,
       setMusicianID: setMusicianID,
       clearSession: clearSession
   }
 
 })();
 
 export default UserProfile;