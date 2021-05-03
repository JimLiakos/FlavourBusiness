using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuItemsEditor
{
    /// <MetaDataID>{eed317a5-9c35-4378-afa8-db4fa3d73b7d}</MetaDataID>
    public class Translator : WPFUIElementObjectBind.ITranslator
    {
        public string TranslateString(string strSource, string languageCode)
        {
            return BingAPILibrary.Translator.TranslateString(strSource, languageCode);
        }
    }
}
