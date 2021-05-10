using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuModel
{
    //public class Multilingual
    //{

    //    public Multilingual()
    //    {
    //    }

    //    public Multilingual(OOAdvantech.IMultilingualMember multilingual)
    //    {
    //        if (multilingual.DefaultLanguage != null)
    //            Def = multilingual.DefaultLanguage.Name;

    //        Values = new Dictionary<string, object>();
    //        foreach (System.Collections.DictionaryEntry entry in multilingual.Values)
    //            Values.Add((entry.Key as System.Globalization.CultureInfo).Name, entry.Value);
    //    }

    //    public string Def { get; set; }

    //    public Dictionary<string, object> Values { get; set; }


    //    public T GetValue<T>()
    //    {

    //        var cultureInfo = OOAdvantech.CultureContext.CurrentNeutralCultureInfo;
    //        bool useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
    //        object value = default(T);
    //        if (Values.TryGetValue(cultureInfo.Name, out value))
    //        {
    //            if (value is T)
    //                return (T)value;
    //            else
    //                return default(T);
    //        }
    //        else
    //        {
    //            if (useDefaultCultureValue && !string.IsNullOrWhiteSpace(Def))
    //            {
    //                if (Values.TryGetValue(Def, out value))
    //                {
    //                    if (value is T)
    //                        return (T)value;
    //                    else
    //                        return default(T);

    //                }
    //            }

    //            return default(T);
    //        }
    //    }
    //}
}
