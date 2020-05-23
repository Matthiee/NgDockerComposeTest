using API.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Data
{
    public static class ProductsDbContextSeed
    {
        public static async Task SeedAsync(this ProductsDbContext context, ILogger logger)
        {
			if (!context.Products.Any())
			{
				logger.LogInformation("No products exist yet, seeding..");

				var productsData = File.ReadAllText("./Data/SeedData/products.json");

				var products = JsonSerializer.Deserialize<List<Product>>(productsData);

				foreach (var item in products)
				{
					context.Products.Add(item);
					logger.LogInformation("Added {Product} seed", item.Name);
				}

				await context.SaveChangesAsync();

				logger.LogInformation("Seeding completed");
			}
		}
    }
}
