using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FloorLayoutDesigner.Views
{
    /// <summary>
    /// Interaction logic for ShapeLabelBackgroundWindow.xaml
    /// </summary>
    /// <MetaDataID>{aceb5609-c4fd-4045-9b01-4a8d15bd6582}</MetaDataID>
    public partial class ShapeLabelBackgroundWindow : StyleableWindow.Window
    {
        public ShapeLabelBackgroundWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var ss = LabelCtrl.ActualHeight;
            var sas = this.GetDataContextObject<ViewModel.HallLayoutViewModel>().FontPresantation.Font.Html5MeasureText("Label");
            var baseLine = this.GetDataContextObject<ViewModel.HallLayoutViewModel>().FontPresantation.Font.GetHtml5TextBaseLine("Label");
            var medline = this.GetDataContextObject<ViewModel.HallLayoutViewModel>().FontPresantation.Font.GetTextMedline("Label");
            var capsLine = this.GetDataContextObject<ViewModel.HallLayoutViewModel>().FontPresantation.Font.GetTextCapsLine("Label");
            var descentLine = this.GetDataContextObject<ViewModel.HallLayoutViewModel>().FontPresantation.Font.GetTextDescenLine("Label");
            LabelCtrl.PointToScreen(new Point(0, 0));


        }
    }
}
