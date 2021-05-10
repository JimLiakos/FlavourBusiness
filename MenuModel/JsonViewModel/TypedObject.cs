using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuModel.JsonViewModel
{
    /// <MetaDataID>{2e20bbd8-2194-4dab-a91a-0a68b69e4b80}</MetaDataID>
    public class TypedObject
    {
        /// <MetaDataID>{3c57f49f-73b2-4d24-81bb-861fd0a3ed4b}</MetaDataID>
        public string TypeName
        {
            get; set;

        }
        /// <MetaDataID>{3a461895-1b0c-4ebe-9d8e-47fb5cd580d0}</MetaDataID>
        public TypedObject()
        {
            TypeName = GetType().Name;
        }


    }
}
