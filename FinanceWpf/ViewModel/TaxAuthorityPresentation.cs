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

        FinanceFacade.ITaxAuthority TaxAuthority;
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
                            if (_SelectedTaxableType != null && _SelectedTaxesContext != null)
                            {
                                _TaxOverrides = SelectedTaxableType.Taxes.Select(x => new TaxOverrideViewModel(x.Tax, _SelectedTaxesContext.TaxesContext)).ToList();
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxOverrides)));
                            }
                            else
                            {
                                _TaxOverrides = new List<TaxOverrideViewModel>();
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxOverrides)));
                            }

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
                 if(this.TaxOverrides!=null&& this.TaxOverrides.Where(x=>!x.Validate()).FirstOrDefault()!=null)
                    objectContext.InvalidData = true;

             });


        }

        List<TaxOverrideViewModel> _TaxOverrides;
        public List<TaxOverrideViewModel> TaxOverrides
        {
            get
            {
                return _TaxOverrides;
            }
        }

        public string Name
        {
            get
            {
                return TaxAuthority.Name;
            }
        }

        bool CanDeleteSelectedTaxableType()
        {
            if (SelectedTaxableType != null)
                return SelectedTaxableType.TaxableType.TaxableSubjects.Count == 0;
            else
                return false;
        }

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
                    }
                    else
                    {
                        _TaxOverrides = new List<TaxOverrideViewModel>();
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxOverrides)));
                    }

                }


            }
        }

        /// <exclude>Excluded</exclude>
        TaxesContextViewModel _SelectedTaxesContext;

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
                    }
                    else
                    {
                        _TaxOverrides = new List<TaxOverrideViewModel>();
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxOverrides)));
                    }

                }
            }
        }

        public TaxAuthorityPresentation()
        {
        }

        public RelayCommand AddTaxableTypeCommand { get; protected set; }




        public RelayCommand EditSelectedTaxableTypeCommand { get; protected set; }
        /// <MetaDataID>{50a25739-a329-4358-be0e-b61f130a8205}</MetaDataID>
        public RelayCommand DeleteSelectedTaxableTypeCommand { get; protected set; }
        /// <MetaDataID>{9174515a-3173-4590-a5c8-8f8b81afe4d5}</MetaDataID>
        public RelayCommand RenameSelectedTaxableTypeCommand { get; protected set; }


        public RelayCommand AddTaxesContexCommand { get; protected set; }
        public RelayCommand DeleteSelectedTaxesContexCommand { get; }
        public RelayCommand RenameSelectedTaxesContexCommand { get; }
        public RelayCommand BeforeSaveCommand { get; }
        public RelayCommand DeleteTaxesContexCommand { get; protected set; }

        public RelayCommand RenameTaxesContexCommand { get; protected set; }

    }
}
