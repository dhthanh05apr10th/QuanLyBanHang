using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace QuanLyBanHang.QuanLyChung
{
    public partial class BaoCao : Form
    {
        public BaoCao(DataTable dtHeader, DataTable dtDetails)
        {
            InitializeComponent();
            LoadReportData(dtHeader, dtDetails);
        }

        private void BaoCao_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private void LoadReportData(DataTable dtHeader, DataTable dtDetails)
        {
            try
            {
                reportViewer1.LocalReport.DataSources.Clear();

                ReportDataSource rdsHeader = new ReportDataSource("dsHeader", dtHeader);
                reportViewer1.LocalReport.DataSources.Add(rdsHeader);

                ReportDataSource rdsDetails = new ReportDataSource("ChiTietHoaDon", dtDetails);
                reportViewer1.LocalReport.DataSources.Add(rdsDetails);

                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo: " + ex.Message, "Lỗi RDLC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
