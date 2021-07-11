using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech;

namespace MenuModel.JsonViewModel
{
    /// <MetaDataID>{5c63f66d-f7fc-4a4c-ab6d-29de1c0d3cf5}</MetaDataID>
    public class OptionGroup : TypedObject, IPreparationOptionsGroup
    {


        public Multilingual MultilingualName
        {
            get => _Name; 
            set
            {
                _Name = value;
            }
        }

        /// <exclude>Excluded</exclude>
        protected Multilingual _Name = new Multilingual();

        public event ObjectChangeStateHandle ObjectChangeState;




        /// <MetaDataID>{36b4e726-1744-4bab-9b5d-1c59f3509b3d}</MetaDataID>
        public string Name
        {
            get
            {
                return _Name.GetValue<string>();
            }
            set
            {
                _Name.SetValue<string>(value);
            }
        }

        public bool HideName { get; set; }

        /// <MetaDataID>{28e78549-65a7-4839-b38c-7dc6125e3364}</MetaDataID>
        public List<Option> Options { get; set; }

        public List<OptionGroup> NestedOptionsGroups { get; set; }

        public SelectionType SelectionType { get; set; }

        /// <MetaDataID>{4a7fe91d-f2ce-4868-9d12-63ebd27fc19c}</MetaDataID>
        public bool CheckUncheck { get; set; }

        /// <MetaDataID>{2822855a-62a9-468f-bc8e-fa4cafbf7be8}</MetaDataID>
        public bool ItemSelectorOptionsGroup { get; set; }

        public bool IsStepOptionsGroups { get; set; }


        public string Uri { get; set; }

        public IList<IPreparationScaledOption> GroupedOptions => Options?.OfType<IPreparationScaledOption>().ToList();

        public IMenuItemType Owner => null;

        public IMenuItemType MenuItemType => null;

        public OptionGroup StepGroup { get; set; }

#if !FlavourBusinessDevice


        internal OptionGroup Init(IPreparationOptionsGroup optionGroup, Dictionary<object, object> mappedObject)
        {

            Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(optionGroup).GetPersistentObjectUri(optionGroup);
            ItemSelectorOptionsGroup = optionGroup is MenuModel.ItemSelectorOptionsGroup;
            CheckUncheck = optionGroup.SelectionType != MenuModel.SelectionType.SimpleGroup;

            _Name = new Multilingual(optionGroup.MultilingualName);
            mappedObject[optionGroup] = this;
            if (optionGroup is PreparationOptionsGroup && (optionGroup as PreparationOptionsGroup).Stepper != null)
                this.StepGroup = mappedObject[(optionGroup as PreparationOptionsGroup).Stepper] as OptionGroup;
            HideName = optionGroup.HideName;

            if (optionGroup is MenuModel.StepperOptionsGroup)
            {
                IsStepOptionsGroups = true;
                NestedOptionsGroups = (from nestedOptionGroup in (optionGroup as MenuModel.StepperOptionsGroup).NestedOptionsGroups
                                       select new JsonViewModel.OptionGroup().Init(nestedOptionGroup, mappedObject)).ToList();


            }


            Options = (from scaledOption in optionGroup.GroupedOptions
                       select JsonViewModel.Option.GetOption(scaledOption, mappedObject/*, menuItem*/)).ToList();
            foreach (var option in Options)
                option.OptionGroup = this;

            SelectionType = optionGroup.SelectionType;
            return this;
        }

        internal OptionGroup Init(List<IPreparationScaledOption> unGroupedOtions, Dictionary<object, object> mappedObject)
        {
            Uri = "";

            CheckUncheck = false;

            Options = (from scaledOption in unGroupedOtions
                       select JsonViewModel.Option.GetOption(scaledOption, mappedObject/*, menuItem*/)).ToList();
            return this;
        }


#endif
        public IPreparationScaledOption NewPreparationOption()
        {
            throw new NotImplementedException();
        }

        public void AddPreparationOption(IPreparationScaledOption preparationOption)
        {

        }

        public void RemovePreparationOption(IPreparationScaledOption preparationOption)
        {

        }

        public void MovePreparationOption(IPreparationScaledOption preparationOption, int newpos)
        {

        }

        public void GroupedOptionChanged(IPreparationScaledOption preparationOption, IMenuItem menuItem)
        {

        }
    }
}
