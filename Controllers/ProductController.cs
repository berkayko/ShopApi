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
    public class ProductController : ControllerBase
    {
        private readonly MMTShopContext _context;
        public ProductController(MMTShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.FromSqlRaw("dbo.GET_ALL_PRODUCT_SP").ToListAsync();
            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        //GET: api/Product/5
        [HttpGet("{sku}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int sku)
        {
            var products = await _context.Products.FromSqlRaw("dbo.GET_PRODUCT_INFO_SP {0}", sku).ToListAsync();

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }
        [HttpGet("all/{sku}")]
       
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsCategory(int sku)
        {
            object[] param = new object[] { (sku - 999), sku };
            return await _context.Products.FromSqlRaw("dbo.GET_PRODUCT_FROM_CATEGORY_SP {0},{1} ", param).ToListAsync();
        }
        [HttpPost]
        public ActionResult<Product> PostProduct(Product product)
        {
            //var category = _context.Categories.FromSqlRaw("dbo.GET_ALL_CATEGORY_SP ").Where(x => x.Sku == product.Sku).Single();
            var c = _context.Categories.FromSqlRaw("dbo.GET_CATEGORY_SP {0}", product.Sku).ToList();

            if (c.Count == 0)
            {
                string cName = "";
                int level = Convert.ToInt32(product.Sku.ToString().Substring(0, 1));
                switch (level)
                {
                    case 1:
                        cName = "Home";
                        break;
                    case 2:
                        cName = "Garden";
                        break;
                    case 3:
                        cName = "Electronics";
                        break;
                    case 4:
                        cName = "Fitness";
                        break;
                    case 5:
                        cName = "Toys";
                        break;

                }
                object[] param = new object[] { product.Sku, product.Name, product.Description, product.Price, cName };
                _ = _context.Products.FromSqlRaw("dbo.INSERT_PRODUCT_SP {0},{1},{2},{3},{4}", param).ToListAsync();
                return CreatedAtAction("GetProducts", new { sku = product.Sku }, product);

            }
            else
            {
                return Problem("This Sku Exists");
            }
        }
        [HttpPut("{sku}")]
        public async Task<IActionResult> PutProducts(int sku, Product product)
        {

            try
            {
                string cName = "";
                int level = Convert.ToInt32(product.Sku.ToString().Substring(0, 1));
                switch (level)
                {
                    case 1:
                        cName = "Home";
                        break;
                    case 2:
                        cName = "Garden";
                        break;
                    case 3:
                        cName = "Electronics";
                        break;
                    case 4:
                        cName = "Fitness";
                        break;
                    case 5:
                        cName = "Toys";
                        break;

                }
                object[] param = new object[] { product.Sku, product.Name, product.Description, product.Price, cName,sku };
                _ = _context.Products.FromSqlRaw("dbo.UPDATE_PRODUCT_SP {0},{1},{2},{3},{4},{5}", param).ToListAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                return Problem("Couldn''t update record");
            }

            return NoContent();
        }
        [HttpDelete("{sku}")]
        public async Task<ActionResult<Product>> DeleteProduct(int sku)
        {
            try {
                _ = _context.Products.FromSqlRaw("dbo.DELETE_PRODUCT_SP {0}}", sku).ToListAsync();
                return NoContent();
            }
            catch (Exception)
            {
                 return Problem("Couldn''t Delete record");
            }
            
            
        }


    }
}
