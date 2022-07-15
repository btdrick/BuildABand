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


        public List<Project> GetProjectByMusicianID(int musicianID)
        {
            List<Project> projects = new List<Project>();
            string selectStatement = @"
                SELECT ProjectID, Name, OwnerID, StartDate, EndDate,
                CONCAT(M.Fname, ' ', M.Lname) AS OwnerNames,
                p.musicID, Ms.azure_file_name, ms.file_name
                FROM project p
                JOIN Musician M ON p.OwnerID = m.MusicianID
                JOIN Music Ms ON p.MusicID = ms.ID
                WHERE Music
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
                                ProjectInviteID = (int)reader["ProjectInviteID"],
                                ProjectID = (int)reader["ProjectID"],
                                InviteeID = (int)reader["InviteeID"],
                                InviteeNames = reader["InviteeNames"].ToString(),
                                InviterID = (int)reader["InviterID"],
                                InviterNames = reader["InviterNames"].ToString(),
                                CreatedTime = (DateTime)reader["createdTime"],
                                Status = Convert.ToInt32((reader["Status"]))
                            };
                            projects.Add(project);
                        }

                    }
                }
            }
            return projects;
        }



    }
}
