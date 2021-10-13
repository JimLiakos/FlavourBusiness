using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{a5029aea-1f16-402d-8b23-3e958c492af4}</MetaDataID>
    public class TagViewModel
    {

        public TagViewModel(string name,int index)
        {
            Name = name;
            Index = index;
        }

        public string Name { get;  set; }

        public readonly int Index;
    }
}
