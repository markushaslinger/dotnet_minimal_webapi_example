using Microsoft.EntityFrameworkCore;

namespace MinimalBackend;

public static class Endpoints
{
    public static void ConfigureEndpoints(this WebApplication app)
    {
        app.MapGet("/demo/foo", () => "bar");
        app.MapGet("/demo/add", (int num1, int? num2) => $"Your result: {num1 + num2 ?? 0}");

        ConfigureProductEndpoints(app);
        ConfigureMenuEndpoints(app);
    }

    private static void ConfigureMenuEndpoints(IEndpointRouteBuilder app)
    {
        static IQueryable<Menu> GetMenus(DataContext ctx) => ctx.Menus
            .Include(m => m.Products)
            .ThenInclude(p => p.Product);

        app.MapGet("/menu", async (DataContext ctx) => await GetMenus(ctx).ToListAsync());
        app.MapGet("/menu/{no}/total-price", async (DataContext ctx, int no) =>
        {
            return await GetMenus(ctx).Where(m => m.MenuNo == no)
                .SelectMany(m => m.Products)
                .Select(mi => mi.Amount * mi.Product.Price)
                .FirstOrDefaultAsync();
        });

        app.MapDelete("/menu/{no}", async (DataContext ctx, int no) =>
        {
            var menu = await ctx.Menus.FirstOrDefaultAsync(m => m.MenuNo == no);
            if (menu == null)
            {
                return;
            }

            ctx.Menus.Remove(menu);
            await ctx.SaveChangesAsync();
        });
    }

    private static void ConfigureProductEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/product", async (DataContext ctx) => await ctx.Products.ToListAsync());
        app.MapGet("/product/{id}", async (DataContext ctx, int id) =>
            await ctx.Products.FirstOrDefaultAsync(p => p.Id == id));
        app.MapGet("/product/type", async (DataContext ctx, ProductTypes type) =>
            await ctx.Products.Where(p => p.Type == type).ToListAsync());

        app.MapPost("/product", async (DataContext ctx, NewProduct newProduct) =>
        {
            var newEntity = new Product
            {
                Price = newProduct.Price,
                Hot = newProduct.Hot,
                Name = newProduct.Name,
                Type = newProduct.Type,
                Vegetarian = newProduct.Vegetarian
            };
            await ctx.AddAsync(newEntity);
            await ctx.SaveChangesAsync();
        });
    }
}