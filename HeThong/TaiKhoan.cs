using QuanLyBanHang.QuanLyChung;
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

namespace QuanLyBanHang.HeThong
{
    public partial class TaiKhoan : Form
    {
        MyDataTable dtTaiKhoan = new MyDataTable();
        string maTaiKhoan = "";
        public TaiKhoan()
        {
            InitializeComponent();
            dtTaiKhoan.OpenConnection();
        }
        private void LoadData()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT t.*, n.HoTen
                                             FROM TaiKhoan t,NhanVien n   
                                             WHERE t.MaNV = n.MaNV");
            dtTaiKhoan.Fill(cmd);
            // Lấy dữ liệu vào MaNV
            MyDataTable MaNV = new MyDataTable();
            MaNV.OpenConnection();
            string nhanViensql = "SELECT * FROM NhanVien";
            SqlCommand MaNVcmd = new SqlCommand(nhanViensql);
            MaNV.Fill(MaNVcmd);
            cboMaNV.DataSource = MaNV;
            cboMaNV.DisplayMember = "HoTen";
            cboMaNV.ValueMember = "MaNV";

            cboQuyenHan.DataSource = null;

            // 2. Xóa tất cả các mục đã có (chỉ cần nếu bạn đã thêm Items thủ công)
            cboQuyenHan.Items.Clear();

            // 3. Nạp lại dữ liệu tĩnh (đã được sửa logic ở trước)
            // TẠO DATA TABLE TĨNH
            DataTable dtQuyenHan = new DataTable();
            dtQuyenHan.Columns.Add("QuyenHan", typeof(string));
            dtQuyenHan.Rows.Add("Admin");
            dtQuyenHan.Rows.Add("Manager");
            dtQuyenHan.Rows.Add("Employee");

            // 4. Gán lại DataSource MỚI
            cboQuyenHan.DataSource = dtQuyenHan;
            cboQuyenHan.DisplayMember = "QuyenHan";
            cboQuyenHan.ValueMember = "QuyenHan";




            BindingSource binding = new BindingSource();
            binding.DataSource = dtTaiKhoan;
            // Hiển thị dữ liệu vào DataGridView
            dgvTaiKhoan.DataSource = binding;
            // Liên kết dữ liệu từ DataGridView lên các control
            txtMaTaiKhoan.DataBindings.Clear();
            cboMaNV.DataBindings.Clear();
            cboQuyenHan.DataBindings.Clear();
            txtTenDangNhap.DataBindings.Clear();
            txtMatKhau.DataBindings.Clear();
            txtGhiChu.DataBindings.Clear();


            txtMaTaiKhoan.DataBindings.Add("Text", binding, "MaTaiKhoan");
            cboMaNV.DataBindings.Add("SelectedValue", binding, "MaNV");
            cboQuyenHan.DataBindings.Add("Text", binding, "QuyenHan");
            txtTenDangNhap.DataBindings.Add("Text", binding, "TenDangNhap");
            txtMatKhau.DataBindings.Add("Text", binding, "MatKhau");
            txtGhiChu.DataBindings.Add("Text", binding, "GhiChu");
        }

        private void BatTat(bool b)
        {
            btnThem.Enabled = b;
            btnSua.Enabled = b;
            btnXoa.Enabled = b;
            btnLuu.Enabled = !b;
            btnHuyBo.Enabled = !b;


            txtMaTaiKhoan.Enabled = !b;
            cboMaNV.Enabled = !b;
            cboQuyenHan.Enabled = !b;
            txtTenDangNhap.Enabled = !b;
            txtMatKhau.Enabled = !b;
            txtGhiChu.Enabled = !b;
        }

