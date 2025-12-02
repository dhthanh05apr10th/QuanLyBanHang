using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang.QuanLyChung
{
    public partial class HoaDon : Form
    {
        MyDataTable dataTable = new MyDataTable();
        private DataTable gioHang = new DataTable();

        public HoaDon()
        {
            InitializeComponent();
            dataTable.OpenConnection();
            KhoiTaoGioHang();
        }

        private void KhoiTaoGioHang()
        {
            gioHang.Columns.Add("MaSP", typeof(string));
            gioHang.Columns.Add("TenSP", typeof(string));
            gioHang.Columns.Add("SoLuong", typeof(int));
            gioHang.Columns.Add("DonGiaBan", typeof(decimal));
            gioHang.Columns.Add("GiamGia", typeof(decimal));
            gioHang.Columns.Add("ThanhTien", typeof(decimal), "SoLuong * DonGiaBan - GiamGia");
            
            dgvHoaDon.DataSource = gioHang;
        }

        private string SinhMaHoaDonMoi()
        {
            string sql = "SELECT TOP 1 MaHD FROM HoaDon ORDER BY MaHD DESC";
            SqlCommand cmd = new SqlCommand(sql);

            object maxHD_obj = dataTable.ExecuteScalar(cmd);

            string nextMaHD = "HD001";

            if(maxHD_obj != null && maxHD_obj != DBNull.Value)
            {
                string maxMaHD = maxHD_obj.ToString();
                
                string maSo_str = maxMaHD.Substring(2);

                if(int.TryParse(maSo_str, out int maSo))
                {
                    int newMaSo = maSo + 1;

                    nextMaHD = "HD" + newMaSo.ToString("D3");
                }
            }
            return nextMaHD;
        }

        private void LoadDuLieuChung()
        {
            try
            {
                string sqlSanPham = "SELECT MaSP, TenSp, DonGia From SanPham WHERE TrangThai = 1";
                SqlCommand cmdSanPham = new SqlCommand(sqlSanPham);

                MyDataTable dtSanPham = new MyDataTable();

                dtSanPham.OpenConnection();
                dtSanPham.Fill(cmdSanPham);

                cboMaSP.DataSource = dtSanPham;
                cboMaSP.DisplayMember = "MaSP";
                cboMaSP.ValueMember = "MaSP";
                cboMaSP.SelectedIndex = -1;
            }catch(Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm: " + ex.Message, "Lỗi");
            }

            try
            {
                string sqlKhachHang = "SELECT MaKH, HoTen, SoDienThoai FROM KhachHang ORDER BY MaKH ASC";
                SqlCommand cmdKhachHang = new SqlCommand(sqlKhachHang);

                MyDataTable dtKhachHang = new MyDataTable();

                dtKhachHang.OpenConnection();
                dtKhachHang.Fill(cmdKhachHang);

                cboMaKH.DataSource = dtKhachHang;
                cboMaKH.DisplayMember = "MaKH";
                cboMaKH.ValueMember = "MaKH";
                cboMaKH.SelectedIndex = -1;
            }catch(Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách khách hàng" + ex.Message, "Lỗi");
            }

            try
            {
                string sqlNhanVien = "SELECT MaNV, HoTen FROM NhanVien ORDER BY MaNV ASC";
                SqlCommand cmdNhanVien = new SqlCommand(sqlNhanVien);

                MyDataTable dtNhanVien = new MyDataTable();

                dtNhanVien.OpenConnection();
                dtNhanVien.Fill(cmdNhanVien);

                cboMaNV.DataSource = dtNhanVien;
                cboMaNV.DisplayMember = "MaNV";
                cboMaNV.ValueMember = "MaNV";
                cboMaNV.SelectedIndex = -1;
            }catch(Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên" + ex.Message, "Lỗi");
            }
        }

        private void DatTrangThaiBanDau()
        {
            txtMaHoaDon.Enabled = false;
            txtTenNV.Enabled = false;
            txtTenKH.Enabled = false;
            txtDonGia.Enabled = false;
            txtSDT.Enabled = false;
            txtThanhTien.Enabled = false;
            txtTongTien.Enabled = false;
            txtTenSP.Enabled = false;

            btnLuu.Enabled = true;
            btnHuy.Enabled = false;

            txtSoLuong.Text = "1";
            txtGiamGia.Text = "0";
        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            txtMaHoaDon.Text = SinhMaHoaDonMoi();
            dtpNgayLap.Value = DateTime.Now;

            LoadDuLieuChung();

            DatTrangThaiBanDau();

            txtTongTien.Text = "0";
            gioHang.Clear();
        }

        private void TinhTongTien()
        {
            decimal tong = gioHang.AsEnumerable().Sum(row => row.Field<Decimal>("ThanhTien"));
            txtTongTien.Text = tong.ToString("N0");
        }

        private void TinhThanhTienChiTiet()
        {
            decimal donGia, giamGia, thanhTien;
            int soLuong;

            if (!decimal.TryParse(txtDonGia.Text, out donGia)) donGia = 0;
            if (!int.TryParse(txtSoLuong.Text, out soLuong)) soLuong = 0;
            if (!decimal.TryParse(txtGiamGia.Text, out giamGia)) giamGia = 0;

            thanhTien = (donGia * soLuong) - giamGia;

            txtThanhTien.Text = thanhTien >= 0 ? thanhTien.ToString("N0") : "0";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboMaSP.SelectedValue == null || string.IsNullOrEmpty(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm và nhập số lượng", "Lỗi");
                return;
            }
            string maSP = cboMaSP.SelectedValue.ToString();
            string tenSP = txtTenSP.Text;
            int soLuong;
            decimal donGia, giamGia;

            if (!int.TryParse(txtSoLuong.Text, out soLuong) || soLuong <= 0)
                return;
            if (!decimal.TryParse(txtDonGia.Text, out donGia))
                donGia = 0;
            if (!decimal.TryParse(txtGiamGia.Text, out giamGia))
                giamGia = 0;

            DataRow existingRow = gioHang.AsEnumerable().FirstOrDefault(row => row.Field<string>("MaSP") == maSP);

            if(existingRow != null)
            {
                existingRow["SoLuong"] = existingRow.Field<int>("SoLuong") + soLuong;
                existingRow["GiamGia"] = existingRow.Field<decimal>("GiamGia") + giamGia;
            }
            else
            {
                gioHang.Rows.Add(maSP, tenSP, soLuong, donGia, giamGia);
            }
            gioHang.AcceptChanges();
            TinhTongTien();

            cboMaSP.SelectedIndex = -1;
            txtTenSP.Clear();
            txtDonGia.Text = "0";
            txtSoLuong.Text = "1";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";

            cboMaSP.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if(gioHang.Rows.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm nào trong hóa đơn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maHD = txtMaHoaDon.Text;
            string maNV = cboMaNV.SelectedValue.ToString();

            string maKH = cboMaKH.SelectedValue != null ? cboMaKH.SelectedValue.ToString() : null;

            decimal tongTien = gioHang.AsEnumerable()
                .Sum(row => (row.Field<int?>("SoLuong") ?? 0) * (row.Field<decimal?>("DonGiaBan") ?? 0));

            decimal tongGiamGia = gioHang.AsEnumerable()
                .Sum(row => row.Field<decimal?>("GiamGia") ?? 0);

            SqlConnection connection = null;
            SqlTransaction transaction = null;
            try
            {
                if(dataTable.connection.State != ConnectionState.Open)
                {
                    dataTable.OpenConnection();
                }
                connection = dataTable.connection;
                transaction = connection.BeginTransaction();

                string sqlInsertHD = "INSERT INTO HoaDon (MaHD, MaNV, MaKH, NgayLap, TongTien, GiamGia)" +
                                     "VALUES (@MaHD, @MaNV, @MaKH, @NgayLap, @TongTien, @TongGiamGia)";
                SqlCommand cmd = new SqlCommand(sqlInsertHD, connection, transaction);

                cmd.Parameters.AddWithValue("@MaHD", maHD);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@MaKH", maKH ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayLap", dtpNgayLap.Value);
                cmd.Parameters.AddWithValue("@TongTien", tongTien);
                cmd.Parameters.AddWithValue("@TongGiamGia", tongGiamGia);
                cmd.ExecuteNonQuery();

                string sqlInsertCT = "INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong, DonGiaBan, ThanhTien) " +
                             "VALUES (@MaHD, @MaSP, @SL, @DonGiaBan, @ThanhTien)";

                foreach (DataRow row in gioHang.Rows)
                {
                    SqlCommand cmdCT = new SqlCommand(sqlInsertCT, connection, transaction);
                    cmdCT.Parameters.AddWithValue("@MaHD", maHD);
                    cmdCT.Parameters.AddWithValue("@MaSP", row["MaSP"]);
                    cmdCT.Parameters.AddWithValue("@SL", row["SoLuong"]);
                    cmdCT.Parameters.AddWithValue("@DonGiaBan", row["DonGiaBan"]);
                    cmdCT.Parameters.AddWithValue("@ThanhTien", row["ThanhTien"]);
                    cmdCT.ExecuteNonQuery();
                }
                transaction.Commit();
                MessageBox.Show("Lập hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string maHDVuaLuu = txtMaHoaDon.Text;
                gioHang.Clear();
                HoaDon_Load(null, null);
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            HoaDon_Load(sender, e);
        }

        private DataTable GetHoaDonHeader(string maHD)
        {
            string sql = "SELECT HD.*, NV.HoTen AS TenNV, KH.HoTen AS TenKH, KH.SoDienThoai " +
                         "FROM HoaDon HD " +
                         "JOIN NhanVien NV ON HD.MaNV = NV.MaNV " +
                         "LEFT JOIN KhachHang KH ON HD.MaKH = KH.MaKH " +
                         "WHERE HD.MaHD = @MaHD";

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@MaHD", maHD);

            MyDataTable dtHeader = new MyDataTable();
            dtHeader.OpenConnection();

            cmd.Connection = dtHeader.connection;

            dtHeader.Fill(cmd);

            if (dtHeader.Rows.Count > 0 && !dtHeader.Columns.Contains("ThucThu"))
            {
                dtHeader.Columns.Add("ThucThu", typeof(decimal), "TongTien - GiamGia");
            }
            return dtHeader;
        }

        private DataTable GetHoaDonDetails(string maHD)
        {
            string sql = "SELECT CT.MaSP, SP.TenSP, CT.SoLuong, CT.DonGiaBan, CT.ThanhTien " +
                         "FROM ChiTietHoaDon CT JOIN SanPham SP ON CT.MaSP = SP.MaSP " +
                         "WHERE CT.MaHD = @MaHD";

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@MaHD", maHD);

            MyDataTable dtDetails = new MyDataTable();
            dtDetails.OpenConnection();

            cmd.Connection = dtDetails.connection;

            dtDetails.Fill(cmd);
            return dtDetails;
        }

        private void btnXuatHD_Click(object sender, EventArgs e)
        {
            string maHDCanXuat = txtMaHoaDon.Text;

            if(string.IsNullOrEmpty(maHDCanXuat) || maHDCanXuat.StartsWith("HD_TEMP"))
            {
                MessageBox.Show("Vui lòng lưu hóa đơn trước khi xuất.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DataTable dtHeader = GetHoaDonHeader(maHDCanXuat);
                DataTable dtDetails = GetHoaDonDetails(maHDCanXuat);

                if(dtHeader.Rows.Count > 0 && dtDetails.Rows.Count > 0)
                {
                    BaoCao viewer = new BaoCao(dtHeader, dtDetails);
                    viewer.ShowDialog();

                    MessageBox.Show($"Đã chuẩn bị dữ liệu cho HĐ {maHDCanXuat}. Vui lòng tích hợp Report Viewer.", "Hoàn tất");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu cho hóa đơn này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Lỗi khi chuẩn bị dữ liệu xuất hóa đơn: " + ex.Message, "Lỗi");
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc chắn muốn thoát không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(kq == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void cboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNV.SelectedValue == null || cboMaNV.SelectedIndex == -1)
            {
                txtTenNV.Text = "";
                return;
            }

            try
            {
                DataTable dtSource = (DataTable)cboMaNV.DataSource;

                DataRow selectedRow = dtSource.AsEnumerable()
                    .FirstOrDefault(r => r.Field<string>("MaNV") == cboMaNV.SelectedValue.ToString());

                if (selectedRow != null)
                {
                    txtTenNV.Text = selectedRow["HoTen"].ToString();
                }
            }
            catch (Exception ex)
            {
                txtTenNV.Text = "Lỗi hiển thị tên" + ex.Message;
            }
        }

        private void cboMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaKH.SelectedValue == null || cboMaKH.SelectedIndex == -1)
            {
                txtTenKH.Text = "";
                txtSDT.Text = "";
                return;
            }

            try
            {
                DataTable dtSource = (DataTable)cboMaKH.DataSource;
                string maKH = cboMaKH.SelectedValue.ToString();

                DataRow selectedRow = dtSource.AsEnumerable()
                    .FirstOrDefault(r => r.Field<string>("MaKH") == maKH);

                if (selectedRow != null)
                {
                    txtTenKH.Text = selectedRow["HoTen"].ToString();
                    txtSDT.Text = selectedRow["SoDienThoai"].ToString();
                }
                else
                {
                    txtTenKH.Text = "Không tìm thấy KH";
                    txtSDT.Text = "";
                }
            }
            catch (Exception ex)
            {
                txtTenKH.Text = "Lỗi dữ liệu (Cột không tồn tại)";
                txtSDT.Text = ex.Message;
            }
        }

        private void cboMaSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaSP.SelectedValue == null || cboMaSP.SelectedIndex == -1)
            {
                txtTenSP.Text = "";
                txtDonGia.Text = "0";
                return;
            }

            try
            {
                DataTable dtSource = (DataTable)cboMaSP.DataSource;

                DataRow selectedRow = dtSource.AsEnumerable()
                    .FirstOrDefault(r => r.Field<string>("MaSP") == cboMaSP.SelectedValue.ToString());

                if (selectedRow != null)
                {
                    txtTenSP.Text = selectedRow["TenSP"].ToString();
                    txtDonGia.Text = selectedRow["DonGia"].ToString();

                    txtSoLuong.Text = "1";
                    txtGiamGia.Text = "0";
                }
            }
            catch (Exception ex)
            {
                txtTenSP.Text = "Lỗi dữ liệu" + ex.Message;
            }
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTienChiTiet();
        }

        private void txtDonGia_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTienChiTiet();
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTienChiTiet();
        }
    }
}
