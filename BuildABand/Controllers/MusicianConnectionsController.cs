using BuildABand.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicianConnectionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MusicianConnectionDAL connectionSource;

        public MusicianConnectionsController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.connectionSource = new MusicianConnectionDAL(configuration);
        }


        /// <summary>
        /// Gets Musician Connections
        /// GET: api/MusicianConnections/MusicianID
        /// </summary>
        /// <param name="MusicianID"></param>
        /// <returns>JsonResult with connections</returns
        [HttpGet("{MusicianID}")]
        public JsonResult GetMusicianConnections(int musicianID)
        {
            if (musicianID < 0)
            {
                throw new ArgumentException("Invalid connection request");
            }

            try
            {
                return new JsonResult(
                    this.connectionSource.GetMusicianConnectionsByID(musicianID));
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);
            }
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
            return this.connectionSource.GetActiveMusicianConnectionsByMusicianID(musicianID);
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
                this.connectionSource.SendConnectionRequest(fromMusicianID, toMusicianID);
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
                this.connectionSource.AcceptConnectionRequest(connectionRequestID);
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
                this.connectionSource.RejectConnectionRequest(connectionRequestID);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);

            }
           
            return new JsonResult("Connection request removed.");
        }


    }
}
