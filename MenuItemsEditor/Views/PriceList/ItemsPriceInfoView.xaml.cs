using MenuItemsEditor.ViewModel.PriceList;
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

namespace MenuItemsEditor.Views.PriceList
{
    /// <summary>
    /// Interaction logic for ItemsPriceInfoView.xaml
    /// </summary>
    public partial class ItemsPriceInfoView : UserControl
    {
        public ItemsPriceInfoView()
        {
            InitializeComponent();

            Unloaded += ItemsPriceInfoView_Unloaded;
            Loaded += ItemsPriceInfoView_Loaded;


        }

        private void ItemsPriceInfoView_Loaded(object sender, RoutedEventArgs e)
        {
            var itemsPreparationInfo = this.GetDataContextObject<ItemsPriceInfoPresentation>();
            if (itemsPreparationInfo != null && itemsPreparationInfo.Edit)
                PreviewMouseDown += GlobalPreviewMouseDown;
        }

        private void ItemsPriceInfoView_Unloaded(object sender, RoutedEventArgs e)
        {
            PreviewMouseDown -= GlobalPreviewMouseDown;
        }

        private void GlobalPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var po = PointToScreen(new Point(0, 0));
            var mousepos = Mouse.GetPosition(Grid);
            if (mousepos.X < 0 || mousepos.Y < 0 || mousepos.X > ActualWidth || mousepos.Y > ActualHeight)
            {


                PreviewMouseDown -= GlobalPreviewMouseDown;
                var itemsPreparationInfo = this.GetDataContextObject<ItemsPriceInfoPresentation>();
                if (itemsPreparationInfo != null&&!itemsPreparationInfo.PriceOverrideTypesPopupOpen)
                {

                    foreach (var unitTextBox in WPFUIElementObjectBind.ObjectContext.FindChilds<TextBoxNumberWithUnit>(Content as DependencyObject))
                        unitTextBox.Focus();
                    foreach (var checkBox in WPFUIElementObjectBind.ObjectContext.FindChilds<CheckBox>(Content as DependencyObject))
                        checkBox.Focus();
                    Keyboard.ClearFocus();

                    Task.Run(() =>
                    {
                        System.Threading.Thread.Sleep(300);
                        this.Dispatcher.Invoke(new Action(() => { itemsPreparationInfo.Edit = false; }));
                    });

                }
            }
        }
        static void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _PreviewMouseDown?.Invoke(sender, e);
        }




        delegate void PreviewMouseDownHandle(object sender, MouseButtonEventArgs e);

        static event PreviewMouseDownHandle _PreviewMouseDown;
        /// <MetaDataID>{1f884576-3cd3-4060-8381-5ecd5a4cabe7}</MetaDataID>
        static bool PreviewMouseDownIntitialized;

        private static event PreviewMouseDownHandle PreviewMouseDown
        {
            add
            {
                if (!PreviewMouseDownIntitialized)
                {
                    PreviewMouseDownIntitialized = true;
                    EventManager.RegisterClassHandler(typeof(UIElement), Window.PreviewMouseDownEvent, new MouseButtonEventHandler(OnPreviewMouseDown));
                }
                _PreviewMouseDown += value;
            }
            remove
            {
                _PreviewMouseDown -= value;
            }
        }


        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ClickCount == 2)
            {
                this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                {

                    PreviewMouseDown += GlobalPreviewMouseDown;
                    var itemsPreparationInfo = this.GetDataContextObject<ItemsPriceInfoPresentation>();
                    if (itemsPreparationInfo != null)
                    {
                        if (itemsPreparationInfo.DefinesNewPrice)
                            itemsPreparationInfo.Edit = true;
                        e.Handled = true;
                    }
                }));
            }
        }

        private void PriceOverrideTypeMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {
                var itemsPreparationInfo = this.GetDataContextObject<ItemsPriceInfoPresentation>();

                if (itemsPreparationInfo != null)
                {
                    itemsPreparationInfo.ToggleDiscoundType();
                    //e.Handled = true;
                }
            }));
        }
    }
}
