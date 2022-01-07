using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace DXReportPringTest
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

        private void PrintTest_Click(object sender, RoutedEventArgs e)
        {


            //var devExpressFiles = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("DevExpress.*.dll").ToArray();


            //var modulesPath = Directory.GetCurrentDirectory() + "\\modules";
            //if (!Directory.Exists(modulesPath))
            //    Directory.CreateDirectory(modulesPath);

            //foreach (var devXpresDll in devExpressFiles)
            //{
            //    File.Copy(devXpresDll.FullName, modulesPath + "\\" + devXpresDll.Name,true );

            //    if (System.Reflection.AssemblyName.GetAssemblyName(modulesPath + "\\" + devXpresDll.Name).ProcessorArchitecture != ProcessorArchitecture.MSIL)
            //        return;

            //    AssemblyNativeCode.AssemblyLoader.MakeNadive(modulesPath + "\\" + devXpresDll.Name);
            //}

            

            MemoryStream memoryStream = new MemoryStream(Properties.Resources.InvoiceReport);
            var report = DXConnectableControls.XtraReports.UI.Report.FromStream(memoryStream, true);
            report.DataSource = new List<Invoice>() { new Invoice() };
            report.CreateDocument();
            report.ShowPreview();

            //DXConnectableControls.XtraReports.Design.ReportDesignForm reportDesignForm = new DXConnectableControls.XtraReports.Design.ReportDesignForm();
            //reportDesignForm.OpenReport(@"F:\myproject\terpo\OpenVersions\FlavourBusiness\DXReportPringTest\InvoiceReport.repx");
            //reportDesignForm.Report.DataSource = new List<Invoice>() { new Invoice() };
            //reportDesignForm.ShowDialog();


            //DevExpress.Utils.v9.3.dll
            //DevExpress.XtraPrinting.v9.3.dll
            //DevExpress.XtraCharts.v9.3.dll
            //DevExpress.Charts.v9.3.Core.dll
            //DevExpress.XtraPivotGrid.v9.3.Core.dll
            //DevExpress.XtraPivotGrid.v9.3.dll
            //DevExpress.XtraEditors.v9.3.dll
            //DevExpress.XtraBars.v9.3.dll
            //DevExpress.XtraTreeList.v9.3.dll


        }
    }
}
