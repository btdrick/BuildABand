var UserProfile = (function() {
   
    // pull id from sessionStorage 
   var getMusicianID = function() {
     return parseInt(sessionStorage.musicianID);   
   };
 
   //Set musician id to sessionStorage 
   var setMusicianID = function(ID) {
       sessionStorage.musicianID = ID;
   };

   var getProfileID = function() {
    return sessionStorage.profileID;
   }

   var setProfileID = function(ID) {
        sessionStorage.profileID = ID;
   };

   var getIsAdmin = function() {
    return sessionStorage.isAdmin;
   }

   /* Set isAdmin value based on value */
   var setIsAdmin = function(musicianIsAdminValue){
    if (musicianIsAdminValue === 1) {
        sessionStorage.isAdmin = true;
    }
    if (musicianIsAdminValue !== 1) {
        sessionStorage.isAdmin = false;
    }
   }

   var clearSession = function (){
       sessionStorage.clear();
   }
 
   return {
       getProfileID: getProfileID,
       setProfileID: setProfileID,
       getMusicianID: getMusicianID,
       setMusicianID: setMusicianID,
       setIsAdmin: setIsAdmin,
       getIsAdmin: getIsAdmin,
       clearSession: clearSession
   }
 
 })();
 
 export default UserProfile;