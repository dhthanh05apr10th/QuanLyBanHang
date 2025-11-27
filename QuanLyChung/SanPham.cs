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
    public partial class SanPham : Form
    {
        MyDataTable dataTable = new MyDataTable();
        string maSanPham = "";

        public SanPham()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        public void LayDuLieu()
        {
            MyDataTable danhMucTable = new MyDataTable();
            danhMucTable.OpenConnection();
            string danhMucSQL = "SELECT MaDM, TenDM FROM DanhMuc";
            SqlCommand danhMucCmd = new SqlCommand(danhMucSQL);
            danhMucTable.Fill(danhMucCmd);

            cboDanhMuc.DataSource = danhMucTable;
            cboDanhMuc.DisplayMember = "TenDM";
            cboDanhMuc.ValueMember = "MaDM";

            string sanPhamSQL = "SELECT S.*, D.TenDM " +
                                "FROM SanPham S INNER JOIN DanhMuc D ON S.MaDM = D.MaDM";
            SqlCommand sanPhamCmd = new SqlCommand(sanPhamSQL);
            dataTable.Fill(sanPhamCmd);

            BindingSource binding = new BindingSource();
            binding.DataSource = dataTable;
            dataGridView1.DataSource = binding;

            txtMaSP.DataBindings.Clear();
            txtTenSP.DataBindings.Clear();
            txtDonGia.DataBindings.Clear();
            cboDanhMuc.DataBindings.Clear();
            txtTrangThai.DataBindings.Clear();

            txtMaSP.DataBindings.Add("Text", binding, "MaSP");
            txtTenSP.DataBindings.Add("Text", binding, "TenSP");
            txtDonGia.DataBindings.Add("Text", binding, "DonGia");
            cboDanhMuc.DataBindings.Add("SelectedValue", binding, "MaDM");
            txtTrangThai.DataBindings.Add("Text", binding, "TrangThai");
        }

        private void BatTat(bool giaTri)
        {
            txtMaSP.Enabled = giaTri;
            txtTenSP.Enabled = giaTri;
            txtDonGia.Enabled = giaTri;
            cboDanhMuc.Enabled = giaTri;
            txtTrangThai.Enabled = giaTri;
            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;
            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
            btnThoat.Enabled = !giaTri;
        }

        private void SanPham_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;

            LayDuLieu();
            BatTat(false);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            maSanPham = "";

            txtMaSP.Clear();
            txtTenSP.Clear();
            txtDonGia.Clear();
            cboDanhMuc.Text = "";
            txtTrangThai.Clear();

            txtMaSP.Focus();

            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maSanPham = txtMaSP.Text;

            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(kq == DialogResult.Yes)
            {
                string sql = "DELETE FROM SanPham WHERE MaSP = @MaSP";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@maSP", SqlDbType.NVarChar, 5).Value = txtMaSP.Text;
                dataTable.Update(cmd);

                SanPham_Load(sender, e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaSP.Text == "" || txtTenSP.Text == "" || txtDonGia.Text == "" || cboDanhMuc.Text == "" || txtTrangThai.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    if (maSanPham == "")
                    {
                        string sql = "INSERT INTO SanPham (MaSP, TenSP, DonGia, MaDM, TrangThai) " +
                                     "VALUES (@MaSP, @TenSP, @DonGia, @MaDM, @TrangThai)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MaSP", SqlDbType.NVarChar, 5).Value = txtMaSP.Text;
                        cmd.Parameters.Add("@TenSP", SqlDbType.NVarChar, 50).Value = txtTenSP.Text;
                        cmd.Parameters.Add("@DonGia", SqlDbType.Decimal).Value = Convert.ToDecimal(txtDonGia.Text);
                        cmd.Parameters.Add("@MaDM", SqlDbType.NVarChar, 5).Value = cboDanhMuc.SelectedValue.ToString();
                        cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 20).Value = txtTrangThai.Text;
                        dataTable.Update(cmd);
                    }
                    else
                    {
                        string sql = "UPDATE SanPham SET TenSP = @TenSP, DonGia = @DonGia, MaDM = @MaDM, TrangThai = @TrangThai " +
                                     "WHERE MaSP = @MaSP";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MaSP", SqlDbType.NVarChar, 5).Value = txtMaSP.Text;
                        cmd.Parameters.Add("@TenSP", SqlDbType.NVarChar, 50).Value = txtTenSP.Text;
                        cmd.Parameters.Add("@DonGia", SqlDbType.Decimal).Value = Convert.ToDecimal(txtDonGia.Text);
                        cmd.Parameters.Add("@MaDM", SqlDbType.NVarChar, 5).Value = cboDanhMuc.SelectedValue.ToString();
                        cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 20).Value = txtTrangThai.Text;
                        dataTable.Update(cmd);
                    }
                    SanPham_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            SanPham_Load(sender, e);
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
