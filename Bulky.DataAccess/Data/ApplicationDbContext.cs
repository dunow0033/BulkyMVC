using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        
        }

        public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>().HasData(

				new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
            );

			modelBuilder.Entity<Product>().HasData(
				new Product
				{
					Id = 1,
					Title = "Cotton Candy",
					Author = "Abby Muscles",
					Description = "Praesent vitae sodales libero",
					ISBN = "WS333333301",
					ListPrice = 70,
					Price = 65,
					Price50 = 60,
					Price100 = 55,
					CategoryId = 1,
					ImageUrl=""
				},
				new Product
				{
					Id = 2,
					Title = "Rock in the Ocean",
					Author = "Ron Parker",
					Description = "Praesent vitae sodales libero",
					ISBN = "WS33333330178787",
					ListPrice = 30,
					Price = 27,
					Price50 = 25,
					Price100 = 20,
					CategoryId = 2,
					ImageUrl = ""
				},
				new Product
				{
					Id = 3,
					Title = "Rock in the Ocean2",
					Author = "Ron Parker2",
					Description = "Praesent vitae sodales libero",
					ISBN = "WS33333330173434",
					ListPrice = 22,
					Price = 34,
					Price50 = 22,
					Price100 = 21,
					CategoryId = 1,
					ImageUrl = ""
				},
				new Product
				{
					Id = 4,
					Title = "Rock in the Ocean3",
					Author = "Ron Parker3",
					Description = "Praesent vitae sodales libero",
					ISBN = "WS33333330102398",
					ListPrice = 18,
					Price = 25,
					Price50 = 19,
					Price100 = 17,
					CategoryId = 2,
					ImageUrl = ""
				}
			);
		}
    }
}
