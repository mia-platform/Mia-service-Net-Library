using System.Linq;
using System.Reflection;

namespace MiaServiceDotNetLibrary.Environment
{
    public class ConfigValidator
    {
        public static void ValidateConfig(MiaEnvConfiguration configuration)
        {
            var props = configuration.GetType().GetProperties()
                .ToDictionary(
                    prop => prop.Name,
                    prop => (string) prop.GetValue(configuration)
                );

            foreach (var (key, value) in props)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidEnvConfigurationException($"Required environment variable not found: {key}.");
                }
            }
        }
    }
}
