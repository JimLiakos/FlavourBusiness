using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
namespace MenuModel
{
    /// <MetaDataID>{52f7dd82-cbf1-44f7-afff-a648f960e989}</MetaDataID>
    public interface IPreparationOptionsGroup : MenuModel.IPreparationOption
    {


        /// <MetaDataID>{dcc3e5f9-e548-4adf-a1ee-a502ac4385a7}</MetaDataID>
        MenuModel.IPreparationScaledOption NewPreparationOption();



        /// <exclude>Excluded</exclude>
        SelectionType SelectionType
        {
            get;
            set;
        }
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [Association("OptionsGroup", Roles.RoleA, true, "40a09710-0820-4640-9989-655ea652dc97"), OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(2)]
        IList<MenuModel.IPreparationScaledOption> GroupedOptions
        {
            get;
        }

        /// <MetaDataID>{c56048bc-b5af-46f0-9e25-c1f7c07b3890}</MetaDataID>
        void AddPreparationOption(MenuModel.IPreparationScaledOption preparationOption);

        /// <MetaDataID>{9232b10d-0414-4593-8d55-3aa333e3aa91}</MetaDataID>
        void RemovePreparationOption(MenuModel.IPreparationScaledOption preparationOption);
        /// <MetaDataID>{07e6f4d6-0142-415b-80ab-5e2b71d3cb61}</MetaDataID>
        void MovePreparationOption(IPreparationScaledOption preparationOption, int newpos);

        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{796f4bff-8805-464a-aa11-bfaa22b35caa}</MetaDataID>
        void GroupedOptionChanged(IPreparationScaledOption preparationOption,IMenuItem menuItem);
    }
}
