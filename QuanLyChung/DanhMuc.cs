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
    public partial class DanhMuc : Form
    {
        MyDataTable dataTable = new MyDataTable();
        string maDanhMuc = "";

        public DanhMuc()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        public void LayDuLieu()
        {
            string danhMucSql = "SELECT * FROM DanhMuc";
            SqlCommand danhMucCmd = new SqlCommand(danhMucSql);
            dataTable.Fill(danhMucCmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = dataTable;
            dgvDanhMuc.DataSource = binding;

            txtMaDM.DataBindings.Clear();
            txtTenDM.DataBindings.Clear();

            txtMaDM.DataBindings.Add("Text", binding, "MaDM");
            txtTenDM.DataBindings.Add("Text", binding, "TenDM");
        }

        private void BatTat(bool giaTri)
        {
            txtMaDM.Enabled = giaTri;
            txtTenDM.Enabled = giaTri;
            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;

            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
            btnThoat.Enabled = !giaTri;
        }

        private void DanhMuc_Load(object sender, EventArgs e)
        {
            dgvDanhMuc.AutoGenerateColumns = false;
            LayDuLieu();
            BatTat(false);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            maDanhMuc = "";

            txtMaDM.Clear();
            txtTenDM.Clear();
            txtMaDM.Focus();

            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maDanhMuc = txtMaDM.Text;

            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có muốn chắc xóa danh mục này không ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(kq == DialogResult.Yes)
            {
                string sql = "DELETE FROM DanhMuc WHERE MaDM = @MaDM";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MaDM", SqlDbType.NVarChar, 10).Value = txtMaDM.Text;
                dataTable.Update(cmd);

                DanhMuc_Load(sender, e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if(txtMaDM.Text == "" || txtTenDM.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin danh mục", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    if(maDanhMuc == "")
                    {
                        string sql = "INSERT INTO DanhMuc (MaDM, TenDM) VALUES (@MaDM, @TenDM)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("MaDM", SqlDbType.NVarChar, 10).Value = txtMaDM.Text;
                        cmd.Parameters.Add("TenDM", SqlDbType.NVarChar, 50).Value = txtTenDM.Text;
                        dataTable.Update(cmd);
                    }
                    else
                    {
                        string sql = "UPDATE DanhMuc SET TenDM = @TenDM WHERE MaDM = @MaDM";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("MaDM", SqlDbType.NVarChar, 10).Value = txtMaDM.Text;
                        cmd.Parameters.Add("TenDM", SqlDbType.NVarChar, 50).Value = txtTenDM.Text;
                        dataTable.Update(cmd);
                    }
                    DanhMuc_Load(sender, e);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DanhMuc_Load(sender, e);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc chắn muốn thoát không ? ", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(kq == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
