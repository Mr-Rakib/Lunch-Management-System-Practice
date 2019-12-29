using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LMS.Services
{
    public class Connection
    {
        string connectionString = @"Data Source= MWL-UIUX-01\MSSQLSERVERDEV;Initial Catalog=RHHK;Integrated Security=True";
        public SqlConnection getConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}