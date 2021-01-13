using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FGrid.Models
{
    public interface IFGridColumn<T> where T : class
    {
        string DisplayName { get; }
        string Name { get; }
        bool IsSearchable { get; }
        bool IsOrderable { get; }
        IEnumerable<FGridFilterOption> FilterOptions { get; set; }
    }
    
    public class FGridColumn<T, TValue> : IFGridColumn<T> where T : class
    {
        public string DisplayName { get; private set; }
        public string Name { get; }
        public bool IsSearchable { get; private set; }
        public bool IsOrderable { get; private set; }
        public IEnumerable<FGridFilterOption> FilterOptions { get; set; }

        public FGridColumn(Expression<Func<T, TValue>> expression)
        {
            Name = NameFor(expression);
        }

        public FGridColumn()
        {
            
        }

        private static string NameFor(Expression<Func<T, TValue>> expression)
        {
            var text = expression.Body is MemberExpression member ? member.ToString() : "";

            return text.IndexOf('.') > 0 ? text.Substring(text.IndexOf('.') + 1) : text;
        }

        public FGridColumn<T, TValue> Searchable(bool searchable)
        {
            IsSearchable = searchable;
            return this;
        }

        public FGridColumn<T, TValue> Orderable(bool orderable)
        {
            IsOrderable = orderable;
            return this;
        }
        
        public FGridColumn<T, TValue> SetDisplayName(string displayName)
        {
            DisplayName = displayName;
            return this;
        }

        public FGridColumn<T, TValue> SetFilterOptions(IEnumerable<FGridFilterOption> filterOptions)
        {
            FilterOptions = filterOptions;
            return this;
        }
    }
}