using BuildABand.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BuildABand.Controllers
{
    /// <summary>
    /// This class serves as the controller
    /// for data related to Project_Workon table in DB.
    /// It is a mediator between the front-end 
    /// and data access layer for Project_Workon media.
    /// </summary>
    [Route("api/collaboration")]
    [ApiController]
    public class ProjectCollaboratorController : ControllerBase
    {
        private readonly ProjectCollaboratorDAL projectCollaboratorDAL;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public ProjectCollaboratorController(IConfiguration configuration)
        {
            this.projectCollaboratorDAL = new ProjectCollaboratorDAL(configuration);
        }

        /// <summary>
        /// Gets all project collaborations
        /// GET: api/collaboration
        /// </summary>
        /// <returns>JsonResult table of all project collaborations</returns>
        [HttpGet]
        public JsonResult GetAllProjectCollaborations()
        {
            return this.projectCollaboratorDAL.GetAllProjectCollaborations();
        }

        /// <summary>
        /// Gets all project collaborations
        /// GET: api/project/collaborator
        /// </summary>
        /// <returns>JsonResult table of all project collaborations</returns>
        [HttpGet("collaborator/{MusicianID}")]
        public JsonResult GetAllCollaborationsByMusicianID(int musicianID)
        {
            return this.projectCollaboratorDAL.GetAllProjectCollaborationsByMusicianID(musicianID);
        }
    }
}
