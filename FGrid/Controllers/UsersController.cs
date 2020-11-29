using System.Linq;
using System.Threading.Tasks;
using FGrid.Extensions;
using FGrid.Persistence;
using FGrid.Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FGrid.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : Controller
    { 
        private readonly ApplicationDbContext _dbContext;
        
        public UsersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpPost]
        public async Task<JsonResult> GetUsers(FGridParameters dtParameters)
        {
            var users = _dbContext.Users.AsQueryable();
            var result = await users.ApplyFGridFilters(dtParameters);
            
            return Json(result);
        }
    }
}