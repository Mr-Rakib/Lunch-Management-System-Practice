using LMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LMS.Services
{
    public class OrderService
    {
        Connection connection = new Connection();
        AdminService adminService = new AdminService();

        internal List<Order> findOrderInfoById(string name)
        {
            return findOrderInfo().Where(uo => uo.userID == adminService.findAllInfoByEmail(name).id).ToList();
        }

        internal bool setOrderInfoById(int userID, DateTime date, int quantity)
        {
            if (adminService.findAllInfoById(userID) != null)
            {
                SqlConnection connected = connection.getConnection();
                using (connected)
                {
                    SqlCommand cmd = new SqlCommand("spSetOrder", connected);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userID", userID));
                    cmd.Parameters.Add(new SqlParameter("@date", date));
                    cmd.Parameters.Add(new SqlParameter("@quantity", quantity));

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

       

        internal List<Order> findOrderInfo()
        {
            List<Order> orderList = new List<Order>();
            SqlConnection connected = connection.getConnection();
            using (connected)
            {
                SqlCommand cmd = new SqlCommand("spAllOrders", connected);
                cmd.CommandType = CommandType.StoredProcedure;
                connected.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orderList.Add(new Order(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2), reader.GetInt32(3)));
                    }
                }

            }
            return orderList;
        }

        internal List<Order> findOrderInfoByUserId(int id)
        {
            return findOrderInfo().FindAll(u => u.userID == id);

        }

        internal bool updateOrderInfoById(int userID, DateTime orderDate, int quantity)
        {
            if (adminService.findAllInfoById(userID) != null)
            {
                SqlConnection connected = connection.getConnection();
                using (connected)
                {
                    SqlCommand cmd = new SqlCommand("spUpdateOrder", connected);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userID", userID));
                    cmd.Parameters.Add(new SqlParameter("@date", orderDate));
                    cmd.Parameters.Add(new SqlParameter("@quantity", quantity));

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

        internal bool deleteOrderByID(int id, DateTime date)
        {
            if (adminService.findAllInfoById(id) != null)
            {
                SqlConnection connected = connection.getConnection();
                using (connected)
                {
                    SqlCommand cmd = new SqlCommand("spDeleteOrder", connected);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userID", id));
                    cmd.Parameters.Add(new SqlParameter("@date", date));

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

        internal List<MealCost> getPerMealCost()
        {
            List<MealCost> costPer = new List<MealCost>();
            SqlConnection connected = connection.getConnection();
            using (connected)
            {
                SqlCommand cmd = new SqlCommand("select * from MealCost order by modifyDate DESC", connected);

                connected.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        costPer.Add(new MealCost(reader.GetDateTime(0).Date, reader.GetInt32(1)));
                    }
                }
            }
            return costPer;
        }

        internal List<Order> SearchHistory(int id, DateTime date1, DateTime date2)
        {

            List<Order> orderList = new List<Order>();
            if (adminService.findAllInfoById(id) != null)
            {
                SqlConnection connected = connection.getConnection();
                using (connected)
                {
                    SqlCommand cmd = new SqlCommand("spSearchOrderHistory", connected);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userID", id));
                    cmd.Parameters.Add(new SqlParameter("@date1", date1));
                    cmd.Parameters.Add(new SqlParameter("@date2", date2));

                    connected.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderList.Add(new Order(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2), reader.GetInt32(3)));
                        }
                    }

                }
            }
            return orderList;
        }

        internal List<Order> getPerHeadMealCost()
        {
            string date = (DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + 1).ToString();
            DateTime date2 = DateTime.Parse(date);
            List<Order> orderList = new List<Order>();
            SqlConnection connected = connection.getConnection();
            using (connected)
            {
                SqlCommand cmd = new SqlCommand("SpMealCostPerHaead", connected);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@date1", DateTime.Today));
                cmd.Parameters.Add(new SqlParameter("@date2", date2));

                connected.Open();
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            orderList.Add(
                            new Order(
                            reader.GetInt32(0), reader.GetString(1),
                            reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                            ));
                        }
                    }
                }
                return orderList;
            }
        }

      

       
    }
}