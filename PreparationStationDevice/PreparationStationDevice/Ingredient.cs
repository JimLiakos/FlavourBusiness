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

        public OOAdvantech.Multilingual MultilingualFullName;
        public OOAdvantech.Multilingual MultilingualName;
        public OOAdvantech.Multilingual MultilingualNewLevelName;

        public Ingredient(IPreparationScaledOption preparationScaledOption)
        {
            PreparationScaledOption = preparationScaledOption;
            MultilingualName = preparationScaledOption.MultilingualName;
            GetMultilingualFullName();
            //MultilingualFullName = preparationScaledOption.MultilingualFullName;
        }



        public Ingredient(OptionChange optionChange)
        {
            OptionChange = optionChange;
            PreparationScaledOption = optionChange.itemSpecificOption.Option;
            MultilingualName = PreparationScaledOption.MultilingualName;
            GetMultilingualFullName();
            MultilingualNewLevelName = optionChange.NewLevel.MultilingualName;

        }

        public void GetMultilingualFullName()
        {
            MultilingualFullName = new OOAdvantech.Multilingual();
            MultilingualFullName.Def = MultilingualName.Def;
            foreach (var entry in MultilingualName.Values)
            {

                using (OOAdvantech.CultureContext context = new OOAdvantech.CultureContext(System.Globalization.CultureInfo.GetCultureInfo(entry.Key), true))
                {
                    if (PreparationScaledOption.MultilingualFullName.Values.ContainsKey(entry.Key))
                        MultilingualFullName.SetValue(PreparationScaledOption.MultilingualFullName.GetValue<string>());
                    else
                    {
                        if (PreparationScaledOption.AutoGenFullName)
                            MultilingualFullName.SetValue(PreparationScaledOption.OptionGroup.MultilingualName.GetValue<string>() + " " + entry.Value);
                        else
                            MultilingualFullName.SetValue(entry.Value);
                    }
                }
            }
        }


        public String Name
        {
            get
            {
                if (IsExtra && (this.PreparationScaledOption.OptionGroup==null|| this.PreparationScaledOption.OptionGroup.SelectionType == SelectionType.SimpleGroup))
                    return MultilingualNewLevelName.GetValue<string>() + " " + MultilingualFullName.GetValue<string>();
                return MultilingualFullName.GetValue<string>();
            }
            set
            {
            }
        }

        public bool Without { get; set; }

        public bool IsExtra { get; set; }

        public bool IsCheckBoxOption
        {
            get => this.PreparationScaledOption.OptionGroup==null|| this.PreparationScaledOption.OptionGroup.SelectionType != SelectionType.SimpleGroup;
            set
            {
            }
        }

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
