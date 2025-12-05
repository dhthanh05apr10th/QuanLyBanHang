using QuanLyBanHang.HeThong;
using QuanLyBanHang.NghiepVu;
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
using BC = BCrypt.Net.BCrypt;

namespace QuanLyBanHang.Chung
{
    public partial class Main : Form
    {
        public Main()
        {
            Flash flash = new Flash();
            flash.ShowDialog();
            InitializeComponent();  
        }

        #region Biến toàn cục
        ChucVu chucVu = null;
        DanhMuc danhMuc = null;
        KhachHang KhachHang = null;
        KhoThucPham khoThucPham = null;
        NhanVien nhanVien = null;
        SanPham sanPham = null;
        HoaDon hoaDon = null;
        TaiKhoan taiKhoan = null;
        DangNhap dangNhap = null;
        ThongKeDoanhThu thongKeDoanhThu = null;
        String hoTen = "";

        #endregion

        #region Hệ Thống
        private void mnuDangNhap_Click(object sender, EventArgs e)
        {
             dangNhap = new DangNhap();

        LamLai:
            if (dangNhap.ShowDialog() == DialogResult.OK)
            {
                string tenDangNhap = dangNhap.TenDangNhapValue;
                string matKhau = dangNhap.MatKhauValue;
                if (tenDangNhap == "")
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    goto LamLai;

                }
                else if (matKhau == "")
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    goto LamLai;

                }
                else
                {
                    MyDataTable dataTable = new MyDataTable();
                    dataTable.OpenConnection();

                    string sql = @"SELECT t.*, n.HoTen
                                   FROM TaiKhoan t,NhanVien n
                                   WHERE t.MaNV = n.MaNV AND 
                                         t.TenDangNhap = @Username";
                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 20).Value = tenDangNhap;
                    dataTable.Fill(cmd);

                    if (dataTable.Rows.Count > 0)
                    {
                        hoTen = dataTable.Rows[0]["HoTen"].ToString();
                        string quyenHan = dataTable.Rows[0]["QuyenHan"].ToString();
                        string matKhauMaHoa = dataTable.Rows[0]["MatKhau"].ToString();

                        bool passwordMatches = false;

                        try
                        {
                            if (!string.IsNullOrWhiteSpace(matKhauMaHoa) && matKhauMaHoa.StartsWith("$2"))
                            {
                                passwordMatches = BC.Verify(matKhau, matKhauMaHoa);
                            }
                            else
                            {
                                throw new BCrypt.Net.SaltParseException("Legacy or invalid stored password format");
                            }
                        }
                        catch (BCrypt.Net.SaltParseException)
                        {
                            if (matKhau == matKhauMaHoa)
                            {
                                passwordMatches = true;

                                try
                                {
                                    string newHash = BC.HashPassword(matKhau);
                                    string updateSql = "UPDATE TaiKhoan SET MatKhau = @MatKhau WHERE TenDangNhap = @Username";
                                    SqlCommand updateCmd = new SqlCommand(updateSql);
                                    updateCmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, 100).Value = newHash;
                                    updateCmd.Parameters.Add("@Username", SqlDbType.NVarChar, 20).Value = tenDangNhap;
                                    dataTable.Update(updateCmd);
                                }
                                catch
                                {
                                    // If migration fails, continue login flow but consider logging the issue.
                                }
                            }
                            else
                            {
                                passwordMatches = false;
                            }
                        }
                        catch (Exception)
                        {
                            passwordMatches = false;
                        }

