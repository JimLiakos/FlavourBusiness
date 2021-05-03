using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{cb6c46c1-2614-4e2f-81c6-19b203c14a5a}</MetaDataID>
    public class ScaleTypesViewModel : MarshalByRefObject, System.ComponentModel.INotifyPropertyChanged
    {

        CultureInfo _SelectedCulture;
        public CultureInfo SelectedCulture
        {
            get
            {
                if (_SelectedCulture == null)
                {
                    _SelectedCulture = Cultures.Where(x => x.Name == OOAdvantech.CultureContext.CurrentCultureInfo.Name).FirstOrDefault();
                    if (_SelectedCulture == null)
                        _SelectedCulture = Cultures.Where(x => x.Name == OOAdvantech.CultureContext.CurrentCultureInfo.Parent.Name).FirstOrDefault();
                }
                return _SelectedCulture;
            }
            set
            {
                _SelectedCulture = value;
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExtrasDescription)));
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public List<CultureInfo> Cultures
        {
            get
            {
                return CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
            }
        }
        public ScaleTypesViewModel()
        {

        }

        ObjectStorage ObjectStorage;
        public ScaleTypesViewModel(ObjectStorage objectStorage)
        {
            ObjectStorage = objectStorage;

            AddScaleTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    MenuModel.ScaleType scaleType = new MenuModel.ScaleType(Properties.Resources.ScaleTypeDefaultName);
                    ObjectStorage.CommitTransientObjectState(scaleType);

                    _ScaleTypes.Add(ScaleTypesDictionary.GetViewModelFor(scaleType, scaleType));

                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScaleTypes)));

            });

            RenameSelectedScaleTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                SelectedScaleType.Edit = true;

            }, (object sender) => SelectedScaleType != null);


            DeleteSelectedScaleTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(SelectedScaleType.ScaleType);

                _ScaleTypes.Remove(SelectedScaleType);
                SelectedScaleType = null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScaleTypes)));

            }, (object sender) => CanBeDeletedSelectedScaleType());

        }

        bool CanBeDeletedSelectedScaleType()
        {
            if (SelectedScaleType != null)
            {
                if (!(SelectedScaleType.ScaleType is MenuModel.FixedScaleType))
                {
                    var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(SelectedScaleType.ScaleType);
                    if (objectStorage == null || !objectStorage.HasReferencialintegrityConstrain(SelectedScaleType.ScaleType))
                        return true;
                }
            }
            return false;
        }

        public string nar { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        List<ScaleTypeViewModel> _ScaleTypes;
        public List<ScaleTypeViewModel> ScaleTypes
        {
            get
            {
                if(_ScaleTypes==null)
                {
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage);
                    var selectedTypes = (from scaleType in storage.GetObjectCollection<MenuModel.IScaleType>() select scaleType).ToList();
                    _ScaleTypes = (from theScaleType in selectedTypes select ScaleTypesDictionary.GetViewModelFor(theScaleType, theScaleType)).ToList();

                 }
                return _ScaleTypes;
            }
        }

        /// <exclude>Excluded</exclude>
        ScaleTypeViewModel _SelectedScaleType;

        public ScaleTypeViewModel SelectedScaleType
        {
            get
            {
                return _SelectedScaleType;
            }
            set
            {
                _SelectedScaleType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedScaleType)));
            }
        }

        public WPFUIElementObjectBind.RelayCommand AddScaleTypeCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand RenameSelectedScaleTypeCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand DeleteSelectedScaleTypeCommand { get; protected set; }


        ViewModelWrappers<MenuModel.IScaleType, ScaleTypeViewModel> ScaleTypesDictionary = new ViewModelWrappers<MenuModel.IScaleType, ScaleTypeViewModel>();
    }
}
