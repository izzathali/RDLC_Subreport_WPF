using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ubiety.Dns.Core;

namespace NewSubreportWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ShowReport();
        }
        private void ShowReport()
        {
            DataTable dtOrders = MainReport(Convert.ToInt32(txtSearch.Text));
            ReportViewer viewer = new ReportViewer();
            viewer.Reset();
            viewer.LocalReport.ReportPath = @"F:\Upwork\Backup 21.01.09\RDLC_Subreport_WPF\NewSubreportWPF\MainReport1.rdlc";
            ReportDataSource ds = new ReportDataSource("DataSet1", dtOrders);
            viewer.LocalReport.DataSources.Add(ds);
            viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(OrderDetailsSubreportProcessing);
            viewer.LocalReport.Refresh();

            string[] streamIds;
            Warning[] warnings;

            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = @"E:\Report.pdf";
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            File.WriteAllBytes(fileName, bytes);
            System.Diagnostics.Process.Start(fileName);
        }
        private DataTable MainReport(int OrderID)
        {
            /*
            string connStr = @"Data Source=IZZATH-BIN-IBRA\SQLEXPRESS2019;Initial Catalog=POS1;Integrated Security=True;Pooling=False";
            SqlConnection cn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Order] WHERE OrderId ='" + OrderID + "'", cn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);

            string[] streamIds;
            Warning[] warnings;

            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = @"F:\Upwork\NewSubreportWPF\NewSubreportWPF\MainReport1.rdlc";
            viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            //Sub
            viewer.LocalReport.SubreportProcessing+=new SubreportProcessingEventHandler(OrderDetailsSubreportProcessing);
            viewer.LocalReport.Refresh();

            string fileName = @"E:\MainReport1.pdf";
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            File.WriteAllBytes(fileName, bytes);
            System.Diagnostics.Process.Start(fileName);
            */
            DataTable dt = new DataTable();
            string conStr = @"Data Source=IZZATH-BIN-IBRA\SQLEXPRESS2019;Initial Catalog=POS1;Integrated Security=True;Pooling=False";
            using (SqlConnection cn = new SqlConnection(conStr))
            {
                string query = @"SELECT * FROM [dbo].[Order] WHERE OrderId ='" + OrderID + "'";
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }

            return dt;

        }
        void OrderDetailsSubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            int orderID = int.Parse(e.Parameters["OrderId"].Values[0].ToString());
            DataTable dtOrderDetails = Subreport(orderID);
            ReportDataSource ds = new ReportDataSource("DataSet1", dtOrderDetails);
            e.DataSources.Add(ds);
        }
        private DataTable Subreport(int OrderID)
        {
            /*
            string connStr = @"Data Source=IZZATH-BIN-IBRA\SQLEXPRESS2019;Initial Catalog=POS1;Integrated Security=True;Pooling=False";
            SqlConnection cn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[OrderDetail] WHERE OrderId ='" + OrderID + "'", cn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);

            string[] streamIds;
            Warning[] warnings;

            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = @"F:\Upwork\NewSubreportWPF\NewSubreportWPF\SubReport.rdlc";
            viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dt));
            viewer.LocalReport.Refresh();

            string fileName = @"E:\SubReport.pdf";
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            File.WriteAllBytes(fileName, bytes);
            System.Diagnostics.Process.Start(fileName);
            */
            DataTable dt = new DataTable();
            string conStr = @"Data Source=IZZATH-BIN-IBRA\SQLEXPRESS2019;Initial Catalog=POS1;Integrated Security=True;Pooling=False";
            using (SqlConnection cn = new SqlConnection(conStr))
            {
                string query = @"SELECT * FROM [dbo].[OrderDetail] WHERE OrderId ='" + OrderID + "'";
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }

            return dt;
        }
        
    }
}
