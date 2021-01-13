using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FGrid.Models;

namespace FGrid.Models
{
    public class FGridColumns<T> : List<IFGridColumn<T>> where T : class
    {
        public FGrid<T> Grid { get; set; }

        public FGridColumns(FGrid<T> grid)
        {
            Grid = grid;
        }

        public FGridColumn<T, TValue> Add<TValue>(Expression<Func<T, TValue>> expression)
        {
            var column = new FGridColumn<T, TValue>(expression);
            Grid.Columns.Add(column);

            return column;
        }
        
        public FGridColumn<T, string> Add()
        {
            var column = new FGridColumn<T, string>();
            Grid.Columns.Add(column);

            return column;
        } 
    }
}