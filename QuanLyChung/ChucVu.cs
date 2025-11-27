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
    public partial class ChucVu : Form
    {
        MyDataTable dataTable = new MyDataTable();
        string maChucVu = "";

        public ChucVu()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        public void LayDuLieu()
        {
            string chucVuSQL = "SELECT * FROM ChucVu";
            SqlCommand chucVuCmd = new SqlCommand(chucVuSQL);
            dataTable.Fill(chucVuCmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = dataTable;
            dgvChucVu.DataSource = binding;

            txtMaCV.DataBindings.Clear();
            txtTenCV.DataBindings.Clear();

            txtMaCV.DataBindings.Add("Text", binding, "MaCV");
            txtTenCV.DataBindings.Add("Text", binding, "TenCV");
        }

        private void BatTat(bool giaTri)
        {
            txtMaCV.Enabled = giaTri;
            txtTenCV.Enabled = giaTri;
            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;

            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
            dgvChucVu.Enabled = !giaTri;
        }

        private void ChucVu_Load(object sender, EventArgs e)
        {
            dgvChucVu.AutoGenerateColumns = false;

            LayDuLieu();
            BatTat(false);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            maChucVu = "";

            txtMaCV.Clear();
            txtTenCV.Clear();
            txtMaCV.Focus();

            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maChucVu = txtMaCV.Text;

            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc chắn muốn xóa chức vụ này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(kq == DialogResult.Yes)
            {
                string sql = "DELETE FROM ChucVu WHERE MaCV = @MaCV";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MaCV", SqlDbType.NVarChar, 100).Value = txtMaCV.Text;
                dataTable.Update(cmd);

                ChucVu_Load(sender, e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaCV.Text == "" || txtTenCV.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin chức vụ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    if (maChucVu == "")
                    {
                        string sql = "INSERT INTO ChucVu (MaCV, TenCV) VALUES (@MaCV, @TenCV)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MaCV", SqlDbType.NVarChar, 100).Value = txtMaCV.Text;
                        cmd.Parameters.Add("@TenCV", SqlDbType.NVarChar, 100).Value = txtTenCV.Text;
                        dataTable.Update(cmd);
                    }
                    else
                    {
                        string sql = "UPDATE ChucVu SET TenCV = @TenCV WHERE MaCV = @MaCV";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MaCV", SqlDbType.NVarChar, 100).Value = txtMaCV.Text;
                        cmd.Parameters.Add("@TenCV", SqlDbType.NVarChar, 100).Value = txtTenCV.Text;
                        dataTable.Update(cmd);
                    }
                    ChucVu_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ChucVu_Load(sender, e);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
