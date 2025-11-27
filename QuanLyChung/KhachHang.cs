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
    public partial class KhachHang : Form
    {
        MyDataTable dataTable = new MyDataTable();
        string maKhachHang = "";

        public KhachHang()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        public void LayDuLieu()
        {
            string khachHangSQL = "SELECT * FROM KhachHang";
            SqlCommand khachHangCmd = new SqlCommand(khachHangSQL);
            dataTable.Fill(khachHangCmd);

            BindingSource binding = new BindingSource();
            binding.DataSource = dataTable;
            dgvKhachHang.DataSource = binding;

            txtMaKH.DataBindings.Clear();
            txtHoTen.DataBindings.Clear();
            txtSoDienThoai.DataBindings.Clear();
            txtDiemTichLuy.DataBindings.Clear();
            dtpNgayDangKy.DataBindings.Clear();

            txtMaKH.DataBindings.Add("Text", binding, "MaKH");
            txtHoTen.DataBindings.Add("Text", binding, "HoTen");
            txtSoDienThoai.DataBindings.Add("Text", binding, "SoDienThoai");
            txtDiemTichLuy.DataBindings.Add("Text", binding, "DiemTichLuy");
            dtpNgayDangKy.DataBindings.Add("Value", binding, "NgayDangKy");
        }

        private void BatTat(bool giaTri)
        {
            txtMaKH.Enabled = giaTri;
            txtHoTen.Enabled = giaTri;
            txtSoDienThoai.Enabled = giaTri;
            txtDiemTichLuy.Enabled = giaTri;
            dtpNgayDangKy.Enabled = giaTri;

            btnLuu.Enabled = giaTri;
            btnHuy.Enabled = giaTri;

            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
            btnThoat.Enabled = !giaTri;

            dgvKhachHang.Enabled = !giaTri;
        }

        private void KhachHang_Load(object sender, EventArgs e)
        {
            dgvKhachHang.AutoGenerateColumns = false;

            LayDuLieu();
            BatTat(false);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            maKhachHang = "";

            txtMaKH.Clear();
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            txtDiemTichLuy.Clear();
            dtpNgayDangKy.Value = DateTime.Now;

            txtMaKH.Focus();

            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maKhachHang = txtMaKH.Text;

            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(kq == DialogResult.Yes)
            {
                string sql = "DELETE FROM KhachHang Where MaKH = @MaKH";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MaKH", SqlDbType.NVarChar, 5).Value = txtMaKH.Text;
                dataTable.Update(cmd);

                KhachHang_Load(sender, e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaKH.Text == "" || txtHoTen.Text == "" || txtSoDienThoai.Text == "" || txtDiemTichLuy.Text == "" || dtpNgayDangKy.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    if (maKhachHang == "")
                    {
                        string sql = "INSERT INTO KhachHang (MaKH, HoTen, SoDienThoai, DiemTichLuy, NgayDangKy) " +
                                     "VALUES (@MaKH, @HoTen, @SoDienThoai, @DiemTichLuy, @NgayDangKy)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MaKH", SqlDbType.NVarChar, 5).Value = txtMaKH.Text;
                        cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 50).Value = txtHoTen.Text;
                        cmd.Parameters.Add("@SoDienThoai", SqlDbType.NVarChar, 15).Value = txtSoDienThoai.Text;
                        cmd.Parameters.Add("@DiemTichLuy", SqlDbType.Int).Value = Convert.ToInt32(txtDiemTichLuy.Text);
                        cmd.Parameters.Add("@NgayDangKy", SqlDbType.Date).Value = dtpNgayDangKy.Value;
                        dataTable.Update(cmd);
                    }
                    else
                    {
                        string sql = "UPDATE KhachHang SET HoTen = @HoTen, SoDienThoai = @SoDienThoai, DiemTichLuy = @DiemTichLuy, NgayDangKy = @NgayDangKy " +
                                     "WHERE MaKH = @MaKH";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MaKH", SqlDbType.NVarChar, 5).Value = txtMaKH.Text;
                        cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 50).Value = txtHoTen.Text;
                        cmd.Parameters.Add("@SoDienThoai", SqlDbType.NVarChar, 15).Value = txtSoDienThoai.Text;
                        cmd.Parameters.Add("@DiemTichLuy", SqlDbType.Int).Value = Convert.ToInt32(txtDiemTichLuy.Text);
                        cmd.Parameters.Add("@NgayDangKy", SqlDbType.Date).Value = dtpNgayDangKy.Value;
                        dataTable.Update(cmd);
                    }
                    KhachHang_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            KhachHang_Load(sender, e);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(kq == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
