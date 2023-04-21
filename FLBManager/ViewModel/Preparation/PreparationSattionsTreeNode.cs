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
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{693b6572-cadd-40b4-8ab7-888267b37b31}</MetaDataID>
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
        /// <MetaDataID>{59ff9b8c-0cc4-4617-8c1c-4367e79017ce}</MetaDataID>
        Infrastructure.InfrastructureTreeNode ServiceContextInfrastructure;

        /// <MetaDataID>{538d43ac-584e-4e19-b349-dc5a2b082cdb}</MetaDataID>
        Dictionary<IPreparationStation, PreparationStationPresentation> PreparationStations = new Dictionary<IPreparationStation, PreparationStationPresentation>();

        /// <MetaDataID>{9e7b6800-d297-4665-93f3-169c7e95ca65}</MetaDataID>
        internal void RemovePreparationStation(PreparationStationPresentation preparationStationPresentation)
        {
            this.ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.RemovePreparationStation(preparationStationPresentation.PreparationStation);
            PreparationStations.Remove(preparationStationPresentation.PreparationStation);
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        /// <MetaDataID>{bd1d6ad9-e654-4595-90ba-d9e6c11b60f0}</MetaDataID>
        private void NewPreparationSation()
        {
            var menuViewModel = ServiceContextInfrastructure.ServicesContextPresentation.Company.RestaurantMenus.Members[0] as MenuViewModel;

            var preparationStation = ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.NewPreparationStation();
            var preparationStationPresentation = new PreparationStationPresentation(this, preparationStation, menuViewModel);
            preparationStationPresentation.Edit = true;
            PreparationStations.Add(preparationStation, preparationStationPresentation);


            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            IsNodeExpanded = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));

        }

        /// <MetaDataID>{9c7832de-c173-402e-bccd-b1922229df81}</MetaDataID>
        public override string Name
        {
            get
            {
                return Properties.Resources.PreparationStationsTitle;
            }

            set
            {
            }
        }

        /// <MetaDataID>{5e6a6750-2fcd-4c77-84b6-da17faf82791}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/kitchen16.png"));
            }
        }
        /// <MetaDataID>{3e506a85-51f2-4874-8bdf-0019863e1e67}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var members = this.PreparationStations.Values.OfType<FBResourceTreeNode>().ToList();

                return members;
            }
        }
        /// <MetaDataID>{45476233-5c6c-4fe9-ac7a-51598ed22856}</MetaDataID>
        public RelayCommand NewPreparationSationCommand { get; protected set; }

        /// <MetaDataID>{87bcd558-ccdc-4ba6-8577-b17d80936925}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{dd84b653-2a56-4291-b4b6-47eebb607f35}</MetaDataID>
        List<MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{9a27878b-6d99-4caa-85d0-c08564a0f0cb}</MetaDataID>
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
        /// <MetaDataID>{88f89b5c-5f5a-4a00-8676-59cd5550be59}</MetaDataID>
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
        /// <MetaDataID>{7ac411c9-f0d0-4080-986f-a006705272f8}</MetaDataID>
        public override void SelectionChange()
        {
        }

        /// <MetaDataID>{a9b06a88-e000-4441-a8ed-47393e4f1feb}</MetaDataID>
        DateTime DragEnterStartTime;
        /// <MetaDataID>{ca6d98f4-ad75-400c-8b54-8074ac2694a9}</MetaDataID>
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

        /// <MetaDataID>{7685fc3f-422b-4f0f-bbc2-ce73147fc26b}</MetaDataID>
        public void DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{50554c50-c0c4-4324-bc16-4e2494ee9e78}</MetaDataID>
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

        /// <MetaDataID>{6d4862ac-5434-4fc0-8e58-f8ec7710fec4}</MetaDataID>
        public void Drop(object sender, DragEventArgs e)
        {
        }

    }
}
