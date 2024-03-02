using Newtonsoft.Json;

namespace Henry.Scheduling.Api.Extensions
{
    public static class JsonExtensions
    {
        public static T FromJson<T>(this string json, JsonSerializerSettings? serializerSettings = null)
            where T : class
        {
            if (string.IsNullOrEmpty(json))
                return default;
            return JsonConvert.DeserializeObject<T>(json, serializerSettings);
        }

        public static string ToJson(this object obj, JsonSerializerSettings? serializerSettings = null)
        {
            var defaultSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return JsonConvert.SerializeObject(obj, Formatting.None, serializerSettings ?? defaultSerializerSettings);
        }
    }
}
