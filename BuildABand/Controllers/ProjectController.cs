using BuildABand.DAL;
using BuildABand.Models;
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
    public class ProjectController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ProjectDAL projectSource;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public ProjectController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.projectSource = new ProjectDAL(_configuration);
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
                projects = this.projectSource.getProjectByMusicianID(musicianID);
                if (projects is null)
                    throw new ArgumentException("No Project");

                foreach (var project in projects)
                {
                    project.Collaborators = this.projectSource.GetCollaboratorsByProjectID
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
        ///   post: api/project/project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult addProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentException("Invalid project");
            }

            try
            {
                this.projectSource.addProject(project);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            return new JsonResult("Project added");
        }


        /// <summary>
        ///   Post add project collaborator 
        ///   post: api/project/addcollaborator/projectID/musicianID
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost("addcollaborator/{projectID}/{musicianID}")]
        public JsonResult addProjectCollaborator(int projectID, int musicianID)
        {
            if (projectID < 0)
            {
                throw new ArgumentException("Invalid projectID");
            }

            if (musicianID < 0)
            {
                throw new ArgumentException("Invalid musicianID");
            }

            try
            {
                this.projectSource.addCollaborator(projectID, musicianID);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            return new JsonResult("Collaborator added");
        }

        /// <summary>
        ///   Post remove collaborator expect project owner
        ///   post: api/project/removecollaborator/projectID/musicianID
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost("removecollaborator/{projectID}/{musicianID}")]
        public JsonResult removeCollaborator(int projectID, int musicianID)
        {
            if (projectID < 0)
            {
                throw new ArgumentException("Invalid projectID");
            }

            try
            {
               int result = this.projectSource.removeCollaborator(projectID, musicianID);
                if (result == 1)
                    return new JsonResult("Can't remove project owner");

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            return new JsonResult("Collabrator removed");
        }


    }
}
