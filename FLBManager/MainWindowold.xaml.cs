using FlavourBusinessFacade;
using FLBAuthentication.ViewModel;

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
using WPFUIElementObjectBind;

namespace FLBManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <MetaDataID>{adc03bd4-30b4-4fde-ba4e-7c78c7c8f02a}</MetaDataID>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.GetObjectContext().Initialize(this);
            FlavourBusinessManager = new ViewModel.FlavourBusinessManagerViewModel(MenuDesigner.RestaurantMenusMenu);
            this.GetObjectContext().SetContextInstance(FlavourBusinessManager);

            
            // _SignInUserPopup = new SignInUserPopupViewModel();


        }

        ViewModel.FlavourBusinessManagerViewModel FlavourBusinessManager;

  
    }
}