                        // Kiểm tra mật khẩu
                        if (passwordMatches)
                        {
                            if (quyenHan == "Admin")
                                QuanTriVien();
                            else if (quyenHan == "Manager")
                                QuanLy();
                            else if (quyenHan == "Employee")
                                ThanhVien();

                            else
                                ChuaDangNhap();
                        }
                        else
                        {
                            MessageBox.Show("Mật khẩu không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto LamLai;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto LamLai;
                    }
                }
            }
        }
                 
        public void QuanTriVien()
        {
            mnuDangNhap.Enabled = false;
            btnDangNhap.Enabled = false;

            
            btnDangXuat.Enabled = true;
            mnuDangXuat.Enabled = true;
            mnuThongKe.Enabled = true;

            mnuDoanhThu.Enabled = true;
            mnuChucVu.Enabled = true;
            btnChucVu.Enabled = true;
            mnuDanhMuc.Enabled = true;
            btnDanhMuc.Enabled = true;
            mnuKhachHang.Enabled = true;
            btnKhachHang.Enabled = true;
            mnuKho.Enabled = true;
            btnKhoThucPham.Enabled = true;
            mnuNhanVien.Enabled = true;
            btnNhanVien.Enabled = true;
            mnuSanPham.Enabled = true;
            btnSanPham.Enabled = true;
            mnuHoaDon.Enabled = true;
            btnHoaDon.Enabled = true;

            mnuTaiKhoan.Enabled = true;
            btnTaiKhoan.Enabled = true;
            mnuDoanhThu.Enabled = true;

            lblTrangThai.Text = "Quản trị viên: " + hoTen;
        }

        public void QuanLy()
        {
            mnuDangNhap.Enabled = false;
            btnDangNhap.Enabled = false;

            mnuDangXuat.Enabled = true;
            btnDangXuat.Enabled = true;

            mnuDoanhThu.Enabled = true;
            mnuChucVu.Enabled = true;
            btnChucVu.Enabled = true;
            mnuDanhMuc.Enabled = true;
            btnDanhMuc.Enabled = true;
            mnuKhachHang.Enabled = true;
            btnKhachHang.Enabled = true;
            mnuKho.Enabled = true;
            btnKhoThucPham.Enabled = true;
            mnuNhanVien.Enabled = true;
            btnNhanVien.Enabled = true;
            mnuSanPham.Enabled = true;
            btnSanPham.Enabled = true;
            mnuHoaDon.Enabled = true;
            btnHoaDon.Enabled = true;

            mnuTaiKhoan.Enabled = false;
            btnTaiKhoan.Enabled = false;


            lblTrangThai.Text = "Quản Lý: " + hoTen;
        }
        private void ThanhVien()
        {
            mnuDangNhap.Enabled = false;
            btnDangNhap.Enabled = false;

            mnuDangXuat.Enabled = true;
            btnDangXuat.Enabled = true;

            mnuDoanhThu.Enabled = false;
            mnuChucVu.Enabled = false;
            btnChucVu.Enabled = false;
            mnuDanhMuc.Enabled = false;
            btnDanhMuc.Enabled = false;

            mnuKhachHang.Enabled = true;
            btnKhachHang.Enabled = true;
            mnuKho.Enabled = true;
            btnKhoThucPham.Enabled = true;

            mnuNhanVien.Enabled = false;
            btnNhanVien.Enabled = false;
            mnuSanPham.Enabled = false;
            btnSanPham.Enabled = false;

            mnuHoaDon.Enabled = true;
            btnHoaDon.Enabled = true;

            mnuTaiKhoan.Enabled = false;
            btnTaiKhoan.Enabled = false;


            lblTrangThai.Text = "NhanVien: " + hoTen;
        }

        public void ChuaDangNhap()
        {
            mnuDangNhap.Enabled = true;
            btnDangNhap.Enabled = true;

            mnuDangXuat.Enabled = false;
            btnDangXuat.Enabled = false;

            mnuChucVu.Enabled = false;
            btnChucVu.Enabled = false;
            mnuDanhMuc.Enabled = false;
            btnDanhMuc.Enabled = false;
            mnuKhachHang.Enabled = false;
            btnKhachHang.Enabled = false;
            mnuKho.Enabled = false;
            btnKhoThucPham.Enabled = false;
            mnuNhanVien.Enabled = false;
            btnNhanVien.Enabled = false;
            mnuSanPham.Enabled = false;
            btnSanPham.Enabled = false;
            mnuHoaDon.Enabled = false;
            btnHoaDon.Enabled = false;

            mnuTaiKhoan.Enabled = false;
            btnTaiKhoan.Enabled = false;

            lblTrangThai.Text = "Chưa đăng nhập.";
        }
        private void mnuThoat_Click(object sender, EventArgs e)
        {
            ChuaDangNhap();
            foreach (Form Child in this.MdiChildren)
            {
                Child.Close();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Quản Lý Chung
        private void mnuChucVu_Click(object sender, EventArgs e)
        {
            if (chucVu == null || chucVu.IsDisposed)
            {
                chucVu = new ChucVu();
                chucVu.MdiParent = this;
                chucVu.FormClosed += (s, args) => chucVu = null;
                chucVu.Show();
            }
            else
            {
                chucVu.Activate();
            }
        }
        private void Main_Load(object sender, EventArgs e)
        {
            ChuaDangNhap();
            mnuDangNhap_Click(sender, e);
        }

        private void mnuDanhMuc_Click_1(object sender, EventArgs e)
        {
            if (danhMuc == null || danhMuc.IsDisposed)
            {
                danhMuc = new DanhMuc();
                danhMuc.MdiParent = this;
                danhMuc.FormClosed += (s, args) => danhMuc = null;
                danhMuc.Show();
            }
            else
            {
                danhMuc.Activate();
            }
        }

        private void mnuKhachHang_Click_1(object sender, EventArgs e)
        {
            if (KhachHang == null || KhachHang.IsDisposed)
            {
                KhachHang = new KhachHang();
                KhachHang.MdiParent = this;
                KhachHang.FormClosed += (s, args) => KhachHang = null;
                KhachHang.Show();
            }
            else
            {
                KhachHang.Activate();
            }
        }

        private void mnuKho_Click_1(object sender, EventArgs e)
        {
            if (khoThucPham == null || khoThucPham.IsDisposed)
            {
                khoThucPham = new KhoThucPham();
                khoThucPham.MdiParent = this;
                khoThucPham.FormClosed += (s, args) => khoThucPham = null;

                khoThucPham.Show();
            }
            else
            {
                khoThucPham.Activate();
            }
        }

        private void mnuNhanVien_Click_1(object sender, EventArgs e)
        {
            if (nhanVien == null || nhanVien.IsDisposed)
            {
                nhanVien = new NhanVien();
                nhanVien.MdiParent = this;
                nhanVien.FormClosed += (s, args) => nhanVien = null;

                nhanVien.Show();
            }
            else
            {
                nhanVien.Activate();
            }
        }

        private void mnuTaiKhoan_Click_1(object sender, EventArgs e)
        {
            if (taiKhoan == null || taiKhoan.IsDisposed)
            {
                taiKhoan = new TaiKhoan();
                taiKhoan.MdiParent = this;
                taiKhoan.FormClosed += (s, args) => taiKhoan = null;

                taiKhoan.Show();
            }
            else
            {
                taiKhoan.Activate();
            }
        }

        private void mnuSanPham_Click_1(object sender, EventArgs e)
        {
            if (sanPham == null || sanPham.IsDisposed)
            {
                sanPham = new SanPham();
                sanPham.MdiParent = this;
                sanPham.FormClosed += (s, args) => sanPham = null;
                sanPham.Show();
            }
            else
            {
                sanPham.Activate();
            }
        }

        private void mnuHoaDon_Click_1(object sender, EventArgs e)
        {
            if (hoaDon == null || hoaDon.IsDisposed)
            {
                hoaDon = new HoaDon();
                hoaDon.MdiParent = this;
                hoaDon.FormClosed += (s, args) => hoaDon = null;

                hoaDon.Show();
            }
            else
            {
                hoaDon.Activate();
            }
        }
        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            ChuaDangNhap();
            foreach (Form Child in this.MdiChildren)
            {
                Child.Close();
            }
        }
    
       
        #endregion

        #region Thống Kê Doanh Thu
        private void mnuDoanhThu_Click(object sender, EventArgs e)
        {
            if (thongKeDoanhThu == null || thongKeDoanhThu.IsDisposed)
            {
                thongKeDoanhThu = new ThongKeDoanhThu();
                thongKeDoanhThu.MdiParent = this;
                thongKeDoanhThu.FormClosed += (s, args) => thongKeDoanhThu = null;
                thongKeDoanhThu.Show();
            }
            else
            {
                thongKeDoanhThu.Activate();
            }
        }
        #endregion

        #region Trợ Giúp
       private void mnuHuongDan_Click(object sender, EventArgs e)
        {
            HuongDanSuDung huongDanSu = new HuongDanSuDung();
            huongDanSu.ShowDialog();
        }        
        private void mnuHoTro_Click(object sender, EventArgs e)
        {
            Support hoTro = new Support();
            hoTro.ShowDialog();
        }     

        #endregion

        #region Thông tin phân mềm
        private void mnuThongTin_Click(object sender, EventArgs e)
        {
            AboutBox thongTinPhanMem = new AboutBox();
            thongTinPhanMem.ShowDialog();

        }
        #endregion

        #region Các nút trên thanh công cụ

        private void btnDangNhap_Click_1(object sender, EventArgs e)
        {
            mnuDangNhap_Click(sender, e);
        }

        private void btnDangXuat_Click_1(object sender, EventArgs e)
        {
            mnuDangXuat_Click(sender, e);
        }

        private void btnChucVu_Click_1(object sender, EventArgs e)
        {
            mnuChucVu_Click(sender, e);
        }

        private void btnDanhMuc_Click_1(object sender, EventArgs e)
        {
            mnuDanhMuc_Click_1(sender, e);
        }

        private void btnKhachHang_Click_1(object sender, EventArgs e)
        {
            mnuKhachHang_Click_1(sender, e);
        }

        private void btnKhoThucPham_Click_1(object sender, EventArgs e)
        {
            mnuKho_Click_1(sender, e);
        }

        private void btnNhanVien_Click_1(object sender, EventArgs e)
        {
            mnuNhanVien_Click_1(sender, e);
        }

        private void btnTaiKhoan_Click_1(object sender, EventArgs e)
        {
            mnuTaiKhoan_Click_1(sender, e);
        }

        private void btnSanPham_Click_1(object sender, EventArgs e)
        {
            mnuSanPham_Click_1(sender, e);
        }

        private void btnHoaDon_Click_1(object sender, EventArgs e)
        {
            mnuHoaDon_Click_1(sender, e);
        }
        #endregion

        
    }
}

