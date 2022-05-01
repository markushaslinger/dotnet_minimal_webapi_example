namespace RubbleMenuAPI.FullMock.Model;

internal class StandardMenuConfig
{
    public string[] Locations = new string[] { };
    public List<MenuConfig> Items { get; set; } = new();
}