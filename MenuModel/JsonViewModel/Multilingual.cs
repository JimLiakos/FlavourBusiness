using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuModel.JsonViewModel
{
    public class Multilingual
    {

        public Multilingual()
        {
        }

        public Multilingual(OOAdvantech.IMultilingualMember multilingual)
        {
            if (multilingual.DefaultLanguage != null)
                Def = multilingual.DefaultLanguage.Name;

            Values = new Dictionary<string, object>();
            foreach (System.Collections.DictionaryEntry entry in multilingual.Values)
                Values.Add((entry.Key as System.Globalization.CultureInfo).Name, entry.Value);
        }

        public string Def { get; set; }

        public Dictionary<string, object> Values { get; set; }
    }
}
