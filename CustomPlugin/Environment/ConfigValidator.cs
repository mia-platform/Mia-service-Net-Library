using System.Linq;

namespace CustomPlugin.Environment
{
    public class ConfigValidator
    {
        public static void ValidateConfig(MiaEnvConfiguration configuration)
        {
            if (IsConfigInvalid(configuration))
            {
                throw new InvalidEnvConfigurationException("Required environment variable not found");
            }
        }

        private static bool IsConfigInvalid(MiaEnvConfiguration configuration)
        {
            return configuration.GetType().GetProperties()
                .Where(pi => pi.PropertyType == typeof(string))
                .Select(pi => (string) pi.GetValue(configuration))
                .Any(string.IsNullOrEmpty);
        }
    }
}