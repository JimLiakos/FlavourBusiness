using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using FinanceFacade;
using GenWebBrowser;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.Views
{
    /// <summary>
    /// Interaction logic for MenuItemWindow.xaml
    /// </summary>
    /// <MetaDataID>{2314c127-1b78-42bd-b246-ecb37b941e8f}</MetaDataID>
    public partial class MenuItemWindow : StyleableWindow.Window
    {


      


     
        public MenuItemWindow()
        { 

            InitializeComponent();
            this.GetObjectContext().Initialize(this);

        }


    }



}
