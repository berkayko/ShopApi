using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MmtShopApi.Models;


namespace MmtShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly MMTShopContext _context;
        public CategoryController(MMTShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            return await _context.Categories.FromSqlRaw("dbo.GET_ALL_CATEGORY_SP ").ToListAsync();
        }
        [HttpGet("{sku}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory(int sku)
        {
            object[] param = new object[] { (sku - 999), sku };
            return await _context.Categories.FromSqlRaw("dbo.GET_AVAILABLE_CATEGORY_SP {0},{1} ", param).ToListAsync();
        }

    }
}
