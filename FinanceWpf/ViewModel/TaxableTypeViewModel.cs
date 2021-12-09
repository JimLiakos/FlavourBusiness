using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUIElementObjectBind;

namespace Finance.ViewModel
{
    /// <MetaDataID>{64e21e03-560a-46a9-95ec-d25b6c3443ce}</MetaDataID>
    [OOAdvantech.Transactions.Transactional]
    public class TaxableTypeViewModel : MarshalByRefObject, INotifyPropertyChanged
    {

        public TaxableTypeViewModel()
        {

        }
        public readonly FinanceFacade.ITaxableType TaxableType;
        public TaxableTypeViewModel(FinanceFacade.ITaxableType taxableType)
        {
            TaxableType = taxableType;
            _Taxes = new OOAdvantech.Collections.Generic.Set<TaxViewModel>(TaxableType.Taxes.Select(x => new TaxViewModel(x)).ToList());

            AddTaxCommand = new RelayCommand((object sender) =>
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Taxes.Add(new TaxViewModel(TaxableType.NewTax())); 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Taxes)));
            });
            DeleteSelectedTaxCommand = new RelayCommand((object sender) =>
            {
                _Taxes.Remove(_SelectedTax);
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {

                    TaxableType.RemoveTax(_SelectedTax.Tax);

                    stateTransition.Consistent = true;
                }


                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Taxes)));
            }, (object sender) => _SelectedTax != null);

        }


        public string Description
        {
            get
            {
                return TaxableType?.Description;
            }
            set
            {
                TaxableType.Description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<TaxViewModel> _Taxes;
        public List<TaxViewModel> Taxes
        {
            get
            {

                return _Taxes.ToThreadSafeList();
            }

        }
        /// <exclude>Excluded</exclude>
        TaxViewModel _SelectedTax;
        public TaxViewModel SelectedTax
        {
            get
            {
                return _SelectedTax;
            }
            set
            {
                _SelectedTax = value;
            }
        }

        public RelayCommand AddTaxCommand { get; protected set; }


        public RelayCommand DeleteSelectedTaxCommand { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
