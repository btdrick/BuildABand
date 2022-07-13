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
                    selectCommand.Parameters.AddWithValue("MusicianID", MusicianID);
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
        }
    }
}
