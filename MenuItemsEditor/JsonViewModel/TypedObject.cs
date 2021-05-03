using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuItemsEditor.JsonViewModel
{
    /// <MetaDataID>{2e20bbd8-2194-4dab-a91a-0a68b69e4b80}</MetaDataID>
    public class TypedObject
    {
        public string TypeName
        {
            get;set;
           
        }
        public TypedObject()
        {
            TypeName = GetType().Name;
        }


    }
}
