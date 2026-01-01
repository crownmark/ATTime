using CrownATTime.Server.Models;
using CrownATTime.Server.Models.ATTime;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Net;

namespace CrownATTime.Client
{
    public class EmailService
    {
        private readonly Uri baseUri;
        private readonly HttpClient httpClient;
        private readonly NavigationManager navigationManager;
        private readonly JsonSerializerOptions jsonOptions;
        public EmailService(
            NavigationManager navigationManager,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;

            this.baseUri = new Uri($"{navigationManager.BaseUri}");

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        private static string BuildSignature(string displayName) => $@"
<p>
    Sincerely,<br /><br />
    {displayName}<br />
    Crown Enterprises Support Team<br />
    Phone: (209) 390-4670<br />
    Email: support@ce-technology.com<br />
    <a href='https://customerportal.crown.software'>Customer Portal & Chat</a><br /><br />


    <img src=""https://ww5.autotask.net/custData/210271/images/CrownWideTransparent140.png""
         alt=""Crown IT Support Team""
         style=""max-width:250px;height:auto;"" />
</p>";

        public async Task SendEmail(CrownATTime.Server.Models.EmailMessage emailMessage, ResourceCache resource)
        {
            var fullBody = $"{emailMessage.Body}{BuildSignature($"{resource.FullName}")}";
            emailMessage.Body = fullBody;
            var uri = new Uri(baseUri, $"Email/SendEmailFromSupport");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            
            var json = JsonSerializer.Serialize(emailMessage, jsonOptions);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(httpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                
            }
            else
            {
                throw new Exception($"Error Sending Email.  {content}");
            }
        }

        public static string ToEmailCsv(IEnumerable<string>? emails)
        {
            return string.Join(", ",
                emails?.Where(e => !string.IsNullOrWhiteSpace(e))
                ?? Enumerable.Empty<string>());
        }

        public static IReadOnlyDictionary<string, IReadOnlyDictionary<int, string>> BuildPicklistMaps(IEnumerable<TicketEntityPicklistValueCache> rows)
        {
            return rows
                .Where(r => !string.IsNullOrWhiteSpace(r.PicklistName))
                .GroupBy(r => r.PicklistName, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var dict = new Dictionary<int, string>();

                        foreach (var r in g)
                        {
                            // Prefer ValueInt
                            int? key = r.ValueInt;

                            // Fallback: parse Value if ValueInt is null
                            if (!key.HasValue && !string.IsNullOrWhiteSpace(r.Value) && int.TryParse(r.Value, out var parsed))
                                key = parsed;

                            if (!key.HasValue) continue;                    // can't map
                            if (string.IsNullOrWhiteSpace(r.Label)) continue;

                            // last write wins (or keep first—your choice)
                            dict[key.Value] = r.Label;
                        }

                        return (IReadOnlyDictionary<int, string>)dict;
                    }
                );
        }



        // {{Contact.FirstName}}, {{Ticket.Id}}, {{Now:yyyy-MM-dd}}
        private static readonly Regex TokenRegex =
            new(@"\{\{\s*(?<expr>[^}:]+)(?::(?<fmt>[^}]+))?\s*\}\}",
                RegexOptions.Compiled | RegexOptions.CultureInvariant);

        //public static string Render(string template, TemplateContext ctx)
        //{
        //    if (string.IsNullOrEmpty(template)) return template;

        //    return TokenRegex.Replace(template, m =>
        //    {
        //        var expr = m.Groups["expr"].Value.Trim(); // e.g. Contact.FirstName
        //        var fmt = m.Groups["fmt"].Success ? m.Groups["fmt"].Value.Trim() : null;

        //        var value = Resolve(expr, ctx);

        //        if (value is null) return ""; // or return m.Value to leave token in place

        //        if (!string.IsNullOrEmpty(fmt))
        //        {
        //            // date/number formatting support
        //            if (value is IFormattable f)
        //                return f.ToString(fmt, System.Globalization.CultureInfo.InvariantCulture);
        //        }

        //        return value.ToString() ?? "";
        //    });
        //}

        public static string Render(string template, TemplateContext ctx)
        {
            if (string.IsNullOrEmpty(template)) return template;

            return TokenRegex.Replace(template, m =>
            {
                var expr = m.Groups["expr"].Value.Trim(); // e.g. Ticket.status
                var fmt = m.Groups["fmt"].Success ? m.Groups["fmt"].Value.Trim() : null;

                var value = Resolve(expr, ctx);

                // ✅ Picklist translation for Ticket.status / Ticket.priority
                value = TryTranslatePicklist(expr, value, ctx);

                if (value is null) return "";

                if (!string.IsNullOrEmpty(fmt) && value is IFormattable f)
                    return f.ToString(fmt, System.Globalization.CultureInfo.InvariantCulture);

                return value.ToString() ?? "";
            });
        }

        private static object? TryTranslatePicklist(string expr, object? value, TemplateContext ctx)
        {
            if (value is null) return null;
            if (ctx.Picklists is null) return value;

            // Only translate the fields you want (accept both styles)
            if (!IsTicketPicklistExpr(expr))
                return value;

            if (!TryToInt(value, out var intVal))
                return value;

            // Normalize: "Ticket.priority" -> "priority"
            var key = NormalizePicklistKey(expr);

