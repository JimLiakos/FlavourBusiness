
using DXConnectableControls.XtraReports.UI;
using PdfiumViewer;
using System.Collections.Generic;
using System.Drawing.Printing;
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
            var report = Report.FromStream(memoryStream, true);
            report.DataSource = new List<Invoice>() { new Invoice() };
            report.CreateDocument();
            using (MemoryStream ms = new MemoryStream())
            {
                report.ExportToPdf(ms);
                ms.Position = 0;
                var printerSettings = new PrinterSettings
                {
                    PrinterName = "Microsoft Print to PDF",
                    Copies = 1
                };
              var sds=  PrinterSettings.InstalledPrinters.OfType<string>().ToList();
                // Create our page settings for the paper size selected
                var pageSettings = new PageSettings(printerSettings)
                {
                    Margins = new Margins(0, 0, 0, 0),
                };
                //foreach (PaperSize paperSize in printerSettings.PaperSizes)
                //{
                //    if (paperSize.PaperName == "A4")
                //    {
                //        pageSettings.PaperSize = paperSize;
                //        break;
                //    }
                //}

                // Now print the PDF document
                using (var document = PdfDocument.Load(ms))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings = pageSettings;
                        printDocument.PrintController = new StandardPrintController();
                        printDocument.Print();
                    }
                }
            }
            //report.ShowPreview();

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
