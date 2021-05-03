using System;
using System.Collections.Generic;
using System.Text;
using MenuModel;

namespace PreparationStationDevice
{
    /// <MetaDataID>{b2826fbf-4bd5-4f9f-a21f-e1f9cbb90cbf}</MetaDataID>
    public class Ingredient
    {
        IPreparationScaledOption PreparationScaledOption;
        public Ingredient(IPreparationScaledOption preparationScaledOption)
        {
            PreparationScaledOption = preparationScaledOption;
            Name = preparationScaledOption.FullName;
        }
        public String Name { get; set; }

    }
}
