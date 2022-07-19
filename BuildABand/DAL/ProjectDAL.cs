using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BuildABand.DAL
{
    /// <summary>
    /// Project table data access layer (DAL).
    /// </summary>
    public class ProjectDAL
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 1-param constructor
        /// </summary>
        /// <param name="configuration"></param>
        public ProjectDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// DAL to return project by musicianID
        /// </summary>
        /// <param name="project"> project</param>
        public List<Project> GetProjectByMusicianID(int musicianID)
        {
            if (!this.MusicianExists(musicianID))
            {
                throw new ArgumentException("Error: musician does not exist");
            }

            List<Project> projects = new List<Project>();
            string selectStatement = @"
                SELECT PW.ProjectID, P.Name, P.Description, 
                OwnerID, StartDate, EndDate, P.AudioID,
                azure_file_name, file_name, P.is_Private,
                CONCAT(M.Fname, ' ', M.Lname) AS OwnerNames
                FROM Project_Workon PW
                JOIN Project P ON PW.ProjectID = P.ProjectID
                fULL OUTER JOIN Music MS ON P.AudioID = MS.ID
                JOIN Musician M ON P.OwnerID = M.MusicianID
                WHERE PW.MusicianID = @MusicianID
                ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    selectCommand.Parameters.AddWithValue("@MusicianID", musicianID);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project project = new Project();

                            project.ProjectID = (int)reader["ProjectID"];
                            project.Name = reader["Name"].ToString();
                            project.Description = reader["Description"].ToString();
                            project.OwnerID = (int)reader["OwnerID"];
                            project.OwnerNames = reader["OwnerNames"].ToString();
                            project.StartDate = (DateTime)reader["StartDate"];
                            project.AudioID = (int)reader["AudioID"];
                            project.FileName = reader["file_name"].ToString();
                            project.AzureFileName = reader["azure_file_name"].ToString();
                            project.IsPrivate = (byte)reader["is_Private"];

                            if (reader["EndDate"] == DBNull.Value)
                                project.EndDate = null;
                            else
                                project.EndDate = (DateTime)reader["EndDate"];
                            projects.Add(project);
                        }

                    }
                }
            }
            return projects;
        }

        /// <summary>
        /// Add new project to database
        /// </summary>
        /// <param name="project"> project</param>
        public JsonResult AddProject(Project project)
        {
            if (project is null)
            {
                throw new ArgumentException("Error: project cannot be null");
            }

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                try
                {
                    {
                        using (SqlCommand insertCommand = new SqlCommand("dbo.addProject", connection))
                        {
                            insertCommand.CommandType = CommandType.StoredProcedure;
                            insertCommand.Parameters.AddWithValue("@Name", project.Name);
                            insertCommand.Parameters.AddWithValue("@OwnerID", project.OwnerID);
                            insertCommand.Parameters.AddWithValue("@StartDate", DateTime.Now);
                            if (project.EndDate is null)
                                insertCommand.Parameters.AddWithValue("@EndDate", DBNull.Value);
                            else
                                insertCommand.Parameters.AddWithValue("@EndDate", project.EndDate);
                            insertCommand.Parameters.AddWithValue("@AudioID", project.AudioID);
                            insertCommand.Parameters.AddWithValue("@JoinedDate", DateTime.Now);

                            insertCommand.ExecuteNonQuery();
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return new JsonResult("Project Added Successfully");
        }

        /// <summary>
        /// Remove new project to database
        /// </summary>
        /// <param name="project"> project</param>
        public JsonResult AddCollaborator(int projectID, int musicianID)
        {
            if (!this.ProjectExists(projectID))
            {
                throw new ArgumentException("Error: project does not exist");
            }
            if (!this.MusicianExists(musicianID))
            {
                throw new ArgumentException("Error: musician does not exist");
            }

            string insertStatement = @"
                    INSERT Project_workon 
                    VALUES (@ProjectID, @MusicianID, @JoinedDate)
                    ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                try
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertStatement, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@ProjectID", projectID);
                        insertCommand.Parameters.AddWithValue("@MusicianID", musicianID);
                        insertCommand.Parameters.AddWithValue("@JoinedDate", DateTime.Now);

                        insertCommand.ExecuteNonQuery();
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return new JsonResult("Collaborator Added Successfully");
        }

        /// <summary>
        /// Remove collaborator except project owner,
        /// return 1 = error(Can't delete project owner)
        /// </summary>
        /// <param name="project"> project</param>
        public JsonResult RemoveCollaborator(int projectID, int musicianID)
        {
            if (!this.ProjectExists(projectID))
            {
                throw new ArgumentException("Error: project does not exist");
            }
            if(!this.MusicianExists(musicianID))
            {
                throw new ArgumentException("Error: musician does not exist");
            }

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                try
                {
                    using (SqlCommand insertCommand = new SqlCommand("dbo.removeCollaborator", connection))
                    {
                        insertCommand.CommandType = CommandType.StoredProcedure;
                        insertCommand.Parameters.AddWithValue("@projectID", projectID);
                        insertCommand.Parameters.AddWithValue("@musicianID", musicianID);

                        var returnParameter = insertCommand.Parameters.Add("@result", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        insertCommand.ExecuteNonQuery();
                        if ((int)returnParameter.Value == 1)
                        {
                            throw new Exception("Can't delete project owner");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return new JsonResult("Collaborator Successfully Removed");
        }

        /// <summary>
        /// DAL to return all collaborator for a project
        /// </summary>
        /// <param name="project"> project</param>
        public List<Musician> GetCollaboratorsByProjectID(int projectID)
        {
            List<Musician> musicians = new List<Musician>();
            string selectStatement = @"
                SELECT PW.MusicianID, Fname, Lname
                FROM Project_Workon PW
                JOIN Musician M ON PW.MusicianID = M.MusicianID
                WHERE PW.projectID = @projectID
                ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    selectCommand.Parameters.AddWithValue("@projectID", projectID);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Musician musician = new Musician()
                            {
                                musicianID = (int)reader["MusicianID"],
                                FirstName = reader["Fname"].ToString(),
                                LastName = reader["Lname"].ToString()

                            };
                            musicians.Add(musician);
                        }

                    }
                }
            }
            return musicians;
        }

        /// <summary>
        /// Toggles whether a project is private
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>JsonResult if successful</returns>
        public JsonResult ToggleProjectIsPrivate(int projectID)
        {
            if (!this.ProjectExists(projectID))
            {
                throw new ArgumentException("Error: project does not exist");
            }

            string updateStatement = @"
            UPDATE dbo.Project
            SET is_Private = CASE
            WHEN is_Private = 0 THEN 1
            ELSE 0
            END
            WHERE ProjectID = @ProjectID
            ";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(updateStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@ProjectID", projectID);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return new JsonResult("Project Toggled Successfully");
        }

        /// <summary>
        /// Returns true if project exists.
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>True if project exists</returns>
        public bool ProjectExists(int projectID)
        {
            if (projectID <= 0)
            {
                throw new ArgumentException("Error: project ID must be greater than 0");
            }

            string selectStatement = @"
            SELECT COUNT(*)
            FROM dbo.Project
            WHERE ProjectID = @ProjectID
            ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                try
                {
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@ProjectID", projectID);
                        bool projectExists = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return projectExists;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }

        /// <summary>
        /// Returns true if musician exists.
        /// </summary>
        /// <param name="musicianID"></param>
        /// <returns>True if musician exists</returns>
        public bool MusicianExists(int musicianID)
        {
            if (musicianID <= 0)
            {
                throw new ArgumentException("Error: musician ID must be greater than 0");
            }

            string selectStatement = @"
            SELECT COUNT(*)
            FROM dbo.Musician
            WHERE MusicianID = @MusicianID
            ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                try
                {
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@MusicianID", musicianID);
                        bool projectExists = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return projectExists;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }
    }
}
