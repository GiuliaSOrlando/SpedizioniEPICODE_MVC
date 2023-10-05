using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SpedizioniEPICODE_MVC.Models
{
    public class Roles : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string conn = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);
            List<string> roles = new List<string>();

            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT Ruolo FROM Utenti WHERE Username=@Username", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Username", username);

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    string ruolo = reader["Ruolo"].ToString();
                    roles.Add(ruolo);
                }
            }
            catch (Exception ex){ }
            finally { sqlConnection.Close(); }
            return roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}