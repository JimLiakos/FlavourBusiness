﻿using System;
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

namespace MenuDesigner.Views
{
    /// <summary>
    /// Interaction logic for RestaurantMenuItemsTreeView.xaml
    /// </summary>
    /// <MetaDataID>{b3a42780-3770-4171-a0d0-e11f1cdbb0bd}</MetaDataID>
    public partial class RestaurantMenuItemsTreeView : UserControl
    {

        /// <MetaDataID>{ada93ecd-ad2c-4dac-afcb-8fcde1851a37}</MetaDataID>
        public RestaurantMenuItemsTreeView()
        {
            InitializeComponent();
        }

        /// <MetaDataID>{8cbc4e66-c9b3-4b58-8972-fc269178a8a1}</MetaDataID>
        private void MenuItemsTreeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            TreeView treeView = sender as TreeView;
            var ss = treeView.GetObjectContext();
            MenuItemsEditor.IMenusTreeNode selectedItem = treeView.GetObjectContext().GetRealObject<MenuItemsEditor.IMenusTreeNode>(treeView.SelectedItem);
            if (selectedItem != null)
            {
                selectedItem.IsSelected = false;
                selectedItem.Edit = false;
            }

        }
        /// <MetaDataID>{c97b6d64-764f-4282-9bb8-81d42981c185}</MetaDataID>
        private Point? dragStartPoint = null;

        /// <MetaDataID>{7548748b-9447-460d-b6f4-c02035791676}</MetaDataID>
        private void ItemMouseMove(object sender, MouseEventArgs e)
        {
            this.dragStartPoint = new Point?(e.GetPosition(this));
        }

        /// <MetaDataID>{69997f3b-ef58-4b9e-8d8f-a0564f97fa4a}</MetaDataID>
        private void ItemMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                this.dragStartPoint = null;

            if (this.dragStartPoint.HasValue)
            {
                var treeFoodItemViewModel = (sender as FrameworkElement).GetDataContextObject<ViewModel.MenuCanvas.TreeFoodItemViewModel>();
                var treeBlankItemViewModel = (sender as FrameworkElement).GetDataContextObject<ViewModel.MenuCanvas.TreeBlankItemViewModel>();
                var itemsCategoryViewModel = (sender as FrameworkElement).GetDataContextObject<ViewModel.MenuCanvas.ItemsCategoryViewModel>();

                if (treeFoodItemViewModel != null)
                    DragDrop.DoDragDrop(this, new ViewModel.MenuCanvas.DragCanvasItem(treeFoodItemViewModel.MenuCanvasFoodItem), DragDropEffects.Copy);

                if (treeBlankItemViewModel != null)
                    DragDrop.DoDragDrop(this, new ViewModel.MenuCanvas.DragCanvasItem(treeBlankItemViewModel.MenuCanvasFoodItem), DragDropEffects.Copy);


                if (itemsCategoryViewModel != null)
                    DragDrop.DoDragDrop(this, new MenuItemsEditor.ViewModel.DragItemsCategory(itemsCategoryViewModel.ItemsCategory), DragDropEffects.Copy);

            }
        }

        private void DragMenuItemObject(object sender)
        {

            var treeFoodItemViewModel = (sender as FrameworkElement).GetDataContextObject<ViewModel.MenuCanvas.TreeFoodItemViewModel>();
            var treeBlankItemViewModel = (sender as FrameworkElement).GetDataContextObject<ViewModel.MenuCanvas.TreeBlankItemViewModel>();
            var itemsCategoryViewModel = (sender as FrameworkElement).GetDataContextObject<ViewModel.MenuCanvas.ItemsCategoryViewModel>();

            if (treeFoodItemViewModel != null)
                DragDrop.DoDragDrop(this, new ViewModel.MenuCanvas.DragCanvasItem(treeFoodItemViewModel.MenuCanvasFoodItem), DragDropEffects.Copy);

            if (treeBlankItemViewModel != null)
                DragDrop.DoDragDrop(this, new ViewModel.MenuCanvas.DragCanvasItem(treeBlankItemViewModel.MenuCanvasFoodItem), DragDropEffects.Copy);


            if (itemsCategoryViewModel != null)
                DragDrop.DoDragDrop(this, new MenuItemsEditor.ViewModel.DragItemsCategory(itemsCategoryViewModel.ItemsCategory), DragDropEffects.Copy);


        }
    }
}