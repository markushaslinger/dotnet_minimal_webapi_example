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
        ConfigureRubbleMenuEndpoints(app);
    }

    private static void ConfigureRubbleMenuEndpoints(IEndpointRouteBuilder app)
    {
        Task<RubbleMenu?> GetMenuByIdAsync(DataContext ctx, Guid id) => 
            ctx.RubbleMenus.FirstOrDefaultAsync(rm => rm.MenuId == id);

        app.MapGet("/rubble-menu", async (DataContext ctx) => await ctx.RubbleMenus.ToListAsync());
        app.MapGet("/rubble-menu/{id}",
            async (DataContext ctx, Guid id) => await GetMenuByIdAsync(ctx, id));
        app.MapGet("/rubble-menu/canteen", async (DataContext ctx, DateTime date, string location) =>
            await ctx.RubbleMenus.Where(rm => rm.Date == date.Date && rm.Location == location).ToListAsync());

        app.MapPut("/rubble-menu",
            async (DataContext ctx, RubbleMenu newMenu) =>
            {
                newMenu.Date = newMenu.Date.Date;

                var existing = await GetMenuByIdAsync(ctx, newMenu.MenuId);
                if (existing == null)
                {
                    await ctx.RubbleMenus.AddAsync(newMenu);
                }
                else
                {
                    existing.Price = newMenu.Price;
                    existing.Date = newMenu.Date;
                    existing.Location = newMenu.Location;
                    existing.Content = newMenu.Content;
                    existing.MenuGroupName = newMenu.MenuGroupName;
                    existing.MenuType = newMenu.MenuType;
                }
                
                await ctx.SaveChangesAsync();
                return Results.Created($"/rubble-menu/{newMenu.MenuId}", newMenu);
            });
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