using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumAnalyzerGUI
{
    public partial class ReportForm : Form
    {
        public ReportForm(DataSet1 ds, string deviceName, int passCount, int failCount, double percentagepass)
        {
            InitializeComponent();

            var report = new CrystalReport1();
            report.SetDataSource(ds);

            report.SetParameterValue("DeviceName", deviceName);
            report.SetParameterValue("PrintDate", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));
            report.SetParameterValue("PassCount", passCount); 
            report.SetParameterValue("FailCount", failCount);
            report.SetParameterValue("TotalCount", passCount + failCount);
            report.SetParameterValue("PercentagePass", percentagepass.ToString("F2") + "%");

            crystalReportViewer1.ReportSource = report;
        }
    }
}