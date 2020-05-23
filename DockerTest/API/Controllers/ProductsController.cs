using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using API.Caching;

namespace API.Controllers
{
    [Cached(10)]
    public class ProductsController : BaseApiController
    {
        private readonly ILogger<ProductsController> logger;
        private readonly ProductsDbContext context;

        public ProductsController(ILogger<ProductsController> logger, ProductsDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            await Task.Delay(2000);

            return Ok(await context.Products.ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
                return NotFound();

            await Task.Delay(2000);

            return Ok(product);
        }
    }
}
