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

namespace QuanLyBanHang.NghiepVu
{
    public partial class ThongKeDoanhThu : Form
    {
        MyDataTable dataTable = new MyDataTable();

        public ThongKeDoanhThu()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        private void ThongKeDoanhThu_Load(object sender, EventArgs e)
        {
            dtpNgayBatDau.Value = DateTime.Now.AddMonths(-1);
            dtpNgayKetThuc.Value = DateTime.Now;


        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpNgayBatDau.Value.Date;

            DateTime denNgay = dtpNgayKetThuc.Value.Date.AddDays(1).AddSeconds(-1);

            string sql = @"SELECT NV.MaNV, NV.HoTen,
                    COUNT(HD.MaHD) AS SoLuongHoaDon, 
                    SUM(HD.TongTien - HD.GiamGia) AS DoanhThu
                FROM HoaDon HD
                JOIN NhanVien NV ON HD.MaNV = NV.MaNV
                WHERE HD.NgayLap >= @TuNgay AND HD.NgayLap <= @DenNgay
                GROUP BY NV.MaNV, NV.HoTen
                ORDER BY DoanhThu DESC";

            try
            {
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                MyDataTable dtBaoCao = new MyDataTable();
                dtBaoCao.OpenConnection();
                dtBaoCao.Fill(cmd);

                dgvTongHop.DataSource = dtBaoCao;

                TinhTongDoanhThuChung(dtBaoCao);

                dgvChiTietHD.DataSource = null; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TinhTongDoanhThuChung(DataTable dtBaoCao)
        {
            decimal tongDoanhThu = dtBaoCao.AsEnumerable()
                .Sum(row => row.Field<decimal?>("DoanhThu") ?? 0);

            txtTongdoanhThu.Text = tongDoanhThu.ToString("N0") + " VNĐ";
        }

        private void dgvTongHop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string maNV = dgvTongHop.Rows[e.RowIndex].Cells["MaNV"].Value.ToString();
            DateTime tuNgay = dtpNgayBatDau.Value.Date;
            DateTime denNgay = dtpNgayKetThuc.Value.Date.AddDays(1).AddSeconds(-1);


            string sql = @"SELECT CTHD.MaHD, CTHD.MaSP,CTHD. SoLuong, CTHD.DonGiaBan, CTHD.ThanhTien
                            FROM ChiTietHoaDon CTHD
                            JOIN HoaDon HD ON CTHD.MaHD = HD.MaHD
                            WHERE HD.MaNV = @MaNV AND HD.NgayLap >= @TuNgay AND HD.NgayLap <= @DenNgay
                            ORDER BY HD.NgayLap DESC, CTHD.MaHD, CTHD.MaSP";

            try
            {
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                MyDataTable dtChiTiet = new MyDataTable();
                dtChiTiet.OpenConnection();
                dtChiTiet.Fill(cmd);

                dgvChiTietHD.DataSource = dtChiTiet;

                if (dgvChiTietHD.Columns.Contains("MaHD")) dgvChiTietHD.Columns["MaHD"].HeaderText = "Mã hóa đơn";
                if (dgvChiTietHD.Columns.Contains("MaSP")) dgvChiTietHD.Columns["MaSP"].HeaderText = "Mã sản phẩm";
                if (dgvChiTietHD.Columns.Contains("SoLuong")) dgvChiTietHD.Columns["SoLuong"].HeaderText = "Số lượng";
                if (dgvChiTietHD.Columns.Contains("DonGiaBan"))
                {
                    dgvChiTietHD.Columns["DonGiaBan"].HeaderText = "Đơn giá bán";
                    dgvChiTietHD.Columns["DonGiaBan"].DefaultCellStyle.Format = "N0";
                }
                if (dgvChiTietHD.Columns.Contains("ThanhTien"))
                {
                    dgvChiTietHD.Columns["ThanhTien"].HeaderText = "Thành tiền";
                    dgvChiTietHD.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            // 1. Làm trống các DataGridViews
            dgvTongHop.DataSource = null;
            dgvChiTietHD.DataSource = null;

            // 2. Làm trống ô tổng doanh thu
            txtTongdoanhThu.Text = string.Empty;

            // 3. Reset ngày về giá trị mặc định ban đầu (giống hàm Load)
            dtpNgayBatDau.Value = DateTime.Now.AddMonths(-1);
            dtpNgayKetThuc.Value = DateTime.Now;
        }
    }
}
