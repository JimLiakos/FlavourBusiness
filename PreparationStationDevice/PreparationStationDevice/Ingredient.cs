using System;
using System.Collections.Generic;
using System.Text;
using FlavourBusinessManager.RoomService;
using MenuModel;
using OOAdvantech.Json;

namespace PreparationStationDevice
{
    /// <MetaDataID>{b2826fbf-4bd5-4f9f-a21f-e1f9cbb90cbf}</MetaDataID>
    public class Ingredient
    {
        public readonly IPreparationScaledOption PreparationScaledOption;

        public readonly OOAdvantech.Multilingual MultilingualFullName;
        public readonly OOAdvantech.Multilingual MultilingualName;
        public OOAdvantech.Multilingual MultilingualNewLevelName;

        public Ingredient(IPreparationScaledOption preparationScaledOption)
        {
            PreparationScaledOption = preparationScaledOption;
            MultilingualName = preparationScaledOption.MultilingualName;
            MultilingualFullName = preparationScaledOption.MultilingualFullName;
        }



        public Ingredient(OptionChange optionChange)
        {
            OptionChange = optionChange;
            PreparationScaledOption = optionChange.itemSpecificOption.Option;
            MultilingualName = PreparationScaledOption.MultilingualName;
            MultilingualFullName = PreparationScaledOption.MultilingualFullName;
            MultilingualNewLevelName = optionChange.NewLevel.MultilingualName;

        }

        public String Name { get; set; }

        public bool Without { get; set; }

        public bool IsExtra { get; set; }

        [JsonIgnore]
        OptionChange _OptionChange;

        [JsonIgnore]
        public OptionChange OptionChange
        {
            get
            {

                return _OptionChange;
            }
            set
            {
                _OptionChange = value;
                MultilingualNewLevelName = _OptionChange.NewLevel.MultilingualName;
            }
        }
    }
}
