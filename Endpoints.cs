using Microsoft.EntityFrameworkCore;

namespace MinimalBackend;

public static class Endpoints
{
    public static void ConfigureEndpoints(this WebApplication app)
    {
        ConfigureDemoEndpoints(app);
        ConfigureProductEndpoints(app);
        ConfigureMenuEndpoints(app);
    }

    private static void ConfigureDemoEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/demo/foo", () => "bar");
        app.MapGet("/demo/add", (int num1, int? num2) => $"Your result: {num1 + num2 ?? 0}");
    }

    private static void ConfigureMenuEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/menu", async (DataContext ctx) => await GetMenus(ctx).ToListAsync());
        app.MapGet("/menu/{no:int}/total-price", async (DataContext ctx, int no) =>
        {
            // not handling 404 here to demonstrate a more complex query (see below for status code return)
            return await GetMenus(ctx).Where(m => m.MenuNo == no)
                                      .SelectMany(m => m.Products)
                                      .Select(mi => mi.Amount * mi.Product.Price)
                                      .FirstOrDefaultAsync();
        });

        app.MapDelete("/menu/{no:int}", async (DataContext ctx, int no) =>
        {
            var menu = await ctx.Menus.FirstOrDefaultAsync(m => m.MenuNo == no);
            if (menu is null)
            {
                return Results.NotFound();
            }

            ctx.Menus.Remove(menu);
            await ctx.SaveChangesAsync();

            return Results.NoContent();
        });

        return;

        static IQueryable<Menu> GetMenus(DataContext ctx) =>
            ctx.Menus
               .Include(m => m.Products)
               .ThenInclude(p => p.Product);
    }

    private static void ConfigureProductEndpoints(IEndpointRouteBuilder app)
    {
        const string BasePath = "/product";
        app.MapGet(BasePath, async (DataContext ctx) => await ctx.Products.ToListAsync());
        app.MapGet($"{BasePath}/{{id:int}}", async (DataContext ctx, int id) =>
                       await ctx.Products.FirstOrDefaultAsync(p => p.Id == id));
        app.MapGet($"{BasePath}/type", async (DataContext ctx, ProductTypes type) =>
                       await ctx.Products.Where(p => p.Type == type).ToListAsync());
        
        app.MapPost(BasePath, async (DataContext ctx, NewProduct newProduct) =>
        {
            var newEntity = new Product
            {
                Price = newProduct.Price,
                Hot = newProduct.Hot,
                Name = newProduct.Name,
                Type = newProduct.Type,
                Vegetarian = newProduct.Vegetarian,
                Ingredients = newProduct.Ingredients.ToList()
            };
            await ctx.AddAsync(newEntity);
            await ctx.SaveChangesAsync();
        });
    }
}
