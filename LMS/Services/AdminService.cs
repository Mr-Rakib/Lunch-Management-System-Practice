using LMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LMS.Services
{
    public class AdminService : CRUDRepository<User, int>
    {
        Connection connection = new Connection();

        public bool deleteById(int id)
        {
            if (findAllInfoById(id) != null)
            {
                SqlConnection connected = connection.getConnection();
                using (connected)
                {
                    SqlCommand cmd = new SqlCommand("spDeleteUserInfo", connected);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
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
            return false;
        }

        internal List<SignUp> findAllInfo()
        {
            List<SignUp> userList = new List<SignUp>();
            SqlConnection connected = connection.getConnection();
            using (connected)
            {
                SqlCommand cmd = new SqlCommand("spFindAllInfo", connected);
                cmd.CommandType = CommandType.StoredProcedure;
                connected.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userList.Add(new SignUp(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                                                reader.GetString(3), reader.GetString(4), reader.GetString(5)));
                    }
                }
            }
            return userList;
        }
        internal SignUp findAllInfoById(int id)
        {
            return findAllInfo().Find(u=> u.id == id);
        }
        internal bool save(SignUp user)
        {
            if (findAllInfoByEmail(user.email) == null)
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

        internal UsersRole findRoleByID(int id , string role)
        {
            return findRoles().Find(user => user.userID == id && user.userRole == role);
        }

        internal bool SetRole(UsersRole user)
        {
            if (findAllInfoById(user.userID) != null)
            {
                SqlConnection connected = connection.getConnection();
                using (connected)
                {
                    SqlCommand cmd = new SqlCommand("spSetRole", connected);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userID", user.userID));
                    cmd.Parameters.Add(new SqlParameter("@role", user.userRole));
                    cmd.Parameters.Add(new SqlParameter("@previousRole", user.userCurrentRole));
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
            return false;
        }


        internal bool DeleteRole(int id, string userRole)
        {
            if (findAllInfoById(id) != null && findRoles().FindAll(u=> u.userID ==id).Count>1)
            {
                SqlConnection connected = connection.getConnection();
                using (connected)
                {
                    SqlCommand cmd = new SqlCommand("spDeleteRole", connected);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userID", id));
                    cmd.Parameters.Add(new SqlParameter("@role", userRole));
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
            return false;
        }

        internal bool AddRole(UsersRole user)
        {
            if (findAllInfoById(user.userID) != null && findRoleByID(user.userID,user.userRole)==null)
            {
                SqlConnection connected = connection.getConnection();
                using (connected)
                {
                    SqlCommand cmd = new SqlCommand("spAddRole", connected);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userID", user.userID));
                    cmd.Parameters.Add(new SqlParameter("@role", user.userRole));
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
            return false;
        }

        internal List<UsersRole> findRoles()
        {
            List<UsersRole> roleList = new List<UsersRole>();
            SqlConnection connected = connection.getConnection();
            using (connected)
            {
                SqlCommand cmd = new SqlCommand("spUserRole", connected);
                connected.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roleList.Add(new UsersRole(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),reader.GetString(3)));
                    }
                }
            }
            return roleList;
        }

        public SignUp findAllInfoByEmail(string email)
        {
            return findAllInfo().Find(u => u.email == email);
        }

        internal bool Update(int id, SignUp user)
        {
           if(findAllInfoById(id) != null){
                SqlConnection connected = connection.getConnection();
                using (connected)
                {
                    SqlCommand cmd = new SqlCommand("spSignupUpdate", connected);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id" ,id ));
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
            return false;
        }

        public List<User> findAll()
        {
            List<User> userList = new List<User>();
            SqlConnection connected = connection.getConnection();
            using (connected)
            {
                SqlCommand cmd = new SqlCommand("select * from users", connected);
                connected.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userList.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                                                reader.GetString(3), reader.GetString(4)));
                    }
                }
            }
            return userList;
        }

        

        public User findById(int id)
        {
            return findAll().Find(u => u.id==id);
        }

        public User save(User user)
        {
            throw new NotImplementedException();
        }

    }
}