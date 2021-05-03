using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuItemsEditor.JsonViewModel
{
    /// <MetaDataID>{5c63f66d-f7fc-4a4c-ab6d-29de1c0d3cf5}</MetaDataID>
    public class OptionGroup: TypedObject
    {
        public string Name { get; set; }

        public List<Option> Options { get; set; }

        public bool CheckUncheck { get; set; }

        public bool ItemSelectorOptionsGroup { get; set; }
    }
}
