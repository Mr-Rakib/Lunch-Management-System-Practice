using LMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LMS.Services
{
    public class LoginService
    {

        Connection connection = new Connection();

        public bool findByLogin(Login login)
        {
            Login userLogin = null;
            SqlConnection connected = connection.getConnection();
            using (connected)
            {
                SqlCommand cmd = new SqlCommand("select * from userLogin where username='"+login.username+"' and password='"+login.password+"'", connected);
                cmd.CommandType = CommandType.Text;
                connected.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userLogin = new Login(reader.GetString(0), reader.GetString(1));
                    }
                }
            }
            return (userLogin == null) ? false : true;
        }

        public Login findById(string username)
        {
            Login userLogin = null;
            SqlConnection connected = connection.getConnection();
            using (connected)
            {
                SqlCommand cmd = new SqlCommand("select * from userLogin where username='"+username+"'", connected);
                cmd.CommandType = CommandType.Text;
                connected.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userLogin = new Login(reader.GetString(0), reader.GetString(1));
                    }
                }
            }
            return userLogin;
        }
        public User findByUsername(string username)
        {
            User userLogin = null;
            SqlConnection connected = connection.getConnection();
            using (connected)
            {
                SqlCommand cmd = new SqlCommand("select * from users where email='" + username + "'", connected);
                cmd.CommandType = CommandType.Text;
                connected.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userLogin = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetString(3), reader.GetString(4));
                    }
                }
            }
            return userLogin;
        }


        internal bool save(SignUp user)
        {
            if (findById(user.email) == null)
            {

                SqlConnection connected = connection.getConnection();
                using (connected)
                {
                    SqlCommand cmd = new SqlCommand("spSignup", connected);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", user.name));
                    cmd.Parameters.Add(new SqlParameter("@email", user.email));
                    cmd.Parameters.Add(new SqlParameter("@contact", user.contact));
                    cmd.Parameters.Add(new SqlParameter("@address", user.address));
                    cmd.Parameters.Add(new SqlParameter("@password", user.password));
                    connected.Open();
                    try
                    {
                        return (cmd.ExecuteNonQuery() > 0) ? true : false;
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                }
            }
            else return false;
        }
    }
}