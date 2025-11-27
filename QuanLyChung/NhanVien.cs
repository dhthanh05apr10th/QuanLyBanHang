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
    public partial class NhanVien : Form
    {
        MyDataTable dtNhanVien = new MyDataTable();
        string maNV = "";
        public NhanVien()
        {
            InitializeComponent();
            dtNhanVien.OpenConnection();
        }

        private void LoadData()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT n.*, cv.TenCV
                                             FROM NhanVien n, ChucVu cv
                                             WHERE n.MaCV = cv.MaCV");
            dtNhanVien.Fill(cmd);
            // Lấy dữ liệu vào cboQueQuan
            MyDataTable ChucVu = new MyDataTable();
            ChucVu.OpenConnection();
            string ChucVusql = "SELECT * FROM ChucVu";
            SqlCommand queQuanCmd = new SqlCommand(ChucVusql);
            ChucVu.Fill(queQuanCmd);
            cboChucVu.DataSource = ChucVu;
            cboChucVu.DisplayMember = "TenCV";
            cboChucVu.ValueMember = "MaCV";

            BindingSource binding = new BindingSource();
            binding.DataSource = dtNhanVien;
            // Hiển thị dữ liệu vào DataGridView
            dgvNhanVien.DataSource = binding;
            // Liên kết dữ liệu từ DataGridView lên các control
            txtMaNV.DataBindings.Clear();
            txtHovaTen.DataBindings.Clear();
            txtDiaChi.DataBindings.Clear();
            txtSoDT.DataBindings.Clear();
            dtpNgaySinh.DataBindings.Clear();
            cboChucVu.DataBindings.Clear();


            txtMaNV.DataBindings.Add("Text", binding, "MaNV");
            txtHovaTen.DataBindings.Add("Text", binding, "HoTen");
            txtDiaChi.DataBindings.Add("Text", binding, "DiaChi");
            txtSoDT.DataBindings.Add("Text", binding, "SoDienThoai");
            dtpNgaySinh.DataBindings.Add("Value", binding, "NgaySinh");
            cboChucVu.DataBindings.Add("SelectedValue", binding, "MaCV");
        }

        private void BatTat(bool b)
        {
            btnThem.Enabled = b;
            btnSua.Enabled = b;
            btnXoa.Enabled = b;
            btnLuu.Enabled = !b;
            btnHuy.Enabled = !b;


            txtMaNV.Enabled = !b;
            txtHovaTen.Enabled = !b;
            txtDiaChi.Enabled = !b;
            txtSoDT.Enabled = !b;
            cboChucVu.Enabled = !b;
            dtpNgaySinh.Enabled = !b;
            rdNam.Enabled = !b;
            rdNu.Enabled = !b;
        }

        private void NhanVien_Load(object sender, EventArgs e)
        {
            dgvNhanVien.AutoGenerateColumns = false;

            LoadData();
            BatTat(true);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Đánh dấu là Thêm mới
            maNV = "";
            // Xóa trắng các trường
            txtMaNV.Clear();
            txtHovaTen.Clear();
            txtDiaChi.Clear();
            txtSoDT.Clear();
            dtpNgaySinh.Value = DateTime.Today;
            cboChucVu.Text = "";
            rdNam.Checked = false;
            rdNu.Checked = false;
            // Bật/Tắt các controls
            BatTat(false);

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Đánh dấu là Cập nhật
            maNV = txtMaNV.Text;
            // Bật/Tắt các controls
            BatTat(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xoá học sinh " + txtHovaTen.Text + " không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM NhanVien WHERE MaNV = @MaNV";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MaNV", SqlDbType.NVarChar, 5).Value = txtMaNV.Text;
                dtNhanVien.Update(cmd);
                // Tải lại form
                NhanVien_Load(sender, e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var d = DateTime.Now;
            // Kiểm tra dữ liệu nhập
            if (txtMaNV.Text.Trim() == "")
                MessageBox.Show("Mã nhân viên không được rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtHovaTen.Text.Trim() == "")
                MessageBox.Show("Họ và tên không được rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtDiaChi.Text.Trim() == "")
                MessageBox.Show("Địa chỉ không được rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtSoDT.Text.Trim() == "")
                MessageBox.Show("Số điện thoại không được rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (cboChucVu.Text.Trim() == "")
                MessageBox.Show("Chức vụ không được rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (rdNam.Checked == false && rdNu.Checked == false)
                MessageBox.Show("Giới Tính không được rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (dtpNgaySinh.Value.Date == d.Date)
                MessageBox.Show("Chưa chọn ngày sinh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {
                    if(maNV == "") // Thêm mới
                    {
                        string sql = @"INSERT INTO NhanVien
                            VALUES (@MaNV, @HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @MaCV)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MaNV", SqlDbType.NChar,5).Value= txtMaNV.Text;
                        cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 50).Value= txtHovaTen.Text;
                        cmd.Parameters.Add("@NgaySinh", SqlDbType.Date).Value= dtpNgaySinh.Value;
                        cmd.Parameters.Add("@GioiTinh", SqlDbType.Bit).Value= rdNam.Checked ? 0 : 1;
                        cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value= txtDiaChi.Text;
                        cmd.Parameters.Add("@SoDienThoai", SqlDbType.NChar, 15).Value= txtSoDT.Text;
                        cmd.Parameters.Add("@MaCV", SqlDbType.NChar, 100).Value= cboChucVu.SelectedValue;
                        dtNhanVien.Update(cmd);
                    }
                    else // Cập nhật
                    {
                        string sql = @"UPDATE NhanVien
                            SET MaNV = @MaNVmoi,
                                HoTen = @HoTen,
                                NgaySinh = @NgaySinh,
                                GioiTinh = @GioiTinh,
                                DiaChi = @DiaChi,
                                SoDienThoai = @SoDienThoai,
                                MaCV = @MaCV
                            WHERE MaNV = @MaNVcu";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MaNVMoi", SqlDbType.NChar, 5).Value = txtMaNV.Text;
                        cmd.Parameters.Add("@MaNVCu", SqlDbType.NChar, 5).Value = maNV;
                        cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 50).Value = txtHovaTen.Text;
                        cmd.Parameters.Add("@NgaySinh", SqlDbType.Date).Value = dtpNgaySinh.Value;
                        cmd.Parameters.Add("@GioiTinh", SqlDbType.Bit).Value = rdNam.Checked ? 0 : 1;
                        cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = txtDiaChi.Text;
                        cmd.Parameters.Add("@SoDienThoai", SqlDbType.NChar, 15).Value = txtSoDT.Text;
                        cmd.Parameters.Add("@MaCV", SqlDbType.NChar, 100).Value = cboChucVu.SelectedValue;
                        dtNhanVien.Update(cmd);

                    }
                    // Tải lại dữ liệu
                    NhanVien_Load(sender, e);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            // Hủy thao tác
            NhanVien_Load(sender, e);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvNhanVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (dgvNhanVien.Columns[e.ColumnIndex].Name == "GioiTinh")
            {
                e.Value = (byte)e.Value == 0 ? "Nam" : "Nữ";
            }

        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy hàng hiện tại
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

                string gioiTinhValue = row.Cells["GioiTinh"].Value.ToString();

                    // Logic so sánh: 0 là Nam, 1 là Nữ (hoặc True/False)
                    if (gioiTinhValue == "0" ) // Nam
                    {
                        rdNam.Checked = true;
                        rdNu.Checked = false; 
                    }
                    else
                    {
                        rdNu.Checked = true;
                        rdNam.Checked = false;
                    }
                
            }
        }
    }
}
