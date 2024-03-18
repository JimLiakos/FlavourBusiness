using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFUIElementObjectBind;

namespace Finance.ViewModel
{
    /// <MetaDataID>{3a6dc7cd-83e1-48f1-9705-99e1168a8ebb}</MetaDataID>
    public class TaxAuthorityPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{bbaebd50-a6be-47a7-8be3-80f887b35555}</MetaDataID>
        FinanceFacade.ITaxAuthority TaxAuthority;
        /// <MetaDataID>{14240354-fd8e-46c5-be8a-386fd0e4ed5f}</MetaDataID>
        public TaxAuthorityPresentation(FinanceFacade.ITaxAuthority taxAuthority)
        {
            TaxAuthority = taxAuthority;

            AddTaxableTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                var newTaxableType = TaxAuthority.NewTaxableType();
                _TaxableTypes[newTaxableType] = new TaxableTypeViewModel(newTaxableType);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxableTypes)));


            });


            DeleteSelectedTaxableTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                //{


                var taxableType = SelectedTaxableType.TaxableType;
                SelectedTaxableType = null;

                if (TaxAuthority.RemoveTaxableType(taxableType))
                    _TaxableTypes.Remove(taxableType);


                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxableTypes)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTaxableType)));

                //}
            }, (object sender) => CanDeleteSelectedTaxableType());

            EditSelectedTaxableTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                {
                    try
                    {
                        System.Windows.Window win = System.Windows.Window.GetWindow(AddTaxableTypeCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                        Finance.Views.TaxableTypeWindow window = new Finance.Views.TaxableTypeWindow();
                        window.Owner = win;


                        window.GetObjectContext().SetContextInstance(SelectedTaxableType);
                        if (window.ShowDialog().Value)
                        {
                            if (_SelectedTaxableType != null && _SelectedTaxesContext != null)
                            {
                                _TaxOverrides = SelectedTaxableType.Taxes.Select(x => new TaxOverrideViewModel(x.Tax, _SelectedTaxesContext.TaxesContext)).ToList();
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxOverrides)));
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxesListTitle)));
                            }
                            else
                            {
                                _TaxOverrides = new List<TaxOverrideViewModel>();
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxOverrides)));
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxesListTitle)));
                            }
                            stateTransition.Consistent = true;
                        }

                    }
                    catch (Exception error)
                    {
                    }
                }

                //(MenuItem as MenuModel.MenuItem).Menu.TaxAuthority.NewTaxableType();
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxableTypes)));


            });


            AddTaxesContexCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                var newTaxesContext = TaxAuthority.NewTaxesContext();
                _TaxesContexts[newTaxesContext] = new TaxesContextViewModel(newTaxesContext);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxesContexts)));

            });

            DeleteSelectedTaxesContexCommand = new RelayCommand((object sender) =>
            {



                var taxesContext = SelectedTaxesContext.TaxesContext;
                SelectedTaxesContext = null;

                if (TaxAuthority.RemoveTaxesContext(taxesContext))
                    _TaxesContexts.Remove(taxesContext);


                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxesContexts)));


                //}
            }, (object sender) => CanDeleteSelectedTaxesContext());


            RenameSelectedTaxesContexCommand = new RelayCommand((object sender) =>
            {
                SelectedTaxesContext.Edit = true;

            }, (object sender) => SelectedTaxesContext != null);


            BeforeSaveCommand = new RelayCommand((object sender) =>
             {
                 var objectContext = (BeforeSaveCommand.UserInterfaceObjectConnection?.ContainerControl as FrameworkElement).GetObjectContext();
                 if (this.TaxOverrides != null && this.TaxOverrides.Where(x => !x.Validate()).FirstOrDefault() != null)
                     objectContext.InvalidData = true;

             });


        }

        /// <MetaDataID>{86cf2fb9-eace-4a4b-9b10-30692755585c}</MetaDataID>
        List<TaxOverrideViewModel> _TaxOverrides;
        /// <MetaDataID>{d9411135-823a-4a86-a6a6-ba309f771c87}</MetaDataID>
        public List<TaxOverrideViewModel> TaxOverrides
        {
            get
            {
                return _TaxOverrides;
            }
        }

        /// <MetaDataID>{b6947a22-ef36-4ff3-b5ad-df57bbb849da}</MetaDataID>
        public string Name
        {
            get
            {
                return TaxAuthority.Name;
            }
        }

        /// <MetaDataID>{cd0485aa-a695-40f2-85ed-ce2f25172c5d}</MetaDataID>
        bool CanDeleteSelectedTaxableType()
        {
            if (SelectedTaxableType != null)
                return SelectedTaxableType.TaxableType.TaxableSubjects.Count == 0;
            else
                return false;
        }

        /// <MetaDataID>{79b0df0a-290d-4fc5-886e-5ab491960828}</MetaDataID>
        bool CanDeleteSelectedTaxesContext()
        {
            if (SelectedTaxesContext != null)
                return true;
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
        Dictionary<FinanceFacade.ITaxesContext, TaxesContextViewModel> _TaxesContexts;

        /// <MetaDataID>{a611bd6a-2251-4841-a2d5-458f0343ca5f}</MetaDataID>
        public IList<TaxesContextViewModel> TaxesContexts
        {
            get
            {

                if (_TaxesContexts == null)
                    _TaxesContexts = (from taxesContext in TaxAuthority.TaxesContexts
                                      select new TaxesContextViewModel(taxesContext)).ToDictionary(x => x.TaxesContext);
                return _TaxesContexts.Values.ToList();
            }
        }


        /// <MetaDataID>{2406a063-b9f8-4107-bc3e-4448b487c161}</MetaDataID>
        public string TaxesListTitle
        {
            get
            {
                if (_SelectedTaxableType == null)
                    return "Taxes";

                if (_SelectedTaxesContext == null)
                    return $"{_SelectedTaxableType.Description} Default";
                else
                    return $"{_SelectedTaxableType.Description} {_SelectedTaxesContext.Description}";


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

                if (_SelectedTaxableType != value)
                {
                    _SelectedTaxableType = value;
                    if (_SelectedTaxableType != null && _SelectedTaxesContext != null)
                    {
                        _TaxOverrides = SelectedTaxableType.Taxes.Select(x => new TaxOverrideViewModel(x.Tax, _SelectedTaxesContext.TaxesContext)).ToList();
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxOverrides)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxesListTitle)));
                        
                    }
                    else
                    {
                        _TaxOverrides = SelectedTaxableType.Taxes.Select(x => new TaxOverrideViewModel(x.Tax)).ToList();

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxOverrides)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxesListTitle)));
                    }

                }


            }
        }

        /// <exclude>Excluded</exclude>
        TaxesContextViewModel _SelectedTaxesContext;

        /// <MetaDataID>{4cc2b21c-bfe7-42ce-a141-0ba0aaee49ee}</MetaDataID>
        public TaxesContextViewModel SelectedTaxesContext
        {
            get => _SelectedTaxesContext;

            set
            {
                if (_SelectedTaxesContext != value)
                {
                    _SelectedTaxesContext = value;
                    if (_SelectedTaxableType != null && _SelectedTaxesContext != null)
                    {
                        _TaxOverrides = SelectedTaxableType.Taxes.Select(x => new TaxOverrideViewModel(x.Tax, _SelectedTaxesContext.TaxesContext)).ToList();
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxOverrides)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxesListTitle)));
                    }
                    else
                    {
                        _TaxOverrides = SelectedTaxableType.Taxes.Select(x => new TaxOverrideViewModel(x.Tax)).ToList();
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxOverrides)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxesListTitle)));
                    }

                }
            }
        }

        /// <MetaDataID>{9192dc83-482b-4115-9a08-7575f7d3b46c}</MetaDataID>
        public TaxAuthorityPresentation()
        {
        }

        /// <MetaDataID>{0fab3dcd-8e84-4cae-a09e-5798ed15c795}</MetaDataID>
        public RelayCommand AddTaxableTypeCommand { get; protected set; }




        /// <MetaDataID>{badfb03d-61ce-4153-807d-9b8f006c9a17}</MetaDataID>
        public RelayCommand EditSelectedTaxableTypeCommand { get; protected set; }
        /// <MetaDataID>{50a25739-a329-4358-be0e-b61f130a8205}</MetaDataID>
        public RelayCommand DeleteSelectedTaxableTypeCommand { get; protected set; }
        /// <MetaDataID>{9174515a-3173-4590-a5c8-8f8b81afe4d5}</MetaDataID>
        public RelayCommand RenameSelectedTaxableTypeCommand { get; protected set; }


        /// <MetaDataID>{d3d798fe-6f71-4290-b3ec-83ea335cedeb}</MetaDataID>
        public RelayCommand AddTaxesContexCommand { get; protected set; }
        /// <MetaDataID>{0e9bf4fa-2cfe-48e6-b7f9-549803a70a25}</MetaDataID>
        public RelayCommand DeleteSelectedTaxesContexCommand { get; }
        /// <MetaDataID>{43b1c63d-49fa-44c3-989b-e3650554ae93}</MetaDataID>
        public RelayCommand RenameSelectedTaxesContexCommand { get; }
        /// <MetaDataID>{c0989468-e7a0-4a53-92ad-3c0e526b13b7}</MetaDataID>
        public RelayCommand BeforeSaveCommand { get; }
        /// <MetaDataID>{fb106230-15d8-4f69-882e-f6b32a44a97e}</MetaDataID>
        public RelayCommand DeleteTaxesContexCommand { get; protected set; }

        /// <MetaDataID>{a4eb7826-1454-44a7-94b1-82912260c26f}</MetaDataID>
        public RelayCommand RenameTaxesContexCommand { get; protected set; }

    }
}
