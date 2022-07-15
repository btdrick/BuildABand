using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.DAL
{
    public class ProjectDAL
    {
        private readonly IConfiguration _configuration;

        public ProjectDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Add new project to database
        /// </summary>
        /// <param name="project"> project</param>
        public void addProject(Project project)
        {

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();

                
                {
                    using (SqlCommand insertCommand = new SqlCommand("dbo.addProject", connection))
                    {
                        insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        insertCommand.Parameters.AddWithValue("@Name", project.Name);
                        insertCommand.Parameters.AddWithValue("@OwnerID", project.OwnerID);
                        insertCommand.Parameters.AddWithValue("@StartDate", DateTime.Now);
                        if (project.EndDate == null)
                            insertCommand.Parameters.AddWithValue("@EndDate", DBNull.Value);
                        else
                            insertCommand.Parameters.AddWithValue("@EndDate", project.EndDate);
                        insertCommand.Parameters.AddWithValue("@MusicID", project.MusicID);
                        insertCommand.Parameters.AddWithValue("@JoinedDate", DateTime.Now);


                        insertCommand.ExecuteNonQuery();
                    }

                }
            }
        }

        /// <summary>
        /// Remove collaborator except project owner,
        /// return 1 = error(Can't delete project owner)
        /// </summary>
        /// <param name="project"> project</param>
        public int removeCollaborator(int projectID, int musicianID)
        {
           
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand("dbo.removeCollaborator", connection))
                {
                    insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    insertCommand.Parameters.AddWithValue("@projectID", projectID);
                    insertCommand.Parameters.AddWithValue("@musicianID", musicianID);

                    var returnParameter = insertCommand.Parameters.Add("@result", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    insertCommand.ExecuteNonQuery();  
                    return (int)returnParameter.Value;
                }
            }

        }


        /// <summary>
        /// Remove new project to database
        /// </summary>
        /// <param name="project"> project</param>
        public void addCollaborator(int projectID, int musicianID)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();

                String insertStatement = @"
                    INSERT Project_workon VALUES (@ProjectID, @MusicianID, @JoinedDate)
                    ";

                 using (SqlCommand insertCommand = new SqlCommand(insertStatement, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@ProjectID", projectID);
                        insertCommand.Parameters.AddWithValue("@MusicianID", musicianID);
                        insertCommand.Parameters.AddWithValue("@JoinedDate", DateTime.Now);
 
                        insertCommand.ExecuteNonQuery();
                    }

             
            }
        }

        /// <summary>
        /// DAL to return project by musicianID
        /// </summary>
        /// <param name="project"> project</param>
        public List<Project> getProjectByMusicianID(int musicianID)
        {
            List<Project> projects = new List<Project>();
            string selectStatement = @"
                SELECT PW.ProjectID, P.Name, OwnerID, StartDate, EndDate, P.MusicID,
                azure_file_name, file_name,
                CONCAT(M.Fname, ' ', M.Lname) AS OwnerNames
                FROM Project_Workon PW
                JOIN Project P ON PW.ProjectID = P.ProjectID
                fULL OUTER JOIN Music MS ON P.MusicID = MS.ID
                JOIN Musician M ON P.OwnerID = M.MusicianID
                WHERE PW.MusicianID = @MusicianID ";

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
                            project.OwnerID = (int)reader["OwnerID"];
                            project.OwnerNames = reader["OwnerNames"].ToString();
                            project.StartDate = (DateTime)reader["StartDate"];
                            project.MusicID = (int)reader["MusicID"];
                            project.FileName = reader["file_name"].ToString();
                            project.AzureFileName = reader["azure_file_name"].ToString();

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



    }
}
