using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
        ConfigureBlackBearEndpoints(app);
    }

    private static void ConfigureBlackBearEndpoints(IEndpointRouteBuilder app)
    {
        const string BASE = "/black-bear/user";

        byte[] CreateSalt()
        {
            var salt = new byte[128 / 8];
            RandomNumberGenerator.Fill(salt.AsSpan());
            return salt;
        }

        string HashPassword(string password, byte[] salt)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        // only for demo purposes, actually providing a list of users is not recommended!
        app.MapGet(BASE, async (DataContext ctx) => await ctx.Users.Select(u => u.Username).ToListAsync());
        app.MapGet($"{BASE}/login", async (DataContext ctx, string username, string password) =>
        {
            var existing = await ctx.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existing != null)
            {
                var submittedPassword = HashPassword(password, existing.Salt);
                if (submittedPassword == existing.Password)
                {
                    return Results.Ok();
                }
            }

            // we do not return if the username was unknown or the password incorrect to keep potential attackers in doubt
            return Results.BadRequest();
        });

        app.MapPost(BASE, async (DataContext ctx, NewUser newUser) =>
        {
            var existing = await ctx.Users.FirstOrDefaultAsync(u => u.Username == newUser.Username);
            if (existing != null)
            {
                return Results.BadRequest($"Username {newUser.Username} already exists");
            }

            var salt = CreateSalt();

            var user = new User
            {
                Email = newUser.Email,
                Password = HashPassword(newUser.Password, salt),
                Salt = salt,
                Username = newUser.Username,
                Permissions = newUser.Permissions.Distinct().Select(p => new Permission
                {
                    Username = newUser.Username,
                    PermissionName = p
                }).ToList()
            };

            await ctx.Users.AddAsync(user);
            await ctx.SaveChangesAsync();

            // not created, because there is no by id uri to refer to
            return Results.Ok();
        });
    }

    private static void ConfigureRubbleMenuEndpoints(IEndpointRouteBuilder app)
    {
        const string BASE = "/rubble-menu";

        Task<RubbleMenu?> GetMenuByIdAsync(DataContext ctx, Guid id) => 
            ctx.RubbleMenus.FirstOrDefaultAsync(rm => rm.MenuId == id);

        app.MapGet(BASE, async (DataContext ctx) => await ctx.RubbleMenus.ToListAsync());
        app.MapGet($"{BASE}/{{id}}",
            async (DataContext ctx, Guid id) => await GetMenuByIdAsync(ctx, id));
        app.MapGet($"{BASE}/canteen", async (DataContext ctx, DateTime date, string location) =>
            await ctx.RubbleMenus.Where(rm => rm.Date == date.Date && rm.Location == location).ToListAsync());

        app.MapPut(BASE,
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
                return Results.Created($"{BASE}/{newMenu.MenuId}", newMenu);
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