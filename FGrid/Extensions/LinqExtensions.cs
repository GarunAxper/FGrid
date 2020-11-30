using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using FGrid.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace FGrid.Extensions
{
    public static class LinqExtensions
    {
        public static async Task<FGridResult<T>> ApplyFGridFilters<T>(this IQueryable<T> sourceList,
            FGridModel dtModel)
        {
            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var totalResultsCount = await sourceList.CountAsync();

            var searchBy = dtModel.Search?.Value;

            // if we have an empty search then just order the results by Id ascending
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            if (dtModel.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtModel.Columns[dtModel.Order[0].Column].Data;
                orderAscendingDirection = dtModel.Order[0].Dir.ToString().ToLower() == "asc";
            }

            // if (!string.IsNullOrEmpty(searchBy))
            // {
            //     sourceList = sourceList.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
            //                                        r.Lastname != null &&
            //                                        r.Lastname.ToUpper().Contains(searchBy.ToUpper()) ||
            //                                        r.Notes != null &&
            //                                        r.Notes.ToUpper().Contains(searchBy.ToUpper()));
            // }

            foreach (var column in dtModel.Columns)
            {
                if (column.Searchable && !string.IsNullOrEmpty(column.Search.Value))
                {
                    sourceList = sourceList.Filter(column.Data, column.Search.Value);
                }
            }

            sourceList = orderAscendingDirection
                ? sourceList.OrderByDynamic(orderCriteria, FGridOrderDir.Asc)
                : sourceList.OrderByDynamic(orderCriteria, FGridOrderDir.Desc);

            var filteredResultsCount = await sourceList.CountAsync();

            return new FGridResult<T>
            {
                Draw = dtModel.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = await sourceList
                    .Skip(dtModel.Start)
                    .Take(dtModel.Length)
                    .ToListAsync()
            };
        }

        public static IQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query,
            string orderByMember,
            FGridOrderDir ascendingDirection)
        {
            var param = Expression.Parameter(typeof(T), "c");

            var body = orderByMember.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            var queryable = ascendingDirection == FGridOrderDir.Asc
                ? (IOrderedQueryable<T>) Queryable.OrderBy(query.AsQueryable(),
                    (dynamic) Expression.Lambda(body, param))
                : (IOrderedQueryable<T>) Queryable.OrderByDescending(query.AsQueryable(),
                    (dynamic) Expression.Lambda(body, param));

            return queryable;
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> sourceList, string column, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyType = typeof(T);
            MemberExpression filterProperty = null;
            
            foreach (var member in column.Split("."))
            {
                var property = propertyType.GetProperties()
                    .SingleOrDefault(x => x.CanRead && x.CanWrite && !x.GetGetMethod().IsVirtual && string.Equals(
                        x.Name, member, StringComparison.InvariantCultureIgnoreCase));

                filterProperty = filterProperty == null
                    ? Expression.Property(parameter, property.Name)
                    : Expression.Property(filterProperty, property.Name);

                propertyType = property.PropertyType;
            }

            var someValue = Expression.Constant(value, typeof(string));

            var filterPropertyString = Expression.Convert(Expression.Convert(filterProperty, typeof(object)), typeof(string));
           
            var containsMethodExp = Expression.Call(
                filterPropertyString,
                typeof(string).GetMethods()
                    .First(m => m.Name == "Contains" && m.GetParameters().Length == 1),
                someValue);

            var predicate = Expression.Lambda<Func<T, bool>>(containsMethodExp, parameter);

            return sourceList.Where(predicate);
        }
    }
}