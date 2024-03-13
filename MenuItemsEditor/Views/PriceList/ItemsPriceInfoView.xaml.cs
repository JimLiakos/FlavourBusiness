using FLBManager.ViewModel;
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

        }



        bool inPreviewMouseDown = false;

        private void GlobalPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (inPreviewMouseDown)
                    return;
                inPreviewMouseDown = true;
                FBResourceTreeNode itemsPreparationInfo = this.GetDataContextObject<FBResourceTreeNode>();
                if (!itemsPreparationInfo.Edit)
                    return;

                var po = PointToScreen(new Point(0, 0));
                var mousepos = Mouse.GetPosition(Grid);
                if (mousepos.X < 0 || mousepos.Y < 0 || mousepos.X > ActualWidth || mousepos.Y > ActualHeight)
                {

                    if (itemsPreparationInfo != null && (itemsPreparationInfo as ItemsPriceInfoPresentation)?.PriceOverrideTypesPopupOpen!=true)
                    {
                        Keyboard.ClearFocus();
                        foreach (var unitTextBox in WPFUIElementObjectBind.ObjectContext.FindChilds<TextBoxNumberWithUnit>(Content as DependencyObject))
                            unitTextBox.Focus();
                        foreach (var checkBox in WPFUIElementObjectBind.ObjectContext.FindChilds<CheckBox>(Content as DependencyObject))
                            checkBox.Focus();
                        foreach (var contextPriceTextBox in WPFUIElementObjectBind.ObjectContext.FindChilds<ContextPriceTextBox>(Content as DependencyObject))
                            contextPriceTextBox.UpdateProperty();

                    
                        foreach (var button in WPFUIElementObjectBind.ObjectContext.FindChilds<Button>(Content as DependencyObject))
                        {
                            button.Focus();
                            itemsPreparationInfo.Edit = false;
                        }


                    }
                }
            }
            catch (Exception error)
            {


                var itemsPreparationInfo = this.GetDataContextObject<ItemsPriceInfoPresentation>();
                if (itemsPreparationInfo != null)
                {

                    Task.Run(() =>
                    {
                        System.Threading.Thread.Sleep(300);
                        this.Dispatcher.Invoke(new Action(() => { itemsPreparationInfo.Edit = false; }));
                    });

                }
            }
            finally
            {
                inPreviewMouseDown = false;
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
                this.GetObjectContext()?.RunUnderContextTransaction(new Action(() =>
                {

                    //PreviewMouseDown += GlobalPreviewMouseDown;
                    var itemsPreparationInfo = this.GetDataContextObject<ItemsPriceInfoPresentation>();
                    if (itemsPreparationInfo != null)
                    {

                        if (itemsPreparationInfo.DefinesNewPrice)
                            itemsPreparationInfo.Edit = true;
                        else
                        {
                            itemsPreparationInfo.DefinesNewPrice=true;
                            itemsPreparationInfo.Edit = true;
                        }
                    }
                    else
                    {
                        var itemSelectorPriceInfo = this.GetDataContextObject<ItemSelectorPriceInfoPresetation>();
                        if (itemSelectorPriceInfo != null)
                        {

                            if (itemSelectorPriceInfo.ItemPriceInfoPresentation.DefinesNewPrice)
                                itemSelectorPriceInfo.Edit = true;
                            else
                            {
                                itemSelectorPriceInfo.ItemPriceInfoPresentation.DefinesNewPrice = true;
                                itemSelectorPriceInfo.Edit = true;
                            }

                        }

                    }
                    
                }));
                e.Handled = true;
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

        private void EditView_Loaded(object sender, RoutedEventArgs e)
        {
            FBResourceTreeNode itemsPreparationInfo = this.GetDataContextObject<FBResourceTreeNode>();
            if (itemsPreparationInfo != null && itemsPreparationInfo.Edit)
                PreviewMouseDown += GlobalPreviewMouseDown;
        }

        private void EditView_Unloaded(object sender, RoutedEventArgs e)
        {
            PreviewMouseDown -= GlobalPreviewMouseDown;
        }
    }
}
