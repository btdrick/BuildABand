using BuildABand.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace BuildABand.Controllers
{
    /// <summary>
    /// This class serves as the controller
    /// for data related to Connection table in DB.
    /// It is a mediator between the front-end 
    /// and data access layer for Connection media.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MusicianConnectionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MusicianConnectionDAL musicianConnectionDAL;

        public MusicianConnectionsController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.musicianConnectionDAL = new MusicianConnectionDAL(configuration);
        }


        /// <summary>
        /// Gets Musician Connections
        /// GET: api/MusicianConnections/MusicianID
        /// </summary>
        /// <param name="MusicianID"></param>
        /// <returns>JsonResult with connections</returns
        [HttpGet("{MusicianID}")]
        public JsonResult GetMusicianConnectionsByMusicianID(int musicianID)
        {
            return new JsonResult(this.musicianConnectionDAL.GetMusicianConnectionsByMusicianID(musicianID));
        }

        /// <summary>
        /// Gets Musician Connections
        /// GET: api/MusicianConnections/MusicianID/active
        /// </summary>
        /// <param name="MusicianID"></param>
        /// <returns>JsonResult with activeconnections</returns
        [HttpGet("{MusicianID}/active")]
        public JsonResult GetActiveMusicianConnectionsByMusicianID(int musicianID)
        {
            return this.musicianConnectionDAL.GetActiveMusicianConnectionsByMusicianID(musicianID);
        }

        /// <summary>
        ///   Post new connection 
        ///   post: api/musicianconnections/fromMusicianID/toMusicianID
        /// </summary>
        /// <param name="fromMusicianID"></param>
        /// <param name="toMusicianID"></param>
        /// <returns></returns>
        [HttpPost("{fromMusicianID}/{toMusicianID}")]
        public JsonResult SendConnectionRequest(int fromMusicianID, int toMusicianID)
        {
            if (fromMusicianID < 0 || toMusicianID < 0)
            {
                throw new ArgumentException("Invalid musician");
            }

            try
            {
                this.musicianConnectionDAL.SendConnectionRequest(fromMusicianID, toMusicianID);
            }
            catch (Exception ex)
            {

               return new JsonResult(ex.Message);
            }
           

            return new JsonResult("Connection request sent");
        }

        /// <summary>
        /// Accept pending connection 
        /// Post api/musicianconnection/connectionRequestID
        /// </summary>
        /// <param name="connectionRequestID"></param>
        /// <returns></returns>
        [HttpPost("accept/{connectionRequestID}")]
        public JsonResult AcceptConnectionRequest(int connectionRequestID)
        {
            if (connectionRequestID < 0)
            {
                throw new ArgumentException("Invalid connection request");
            }

            try
            {
                this.musicianConnectionDAL.AcceptConnectionRequest(connectionRequestID);
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);
            }

            

            return new JsonResult("You have a new connection!");
        }

        /// <summary>
        /// Reject pending connection
        /// Post api/musicianconnections/connectionRequestID
        /// </summary>
        /// <param name="connectionRequestID"></param>
        /// <returns></returns>
        [HttpPost("reject/{connectionRequestID}")]
        public JsonResult RejectConnectionRequest(int connectionRequestID)
        {
            if (connectionRequestID < 0)
            {
                throw new ArgumentException("Invalid connection request");
            }

            try
            {
                this.musicianConnectionDAL.RejectConnectionRequest(connectionRequestID);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);

            }
           
            return new JsonResult("Connection request removed.");
        }

         /// <summary>
        /// Reject pending connection
        /// Post api/musicianconnections/connectionRequestID
        /// </summary>
        /// <param name="connectionRequestID"></param>
        /// <returns></returns>
        [HttpPost("disconnect/{connectionRequestID}")]
        public JsonResult DisconnectConnectionRequest(int connectionRequestID)
        {
            if (connectionRequestID < 0)
            {
                throw new ArgumentException("Invalid connection request");
            }

            try
            {
                this.musicianConnectionDAL.DisconnectConnectionRequest(connectionRequestID);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);

            }
           
            return new JsonResult("Connection request removed.");
        }


    }
}
