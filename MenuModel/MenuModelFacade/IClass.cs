using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{d4aba26f-6830-412c-ac44-ea922dfcaf36}</MetaDataID>
    public interface IClass
    {
        [Association("ClassifiedItem", Roles.RoleA,true, "08c2a04a-ed64-4ffb-b460-ba544fbeda1b")]
        IList<MenuModel.IClassified> ClassifiedItems { get; }

        /// <MetaDataID>{70e6215a-4bf8-4ebe-8464-cc39d78ff8ea}</MetaDataID>
        string Name { get; set; }

        /// <MetaDataID>{fede13f8-f187-4191-b1d8-2552b4e2622c}</MetaDataID>
        void AddClassifiedItem(IClassified classifiedItem);

        /// <MetaDataID>{ca42b958-2a74-4f3f-93cf-8eab6da7d648}</MetaDataID>
        void RemoveClassifiedItem(IClassified classifiedItem);

        void InsertClassifiedItem(int index, IClassified classifiedItem);
    }
}