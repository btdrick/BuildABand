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
    /// <summary>
    /// This class serves as the controller
    /// for data related to Project invite table in DB.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectInvite : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ProjectInviteDAL projectInviteSource;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public ProjectInvite(IConfiguration configuration)
        {

            _configuration = configuration;
            this.projectInviteSource = new ProjectInviteDAL(_configuration);
        }


        /// <summary>
        /// Gets specified musician by id
        /// GET: api/projectinvite/MusicianID
        /// </summary>
        /// <returns>JsonResult table of musician</returns>
        [HttpGet("{MusicianID}")]
        public JsonResult GetProjectInviteByMusicianID(int musicianID)
        {
            if (musicianID < 0)
            {
                throw new ArgumentException("Invalid MusicianID");
            }

            try
            {
               return new JsonResult(
                   this.projectInviteSource.GetProjectInvitesByID(musicianID));
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);
            }

        }

        /// <summary>
        ///   Post new invite 
        ///   post: api/projectinvite/fromMusicianID/toMusicianID/projectID
        /// </summary>
        /// <param name="fromMusicianID"></param>
        /// <param name="toMusicianID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpPost("{fromMusicianID}/{toMusicianID}/{projectID}")]
        public JsonResult SendProjectInvite(int fromMusicianID, int toMusicianID, int projectID)
        {
            if (fromMusicianID < 0 || toMusicianID < 0)
            {
                throw new ArgumentException("Invalid musician");
            }

            try
            {
                this.projectInviteSource.SendProjectInvite(fromMusicianID, toMusicianID, projectID);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            return new JsonResult("Invite sent");
        }

        /// <summary>
        /// Accept pending connection 
        /// Post api/projectinvite/projectInviteID
        /// </summary>
        /// <param name="projectInviteID"></param>
        /// <returns></returns>
        [HttpPost("accept/{projectInviteID}")]
        public JsonResult AcceptProjectInvite(int projectInviteID)
        {
            if (projectInviteID < 0)
            {
                throw new ArgumentException("Invalid connection request");
            }

            try
            {
                this.projectInviteSource.AcceptInvitationRequest(projectInviteID);
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);
            }

            return new JsonResult("Invitation Accepted!");
        }

    

    /// <summary>
    /// Reject pending connection 
    /// Post api/projectinvite/reject/projectInviteID
    /// </summary>
    /// <param name="projectInviteID"></param>
    /// <returns></returns>
    [HttpPost("reject/{projectInviteID}")]
    public JsonResult RejectProjectInvite(int projectInviteID)
    {
        if (projectInviteID < 0)
        {
            throw new ArgumentException("Invalid connection request");
        }

        try
        {
                this.projectInviteSource.RejectInvitationRequest(projectInviteID);
        }
        catch (Exception ex)
        {

            return new JsonResult(ex.Message);
        }

        return new JsonResult("Invitation Rejected!");
    }


        /// <summary>
        /// Remove pending connection 
        /// Post api/projectinvite/reject/projectInviteID
        /// </summary>
        /// <param name="projectInviteID"></param>
        /// <returns></returns>
        [HttpPost("reject/{projectInviteID}")]
        public JsonResult removeProjectInvite(int projectInviteID)
        {
            if (projectInviteID < 0)
            {
                throw new ArgumentException("Invalid connection request");
            }

            try
            {
                this.projectInviteSource.RemoveInvite(projectInviteID);
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);
            }

            return new JsonResult("Invite removed!");
        }




    }
}
