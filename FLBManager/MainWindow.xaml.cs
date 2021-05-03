using FLBManager.ViewModel;
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

namespace FLBManager
{
    /// <summary>
    /// Interaction logic for WindowTest.xaml
    /// </summary>
    /// <MetaDataID>{b2f36fef-c260-4c4e-ba32-9605e51ef039}</MetaDataID>
    public partial class MainWindow : Window
    {
        private FlavourBusinessManagerViewModel FlavourBusinessManager;

        public MainWindow()
        {
            InitializeComponent();
            this.GetObjectContext().Initialize(this);
            FlavourBusinessManager = new ViewModel.FlavourBusinessManagerViewModel(MenuDesigner.Views.MenuDesignerControl.RestaurantMenusMenu);
            this.GetObjectContext().SetContextInstance(FlavourBusinessManager);

        }
    }
}
