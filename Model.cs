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

public record NewProduct(string Name, ProductTypes Type, decimal Price, bool Hot, bool? Vegetarian);