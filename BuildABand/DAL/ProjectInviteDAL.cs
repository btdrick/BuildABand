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

        public List<ProjectInvite> GetProjectInvitesByID(int MusicianID)
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
            WHERE(P.InviterID = @MusicianID OR P.InviteeID = @MusicianID)
            ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    selectCommand.Parameters.AddWithValue("@MusicianID", MusicianID);
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
        public void AcceptInvitationRequest(int ProjectInviteID)
        {
            string updateStatement = @"
             UPDATE Project_invite
             SET Status = 1 
             WHERE ProjectInviteID = @ProjectInviteID";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand(updateStatement, connection))
                {
                    insertCommand.Parameters.AddWithValue("@ProjectInviteID", ProjectInviteID);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Reject inviation request, Change status to 2 meaning reject
        /// </summary>
        /// <param name="ProjectInviteID"></param>
        public void RejectInvitationRequest(int ProjectInviteID)
        {
            string updateStatement = @"
             UPDATE Project_invite
             SET Status = 2 
             WHERE ProjectInviteID = @ProjectInviteID";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand(updateStatement, connection))
                {
                    insertCommand.Parameters.AddWithValue("@ProjectInviteID", ProjectInviteID);
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


    }
}
