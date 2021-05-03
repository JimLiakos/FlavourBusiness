using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{037d9268-aad2-4781-9f02-815f70ba6904}</MetaDataID>
    public class MealTypesViewModel : MarshalByRefObject, System.ComponentModel.INotifyPropertyChanged
    {

        public MealTypesViewModel()
        {
        }


        public MealTypesViewModel(ObjectStorage objectStorage)
        {
            ObjectStorage = objectStorage;

            AddMealTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    MenuModel.MealType mealType = new MenuModel.MealType(Properties.Resources.MealTypeDefaultName);
                    ObjectStorage.CommitTransientObjectState(mealType);
                    _MealTypes.Add(MealTypesDictionary.GetViewModelFor(mealType, mealType));
                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MealTypes)));

            });

            RenameSelectedMealTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                SelectedMealType.Edit = true;

            }, (object sender) => SelectedMealType != null);


            DeleteSelectedMealTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(SelectedMealType.MealType);

                MealTypesDictionary.Remove(SelectedMealType.MealType);
                _MealTypes.Remove(SelectedMealType);
                SelectedMealType = null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MealTypes)));

            }, (object sender) => CanBeDeletedSelectedMealType());

            if (MealTypes.Count > 0)
                _SelectedMealType = MealTypes[0];

        }

        private bool CanBeDeletedSelectedMealType()
        {
            if (_SelectedMealType == null || _SelectedMealType.MealType is MenuModel.FixedMealType)
                return false;
            else
                return true;
        }

        public ObjectStorage ObjectStorage { get; }



        public WPFUIElementObjectBind.RelayCommand AddMealTypeCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand RenameSelectedMealTypeCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand DeleteSelectedMealTypeCommand { get; protected set; }

        readonly ViewModelWrappers<MenuModel.IMealType, MealTypeViewModel> MealTypesDictionary = new ViewModelWrappers<MenuModel.IMealType, MealTypeViewModel>();



        List<MealTypeViewModel> _MealTypes;
        public List<MealTypeViewModel> MealTypes
        {
            get
            {

                if (_MealTypes == null)
                {
                    if (ObjectStorage != null)
                    {
                        OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage);
                        var selectedTypes = (from mealType in storage.GetObjectCollection<MenuModel.IMealType>() select mealType).ToList();
                        _MealTypes = (from theMealType in selectedTypes  select MealTypesDictionary.GetViewModelFor(theMealType, theMealType)).ToList();
                    }
                }
                return _MealTypes;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <exclude>Excluded</exclude>
        MealTypeViewModel _SelectedMealType;

        public MealTypeViewModel SelectedMealType
        {
            get
            {
                return _SelectedMealType;
            }
            set
            {
                _SelectedMealType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMealType)));
            }
        }

    }
}
