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
    public required string Name { get; set; }
    public ProductTypes Type { get; set; }
    public decimal Price { get; set; }
    public bool Hot { get; set; }
    public bool? Vegetarian { get; set; }
    public required List<Ingredient> Ingredients { get; set; }
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
    public required List<MenuItem> Products { get; set; }
}

public sealed class Ingredient
{
    public required string Description { get; set; }
    public required List<string> Allergens { get; set; }
}

public sealed record NewProduct(string Name, ProductTypes Type, decimal Price, bool Hot, 
                                bool? Vegetarian, IEnumerable<Ingredient> Ingredients);