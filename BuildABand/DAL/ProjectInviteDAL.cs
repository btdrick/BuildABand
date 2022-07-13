using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.DAL
{
    public class ProjectInviteDAL
    {
        private readonly IConfiguration _configuration;

        public ProjectInviteDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public List<ProjectInvite> GetProjectInvitesByID(int musicianID)
        {
            List<ProjectInvite> projectInvites = new List<ProjectInvite>();
            string selectStatement = @"
            SELECT ProjectInviteID, ProjectID, InviterID,
            CONCAT(A.Fname, ' ', A.Lname) as InviterNames,
            InviteeID, CONCAT(B.Fname, ' ', B.Lname) as InviteeNames,
            CreatedTime, Status
            FROM Project_invite P
            JOIN Musician A on P.InviterID = A.MusicianID
            JOIN Musician B on P.InviteeID = B.MusicianID
            WHERE(P.InviterID = @musicianID OR P.InviteeID = @musicianID)
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
                            ProjectInvite projectInvite = new ProjectInvite()
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
                            projectInvites.Add(projectInvite);
                        }

                    }
                }
            }
            return projectInvites;
        }

        /// <summary>
        /// Accept inviation request
        /// </summary>
        /// <param name="ProjectInviteID"></param>
        public void AcceptInvitationRequest(int projectInviteID)
        {
            string updateStatement = @"
             UPDATE Project_invite
             SET Status = 1 
             WHERE ProjectInviteID = @projectInviteID";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand(updateStatement, connection))
                {
                    insertCommand.Parameters.AddWithValue("@projectInviteID", projectInviteID);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Reject inviation request, Change status to 2 meaning reject
        /// </summary>
        /// <param name="ProjectInviteID"></param>
        public void RejectInvitationRequest(int projectInviteID)
        {
            string updateStatement = @"
             UPDATE Project_invite
             SET Status = 2 
             WHERE ProjectInviteID = @projectInviteID";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand(updateStatement, connection))
                {
                    insertCommand.Parameters.AddWithValue("@projectInviteID", projectInviteID);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Removes invite from table. 
        /// </summary>
        /// <param name="connectionRequestID"></param>
        public void RemoveInvite(int ProjectInviteID)
        {
            string deleteStatement = "DELETE FROM Project_invite " +
               "WHERE ProjectInviteID = @ProjectInviteID";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand(deleteStatement, connection))
                {
                    insertCommand.Parameters.AddWithValue("@ProjectInviteID", ProjectInviteID);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Adds pending Invite to ProjectInvite table
        /// </summary>
        /// <param name="fromMusicianID"></param>
        /// <param name="toMusicianID"></param>
        public void SendProjectInvite(int fromMusicianID, int toMusicianID, int projectID)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand("dbo.addProjectInvite", connection))
                {
                    insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    insertCommand.Parameters.AddWithValue("@projectID", projectID);
                    insertCommand.Parameters.AddWithValue("@inviterID", fromMusicianID);
                    insertCommand.Parameters.AddWithValue("@inviteeID", toMusicianID);
                    insertCommand.Parameters.AddWithValue("@CreatedTime", DateTime.Now);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }




    }
}
