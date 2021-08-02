using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

using System.Linq;
using OOAdvantech;


namespace MenuModel
{
    /// <MetaDataID>{3d473b92-fe1d-47d5-b560-f1cbca807df5}</MetaDataID>
    [BackwardCompatibilityID("{3d473b92-fe1d-47d5-b560-f1cbca807df5}")]
    [Persistent()]
    public class PreparationOptionsGroup :MarshalByRefObject, IPreparationOptionsGroup,OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {

        /// <exclude>Excluded</exclude>
        Member<StepperOptionsGroup> _Stepper = new Member<StepperOptionsGroup>();
        [Association("StepperNestedOptionsGroups", Roles.RoleB, "ce53dbcd-e48a-4a4a-9087-b062d724a08f")]
        [RoleBMultiplicityRange(0, 1)]
        [PersistentMember(nameof(_Stepper))]
        public StepperOptionsGroup Stepper => _Stepper;

   

   


        /// <exclude>Excluded</exclude>
        bool _HideName;
        /// <MetaDataID>{7d9b25c6-ac15-4389-bd3a-b9963b2d38d4}</MetaDataID>
        [PersistentMember(nameof(_HideName))]
        [BackwardCompatibilityID("+5")]
        public bool HideName
        {
            get => _HideName;
            set
            {

                if (_HideName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _HideName = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }



        /// <MetaDataID>{28b4da33-a834-456a-ba99-a84003e86082}</MetaDataID>
        public PreparationOptionsGroup()
        {
            

            _SelectionType = SelectionType.SingleSelection;
        }
        /// <MetaDataID>{87ce1d46-54d6-47d7-8261-915edcca8c33}</MetaDataID>
        public PreparationOptionsGroup(string name):this()
        {
            _Name.Value = name;
        }

        public PreparationOptionsGroup(IPreparationOptionsGroup preparationOptionsGroup, IMenuItemType menuItemType)
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Name = new MultilingualMember<string>(preparationOptionsGroup.MultilingualName.Values);
                _Owner = menuItemType;
                _SelectionType = preparationOptionsGroup.SelectionType;
                _HideName = preparationOptionsGroup.HideName;

                if (preparationOptionsGroup is JsonViewModel.OptionGroup && (preparationOptionsGroup as JsonViewModel.OptionGroup).IsStepOptionsGroups)
                {

                }
                foreach (var option in preparationOptionsGroup.GroupedOptions)
                {
                    if (option is MenuModel.IPreparationScaledOption)
                    {
                        MenuModel.PreparationScaledOption preparationScaledOption = new MenuModel.PreparationScaledOption(option as MenuModel.IPreparationScaledOption, menuItemType);
                        AddPreparationOption(preparationScaledOption);
                    }
                }
                OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItemType);
                objectStorage.CommitTransientObjectState(this);

                stateTransition.Consistent = true;
            }


        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;



        /// <MetaDataID>{e3a5783e-66cc-46ea-80be-7238b6ba3bc3}</MetaDataID>
        public Multilingual MultilingualName => new Multilingual(_Name);


        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.MultilingualMember<string> _Name=new MultilingualMember<string>();
       
