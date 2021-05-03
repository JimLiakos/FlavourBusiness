using MenuModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{cc9887f6-d8c4-4c8b-b0e9-72561b378085}</MetaDataID>
    public class PartOfMealViewModel : MarshalByRefObject
    {

        public PartofMeal PartofMeal { get; }
        MenuItemViewModel MenuItem;

        public MealTypesViewModel MealTypesViewModel { get; private set; }

        public PartOfMealViewModel(PartofMeal partofMeal, MenuItemViewModel menuItem)
        {
            PartofMeal = partofMeal;
            MenuItem = menuItem;
        }
    }
}
