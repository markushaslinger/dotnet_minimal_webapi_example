using System.Text.Json.Serialization;

namespace MinimalBackend;

public enum ProductTypes
{
    Food,
    Drink
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ProductTypes Type { get; set; }
    public decimal Price { get; set; }
    public bool Hot { get; set; }
    public bool? Vegetarian { get; set; }
}

public class MenuItem
{
    [JsonIgnore]
    public int MenuNo { get; set; }
    [JsonIgnore]
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
    [JsonIgnore]
    public Menu Menu { get; set; } = default!;
    public int Amount { get; set; }
}

public class Menu
{
    public int MenuNo { get; set; }
    public DateTime Date { get; set; }
    public List<MenuItem> Products { get; set; } = default!;
}

public class RubbleMenu
{
    public Guid MenuId { get; set; }
    public string MenuGroupName { get; set; } = default!;
    public decimal Price { get; set; }
    public DateTime Date { get; set; }
    public string MenuType { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string Location { get; set; } = default!;
}

public class RubbleOrder
{
    public int OrderNo { get; set; }
    public string OrderingEmployee { get; set; } = default!;
    public Guid MenuId { get; set; }
    public RubbleMenu Menu { get; set; } = default!;
    public int Amount { get; set; }
}

public class User
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public byte[] Salt { get; set; } = default!;
    public string? Email { get; set; }
    public List<Permission> Permissions { get; set; } = default!;
}

public class Permission
{
    public string Username { get; set; } = default!;
    public string PermissionName { get; set; } = default!;
}

public record NewProduct(string Name, ProductTypes Type, decimal Price, bool Hot, bool? Vegetarian);
public record NewUser(string Username, string Password, string? Email, List<string> Permissions);
public record NewOrder(Guid MenuId, string OrderingEmployee, int Amount);