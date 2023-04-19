using FlavourBusinessFacade.ViewModel;
using OOAdvantech.Remoting;

using System.Collections.Generic;
using System.Text;

namespace TakeAwayApp
{
    /// <MetaDataID>{1dca0d48-ef5c-41ac-a964-127385b2e256}</MetaDataID>
    public interface ITakeAwayStation
    {

    }
    /// <MetaDataID>{efe40e2f-68a3-4ee7-afde-5cf1ffd4c62e}</MetaDataID>
    public class TakeAwayStation : MarshalByRefObject, ITakeAwayStation, IExtMarshalByRefObject, ILocalization
    {
        public string Language => throw new System.NotImplementedException();

        public string DefaultLanguage => throw new System.NotImplementedException();

        public string AppIdentity => throw new System.NotImplementedException();

        public string GetString(string langCountry, string key)
        {
            throw new System.NotImplementedException();
        }

        public string GetTranslation(string langCountry)
        {
            throw new System.NotImplementedException();
        }

        public void SetString(string langCountry, string key, string newValue)
        {
            throw new System.NotImplementedException();
        }
    }
}
