using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if !FlavourBusinessDevice
using System.Windows.Controls;
#endif

using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.RoomService;
using MenuModel;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace PreparationStationDevice
{
    /// <MetaDataID>{3608ab58-bcc2-49f4-8a76-cf28cea7ac97}</MetaDataID>
    public class PreparationStationItem
    {
        public List<ITag> Tags { get; private set; }

        public List<Ingredient> Ingredients;

        public ItemPreparation ItemPreparation;



        public Multilingual MultilingualFullName
        {
            get
            {
                return new Multilingual(_FullName);
            }
            set
            {
                _FullName = value;
            }
        }

        /// <exclude>Excluded</exclude>
        Multilingual _Name = new Multilingual();


        public Multilingual MultilingualName
        {
            get
            {
                return new Multilingual(_Name);
            }
            set
            {
                _Name = value;
            }
        }

        /// <exclude>Excluded</exclude>
        Multilingual _FullName = new Multilingual();


        public ServicePointPreparationItems ServicePointPreparationItems;
        public PreparationStationItem(ItemPreparation itemPreparation, ServicePointPreparationItems servicePointPreparationItems, Dictionary<string, MenuModel.JsonViewModel.MenuFoodItem> menuItems, Dictionary<string, List<ITag>> itemsPreparationTags)
        {
            ItemPreparation = itemPreparation;



            if (ItemPreparation.MenuItem == null)
                ItemPreparation.LoadMenuItem(menuItems);

            MenuModel.JsonViewModel.MenuFoodItem menuFoodItem = ItemPreparation.MenuItem as MenuModel.JsonViewModel.MenuFoodItem;

            MultilingualFullName = menuFoodItem.MultilingualFullName;
            MultilingualName = menuFoodItem.MultilingualName;
            Tags = menuFoodItem.PreparationTags.ToList();
            if (itemsPreparationTags.ContainsKey(menuFoodItem.Uri))
                Tags.AddRange(itemsPreparationTags[menuFoodItem.Uri].Select(x => new MenuModel.JsonViewModel.Tag(x)).ToList());



            //ItemPreparation.MenuItem.Types

            Ingredients = (from optionGroup in (ItemPreparation.MenuItem as MenuModel.JsonViewModel.MenuFoodItem).ItemOptions
                           from option in optionGroup.GroupedOptions.OfType<MenuModel.IPreparationScaledOption>()
                           where option.InitialInRecipe(ItemPreparation.MenuItem)
                           select new Ingredient(option, ItemPreparation.MenuItem)).ToList();


            var extraIngredients = (from optionGroup in (ItemPreparation.MenuItem as MenuModel.JsonViewModel.MenuFoodItem).ItemOptions
                                    from option in optionGroup.GroupedOptions.OfType<MenuModel.IPreparationScaledOption>()
                                    where !option.InitialInRecipe(ItemPreparation.MenuItem)
                                    select new Ingredient(option, ItemPreparation.MenuItem)).ToList();


            foreach (var optionChange in itemPreparation.OptionsChanges.OfType<OptionChange>())
            {
                Ingredient ingredient = Ingredients.Where(x => x.PreparationScaledOption == optionChange.itemSpecificOption.Option).FirstOrDefault();
                if (ingredient != null)
                {
                    ingredient.Without = optionChange.Without;
                    if (!ingredient.Without)
                    {
                        ingredient.IsExtra = true;
                        ingredient.OptionChange = optionChange;
                        ingredient.GetMultilingualFullName();
                    }
                }

            }

            foreach (var extraIngredient in extraIngredients)
            {
                foreach (var optionChange in itemPreparation.OptionsChanges.OfType<OptionChange>())
                {
                    if (extraIngredient.PreparationScaledOption == optionChange.itemSpecificOption.Option)
                    {
                        extraIngredient.IsExtra = true;
                        extraIngredient.OptionChange = optionChange;
                        extraIngredient.GetMultilingualFullName();

                        Ingredients.Add(extraIngredient);
                    }
                }
            }

            foreach (var ingredient in Ingredients.ToList())
            {
                if (ingredient.Without && ingredient.PreparationScaledOption.OptionGroup != null && (ingredient.PreparationScaledOption.OptionGroup.SelectionType & MenuModel.SelectionType.SingleSelection) != 0)
                {
                    var optionChangeIgredient = Ingredients.Where(x => x.PreparationScaledOption.OptionGroup == ingredient.PreparationScaledOption.OptionGroup && x.IsExtra).FirstOrDefault();
                    if (optionChangeIgredient != null)
                    {
                        int pos = Ingredients.IndexOf(ingredient);
                        Ingredients.Remove(ingredient);
                        Ingredients.Remove(optionChangeIgredient);
                        Ingredients.Insert(pos, optionChangeIgredient);
                        optionChangeIgredient.IsExtra = false;

                    }
                }
            }

            var names = Ingredients.Select(x => x.Name).ToList();

            Tags.AddRange((from ingredient in Ingredients
                           from tag in ingredient.PreparationScaledOption.PreparationTags
                           select tag).ToList());



            //if (ItemPreparation.MenuItem == null)
            //    ItemPreparation.LoadMenuItem();


            //Ingredients = (from itemType in this.ItemPreparation.MenuItem.Types
            //               from optionGroup in itemType.Options.OfType<MenuModel.IPreparationOptionsGroup>()
            //               from option in optionGroup.GroupedOptions.OfType<MenuModel.IPreparationScaledOption>()
            //               where option.IsRecipeIngredient && option.InitialInRecipe(ItemPreparation.MenuItem)
            //               select new Ingredient(option)).ToList();

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
