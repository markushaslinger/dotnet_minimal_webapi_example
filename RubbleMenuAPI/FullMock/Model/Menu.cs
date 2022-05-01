namespace RubbleMenuAPI.FullMock.Model;

internal class Menu
{
    public Guid MenuId { get; set; } = Guid.NewGuid();
    public string MenuGroupName { get; set; }
    public decimal Price { get; set; } // 2.5€
    public DateTime Date { get; set; } // 2022-01-22
    public string MenuType { get; set; } // Menü1,Menü1Klein,Menü2,Menü2Klein,...
    public string Content { get; set; } // Spaghetti
    public string Location { get; set; } // Pichling/Haid
}