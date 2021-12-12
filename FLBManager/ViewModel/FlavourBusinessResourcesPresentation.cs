using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade;
using FLBManager.ViewModel.Taxes;
using MenuDesigner.ViewModel;
using MenuDesigner.ViewModel.MenuCanvas;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel
{
    /// <MetaDataID>{0172fe25-006a-444f-a96f-9ebf78181d59}</MetaDataID>
    public class FlavourBusinessResourcesPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        private IOrganization Organization;

        public readonly CompanyPresentation _Company;

        public CompanyPresentation Company
        {
            get
            {
                return _Company;
            }
        }

        /// <exclude>Excluded</exclude>
        FinanceFacade.ITaxAuthority _SelectedTaxAuthority;

        public FinanceFacade.ITaxAuthority SelectedTaxAuthority
        {
            get
            {
                if (_SelectedTaxAuthority == null)
                {
                    if (RestaurantMenus.Menus.Count > 0)
                    {
                        _SelectedTaxAuthority = (from taxAuthority in TaxAuthorities
                                                 where RestaurantMenus.Menus[0].TaxAuthority != null && taxAuthority.Identity == RestaurantMenus.Menus[0].TaxAuthority.Identity
                                                 select taxAuthority).FirstOrDefault();
                    }
                }
                return _SelectedTaxAuthority;
            }
            set
            {

                _SelectedTaxAuthority = value;

                if (RestaurantMenus != null)
                {

                    var objectStorage = RestaurantMenus.ObjectStorage;
                    var storage = new OOAdvantech.Linq.Storage(objectStorage);
                    var taxAuthority = (from theTaxAuthority in storage.GetObjectCollection<FinanceFacade.ITaxAuthority>()
                                        where theTaxAuthority.Identity == _SelectedTaxAuthority.Identity
                                        select theTaxAuthority).FirstOrDefault();


                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                    {
                        foreach (var menu in RestaurantMenus.Menus)
                        {

                            if (taxAuthority == null)
                            {
                                objectStorage.CommitTransientObjectState(_SelectedTaxAuthority);
                                taxAuthority = _SelectedTaxAuthority;
                            }
                            menu.TaxAuthority = taxAuthority;
                        }
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        public WPFUIElementObjectBind.RoutedCommand EditSelectedTaxAuthorityCommand { get; private set; }


        List<FinanceFacade.ITaxAuthority> _TaxAuthorities;
        public IList<FinanceFacade.ITaxAuthority> TaxAuthorities
        {
            get
            {
                if (_TaxAuthorities == null)
                    _TaxAuthorities = new List<FinanceFacade.ITaxAuthority>() { this.RestaurantMenus.Menus[0].TaxAuthority };

                return _TaxAuthorities;
            }
        }
        MenuItemsEditor.RestaurantMenus _RestaurantMenus;
        internal MenuItemsEditor.RestaurantMenus RestaurantMenus
        {
            get
            {
                return _RestaurantMenus;
            }
            set
            {
                _RestaurantMenus = value;
                _Company.RestaurantMenus = value;
            }
        }
        public FlavourBusinessResourcesPresentation(IOrganization organization, GraphicMenusPresentation graphicMenusPresentation, MenuItemsEditor.RestaurantMenus restaurantMenus)
        {
            this.Organization = organization;

            _Company = new CompanyPresentation(organization, null, graphicMenusPresentation, restaurantMenus);
            _Resources = new List<FBResourceTreeNode>() { _Company };
            _Company.PropertyChanged += Company_PropertyChanged;
            this.RestaurantMenus = restaurantMenus;

            EditSelectedTaxAuthorityCommand = new RoutedCommand((object sender) =>
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    try
                    {
                        System.Windows.Window win = System.Windows.Window.GetWindow(EditSelectedTaxAuthorityCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);

                        var window = new Finance.Views.TaxAuthorityWindow();
                        window.Owner = win;


                        window.GetObjectContext().SetContextInstance(new Finance.ViewModel.TaxAuthorityPresentation(SelectedTaxAuthority));
                        if (window.ShowDialog().Value)
                        {

                        }
                        stateTransition.Consistent = true;
                    }
                    catch (Exception error)
                    {
                    }
                }

            });
        }

        public void SignOut()
        {
            _Company.SignOut();
        }

        private void Company_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContextMenuItems)));
        }

        List<FBResourceTreeNode> _Resources;

        public List<FBResourceTreeNode> Resources
        {
            get
            {
                return _Resources;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;
        public List<MenuCommand> ContextMenuItems
        {
            get
            {
                var selectedItemContextMenuItems = _Company.SelectedItemContextMenuItems;
                if (selectedItemContextMenuItems != null)
                    return selectedItemContextMenuItems;
                else
                    return _Company.ContextMenuItems;
            }
        }

    }
}
