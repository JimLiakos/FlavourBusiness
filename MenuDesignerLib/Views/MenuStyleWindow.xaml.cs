using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MenuDesigner.Views
{
    /// <summary>
    /// Interaction logic for MenuStyleWindow.xaml
    /// </summary>
    /// <MetaDataID>{9b594180-fa5c-44be-8ada-91561b6e6feb}</MetaDataID>
    public partial class MenuStyleWindow : StyleableWindow.Window, System.ComponentModel.INotifyPropertyChanged
    {
        public MenuStyleWindow()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
