using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessManager.RoomService;
using MenuModel;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;
using UIBaseEx;

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

        string ExtraInfoHtml { get; set; }

        List<IMenuStyleSheet> MenusStyleSheets { get; }

        Task<ExtraInfoStyleSheet> GetExtraInfoStyleSheet(IMenuStyleSheet menuStyleSheet);

        IMenuStyleSheet ActiveMenuStyleSheet { get; set; }

    }

    /// <MetaDataID>{81ddc8c1-f978-4cdd-a091-4816f29760a0}</MetaDataID>
    public class MenuItemTSViewModel : ExtMarshalByRefObject, IMenuItemTSViewModel
    {
        private MenuItemViewModel MenuItemViewModel;

        public MenuItemTSViewModel(MenuItemViewModel menuItemViewModel)
        {
            this.MenuItemViewModel = menuItemViewModel;

            _ExtraInfoJson = menuItemViewModel.MenuItem.ItemInfo;

        }

        public List<IMenuStyleSheet> MenusStyleSheets
        {
            get
            {
                return MenuItemViewModel.OrganizationMenusStyleSheets?.StyleSheets;
            }
        }

        string _ExtraInfoJson;
        public string ExtraInfoHtml
        {
            get => _ExtraInfoJson;// MenuItemViewModel.MenuItem.ItemInfo; 
            set
            {
                _ExtraInfoJson = value;
                MenuItemViewModel.MenuItem.ItemInfo = value;
                ItemChanged();

            }
        }



        IMenuStyleSheet _ActiveMenuStyleSheet;
        public IMenuStyleSheet ActiveMenuStyleSheet
        {
            get => _ActiveMenuStyleSheet; 
            set
            {
                _ActiveMenuStyleSheet = value;
            }
        }



        public async Task<ExtraInfoStyleSheet> GetExtraInfoStyleSheet(IMenuStyleSheet menuStyleSheet)
        {
            if (menuStyleSheet == null)
                return null;
            var styleSheet = await menuStyleSheet?.StyleSheet;

            IMenuItemStyle menuItemStyle = (styleSheet?.Styles["menu-item"] as IMenuItemStyle);

            var extraInfoStyleSheet = new ExtraInfoStyleSheet() { HeadingFont = menuItemStyle.ItemInfoHeadingFont, ParagraphFont = menuItemStyle.ItemInfoParagraphFont };


            return extraInfoStyleSheet;
        }


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

    public class ExtraInfoStyleSheet
    {
        public FontData HeadingFont { get; set; }

        public FontData ParagraphFont { get; set; }
    }
}
