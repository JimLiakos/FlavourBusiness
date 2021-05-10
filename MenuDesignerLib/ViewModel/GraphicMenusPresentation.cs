using FlavourBusinessFacade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MenuDesigner.ViewModel
{
    /// <MetaDataID>{1905b81e-e30b-494b-9bd7-7b37ae25a820}</MetaDataID>
    public class GraphicMenusPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <MetaDataID>{d47baec1-d1e1-4156-bd20-45aee6080701}</MetaDataID>
        List<GraphicMenuPresentation> _GraphicMenus;

        /// <MetaDataID>{c941ce6b-41ae-4a14-9ed9-6c3e4df984f3}</MetaDataID>
        public List<GraphicMenuPresentation> GraphicMenus
        {
            get
            {
                if (_GraphicMenus == null)
                    return new List<GraphicMenuPresentation>();
                return _GraphicMenus.ToList();
            }
        }


        public readonly IResourceManager ResourceManager;
        /// <MetaDataID>{5604d668-6db5-4e35-a240-a0d177563250}</MetaDataID>
        public GraphicMenusPresentation(List<OrganizationStorageRef> graphicMenus,IResourceManager resourceManager)
        {
            ResourceManager = resourceManager;
            _GraphicMenus = (from storageRef in graphicMenus select new GraphicMenuPresentation(storageRef,resourceManager)).ToList();

            if (_GraphicMenus.Count > 0)
                _SelectedMenu = _GraphicMenus[0];

            RenameSelectedMenuCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                SelectedMenu.Edit = true;
            }, (object sender) => SelectedMenu != null);

            NewCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
             {
              NewGraphicMenu();
             });
        }

        public GraphicMenuPresentation NewGraphicMenu()
        {
            var graphicMenuPresentation = new GraphicMenuPresentation(ResourceManager.NewGraphicMenu(System.Globalization.CultureInfo.CurrentCulture.Name), ResourceManager);
            _GraphicMenus.Add(graphicMenuPresentation);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GraphicMenus)));
            return graphicMenuPresentation;
        }

        public bool RemoveGraphicMenu(string graphicMenuStorageIdentity)
        {

            var graphicMenuPresentation = _GraphicMenus.Where(x => x.StorageRef.StorageIdentity == graphicMenuStorageIdentity).FirstOrDefault();
            if (graphicMenuPresentation != null)
            {
                graphicMenuPresentation.ResourceManager.RemoveGraphicMenu(graphicMenuPresentation.StorageRef.StorageIdentity);
                _GraphicMenus.Remove(graphicMenuPresentation);
            }

            return true;
        }

        /// <MetaDataID>{1c5b6d66-0a04-40d4-9f3f-e52efadb66de}</MetaDataID>
        GraphicMenuPresentation _SelectedMenu = null;
        /// <MetaDataID>{7c61890c-96c4-4684-86b0-ca91e327aa2e}</MetaDataID>
        public GraphicMenuPresentation SelectedMenu
        {
            get
            {
                return _SelectedMenu;
            }
            set
            {
                _SelectedMenu = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMenu)));

            }
        }

        /// <MetaDataID>{d28f1907-2695-4e1b-acd8-e3b3ec362de7}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand RenameSelectedMenuCommand { get; protected set; }

        /// <MetaDataID>{59fddcec-708c-45ed-8ca9-61cdf415b3af}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand NewCommand { get; protected set; }
        public IUploadService UploadService { get; internal set; }
    }


    /// <MetaDataID>{45fdc89f-754b-45eb-9876-efbe82d6c386}</MetaDataID>
    public class GraphicMenuPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        OrganizationStorageRef _StorageRef;
        public OrganizationStorageRef StorageRef
        {
            get
            {
                return _StorageRef;
            }
        }
        public readonly IResourceManager ResourceManager;
        public GraphicMenuPresentation(OrganizationStorageRef organizationStorageRef, IResourceManager resourceManager)
        {
            ResourceManager = resourceManager;
            _StorageRef = organizationStorageRef;
        }
        public string Name
        {
            get
            {
                return _StorageRef.Name;
            }
            set
            {
                if (_StorageRef.Name != value)
                {
                    _StorageRef.Name = value;
                    _StorageRef = (FlavourBusinessManager.Organization.CurrentOrganization as FlavourBusinessFacade.IResourceManager).UpdateStorage(StorageRef.Name, StorageRef.Description, StorageRef.StorageIdentity);
                }
            }
        }




        bool _Edit;
        public bool Edit
        {
            get
            {
                return _Edit;
            }
            set
            {
                _Edit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
        }

    }
}
