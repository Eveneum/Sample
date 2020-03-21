using Microsoft.AspNetCore.Html;
using System.Globalization;

namespace EveneumSample.Helpers
{
    public static class CurrencyHelper
    {
        public static IHtmlContent Render(decimal amount)
        {
            var usCulture = new CultureInfo("en-US");
            var color = amount > 0 ? "green" : "red";
            var currency = string.Format(usCulture, "{0:C}", amount);

            return new HtmlString($"<span style=\"color:{color};\" align=\"right\" >{currency}</span>");
        }
    }
}
