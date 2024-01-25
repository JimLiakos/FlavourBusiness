using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessManager.RoomService;
using MenuModel;
using OOAdvantech.MetaDataRepository;

namespace MenuItemsEditor.ViewModel
{


    public delegate void CultureChangeHandle(string newLanguage);

    public delegate void PreparationItemChangedHandle(ItemPreparation preparationItem, MenuModel.IMenuItem MenuItem);

    /// <MetaDataID>{b87abe4e-a090-4874-9b02-80faa27f979f}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface IMenuItemTSViewModel
    {

        FlavourBusinessManager.RoomService.ItemPreparation PreparationItem { get; }

        [GenerateEventConsumerProxy]
        event CultureChangeHandle CultureChange;

        [GenerateEventConsumerProxy]
        event PreparationItemChangedHandle PreparationItemChanged;

        string CurrentLanguage { get; }

        MenuModel.IMenuItem MenuItem { get; }

        MealType MealType { get; }

        string ExtraInfoJson { get; set; }

    }

    /// <MetaDataID>{81ddc8c1-f978-4cdd-a091-4816f29760a0}</MetaDataID>
    public class MenuItemTSViewModel : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IMenuItemTSViewModel
    {
        private MenuItemViewModel MenuItemViewModel;

        public MenuItemTSViewModel(MenuItemViewModel menuItemViewModel)
        {
            this.MenuItemViewModel = menuItemViewModel;
        }

        public string ExtraInfoJson { get; set; }

        public ItemPreparation PreparationItem
        {
            get
            {
                try
                {
                    //List<MenuModel.IMenuItem> menuFoodItems = new List<MenuModel.IMenuItem> { new ItemPreparation(MenuItemViewModel.MenuItem).MenuItem };

                    //var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefSerializeSettings;
                    //string jsonEx = OOAdvantech.Json.JsonConvert.SerializeObject(menuFoodItems, jSetttings);


                    return new ItemPreparation(MenuItemViewModel.MenuItem);
                }
                catch (Exception error)
                {

                    throw;
                }
            }
        }

        CultureInfo _SelectedCulture;
        public CultureInfo SelectedCulture
        {
            get => _SelectedCulture;
            internal set
            {
                if (_SelectedCulture != value)
                {
                    _SelectedCulture = value;
                    CultureChange?.Invoke(_SelectedCulture.Name);
                }
            }
        }

        public string CurrentLanguage
        {
            get
            {
                if (_SelectedCulture != null)
                    return _SelectedCulture.Name;
                else
                    return null;
            }
        }
        public MealType MealType 
        {
            get => MenuItemViewModel?.SelectedMealTypeViewModel?.MealType;
        }

        public IMenuItem MenuItem => new ItemPreparation(MenuItemViewModel.MenuItem).MenuItem;

        public event CultureChangeHandle CultureChange;

        public event PreparationItemChangedHandle PreparationItemChanged;

        internal void ItemChanged()
        {
            var itemPreparation = new ItemPreparation(MenuItemViewModel.MenuItem);
            PreparationItemChanged?.Invoke(itemPreparation, itemPreparation.MenuItem);
        }
    }
}
