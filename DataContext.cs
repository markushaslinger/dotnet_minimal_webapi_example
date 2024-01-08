using Microsoft.EntityFrameworkCore;

namespace MinimalBackend;

public sealed class DataContext(IConfiguration configuration) : DbContext
{
    public required DbSet<Product> Products { get; init; }
    public required DbSet<Menu> Menus { get; init; }
    public required DbSet<MenuItem> MenuItems { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var product = modelBuilder.Entity<Product>();
        product.HasKey(p => p.Id);
        product.Property(p => p.Id).ValueGeneratedOnAdd();
        product.OwnsMany(p => p.Ingredients).ToJson();

        var menu = modelBuilder.Entity<Menu>();
        menu.HasKey(m => m.MenuNo);
        menu.HasMany(m => m.Products)
            .WithOne(mi => mi.Menu)
            .HasForeignKey(mi => mi.MenuNo)
            .OnDelete(DeleteBehavior.Cascade);

        var menuItem = modelBuilder.Entity<MenuItem>();
        menuItem.HasKey(mi => new { mi.MenuNo, mi.ProductId });
        menuItem.HasOne(mi => mi.Product)
                .WithMany()
                .HasForeignKey(mi => mi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
    }
}

internal static class SampleData
{
    public static async Task InsertSampleData(this DataContext ctx)
    {
        await InsertProducts(ctx);
        await InsertMenus(ctx);

        await ctx.SaveChangesAsync();
    }

    private static Task InsertMenus(DbContext ctx)
    {
        var today = DateTime.Today;

        IEnumerable<Menu> menus =
        [
            new Menu
            {
                MenuNo = 1,
                Date = today,
                Products =
                [
                    new MenuItem
                    {
                        Amount = 2,
                        ProductId = 2
                    },
                    new MenuItem
                    {
                        Amount = 1,
                        ProductId = 4
                    }
                ]
            },
            new Menu
            {
                MenuNo = 2,
                Date = today.AddDays(1),
                Products =
                [
                    new MenuItem
                    {
                        Amount = 1,
                        ProductId = 1
                    },

                    new MenuItem
                    {
                        Amount = 1,
                        ProductId = 3
                    }
                ]
            },
            new Menu
            {
                MenuNo = 3,
                Date = today.AddDays(1),
                Products =
                [
                    new MenuItem
                    {
                        Amount = 5,
                        ProductId = 1
                    },

                    new MenuItem
                    {
                        Amount = 4,
                        ProductId = 2
                    }
                ]
            }
        ];

        return ctx.AddRangeAsync(menus);
    }

    private static Task InsertProducts(DbContext ctx)
    {
        IEnumerable<Product> products =
        [
            new Product
            {
                Hot = false,
                Name = "Schnitzisemmal",
                Price = 2.49M,
                Type = ProductTypes.Food,
                Vegetarian = false,
                Ingredients =
                [
                    new Ingredient
                    {
                        Description = "Schnitzi",
                        Allergens = ["Gluten", "Egg", "Dairy"]
                    },
                    new Ingredient
                    {
                        Description = "Semmal",
                        Allergens = ["Gluten"]
                    }
                ]
            },
            new Product
            {
                Hot = true,
                Name = "Warmes Leberkassemmal",
                Price = 1.99M,
                Type = ProductTypes.Food,
                Vegetarian = false,
                Ingredients =
                [
                    new Ingredient
                    {
                        Description = "Leberkas",
                        Allergens = []
                    },
                    new Ingredient
                    {
                        Description = "Semmal",
                        Allergens = ["Gluten"]
                    }
                ]
            },
            new Product
            {
                Hot = false,
                Name = "Softdrink",
                Price = 1.29M,
                Type = ProductTypes.Drink,
                Ingredients =
                [
                    new Ingredient
                    {
                        Description = "Water",
                        Allergens = []
                    },
                    new Ingredient
                    {
                        Description = "Sugar",
                        Allergens = []
                    }
                ]
            },
            new Product
            {
                Hot = false,
                Name = "Mineralwasser",
                Price = 0.99M,
                Type = ProductTypes.Drink,
                Ingredients =
                [
                    new Ingredient
                    {
                        Description = "Water",
                        Allergens = []
                    }
                ]
            },
            new Product
            {
                Hot = true,
                Name = "Gemüsepfanne",
                Price = 3.49M,
                Type = ProductTypes.Food,
                Vegetarian = true,
                Ingredients =
                [
                    new Ingredient
                    {
                        Description = "Rice",
                        Allergens = []
                    },
                    new Ingredient
                    {
                        Description = "Vegetables",
                        Allergens = ["Celery", "Soy"]
                    }
                ]
            }
        ];

        return ctx.AddRangeAsync(products);
    }
}
