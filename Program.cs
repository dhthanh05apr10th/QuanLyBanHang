using System;
using QuanLyBanHang.QuanLyChung;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyBanHang.Chung;
using QuanLyBanHang.HeThong;

namespace QuanLyBanHang
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Data.DataTable dtHeader = new System.Data.DataTable("dtHeader");
            System.Data.DataTable dtDetail = new System.Data.DataTable("dtDetail");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new QuanLyChung.HoaDon());
        }
    }
}
