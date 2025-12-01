using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang.Chung
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void hướngdẫnSửDungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Dùng \n để xuống dòng
            // Dùng \t để thụt đầu dòng (tạo tab)
            string noiDung = "";
            noiDung += "HƯỚNG DẪN SỬ DỤNG PHẦN MỀM\n";
            noiDung += "--------------------------------------------------\n\n";

            noiDung += "1. Quản lý Điểm:\n";
            noiDung += "\t- Thêm: Nhấn nút 'Thêm', nhập đầy đủ thông tin và nhấn 'Lưu'.\n";
            noiDung += "\t- Sửa: Chọn 1 dòng điểm, nhấn 'Sửa', thay đổi điểm và nhấn 'Lưu'.\n";
            noiDung += "\t- Xóa: Chọn 1 dòng điểm và nhấn 'Xóa'.\n\n";

            noiDung += "2. Tìm kiếm & Thống kê:\n";
            noiDung += "\t- Bấm nút kính lúp để hiện ô tìm kiếm theo tên.\n";
            noiDung += "\t- Bấm 'Thống kê TB' để xem các chỉ số của dữ liệu đang hiển thị.";

            // Hiển thị MessageBox
            MessageBox.Show(noiDung, "Hướng dẫn sử dụng", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
