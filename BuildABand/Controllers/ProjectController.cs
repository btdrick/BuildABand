using BuildABand.DAL;
using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace BuildABand.Controllers
{
    /// <summary>
    /// This class serves as the controller
    /// for data related to Project table in DB.
    /// It is a mediator between the front-end 
    /// and data access layer for Project media.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ProjectDAL projectDAL;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public ProjectController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.projectDAL = new ProjectDAL(_configuration);
        }

        /// <summary>
        /// Gets specified project by MusicianID
        /// GET: api/project/MusicianID
        /// </summary>
        /// <returns>JsonResult table of musician</returns>
        [HttpGet("{MusicianID}")]
        public JsonResult GetProjectByMusicianID(int musicianID)
        {
            if (musicianID < 0)
            {
                throw new ArgumentException("Invalid MusicianID");
            }

            try
            {
                List<Project> projects = new List<Project>();
                projects = this.projectDAL.GetProjectByMusicianID(musicianID);
                if (projects is null)
                    throw new ArgumentException("No Project");

                foreach (var project in projects)
                {
                    project.Collaborators = this.projectDAL.GetCollaboratorsByProjectID
                            (project.ProjectID);
                }

                return new JsonResult(projects);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

        }

        /// <summary>
        ///   Post new project 
        ///   post: api/project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult addProject(Project project)
        {
            return this.projectDAL.AddProject(project);
        }


        /// <summary>
        ///   Post add project collaborator 
        ///   post: api/project/addcollaborator/projectID/musicianID
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost("{projectID}/{musicianID}/add")]
        public JsonResult addProjectCollaborator(int projectID, int musicianID)
        {
            return this.projectDAL.AddCollaborator(projectID, musicianID);
        }

        /// <summary>
        ///   Post remove collaborator expect project owner
        ///   post: api/project/removecollaborator/projectID/musicianID
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost("{projectID}/{musicianID}/remove")]
        public JsonResult removeCollaborator(int projectID, int musicianID)
        {
            return this.projectDAL.RemoveCollaborator(projectID, musicianID); 
        }

        /// <summary>
        /// Toggles whether project is private.
        /// PATCH: api/project/ProjectID/private
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>JsonResult if successful</returns>
        [HttpPatch("{ProjectID}/private")]
        public JsonResult ToggleProjectIsPrivate(int projectID)
        {
            return this.projectDAL.ToggleProjectIsPrivate(projectID);
        }
    }
}
