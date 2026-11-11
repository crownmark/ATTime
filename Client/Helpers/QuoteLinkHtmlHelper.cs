using System.Text.RegularExpressions;

public static class QuoteLinkHtmlHelper
{
    public static string RemoveUnselectedQuoteLinks(
        string html,
        bool quoteLinkSelected,
        bool quoteLink1Selected,
        bool quoteLink2Selected,
        bool quoteLink3Selected)
    {
        if (string.IsNullOrEmpty(html))
            return html;

        var links = new Dictionary<string, bool>
        {
            { "QuoteLink", quoteLinkSelected },
            { "QuoteLink1", quoteLink1Selected },
            { "QuoteLink2", quoteLink2Selected },
            { "QuoteLink3", quoteLink3Selected }
        };

        foreach (var link in links)
        {
            if (!link.Value)
            {
                string pattern =
                    $@"<a\b[^>]*href\s*=\s*[""']\{{{Regex.Escape(link.Key)}\}}[""'][^>]*>[\s\S]*?</a>" +
                    @"(?:&nbsp;|\s)*" +
                    @"(?:</div>\s*)?" +
                    @"(?:<div\b[^>]*>\s*<br\s*/?>\s*</div>\s*)?" +
                    @"(?:<div\b[^>]*>\s*<p>\s*</p>\s*</div>\s*)?";

                html = Regex.Replace(
                    html,
                    pattern,
                    "",
                    RegexOptions.IgnoreCase);
            }
        }

        return html;
    }
}