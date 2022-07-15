using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

                String insertStatement = @"
                    INSERT Project VALUES (@Name, @OwnerID, @StartDate, @EndDate)
                    ";

                {
                    using (SqlCommand insertCommand = new SqlCommand(insertStatement, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Name", project.Name);
                        insertCommand.Parameters.AddWithValue("@OwnerID", project.OwnerID);
                        insertCommand.Parameters.AddWithValue("@StartDate", DateTime.Now);
                        if (project.EndDate == null)
                            insertCommand.Parameters.AddWithValue("@Instrument", DBNull.Value);
                        else
                            insertCommand.Parameters.AddWithValue("@EndDate", project.EndDate);

                        insertCommand.ExecuteNonQuery();
                    }

                }
            }
        }

        /// <summary>
        /// Remove new project to database
        /// </summary>
        /// <param name="project"> project</param>
        public void addRemove(int projectID)
        {
            String deleteStatement = "DELETE FROM Project " +
               "WHERE ProjectID = @ProjectID";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand(deleteStatement, connection))
                {
                    insertCommand.Parameters.AddWithValue("@projectID", projectID);
                    insertCommand.ExecuteNonQuery();
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


        public List<Project> GetProjectByMusicianID(int musicianID)
        {
            List<Project> projects = new List<Project>();
            string selectStatement = @"
                SELECT PW.ProjectID, P.Name, OwnerID, StartDate, EndDate, P.MusicID,
                azure_file_name, file_name,
                CONCAT(M.Fname, ' ', M.Lname) AS MusicianNames
                FROM 
                Project_Workon PW
                JOIN Project P ON PW.ProjectID = P.ProjectID
                fULL OUTER JOIN Music MS ON P.MusicID = MS.ID
                JOIN Musician M ON P.OwnerID = M.MusicianID
                WHERE PW.MusicianID = @MusicianID
                ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    selectCommand.Parameters.AddWithValue("@musicianID", musicianID);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project project = new Project()
                            {
                                ProjectID = (int)reader["ProjectID"],
                                Name = reader["Name"].ToString(),
                                OwnerID = (int)reader["OwnerID"],
                                StartDate = (DateTime)reader["createdTime"],
                                EndDate = (DateTime)reader["createdTime"],
                                MusicID = (int)reader["MusicID"],
                                FileName = reader["file_name"].ToString(),
                                AzureFileName = reader["azure_file_name"].ToString(),
                            };
                            projects.Add(project);
                        }

                    }
                }
            }
            return projects;
        }

        public List<Musician> GetCollaboratorsByProjectID(int projectID)
        {
            List<Musician> musicians = new List<Musician>();
            string selectStatement = @"
                SELECT MusicianID, Fname, Lname
                FROM Project_Workon PW
                JOIN Musician M ON PW.OwnerID = M.MusicianID
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
