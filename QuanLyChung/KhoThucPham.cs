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
    public partial class KhoThucPham : Form
    {
        MyDataTable dataTable = new MyDataTable();
        string maNL = "";

        public KhoThucPham()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        public void LayDuLieu()
        {
            string khoThucPhamSql = "SELECT * FROM KhoThucPham";
            SqlCommand khoThucPhamCmd = new SqlCommand(khoThucPhamSql);
            dataTable.Fill(khoThucPhamCmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = dataTable;
            dgvKhoThucPham.DataSource = binding;

            txtMaNguyenLieu.DataBindings.Clear();
            txtTenNguyenLieu.DataBindings.Clear();
            txtSoLuongTon.DataBindings.Clear();
            txtDonViTinh.DataBindings.Clear();

            txtMaNguyenLieu.DataBindings.Add("Text", binding, "MaNL");
            txtTenNguyenLieu.DataBindings.Add("Text", binding, "TenNL");
            txtSoLuongTon.DataBindings.Add("Text", binding, "SoLuongTon");
            txtDonViTinh.DataBindings.Add("Text", binding, "DonViTinh");
        }

        private void BatTat(bool giaTri)
        {
            txtMaNguyenLieu.Enabled = giaTri;
            txtTenNguyenLieu.Enabled = giaTri;
            txtSoLuongTon.Enabled = giaTri;
            txtDonViTinh.Enabled = giaTri;
            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;

            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
            dgvKhoThucPham.Enabled = !giaTri;
        }

        private void KhoThucPham_Load(object sender, EventArgs e)
        {
            dgvKhoThucPham.AutoGenerateColumns = false;

            LayDuLieu();
            BatTat(false);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            maNL = "";

            txtMaNguyenLieu.Clear();
            txtTenNguyenLieu.Clear();
            txtSoLuongTon.Clear();
            txtDonViTinh.Clear();
            txtMaNguyenLieu.Focus();

            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maNL = txtMaNguyenLieu.Text;
            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc chắn muốn xóa nguyên liệu này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = "DELETE FROM KhoThucPham WHERE MaNL = @MaNL";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@MaNL", txtMaNguyenLieu.Text);
                dataTable.Update(cmd);

                KhoThucPham_Load(sender, e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaNguyenLieu.Text == "" || txtTenNguyenLieu.Text == "" || txtSoLuongTon.Text == "" || txtDonViTinh.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nguyên liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    if (maNL == "")
                    {
                        string sql = "INSERT INTO KhoThucPham (MaNL, TenNL, SoLuong, DonViTinh) VALUES (@MaNL, @TenNL, @SoLuong, @DonViTinh)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.AddWithValue("@MaNL", txtMaNguyenLieu.Text);
                        cmd.Parameters.AddWithValue("@TenNL", txtTenNguyenLieu.Text);
                        cmd.Parameters.AddWithValue("@SoLuong", txtSoLuongTon.Text);
                        cmd.Parameters.AddWithValue("@DonViTinh", txtDonViTinh.Text);
                        dataTable.Update(cmd);
                    }
                    else
                    {
                        string sql = "UPDATE KhoThucPham SET TenNL = @TenNL, SoLuong = @SoLuong, DonViTinh = @DonViTinh WHERE MaNL = @MaNL";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.AddWithValue("@MaNL", txtMaNguyenLieu.Text);
                        cmd.Parameters.AddWithValue("@TenNL", txtTenNguyenLieu.Text);
                        cmd.Parameters.AddWithValue("@SoLuong", txtSoLuongTon.Text);
                        cmd.Parameters.AddWithValue("@DonViTinh", txtDonViTinh.Text);
                        dataTable.Update(cmd);
                    }
                    KhoThucPham_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            KhoThucPham_Load(sender, e);
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
