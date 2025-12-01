using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang.QuanLyChung
{
    public partial class HoaDon : Form
    {
        MyDataTable dataTable = new MyDataTable();

        public HoaDon()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        private void BatTat(bool giaTri)
        {
            btnThem.Enabled = giaTri;
            btnHuy.Enabled = !giaTri;
            btnLuu.Enabled = !giaTri;
            btnXuatHD.Enabled = !giaTri;

            txtMaHoaDon.ReadOnly = giaTri;
            txtTenNV.ReadOnly = giaTri;
            txtTenKH.ReadOnly = giaTri;
            txtDiaChi.ReadOnly = giaTri;
            txtSDT.ReadOnly = giaTri;
            txtTenSP.ReadOnly = giaTri;
            txtDonGia.ReadOnly = giaTri;
            txtThanhTien.ReadOnly = giaTri;
            txtTongTien.ReadOnly = giaTri;
            txtGiamGia.Text = "0";
            txtTongTien.Text = "0";

        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            BatTat(true);
        }

        
    }
}
