using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Nodegem.Data.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void StoreAsJson<TProperty>(this PropertyBuilder<TProperty> builder)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            builder.HasConversion(
                v => JsonConvert.SerializeObject(v, settings),
                v => JsonConvert.DeserializeObject<TProperty>(v, settings)
            );
        }
    }
}