            // Try match against stored keys ("priority"), but also fall back to original ("Ticket.priority")
            if ((ctx.Picklists.TryGetValue(key, out var map) || ctx.Picklists.TryGetValue(expr, out map)) &&
                map.TryGetValue(intVal, out var label))
            {
                return label;
            }

            return value;
        }

        private static bool IsTicketPicklistExpr(string expr)
        {
            // Normalize to just the field name for comparison
            var key = NormalizePicklistKey(expr);

            return key.Equals("status", StringComparison.OrdinalIgnoreCase)
                || key.Equals("priority", StringComparison.OrdinalIgnoreCase)
                || key.Equals("issuetype", StringComparison.OrdinalIgnoreCase)
                || key.Equals("subissuetype", StringComparison.OrdinalIgnoreCase)
                || key.Equals("source", StringComparison.OrdinalIgnoreCase)
                || key.Equals("queueid", StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizePicklistKey(string expr)
        {
            expr = (expr ?? "").Trim();

            // Strip "Ticket." prefix if present
            if (expr.StartsWith("Ticket.", StringComparison.OrdinalIgnoreCase))
                expr = expr.Substring("Ticket.".Length);

            // If you ever have nested like "Ticket.priority.id" keep just first segment
            var dotIndex = expr.IndexOf('.');
            if (dotIndex >= 0)
                expr = expr.Substring(0, dotIndex);

            return expr.Trim();
        }

        private static bool TryToInt(object value, out int result)
        {
            switch (value)
            {
                case int i:
                    result = i;
                    return true;

                case long l when l >= int.MinValue && l <= int.MaxValue:
                    result = (int)l;
                    return true;

                case string s when int.TryParse(s, out var parsed):
                    result = parsed;
                    return true;

                default:
                    result = 0;
                    return false;
            }
        }



        private static object? Resolve(string expr, TemplateContext ctx)
        {
            // Allow top-level shortcuts like {{Now}} or {{BaseUrl}}
            if (string.Equals(expr, "Now", StringComparison.OrdinalIgnoreCase))
                return ctx.Now;
            if (string.Equals(expr, "BaseUrl", StringComparison.OrdinalIgnoreCase))
                return ctx.BaseUrl;

            // Expect Namespace.Property chain: Contact.FirstName
            var parts = expr.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length == 0) return null;

            object? current = parts[0] switch
            {
                "Contact" => ctx.Contact,
                "Ticket" => ctx.Ticket,
                "Resource" => ctx.Resource,
                _ => null
            };

            for (int i = 1; i < parts.Length && current != null; i++)
            {
                current = GetPropertyValue(current, parts[i]);
            }

            return current;
        }

        private static object? GetPropertyValue(object obj, string propName)
        {
            // case-insensitive property lookup
            var prop = obj.GetType().GetProperty(propName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            return prop?.GetValue(obj);
        }

        public static string ConvertHtmlToText(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var sb = new StringBuilder();
            ConvertNode(doc.DocumentNode, sb);

            // Decode HTML entities & normalize line breaks
            return WebUtility.HtmlDecode(sb.ToString())
                .Replace("\r\n", "\n")
                .Replace("\n\n\n", "\n\n")
                .Trim();
        }

        private static void ConvertNode(HtmlNode node, StringBuilder sb)
        {
            foreach (var child in node.ChildNodes)
            {
                switch (child.Name.ToLower())
                {
                    case "br":
                        sb.AppendLine();
                        break;

                    case "p":
                    case "div":
                        ConvertNode(child, sb);
                        sb.AppendLine();
                        sb.AppendLine();
                        break;

                    case "ul":
                    case "ol":
                        sb.AppendLine();          // ensure list starts on a new line
                        ConvertNode(child, sb);
                        sb.AppendLine();
                        break;

                    case "li":
                        sb.AppendLine();          // 🔥 KEY FIX
                        sb.Append("• ");
                        ConvertNode(child, sb);
                        sb.AppendLine();
                        break;

                    default:
                        if (child.NodeType == HtmlNodeType.Text)
                        {
                            var text = child.InnerText;
                            if (!string.IsNullOrWhiteSpace(text))
                                sb.Append(text.Trim());
                        }
                        ConvertNode(child, sb);
                        break;
                }
            }
        }


        public static string BuildEmailNoteDescription(string? fromEmail,string? toCsv,string? ccCsv,string? subject,string bodyHtml)
        {
            var from = (fromEmail ?? "").Trim();
            var to = NormalizeEmailCsv(toCsv);
            var cc = NormalizeEmailCsv(ccCsv);
            var sentAt = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz");

            var bodyText = ConvertHtmlToText(bodyHtml); // from the helper we made earlier

            var lines = new List<string>
            {
                "Email sent",
                $"Sent: {sentAt}",
                $"From: {from}",
                $"To: {to}",
            };

            if (!string.IsNullOrWhiteSpace(cc))
                lines.Add($"CC: {cc}");

            if (!string.IsNullOrWhiteSpace(subject))
                lines.Add($"Subject: {subject}");

            lines.Add(""); // blank line
            lines.Add("Body:");
            lines.Add(bodyText);

            return string.Join(Environment.NewLine, lines).Trim();
        }

        private static string NormalizeEmailCsv(string? csv)
        {
            if (string.IsNullOrWhiteSpace(csv)) return "";
            return string.Join(", ",
                csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                   .Where(e => !string.IsNullOrWhiteSpace(e))
                   .Distinct(StringComparer.OrdinalIgnoreCase));
        }

    }
}