        private void TaiKhoan_Load(object sender, EventArgs e)
        {
            dgvTaiKhoan.AutoGenerateColumns = false;
            LoadData();
            BatTat(true);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            maTaiKhoan = "";
            txtMaTaiKhoan.Text = "";
            cboMaNV.Text ="";
            cboQuyenHan.Text = "";
            txtTenDangNhap.Text = "";
            txtMatKhau.Text = "";
            txtGhiChu.Text = "";
            txtMaTaiKhoan.Focus();
            BatTat(false);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maTaiKhoan = txtMaTaiKhoan.Text;
            BatTat(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xoá tài khoản " + txtTenDangNhap.Text + " không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM TaiKhoan WHERE MaTaiKhoan = @MaTaiKhoan";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MaTaiKhoan", SqlDbType.NVarChar, 4).Value = txtMaTaiKhoan.Text;
                dtTaiKhoan.Update(cmd);
                // Tải lại form
                TaiKhoan_Load(sender, e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaTaiKhoan.Text == "")
                MessageBox.Show("Mã tài khoản không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (cboMaNV.Text == "")
                MessageBox.Show("Mã nhân viên không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (cboQuyenHan.Text == "")
                MessageBox.Show("Quyền hạn không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtTenDangNhap.Text == "")
                MessageBox.Show("Tên đăng nhập không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtMatKhau.Text == "")
                MessageBox.Show("Mật khẩu không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtGhiChu.Text == "")
                MessageBox.Show("Ghi chú không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {
                    if (maTaiKhoan == "")
                    {
                        string sql = @"INSERT INTO TaiKhoan
                                      VALUES (@MaTaiKhoan, @MaNV,@TenDangNhap,@MatKhau,@QuyenHan,@GhiChu)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MaTaiKhoan", SqlDbType.NChar, 4).Value = txtMaTaiKhoan.Text;
                        cmd.Parameters.Add("@MaNV", SqlDbType.NVarChar, 5).Value = cboMaNV.SelectedValue;
                        cmd.Parameters.Add("@TenDangNhap", SqlDbType.NVarChar, 20).Value = txtTenDangNhap.Text;
                        cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, 100).Value = txtMatKhau.Text;
                        cmd.Parameters.Add("@QuyenHan", SqlDbType.NVarChar, 10).Value = cboQuyenHan.SelectedValue;
                        cmd.Parameters.Add("@GhiChu", SqlDbType.NVarChar).Value = txtGhiChu.Text;
                        dtTaiKhoan.Update(cmd);

                    }
                    else
                    {
                        string sql = @"UPDATE TaiKhoan 
                                       SET  MaTaiKhoan = @MaTaiKhoanCu,
                                            MaNV = @MaNV, 
                                            TenDangNhap = @TenDangNhap,
                                            MatKhau = @MatKhau, 
                                            QuyenHan = @QuyenHan, 
                                            GhiChu = @GhiChu
                                      WHERE MaTaiKhoan = @MaTaiKhoanCu";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MaTaiKhoan", SqlDbType.NChar, 4).Value = txtMaTaiKhoan.Text;
                        cmd.Parameters.Add("@MaTaiKhoanCu", SqlDbType.NChar, 4).Value = maTaiKhoan;
                        cmd.Parameters.Add("@MaNV", SqlDbType.NVarChar, 5).Value = cboMaNV.SelectedValue;
                        cmd.Parameters.Add("@TenDangNhap", SqlDbType.NVarChar, 20).Value = txtTenDangNhap.Text;
                        cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, 100).Value = txtMatKhau.Text;
                        cmd.Parameters.Add("@QuyenHan", SqlDbType.NVarChar, 10).Value = cboQuyenHan.SelectedValue;
                        cmd.Parameters.Add("@GhiChu", SqlDbType.NVarChar).Value = txtGhiChu.Text;
                        dtTaiKhoan.Update(cmd);

                    }
                    // Tải lại form
                    TaiKhoan_Load(sender, e);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu dữ liệu!\nLỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            TaiKhoan_Load(sender, e);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvTaiKhoan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTaiKhoan.Columns[e.ColumnIndex].Name == "MatKhau")
            {
                e.Value = "••••••••••";
            }
        }
    }
}
