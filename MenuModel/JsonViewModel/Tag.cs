using OOAdvantech;
using OOAdvantech.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuModel.JsonViewModel
{
    /// <MetaDataID>{91c72d35-ecb4-41db-b7b9-3e877ec264c9}</MetaDataID>
    public class Tag : ITag
    {

        public Tag(ITag copy)
        {
            _Name = copy.MultilingualName;
        }
        public Tag()
        {
        }
        /// <exclude>Excluded</exclude>
        protected Multilingual _Name = new Multilingual();

        [JsonIgnore]
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

        public Multilingual MultilingualName { get => new Multilingual(_Name); set { _Name = value; } }
    }
}
