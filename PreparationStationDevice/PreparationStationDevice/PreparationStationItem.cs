using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if !FlavourBusinessDevice
using System.Windows.Controls;
#endif

using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.RoomService;
using OOAdvantech.MetaDataRepository;

namespace PreparationStationDevice
{
    /// <MetaDataID>{3608ab58-bcc2-49f4-8a76-cf28cea7ac97}</MetaDataID>
    public class PreparationStationItem
    {
        public List<Ingredient> Ingredients;

        public ItemPreparation ItemPreparation;

        public ServicePointPreparationItems ServicePointPreparationItems;
        public PreparationStationItem(ItemPreparation itemPreparation, ServicePointPreparationItems servicePointPreparationItems)
        {
            ItemPreparation = itemPreparation;


            if (ItemPreparation.MenuItem == null)
                ItemPreparation.LoadMenuItem();


            Ingredients = (from itemType in this.ItemPreparation.MenuItem.Types
                           from optionGroup in itemType.Options.OfType<MenuModel.IPreparationOptionsGroup>()
                           from option in optionGroup.GroupedOptions.OfType<MenuModel.IPreparationScaledOption>()
                           where option.IsRecipeIngredient&& option.InitialInRecipe(ItemPreparation.MenuItem)
                           select new Ingredient( option)).ToList();

            //foreach (var option in options.ToList())
            //{

            //    var menuItemSpecificOption = option.GetMenuItemSpecific(ItemPreparation.MenuItem);
            //    if (menuItemSpecificOption != null)
            //    {
            //        var index = options.IndexOf(option);
            //        options.Remove(option);
            //        options.Insert(index, menuItemSpecificOption);
            //    }
            //}

            //OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri()


            ServicePointPreparationItems = servicePointPreparationItems;
        }
    }


    /// <MetaDataID>{2f2bc3c7-b454-4da3-9b81-1bae2782f69f}</MetaDataID>
    public static class PreparationOptionExtras
    {
        public static bool InitialInRecipe(this MenuModel.IPreparationScaledOption preparationOption, MenuModel.IMenuItem menuItem)
        {



            if (preparationOption.Initial.DeclaringType.ZeroLevelScaleType && preparationOption.LevelType.Levels.IndexOf(preparationOption.GetInitialFor(menuItem)) == 0)
                return false;
            return true;
        }
    }
}
