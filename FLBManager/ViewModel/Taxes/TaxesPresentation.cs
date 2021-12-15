using Finance.ViewModel;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Taxes
{
    /// <MetaDataID>{3a6dc7cd-83e1-48f1-9705-99e1168a8ebb}</MetaDataID>
    public class TaxesPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        FinanceFacade.ITaxAuthority TaxAuthority;
        public TaxesPresentation(FinanceFacade.ITaxAuthority taxAuthority, MenuModel.IMenu menu)
        {
            TaxAuthority = taxAuthority;


            this.PrintReceiptsItemStates = new List<PrintReceiptsItemStateViewModel>() {
                new PrintReceiptsItemStateViewModel(FlavourBusinessFacade.ServicesContextResources.ServicePointType.Delivery,FlavourBusinessFacade.RoomService.ItemPreparationState.OnRoad),
                new PrintReceiptsItemStateViewModel(FlavourBusinessFacade.ServicesContextResources.ServicePointType.TakeAway,FlavourBusinessFacade.RoomService.ItemPreparationState.OnRoad),
                new PrintReceiptsItemStateViewModel(FlavourBusinessFacade.ServicesContextResources.ServicePointType.HallServicePoint,FlavourBusinessFacade.RoomService.ItemPreparationState.PendingPreparation)
            };

            MenuItemsTaxInfo = new List<MenuItemTaxInfo>() { new MenuItemTaxInfo(null, menu.RootCategory, true) };
            AddTaxableTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                var newTaxableType = TaxAuthority.NewTaxableType();
                _TaxableTypes[newTaxableType] = new TaxableTypeViewModel(newTaxableType);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxableTypes)));


            });

            DeleteSelectedTaxableTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {


                var taxableType = SelectedTaxableType.TaxableType;
                SelectedTaxableType = null;

                if (TaxAuthority.RemoveTaxableType(taxableType))
                    _TaxableTypes.Remove(taxableType);


                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxableTypes)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTaxableType)));

            }, (object sender) => CanDeleteSelectedTaxableType());

            EditSelectedTaxableTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    try
                    {
                        System.Windows.Window win = System.Windows.Window.GetWindow(AddTaxableTypeCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                        Finance.Views.TaxableTypeWindow window = new Finance.Views.TaxableTypeWindow();
                        window.Owner = win;


                        window.GetObjectContext().SetContextInstance(SelectedTaxableType);
                        if (window.ShowDialog().Value)
                        {

                        }
                        stateTransition.Consistent = true;
                    }
                    catch (Exception error)
                    {
                    }
                }

                //(MenuItem as MenuModel.MenuItem).Menu.TaxAuthority.NewTaxableType();
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxableTypes)));


            });

            RenameSelectedTaxableTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                SelectedTaxableType.Edit = true;
                //(MenuItem as MenuModel.MenuItem).Menu.TaxAuthority.NewTaxableType();
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxableTypes)));
            });

        }


        public string Name
        {
            get
            {
                return TaxAuthority.Name;
            }
            set
            {

            }
        }

        public List<PrintReceiptsItemStateViewModel> PrintReceiptsItemStates { get; }

        public List<MenuItemTaxInfo> MenuItemsTaxInfo { get; set; }
        bool CanDeleteSelectedTaxableType()
        {
            if (SelectedTaxableType != null)
                return SelectedTaxableType.TaxableType.TaxableSubjects.Count == 0;
            else
                return false;
        }
        /// <exclude>Excluded</exclude>

        Dictionary<FinanceFacade.ITaxableType, TaxableTypeViewModel> _TaxableTypes;



        /// <MetaDataID>{a611bd6a-2251-4841-a2d5-458f0343ca5f}</MetaDataID>
        public IList<TaxableTypeViewModel> TaxableTypes
        {
            get
            {
                if (_TaxableTypes == null)
                    _TaxableTypes = (from taxableType in TaxAuthority.TaxableTypes
                                     select new TaxableTypeViewModel(taxableType)).ToDictionary(x => x.TaxableType);

                return _TaxableTypes.Values.ToList();

            }
        }


        /// <exclude>Excluded</exclude>
        TaxableTypeViewModel _SelectedTaxableType;

        /// <MetaDataID>{ff0d55fa-b8cf-4e8e-8595-6ed7d7815691}</MetaDataID>
        public TaxableTypeViewModel SelectedTaxableType
        {
            get
            {

                return _SelectedTaxableType;
            }
            set
            {


                _SelectedTaxableType = value;

                if (MenuItemsTaxInfo.FirstOrDefault() != null)
                    MenuItemsTaxInfo.FirstOrDefault().TaxableType = value;


            }
        }

        public TaxesPresentation()
        {
        }

        public RelayCommand AddTaxableTypeCommand { get; protected set; }


        public RelayCommand EditSelectedTaxableTypeCommand { get; protected set; }
        /// <MetaDataID>{50a25739-a329-4358-be0e-b61f130a8205}</MetaDataID>
        public RelayCommand DeleteSelectedTaxableTypeCommand { get; protected set; }
        /// <MetaDataID>{9174515a-3173-4590-a5c8-8f8b81afe4d5}</MetaDataID>
        public RelayCommand RenameSelectedTaxableTypeCommand { get; protected set; }
    }
}
