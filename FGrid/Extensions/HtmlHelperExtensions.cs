﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        // public static IHtmlContent FGrid(this IHtmlHelper htmlHelper, List<FGridColumn> columns)
        // {
        //     var outerDiv = new TagBuilder("div");
        //     outerDiv.AddCssClass("dataTables_wrapper");
        //
        //     var table = new TagBuilder("table");
        //     table.AddCssClass("dataTable table table-hover");
        //
        //     outerDiv.InnerHtml.AppendHtml(table);
        //
        //     var thead = new TagBuilder("thead");
        //     var tr = new TagBuilder("tr");
        //     var filterTr = new TagBuilder("tr");
        //     foreach (var column in columns)
        //     {
        //         var th = new TagBuilder("th");
        //         th.InnerHtml.Append(column.Name);
        //
        //         if (!column.IsSearchable)
        //         {
        //             th.AddCssClass("not-searchable");
        //         }
        //         else
        //         {
        //             var filterTh = new TagBuilder("th");
        //             if (column.FilterOptions != null && column.FilterOptions.Any())
        //             {
        //                 var select = new TagBuilder("select");
        //                 select.InnerHtml.AppendHtml(new TagBuilder("option"));
        //                 foreach (var option in column.FilterOptions)
        //                 {
        //                     var optionTag = new TagBuilder("option");
        //                     optionTag.Attributes.Add(new KeyValuePair<string, string>("value", option.Value));
        //                     optionTag.InnerHtml.Append(option.DisplayText);
        //                     select.InnerHtml.AppendHtml(optionTag);
        //                 }
        //                 select.AddCssClass("form-control");
        //                 select.AddCssClass("form-control-sm");
        //                 filterTh.InnerHtml.AppendHtml(select);
        //             }
        //             else
        //             {
        //                 var input = new TagBuilder("input");
        //                 input.Attributes.Add(new KeyValuePair<string, string>("type", "text"));
        //                 input.AddCssClass("form-control");
        //                 input.AddCssClass("form-control-sm");
        //                 filterTh.InnerHtml.AppendHtml(input);
        //             }
        //             filterTr.InnerHtml.AppendHtml(filterTh);
        //         }
        //
        //         if (!column.IsOrderable)
        //         {
        //             th.AddCssClass("not-orderable");
        //         }
        //
        //         tr.InnerHtml.AppendHtml(th);
        //     }
        //
        //     thead.InnerHtml.AppendHtml(tr);
        //     thead.InnerHtml.AppendHtml(filterTr);
        //     table.InnerHtml.AppendHtml(thead);
        //
        //     string result;
        //     using (var sw = new System.IO.StringWriter())
        //     {
        //         outerDiv.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
        //         result = sw.ToString();
        //     }
        //
        //     return new HtmlString(result);
        // }

        public static FGrid<T> FGrid<T>(this IHtmlHelper htmlHelper, string sourceUrl) where T : class
        {
            return new FGrid<T>(sourceUrl);
        }

        public static IHtmlContent Build<T>(this FGrid<T> grid, Action<FGridColumns<T>> builder) where T : class
        {
            builder(grid.Columns);

            var outerDiv = new TagBuilder("div");
            outerDiv.AddCssClass("dataTables_wrapper");

            var table = new TagBuilder("table");
            table.AddCssClass("dataTable table table-hover");
            table.Attributes.Add(new KeyValuePair<string, string>("data-url", grid.SourceUrl));
            
            outerDiv.InnerHtml.AppendHtml(table);

            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");
            var filterTr = new TagBuilder("tr");
            foreach (var column in grid.Columns)
            {
                var th = new TagBuilder("th");
                th.Attributes.Add(new KeyValuePair<string, string>("data-name", column.Name));
                th.InnerHtml.Append(string.IsNullOrEmpty(column.DisplayName) ? column.Name : column.DisplayName);
                
                if (!column.IsSearchable)
                {
                    th.AddCssClass("not-searchable");
                    filterTr.InnerHtml.AppendHtml(new TagBuilder("th"));
                }
                else
                {
                    var filterTh = new TagBuilder("th");
                    if (column.FilterOptions != null && column.FilterOptions.Any())
                    {
                        var select = new TagBuilder("select");
                        select.InnerHtml.AppendHtml(new TagBuilder("option"));
                        foreach (var option in column.FilterOptions)
                        {
                            var optionTag = new TagBuilder("option");
                            optionTag.Attributes.Add(new KeyValuePair<string, string>("value", option.Value));
                            optionTag.InnerHtml.Append(option.DisplayText);
                            select.InnerHtml.AppendHtml(optionTag);
                        }

                        select.AddCssClass("form-control");
                        select.AddCssClass("form-control-sm");
                        filterTh.InnerHtml.AppendHtml(select);
                    }
                    else
                    {
                        var input = new TagBuilder("input");
                        input.Attributes.Add(new KeyValuePair<string, string>("type", "text"));
                        input.AddCssClass("form-control");
                        input.AddCssClass("form-control-sm");
                        filterTh.InnerHtml.AppendHtml(input);
                    }

                    filterTr.InnerHtml.AppendHtml(filterTh);
                }

                if (!column.IsOrderable)
                {
                    th.AddCssClass("not-orderable");
                }

                tr.InnerHtml.AppendHtml(th);
            }

            thead.InnerHtml.AppendHtml(tr);
            thead.InnerHtml.AppendHtml(filterTr);
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