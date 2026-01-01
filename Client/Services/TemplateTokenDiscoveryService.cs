using CrownATTime.Server.Models;
using System.Reflection;

namespace CrownATTime.Client
{
    public class TemplateTokenDiscoveryService
    {
        public static IReadOnlyList<TemplateTokenInfo> GetAvailableTokens()
        {
            var tokens = new List<TemplateTokenInfo>();

            AddTokens<ContactDtoResult.Item>("Contact", tokens);
            AddTokens<TicketDtoResult.Item>("Ticket", tokens);
            AddTokens<ResourceDtoResult>("Resource", tokens);

            // Optional built-ins
            tokens.Add(new TemplateTokenInfo
            {
                Source = "System",
                PropertyPath = "Now",
                Token = "{{Now}}",
                DisplayName = "Current Date/Time",
                PropertyType = typeof(DateTimeOffset)
            });

            return tokens
                .OrderBy(t => t.Source)
                .ThenBy(t => t.DisplayName)
                .ToList();
        }

        private static void AddTokens<T>(string prefix, List<TemplateTokenInfo> tokens)
        {
            var type = typeof(T);

            foreach (var prop in GetBrowsableProperties(type))
            {
                // Simple property
                tokens.Add(CreateToken(prefix, prop.Name, prop.PropertyType));

                // One-level nested object support (ex: Contact.Account.Name)
                if (IsExpandable(prop.PropertyType))
                {
                    foreach (var child in GetBrowsableProperties(prop.PropertyType))
                    {
                        tokens.Add(CreateToken(
                            prefix,
                            $"{prop.Name}.{child.Name}",
                            child.PropertyType
                        ));
                    }
                }
            }
        }

        private static TemplateTokenInfo CreateToken(string prefix, string path, Type type)
        {
            return new TemplateTokenInfo
            {
                Source = prefix,
                PropertyPath = path,
                Token = $"{{{{{prefix}.{path}}}}}",
                DisplayName = $"{SplitPascalCase(path)}",
                PropertyType = Nullable.GetUnderlyingType(type) ?? type
            };
        }

        private static IEnumerable<PropertyInfo> GetBrowsableProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    p.CanRead &&
                    !p.GetIndexParameters().Any() &&
                    !IsIgnoredType(p.PropertyType));
        }

        private static bool IsIgnoredType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return type == typeof(byte[]) ||
                   typeof(Stream).IsAssignableFrom(type);
        }

        private static bool IsExpandable(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return type.IsClass &&
                   type != typeof(string) &&
                   !type.Namespace!.StartsWith("System");
        }

        private static string SplitPascalCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // Remove leading "Item." or "item."
            if (input.StartsWith("Item.", StringComparison.OrdinalIgnoreCase))
                input = input.Substring(5);

            return System.Text.RegularExpressions.Regex
                .Replace(input, "([a-z])([A-Z])", "$1 $2")
                .Replace(".", " ");
        }

    }
}
