using FlavourBusinessFacade;
using FlavourBusinessToolKit;
using MenuDesigner.ViewModel.MenuCanvas;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MenuDesigner.ViewModel.Menu
{
    /// <MetaDataID>{93dcd0ce-9f23-42d3-8d39-75614fbcafca}</MetaDataID>
    public abstract class MenuDesignerHost
    {
        public abstract void ShowOnGrpaphicMenuDesigner(MenuCanvas.BookViewModel bookViewModel, MenuItemsEditor.RestaurantMenus restaurantMenuData = null);
        public static MenuDesignerHost Current;
        abstract public BookViewModel Menu { get; set; }

        abstract public GraphicMenusPresentation GraphicMenus { get; }

        public static Task Publish(OrganizationStorageRef graphicMenuStorageRef)
        {
            return Current.SaveAndPublish(graphicMenuStorageRef);
        }

        public abstract Task SaveAndPublish(OrganizationStorageRef graphicMenuStorageRef);

        static object OpenGraphicMenuLock = new object();
        public static async Task<BookViewModel> OpenGraphicMenu(OrganizationStorageRef graphicMenuStorageRef, OrganizationStorageRef graphicMenuItemsStorageRef, MenuItemsEditor.ViewModel.PriceList.IPriceListsOwner priceListsOwner=null)
        {


            string storageIdentity = null;

            if (MenuDesignerHost.Current.Menu != null && MenuDesignerHost.Current.Menu.RealObject != null)
            {
                if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(MenuDesignerHost.Current.Menu.RealObject) != null)
                    storageIdentity = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(MenuDesignerHost.Current.Menu.RealObject).StorageMetaData.StorageIdentity;
            }

            HttpClient httpClient = null;
            //if (storageIdentity != GraphicMenuStorageRef.StorageIdentity)
            {
                BookViewModel bookViewModel = null;
                string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme\\DontWaitWater";
                
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    
                    bookViewModel = BookViewModel.OpenMenu(graphicMenuStorageRef, priceListsOwner);

                    stateTransition.Consistent = true;
                }


                RawStorageData graphicMenuItemsStorageData = null;
                if (graphicMenuItemsStorageRef != null)
                {
                    string temporaryStorageLocation =  temporaryStorageLocation = appDataPath + string.Format("\\{0}RestaurantMenuData.xml", graphicMenuItemsStorageRef.StorageIdentity.Replace("-", ""));
                    httpClient = new HttpClient();
                    var dataStream = await httpClient.GetStreamAsync(graphicMenuItemsStorageRef.StorageUrl);
                    graphicMenuItemsStorageData = new RawStorageData(XDocument.Load(dataStream), temporaryStorageLocation, graphicMenuItemsStorageRef, graphicMenuItemsStorageRef.UploadService);
                }


                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    MenuItemsEditor.RestaurantMenus restaurantMenuData= null;
                    if(graphicMenuItemsStorageData!=null)
                        restaurantMenuData = new MenuItemsEditor.RestaurantMenus(graphicMenuItemsStorageData);

                    
                    
                    MenuDesignerHost.Current.ShowOnGrpaphicMenuDesigner(bookViewModel, restaurantMenuData);
                    stateTransition.Consistent = true;
                    return bookViewModel;
                }
            }
        }

  
        public static async Task<BookViewModel> NewGraphicMenu()
        {
            return await OpenGraphicMenu(Current.GraphicMenus.NewGraphicMenu().StorageRef, null);
        }

    }
}
