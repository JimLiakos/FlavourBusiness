using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WPFUIElementObjectBind;

namespace Finance.ViewModel
{
    /// <MetaDataID>{18bb38f9-c1ee-40c4-974f-1c6874fd98ee}</MetaDataID>
    public class TaxOverrideViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{83d6f23c-97e0-4980-bae7-7afd2976a89f}</MetaDataID>
        public TaxOverrideViewModel()
        {

        }

        /// <MetaDataID>{b3d70222-b818-4b32-99da-c875a738ec37}</MetaDataID>
        public readonly FinanceFacade.ITax Tax;

        /// <MetaDataID>{51b25f5b-d7ec-4af2-9cff-715c1cb5c57b}</MetaDataID>
        private FinanceFacade.ITaxOverride TaxOverride;
        /// <MetaDataID>{490deec6-f603-41be-a0c0-0a53b9a1a2d6}</MetaDataID>
        FinanceFacade.ITaxesContext TaxesContext;
        /// <MetaDataID>{19064d85-1602-4ebb-9339-afcf88147803}</MetaDataID>
        public TaxOverrideViewModel(FinanceFacade.ITax tax, FinanceFacade.ITaxesContext taxesContext = null)
        {

            TaxOverride = taxesContext?.GetTaxOverride(tax);

            Tax = tax;
            TaxesContext = taxesContext;

            MaximizeCommand = new RelayCommand((object sender) =>
            {
                IsMaximized = !IsMaximized;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDisabledTextVisible)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTaxDataVisible)));

            });

            DeleteCommand = new RelayCommand((object sender) =>
             {
                 var ovrs = Tax.TaxOverrides.ToArray();

                 taxesContext.RemoveTaxOverride(TaxOverride);
                 TaxOverride = null;

                 ovrs = Tax.TaxOverrides.ToArray();

                 IsMaximized = false;
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridenOpacity)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteVisibility)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxRate)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountID)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDisabledTextVisible)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTaxDataVisible)));

             });
        }

        /// <MetaDataID>{cb5ef7c2-431d-4f24-8230-c01f4fa74c48}</MetaDataID>
        public Visibility DeleteVisibility
        {
            get
            {
                return TaxOverride != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool IsDisabledTextVisible
        {
            get
            {
                return IsMimized && !IsActive;
            }
        }
        public bool IsTaxDataVisible
        {
            get
            {
                return IsMimized && IsActive;
            }
        }



        /// <MetaDataID>{eadf6dc0-7701-4c38-9548-532ad2816d87}</MetaDataID>
        public double OverridenOpacity
        {
            get
            {
                return TaxOverride != null ? 1 : 0.5;
            }

        }

        /// <MetaDataID>{0a0b8acf-4870-40bf-a487-3aa4336ce1dc}</MetaDataID>
        public string Description
        {
            get
            {
                return Tax?.Description;
            }
            set
            {
                Tax.Description = value;
            }
        }
        /// <MetaDataID>{a58cedfa-93dc-4e2e-b5e4-0d07c9cc5603}</MetaDataID>
        public RelayCommand MaximizeCommand { get; set; }

        /// <MetaDataID>{0f28db74-45bf-4d29-94ea-4f73ffb77ec6}</MetaDataID>
        public RelayCommand DeleteCommand { get; set; }



        public bool IsActive
        {
            get
            {
                if (TaxOverride != null)
                    return TaxOverride.IsActive;

                if (Tax == null)
                    return false;
                return Tax.IsActive;
            }
            set
            {
                if (Tax.IsActive != value)
                {
                    if (TaxesContext != null)
                    {
                        if (TaxOverride == null)
                        {

                            TaxOverride = TaxesContext.GetTaxOverride(Tax, true);
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridenOpacity)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteVisibility)));

                        }
                        TaxOverride.IsActive = value;
                    }
                    else
                    {
                        Tax.IsActive = value;

                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDisabledTextVisible)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTaxDataVisible)));

                }
                else if (TaxOverride != null)
                {
                    TaxOverride.IsActive = false;
                }
            }
        }



        /// <MetaDataID>{11c4dba8-1248-451b-8b94-9113ffb2568d}</MetaDataID>
        public double TaxRate
        {
            get
            {
                if (TaxOverride != null)
                    return TaxOverride.TaxRate * 100;

                if (Tax == null)
                    return default(double);
                return Tax.TaxRate * 100;
            }
            set
            {
                if (Tax.TaxRate != value / 100)
                {
                    if (TaxOverride == null)
                    {

                        TaxOverride = TaxesContext.GetTaxOverride(Tax, true);
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridenOpacity)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteVisibility)));

                    }
                    TaxOverride.TaxRate = value / 100;

                }
            }
        }
        /// <MetaDataID>{85c9cc13-a7a0-4108-88d4-c4a97f823a7a}</MetaDataID>
        public decimal TaxFee
        {
            get
            {
                if (TaxOverride != null)
                    return TaxOverride.Fee;

                if (Tax == null)
                    return default(decimal);
                return Tax.Fee;


            }
            set
            {
                if (Tax.Fee != value)
                {
                    if (TaxOverride == null)
                    {

                        TaxOverride = TaxesContext.GetTaxOverride(Tax, true);
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridenOpacity)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteVisibility)));

                    }
                    TaxOverride.Fee = value;

                }
            }
        }


        /// <MetaDataID>{293cd3d4-a2c1-4c55-a509-8f7670be3b0d}</MetaDataID>
        public bool FeePerUnit
        {
            get
            {
                //if (!IsMaximized)
                //    return false;
                if (Tax == null)
                    return default(bool);
                return Tax.FeePerUnit;
            }
            set
            {
                Tax.FeePerUnit = value;
            }
        }

        //public string MaxMinImage
        //{
        //    get
        //    {
        //        "/MenuItemsEditor;component/Image/MaximizeWindow.png"
        //    }
        //}

        /// <MetaDataID>{b25c8f87-d7dd-4c7f-bc26-bf8e05c95fc3}</MetaDataID>
        public ImageSource MaxMinImage
        {
            get
            {
                if (this.IsMaximized)
                    return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FinanceWpf;component/Images/MinimizeWindow.png"));
                else
                    return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FinanceWpf;component/Images/MaximizeWindow.png"));
            }
        }

        /// <exclude>Excluded</exclude>
        double _AccountIDErrorBorder = 0;
        /// <MetaDataID>{cb10cfee-9e98-403c-af3a-2c7e00a3e914}</MetaDataID>
        public double AccountIDErrorBorder
        {
            get
            {
                return _AccountIDErrorBorder;
            }
            set
            {
                _AccountIDErrorBorder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountIDErrorBorder)));
            }
        }

        /// <MetaDataID>{0452af0b-7a4a-464d-a606-89b8db35a72e}</MetaDataID>
        public string AccountID
        {
            get
            {
                return TaxOverride?.AccountID;
            }
            set
            {
                if (Tax.AccountID != value)
                {
                    AccountIDErrorBorder = 0;
                    if (TaxOverride == null)
                    {
                        TaxOverride = TaxesContext.GetTaxOverride(Tax, true);
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridenOpacity)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteVisibility)));
                    }
                    TaxOverride.AccountID = value;


                }
            }
        }

        /// <MetaDataID>{79a55112-4241-4aa4-a77f-3a8a3297f66f}</MetaDataID>
        public bool IsMaximized { get; private set; }
        /// <MetaDataID>{49f4bb4d-bc04-4830-8be2-ce6208ab62be}</MetaDataID>
        public bool IsMimized
        {
            get
            {
                return !IsMaximized;

            }
        }

        /// <MetaDataID>{3635d997-13db-4b27-b115-d3fe70281400}</MetaDataID>
        internal bool Validate()
        {
            if (TaxOverride != null && (TaxOverride.TaxRate == Tax.TaxRate || string.IsNullOrWhiteSpace(TaxOverride.AccountID)))
            {
                IsMaximized = true;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDisabledTextVisible)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTaxDataVisible)));

                AccountIDErrorBorder = 1;
                return false;
            }
            else
            {
                AccountIDErrorBorder = 1;
                return true;
            }
        }
    }
}
