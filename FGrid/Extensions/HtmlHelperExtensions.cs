using System.Collections.Generic;
using System.Text;
using FGrid.Models;
using FGrid.Pages;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FGrid.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent FGrid(this IHtmlHelper htmlHelper, List<FGridColumn> columns)
        {
            var outerDiv = new TagBuilder("div");
            outerDiv.AddCssClass("dataTables_wrapper");

            var table = new TagBuilder("table");
            table.AddCssClass("dataTable table table-hover");

            outerDiv.InnerHtml.AppendHtml(table);

            var thead = new TagBuilder("thead");
            // thead.AddCssClass("thead-light");
            var tr = new TagBuilder("tr");
            foreach (var column in columns)
            {
                var th = new TagBuilder("th");

                th.InnerHtml.Append(column.Name);
                
                if (!column.IsSearchable)
                {
                    th.AddCssClass("not-searchable");
                }

                if (!column.IsOrderable)
                {
                    th.AddCssClass("not-orderable");
                }

                tr.InnerHtml.AppendHtml(th);
            }

            thead.InnerHtml.AppendHtml(tr);
            table.InnerHtml.AppendHtml(thead);

            string result;
            using (var sw = new System.IO.StringWriter())
            {
                outerDiv.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
                result = sw.ToString();
            }

            return new HtmlString(result);
        }
    }
}