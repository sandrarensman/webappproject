using System.Text;

namespace SchoolApp.Helpers;

public abstract class DropdownHtmlHelper
{
    public static string GenerateDropdownHtml(int pageSize)
    {
        if (pageSize < 0) throw new ArgumentException("Page size cannot be negative.", nameof(pageSize));

        var pageSizes = new List<int> { 10, 15, 20 };
        var sb = new StringBuilder();
        sb.AppendLine("<form method='get' style='display: inline; margin: 0; padding: 0; border: none;'>");
        sb.AppendLine("<select name='pageSize' onchange='this.form.submit()' class='page-size-dropdown'>");
        foreach (var size in pageSizes)
        {
            var selected = size == pageSize ? "selected" : "";
            sb.AppendLine($"<option value='{size}' {selected}>{size}</option>");
        }

        sb.AppendLine("</select>");
        sb.AppendLine("</form>");
        sb.AppendLine("");

        return sb.ToString();
    }
}