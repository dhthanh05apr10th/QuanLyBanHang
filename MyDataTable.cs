using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    internal class MyDataTable
    {
        SqlConnection connection;
        SqlDataAdapter adapter;
        SqlCommand command;

        public string ConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["Server"] = @".\SQLEXPRESS";
            builder["Database"] = "QuanLyBanHang";
            builder["Integrated Security"] = true;
            return builder.ConnectionString;
        }
    }
}
