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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpVectors.Converters;
using WPFUIElementObjectBind;

namespace FloorLayoutDesigner
{
    /// <summary>
    /// Interaction logic for SvgHostControl.xaml
    /// </summary>
    /// <MetaDataID>{78af34aa-bab6-4f81-9999-3f35d9947c09}</MetaDataID>
    public partial class SvgHostControl : UserControl
    {
        public SvgHostControl()
        {
            InitializeComponent();

            
        }

        public SvgViewbox svgHost
        {
            get
            {
                return ViewControlObject.FindVisualChildren<SvgViewbox>(this.Content as DependencyObject).FirstOrDefault();
            }
        }
    }
}
