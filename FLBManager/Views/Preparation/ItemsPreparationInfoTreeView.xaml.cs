﻿using FLBManager.ViewModel.Preparation;
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

namespace FLBManager.Views.Preparation
{
    /// <summary>
    /// Interaction logic for ItemsPreparationInfoTreeView.xaml
    /// </summary>
    /// <MetaDataID>FLBManager.Views.Preparation.ItemsPreparationInfoTreeView</MetaDataID>
    public partial class ItemsPreparationInfoTreeView : UserControl
    {
        /// <MetaDataID>{61ee8805-1350-48bb-b110-343579eb0ca1}</MetaDataID>
        public ItemsPreparationInfoTreeView()
        {
            InitializeComponent();
        
            
        }

        private void ItemsPreparationInfoTreeView_Loaded(object sender, RoutedEventArgs e)
        {
            var itemsPreparationInfo = this.GetDataContextObject<ItemsPreparationInfoPresentation>();
            if (itemsPreparationInfo != null && itemsPreparationInfo.CanPrepared&&itemsPreparationInfo.Edit)
                PreviewMouseDown += GlobalPreviewMouseDown;
        }

        private void ItemsPreparationInfoTreeView_Unloaded(object sender, RoutedEventArgs e)
        {
            PreviewMouseDown -= GlobalPreviewMouseDown;
        }

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
        /// <MetaDataID>{f4f370dd-beb4-4761-991c-77152139da16}</MetaDataID>
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ClickCount == 2)
            {
                //PreviewMouseDown += GlobalPreviewMouseDown;

                var itemsPreparationInfo = this.GetDataContextObject<ItemsPreparationInfoPresentation>();
                if (itemsPreparationInfo != null && itemsPreparationInfo.CanPrepared)
                {
                    itemsPreparationInfo.Edit = true;
                    e.Handled = true;
                }
            }
           
        }

        

        /// <MetaDataID>{082d1303-8cd4-4c0f-bf33-90f841c2d52c}</MetaDataID>
        private void GlobalPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var po = PointToScreen(new Point(0, 0));
            var mousepos = Mouse.GetPosition(Grid);

            var itemsPreparationInfo = this.GetDataContextObject<ItemsPreparationInfoPresentation>();
            if (!itemsPreparationInfo.Edit)
                return;



            if (mousepos.X < 0 || mousepos.Y < 0 || mousepos.X > ActualWidth || mousepos.Y > ActualHeight)
            {
                if (DataContext is ItemsPreparationInfoPresentation && 
                    ((DataContext as ItemsPreparationInfoPresentation).TagsPopupOpen
                    || (DataContext as ItemsPreparationInfoPresentation).AppearanceOrderPopupOpen))
                    return;



                //PreviewMouseDown -= GlobalPreviewMouseDown;
                //var itemsPreparationInfo = this.GetDataContextObject<ItemsPreparationInfoPresentation>();
                if (itemsPreparationInfo != null)
                {
                    
                    foreach(var unitTextBox in WPFUIElementObjectBind.ObjectContext.FindChilds<TextBoxNumberWithUnit>(Content as DependencyObject))
                        unitTextBox.Focus();
                    foreach (var checkBox in WPFUIElementObjectBind.ObjectContext.FindChilds<CheckBox>(Content as DependencyObject))
                        checkBox.Focus();
                    Keyboard.ClearFocus();
                    itemsPreparationInfo.Edit = false;
                    //Task.Run(() =>
                    //{
                    //    System.Threading.Thread.Sleep(300);
                    //    this.Dispatcher.Invoke(new Action(() => { itemsPreparationInfo.Edit = false; }));
                    //});
           
                }
            }
        }

        public TextBoxNumberWithUnit PreparationTime
        {
            get
            {

                var sds = WPFUIElementObjectBind.ObjectContext.FindChilds<TextBoxNumberWithUnit>(Content as DependencyObject);
                if (this.ContentTemplate != null)
                    return this.ContentTemplate.FindName("PreparationTime", this) as TextBoxNumberWithUnit;
                else
                    return null;
            }
        }
        public TextBoxNumberWithUnit CookingTime
        {
            get
            {
                if (this.ContentTemplate != null)
                    return this.ContentTemplate.FindName("PreparationTime", this) as TextBoxNumberWithUnit;
                else
                    return null;
            }
        }
        

        /// <MetaDataID>{f91b64cd-fbd9-4bf1-b965-4854af6f4a3c}</MetaDataID>
        static void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _PreviewMouseDown?.Invoke(sender, e);
        }
    }




    delegate void PreviewMouseDownHandle(object sender, MouseButtonEventArgs e);



    //public class HalfConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {

    //        if (value is double)
    //            return ((double)value) / 2;


    //        if (value is int)
    //            return ((int)value) / 2;

    //        return 0;

    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return DependencyProperty.UnsetValue;
    //    }
    //}

}
