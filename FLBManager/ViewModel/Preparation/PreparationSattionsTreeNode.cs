using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade.ServicesContextResources;
using MenuItemsEditor.ViewModel;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Preparation
{
    /// <MetaDataID>{98bdc5c9-e359-40e1-93c6-5140f8c02cb1}</MetaDataID>
    public class PreparationSationsTreeNode : FBResourceTreeNode, INotifyPropertyChanged, IDragDropTarget
    {

        public PreparationSationsTreeNode(Infrastructure.InfrastructureTreeNode parent) : base(parent)
        {
            ServiceContextInfrastructure = parent;

            NewPreparationSationCommand = new RelayCommand((object sender) =>
            {
                NewPreparationSation();
            });



            try
            {
                var menuViewModel = ServiceContextInfrastructure.ServicesContextPresentation.Company.RestaurantMenus.Members[0] as MenuViewModel;

                foreach (var preparationStation in ServiceContextInfrastructure.ServiceContextResources.PreparationStations)
                    PreparationStations.Add(preparationStation, new PreparationStationPresentation(this, preparationStation, menuViewModel));

              }
            catch (System.Exception error)
            {
            }


        }
        Infrastructure.InfrastructureTreeNode ServiceContextInfrastructure;

        Dictionary<IPreparationStation, PreparationStationPresentation> PreparationStations = new Dictionary<IPreparationStation, PreparationStationPresentation>();

        internal void RemovePreparationStation(PreparationStationPresentation preparationStationPresentation)
        {
            this.ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.RemovePreparationStation(preparationStationPresentation.PreparationStation);
            PreparationStations.Remove(preparationStationPresentation.PreparationStation);
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        private void NewPreparationSation()
        {
            var menuViewModel = ServiceContextInfrastructure.ServicesContextPresentation.Company.RestaurantMenus.Members[0] as MenuViewModel;

            var preparationStation = ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.NewPreparationStation();
            PreparationStations.Add(preparationStation, new PreparationStationPresentation(this, preparationStation, menuViewModel));


            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            IsNodeExpanded=true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));

        }

        public override string Name
        {
            get
            {
                return Properties.Resources.PreparationSattionsTitle;
            }

            set
            {
            }
        }

        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/kitchen16.png"));
            }
        }
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var members = this.PreparationStations.Values.OfType<FBResourceTreeNode>().ToList();

                return members;
            }
        }
        public RelayCommand NewPreparationSationCommand { get; protected set; }

        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        List<MenuCommand> _ContextMenuItems;
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();




                    MenuCommand menuItem = new MenuCommand(); ;
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/chef16.png"));
                    menuItem.Header = Properties.Resources.NewPreparationStationPrompt;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewPreparationSationCommand;
                    _ContextMenuItems.Add(menuItem);

                }

                return _ContextMenuItems;
            }
        }
        public override List<MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return ContextMenuItems;
                else
                    foreach (var treeNode in Members)
                    {
                        var contextMenuItems = treeNode.SelectedItemContextMenuItems;
                        if (contextMenuItems != null)
                            return contextMenuItems;
                    }

                return null;
            }
        }
        public override void SelectionChange()
        {
        }

        DateTime DragEnterStartTime;
        public void DragEnter(object sender, DragEventArgs e)
        {
            DragEnterStartTime = DateTime.Now;

            DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(DragItemsCategory)) as DragItemsCategory;
            if (dragItemsCategory != null)
            {
                e.Effects = DragDropEffects.Copy;
                DragEnterStartTime = DateTime.Now;
            }
            else
                e.Effects = DragDropEffects.None;
        }

        public void DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        public void DragOver(object sender, DragEventArgs e)
        {

            DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(DragItemsCategory)) as DragItemsCategory;
            if (dragItemsCategory != null)
            {
                if ((DateTime.Now - DragEnterStartTime).TotalSeconds > 2)
                {
                    IsNodeExpanded = true;
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
                }
            }
            else
                e.Effects = DragDropEffects.None;

      



            System.Diagnostics.Debug.WriteLine("DragOver InfrastructureTreeNode");
        }

        public void Drop(object sender, DragEventArgs e)
        {
        }

    }
}
