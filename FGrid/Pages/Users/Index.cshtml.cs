using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FGrid.Extensions;
using FGrid.Persistence;
using FGrid.Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FGrid.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        public IEnumerable<TestUser> Users { get; set; }
        
        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            Users = _dbContext.Users.ToList();
        }

        public async Task<JsonResult> OnPostGetUsers([FromBody]FGridParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            // if we have an empty search then just order the results by Id ascending
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            var result = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Lastname != null &&
                                           r.Lastname.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Notes != null &&
                                           r.Notes.ToUpper().Contains(searchBy.ToUpper()));
            }

            // var parameter = Expression.Parameter(typeof(TestRegister), "x");
            // var filterValue = Expression.Property(parameter, "FirstSurname");
            //
            // var someValue = Expression.Constant("Li", typeof(string));
            // var containsMethodExp = Expression.Call(filterValue, 
            //     typeof(string).GetMethods()
            //     .First(m => m.Name == "Contains" && m.GetParameters().Length == 1), 
            //     someValue);
            //
            // var predicate = Expression.Lambda<Func<TestRegister, bool>>(containsMethodExp, parameter);
            //
            // result = result.Where(predicate);

            foreach (var column in dtParameters.Columns)
            {
                if (column.Searchable && !string.IsNullOrEmpty(column.Search.Value))
                {
                    result = result.Filter(column.Data, column.Search.Value);
                }
            }

            result = orderAscendingDirection
                ? result.OrderByDynamic(orderCriteria, FGridOrderDir.Asc)
                : result.OrderByDynamic(orderCriteria, FGridOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = await result.CountAsync();
            var totalResultsCount = await _dbContext.Users.CountAsync();
            
            return new JsonResult(new FGridResult<TestUser>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = await result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToListAsync()
            });
        }
    }
}