        /// <MetaDataID>{3f93a7f7-d50b-4d10-a97a-ae15d9594c73}</MetaDataID>
        [PersistentMember("_Name")]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name.Value = value;
                        if (string.IsNullOrWhiteSpace(value))
                            _Name.ClearValue();
                        stateTransition.Consistent = true;
                    }

                }
            }
        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IPreparationScaledOption> _GroupedOptions =new OOAdvantech.Collections.Generic.Set<IPreparationScaledOption>();


        /// <MetaDataID>{d589fe70-9663-424d-8e0a-871026ba668f}</MetaDataID>
        [PersistentMember("_GroupedOptions")]
        [BackwardCompatibilityID("+3")]
        public IList<IPreparationScaledOption> GroupedOptions
        {
            get
            {
                return _GroupedOptions.AsReadOnly();
            }
        }

        /// <MetaDataID>{9ee78248-3db6-4617-89ce-f6286c3e3334}</MetaDataID>
        public void MovePreparationOption(IPreparationScaledOption preparationOption, int newpos)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _GroupedOptions.Remove(preparationOption);
                _GroupedOptions.Insert(newpos, preparationOption);
                stateTransition.Consistent = true;
            }
        }


        /// <exclude>Excluded</exclude>
       protected  SelectionType _SelectionType;

        /// <MetaDataID>{ab8d111e-d7e5-4f9d-b1d9-8bd6875b5aba}</MetaDataID>
        [PersistentMember("_SelectionType")]
        [BackwardCompatibilityID("+2")]
        public virtual SelectionType SelectionType
        {
            get
            {
                return _SelectionType;
            }
            set
            {
                if (_SelectionType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SelectionType = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(SelectionType));
                    bool uncheckedAll = true;
                    foreach (var option in GroupedOptions)
                    {
                        if (!option.Initial.UncheckOption)
                            uncheckedAll = false;
                    }
                    if (uncheckedAll && ((SelectionType & MenuModel.SelectionType.AtLeastOneSelected) != 0) && GroupedOptions.Count > 0)
                    {
                        GroupedOptions[0].Initial = GroupedOptions[0].LevelType.Levels[1];
                        ObjectChangeState?.Invoke(this, nameof(GroupedOptions));
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        MenuModel.IMenuItemType _Owner;

        /// <MetaDataID>{9abd56ba-3b93-4b29-9e85-88b085ed5b13}</MetaDataID>
        [PersistentMember("_Owner")]
        [BackwardCompatibilityID("+4")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public MenuModel.IMenuItemType Owner
        {
            get
            {
                return _Owner;
            }
        }

        /// <MetaDataID>{0a8632a0-18d5-425b-9683-1169f30415f6}</MetaDataID>
        public IMenuItemType MenuItemType
        {
            get
            {
                if (Stepper != null)
                    return Stepper.Owner;
                else
                    return Owner;
               
            }
        }

        

        /// <MetaDataID>{bbb2f5fa-2653-48c2-aa6b-32bc228f2a50}</MetaDataID>
        virtual public void AddPreparationOption(IPreparationScaledOption preparationOption)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _GroupedOptions.Add(preparationOption);
                stateTransition.Consistent = true;
            }

            (MenuItemType as MenuItemType).OnPreparationOptionAdded(preparationOption as ItemSelectorOption);
        }

        /// <MetaDataID>{e2a5b414-72f2-4967-a057-c4d5f020f1ee}</MetaDataID>
        public virtual void RemovePreparationOption(MenuModel.IPreparationScaledOption preparationOption)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _GroupedOptions.Remove(preparationOption);
                stateTransition.Consistent = true;
            }
            (MenuItemType as MenuItemType).OnPreparationOptionRemoved(preparationOption as ItemSelectorOption);
        }

        /// <MetaDataID>{617e3887-fc64-4a5f-a282-70502f828ac6}</MetaDataID>
        public void OnCommitObjectState()
        {

        }

        /// <MetaDataID>{b1a491d6-717a-4b2e-b528-56cc96f81b91}</MetaDataID>
        public void OnActivate()
        {
            //foreach(var preparationOption in _GroupedOptions.OfType<PreparationScaledOption>() )
            //{
            //    preparationOption.ObjectStateChanged += PreparationOption_ObjectStateChanged;
            //}
        }

        /// <MetaDataID>{6db6434b-6ad7-4a30-bcec-d2e1f3262ee9}</MetaDataID>
        public void GroupedOptionChanged(IPreparationScaledOption preparationOption, IMenuItem menuItem)
        {
            if (!preparationOption.LevelType.Levels[0].UncheckOption)
            {
                SelectionType = MenuModel.SelectionType.SimpleGroup;
                //foreach (var groupedOption in GroupedOptions)
                //{
                //    if (groupedOption != preparationOption)
                //        groupedOption.SetInitialFor(menuItem, groupedOption.LevelType.Levels[0]);
                //}
            }

            if (!preparationOption.GetInitialFor(menuItem).UncheckOption && ((SelectionType & MenuModel.SelectionType.SingleSelection) != 0))
            {
                foreach (var option in GroupedOptions)
                {
                    if (option != preparationOption && !option.GetInitialFor(menuItem).UncheckOption)
                        option.SetInitialFor(menuItem, option.LevelType.Levels[0]);
                }
            }
            bool uncheckedAll = true;
            foreach (var option in GroupedOptions)
            {
                if (!option.GetInitialFor(menuItem).UncheckOption)
                    uncheckedAll = false;
            }
            if (uncheckedAll && ((SelectionType & MenuModel.SelectionType.AtLeastOneSelected) != 0) && GroupedOptions.Count > 0)
            {
                foreach (var groupedOption in GroupedOptions)
                {
                    if (!groupedOption.IsHiddenFor(menuItem))
                    {
                        groupedOption.SetInitialFor(menuItem, GroupedOptions[0].LevelType.Levels[1]);
                        break;
                    }
                }
            }

            ObjectChangeState?.Invoke(this, nameof(GroupedOptions));
        }

        /// <MetaDataID>{2976c7f2-ebfb-4862-b0dd-8de3bc3c82d8}</MetaDataID>
        public virtual IPreparationScaledOption NewPreparationOption()
        {
            IPreparationScaledOption option = new PreparationScaledOption();

            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            if (objectStorage != null)
                objectStorage.CommitTransientObjectState(option);


            AddPreparationOption(option);
            return option;
        }
 
        
        /// <MetaDataID>{fbb96838-fae5-484f-87e4-231f3dcb3b5d}</MetaDataID>
        public void OnDeleting()
        {

        }
        /// <MetaDataID>{f6a44dae-e653-40bc-a08a-87fe09058720}</MetaDataID>
        public void BeforeCommitObjectState()
        {
        }
        /// <MetaDataID>{2af8019b-6de4-46cd-b3b2-5b93f3162905}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {

        }

        /// <MetaDataID>{414e5538-1f14-488d-ae72-766f1b48c1f0}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {

        }
        //bool SuspendOptionsGroupSelctionModelUpdate;
        //private void PreparationOption_ObjectStateChanged(object _object, string member)
        //{
        //    if (SuspendOptionsGroupSelctionModelUpdate)
        //        return;

        //    SuspendOptionsGroupSelctionModelUpdate = true;
        //    try
        //    {
        //        IPreparationScaledOption preparationScaledOption = _object as IPreparationScaledOption;
        //        if (preparationScaledOption.Initial != null && preparationScaledOption.Initial.UncheckOption)
        //        {
        //            //if (SelectionType.MultiSelection)
        //            //    foreach (var preparationOption in _GroupedOptions.OfType<PreparationScaledOption>())
        //            //    {

        //            //    }
        //        }
        //    }
        //    finally
        //    {
        //        SuspendOptionsGroupSelctionModelUpdate = false;
        //    }
        //}
    }
}