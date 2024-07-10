using FlavourBusinessFacade.ViewModel;
using OOAdvantech.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessManager.Localization
{
    /// <MetaDataID>{1dd141bf-ff30-481f-8771-07d9861e4558}</MetaDataID>
    public class I18nLocalization: ILocalization
    {
        /// <MetaDataID>{f6ef7bbf-efbb-482d-b20d-ad5507942c5e}</MetaDataID>
        public I18nLocalization(string translationsEmbeddedResourcesPath)
        {

            TranslationsEmbeddedResourcesPath = translationsEmbeddedResourcesPath;
        }

        /// <MetaDataID>{27cb4f87-9ccb-45c8-bb9d-c4f9194f513e}</MetaDataID>
        static public I18nLocalization Current { get; } = new I18nLocalization("i18n");
        /// <MetaDataID>{28ebb9ab-73d2-4989-b514-d03cd3f19833}</MetaDataID>
        public string GetString(string langCountry, string key)
        {
            if (string.IsNullOrWhiteSpace(langCountry))
                langCountry = DefaultLanguage;
            JObject jObject = null;
            if (!Translations.TryGetValue(langCountry, out jObject))
            {
                GetTranslation(langCountry);
                if (Translations.ContainsKey(langCountry))
                    jObject = Translations[langCountry];
                else
                    jObject = Translations[DefaultLanguage];
            }

            var keyParts = key.Split('.');
            int i = 0;
            foreach (string member in keyParts)
            {
                if (jObject == null)
                    return null;
                JToken jToken = null;
                if (i == keyParts.Length - 1)
                {
                    if (jObject.TryGetValue(member, out jToken))
                    {
                        if (jToken is JValue)
                            return (jToken as JValue).Value as string;
                    }
                    return null;
                }

                if (jObject.TryGetValue(member, out jToken))
                {
                    jObject = jToken as JObject;

                }
                else
                    return null;
                i++;
            }

            return null;
        }


        /// <MetaDataID>{b9aa7b69-ee7c-4c79-b9c9-f44bbfcfcff9}</MetaDataID>
        public void SetString(string langCountry, string key, string newValue)
        {
            JObject jObject = null;
            if (!Translations.TryGetValue(langCountry, out jObject))
            {
                GetTranslation(langCountry);
                jObject = Translations[langCountry];

            }

            var keyParts = key.Split('.');
            int i = 0;
            foreach (string member in keyParts)
            {
                if (jObject == null)
                    return;
                JToken jToken = null;
                if (i == keyParts.Length - 1)
                {
                    if (jObject.TryGetValue(member, out jToken))
                    {
                        if (jToken is JValue)
                            (jToken as JValue).Value = newValue;
                    }
                    else
                        jObject.Add(member, new JValue(newValue));
                }
                else
                {
                    if (jObject.TryGetValue(member, out jToken))
                        jObject = jToken as JObject;
                    else
                    {
                        jObject.Add(member, new JObject());
                        jObject = jObject[member] as JObject;
                    }
                }
                i++;
            }

        }

        /// <MetaDataID>{78e3b85a-cefd-4a92-aef3-66ad79121d13}</MetaDataID>
        Dictionary<string, JObject> Translations = new Dictionary<string, JObject>();

        /// <exclude>Excluded</exclude>
        string lan = "el";// OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;


        /// <MetaDataID>{0a8b8bfb-7066-4d62-9c22-6093e6b991ba}</MetaDataID>
        public string Language { get { return lan; } }

        /// <exclude>Excluded</exclude>
        string deflan = "en";


        /// <MetaDataID>{9c2a1a2d-e681-43d3-b2ea-9690b940a2ff}</MetaDataID>
        public string DefaultLanguage { get { return deflan; } }

        /// <MetaDataID>{9ff5893f-629d-460d-b505-8a6097939376}</MetaDataID>
        public string AppIdentity => throw new NotImplementedException();

        /// <MetaDataID>{ca653842-e328-4f94-8e9c-94ada0ef69f8}</MetaDataID>
        public string TranslationsEmbeddedResourcesPath { get; }

        /// <MetaDataID>{daa58ef0-acd5-443f-8fde-ebd20b3d3ec0}</MetaDataID>
        public string GetTranslation(string langCountry)
        {
            if (Translations.ContainsKey(langCountry))
                return Translations[langCountry].ToString();
            string json = "{}";
            var assembly = Assembly.GetExecutingAssembly();

            string path = TranslationsEmbeddedResourcesPath;

            string jsonName = assembly.GetManifestResourceNames().Where(x => x.Contains(path) && x.Contains(langCountry + ".json")).FirstOrDefault();
            if (jsonName == null)
            {
                langCountry = DefaultLanguage;
                jsonName = assembly.GetManifestResourceNames().Where(x => x.Contains(path) && x.Contains(langCountry + ".json")).FirstOrDefault();
            }

            //string jsonName = assembly.GetManifestResourceNames().Where(x => x.Contains("WaiterApp.WPF.i18n") && x.Contains(langCountry + ".json")).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(jsonName))
            {
                using (var reader = new System.IO.StreamReader(assembly.GetManifestResourceStream(jsonName), Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                    Translations[langCountry] = JObject.Parse(json);
                    // Do something with the value
                }
            }
            return json;

        }


    }
}
