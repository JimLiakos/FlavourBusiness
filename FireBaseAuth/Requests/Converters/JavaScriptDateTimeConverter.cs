using Newtonsoft.Json;
using System;

namespace Firebase.Auth.Requests.Converters
{
    /// <MetaDataID>{7219d556-f7e7-49b5-8267-c6133f1edf08}</MetaDataID>
    internal class JavaScriptDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = long.Parse((string)reader.Value);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddMilliseconds(t);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
