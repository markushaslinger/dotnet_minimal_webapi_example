using Microsoft.EntityFrameworkCore;

namespace MinimalBackend;

public sealed class DataContext : DbContext
{
    private readonly string _dbPath;

    public DataContext()
    {
        var rootDir = Directory.GetCurrentDirectory();
        var databaseName = Path.Combine(rootDir, "Data/db.sqlite3");
        this._dbPath = databaseName;
    }

    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Menu> Menus { get; set; } = default!;
    public DbSet<MenuItem> MenuItems { get; set; } = default!;
    public DbSet<RubbleMenu> RubbleMenus { get; set; } = default!;
    public DbSet<RubbleOrder> RubbleOrders { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Permission> Permissions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Product>().Property(p => p.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Menu>().HasKey(m => m.MenuNo );
        modelBuilder.Entity<Menu>().HasMany(m => m.Products)
            .WithOne(mi => mi.Menu)
            .HasForeignKey(mi => mi.MenuNo)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MenuItem>().HasKey(mi => new { mi.MenuNo, mi.ProductId });
        modelBuilder.Entity<MenuItem>().HasOne(mi => mi.Product)
            .WithMany()
            .HasForeignKey(mi => mi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RubbleMenu>().HasKey(rm => rm.MenuId);
        modelBuilder.Entity<RubbleMenu>()
            .HasIndex(rm => new { rm.Date, rm.Location });

        modelBuilder.Entity<RubbleOrder>().HasKey(o => o.OrderNo);
        modelBuilder.Entity<RubbleOrder>().Property(o => o.OrderNo).ValueGeneratedOnAdd();
        modelBuilder.Entity<RubbleOrder>().HasOne(o => o.Menu)
            .WithMany()
            .HasForeignKey(o => o.MenuId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>().HasKey(u => u.Username);
        modelBuilder.Entity<User>().HasMany(u => u.Permissions)
            .WithOne()
            .HasForeignKey(p => p.Username)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Permission>().HasKey(p => new { p.Username, p.PermissionName });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite($"Data Source={this._dbPath}");
    }
}

public static class SampleData
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

        var menus = new List<Menu>
        {
            new()
            {
                MenuNo = 1,
                Date = today,
                Products = new()
                {
                    new()
                    {
                        Amount = 2,
                        ProductId = 2
                    },
                    new()
                    {
                        Amount = 1,
                        ProductId = 4
                    }
                }
            },
            new()
            {
                MenuNo = 2,
                Date = today.AddDays(1),
                Products = new()
                {
                    new()
                    {
                        Amount = 1,
                        ProductId = 1
                    },
                    new()
                    {
                        Amount = 1,
                        ProductId = 3
                    }
                }
            },
            new()
            {
                MenuNo = 3,
                Date = today.AddDays(1),
                Products = new()
                {
                    new()
                    {
                        Amount = 5,
                        ProductId = 1
                    },
                    new()
                    {
                        Amount = 4,
                        ProductId = 2
                    }
                }
            }
        };

        return ctx.AddRangeAsync(menus);
    }

    private static Task InsertProducts(DbContext ctx)
    {
        var products = new List<Product>
        {
            new()
            {
                Hot = false,
                Name = "Schnitzelsemmal",
                Price = 2.49M,
                Type = ProductTypes.Food,
                Vegetarian = false
            },
            new()
            {
                Hot = true,
                Name = "Warmes Leberkassemmal",
                Price = 1.99M,
                Type = ProductTypes.Food,
                Vegetarian = false
            },
            new()
            {
                Hot = false,
                Name = "Softdrink",
                Price = 1.29M,
                Type = ProductTypes.Drink
            },
            new()
            {
                Hot = false,
                Name = "Mineralwasser",
                Price = 0.99M,
                Type = ProductTypes.Drink
            },
            new()
            {
                Hot = true,
                Name = "Gemüsepfanne",
                Price = 3.49M,
                Type = ProductTypes.Food,
                Vegetarian = true
            }
        };

        return ctx.AddRangeAsync(products);
    }
}