using Newtonsoft.Json;
using RubbleMenuAPI.FullMock.Model;
using Menu = RubbleMenuAPI.FullMock.Model.Menu;

namespace RubbleMenuAPI.FullMock;

internal class MenuManagerMock
{

    private StandardMenuConfig cachedMenuConfig;

    private IEnumerable<Menu> GetAllMenus()
	{
		var files = (new System.IO.DirectoryInfo(FullMockRunner.Directory)).GetFiles("*.menu");
		foreach (var f in files)
		{
			yield return Newtonsoft.Json.JsonConvert.DeserializeObject<Menu>(System.IO.File.ReadAllText(f.FullName));
		}
	}

	public void CreateMenu(Menu m)
	{
		if (!System.IO.File.Exists($"{FullMockRunner.Directory}\\{m.Date:yyyy-MM-dd} {m.MenuType}.menu"))
		{
			System.IO.File.WriteAllText($"{FullMockRunner.Directory}\\{m.Date:yyyy-MM-dd} {m.MenuType}.menu", Newtonsoft.Json.JsonConvert.SerializeObject(m, Newtonsoft.Json.Formatting.Indented));
		}
	}

	public void UpdateMenu(Menu m)
	{
		System.IO.File.WriteAllText($"{FullMockRunner.Directory}\\{m.Date:yyyy-MM-dd} {m.MenuType}.menu", Newtonsoft.Json.JsonConvert.SerializeObject(m, Newtonsoft.Json.Formatting.Indented));
	}
	public void DeleteMenu(Menu m)
	{
		System.IO.File.Delete($"{FullMockRunner.Directory}\\{m.Date:yyyy-MM-dd} {m.MenuType}.menu");
	}

	public void CreateStandardMenu(DateTime dt, string menu1, string menu2, string vitalMenu)
	{

		foreach (var m in NewStandardMenu(dt, "Menü 1", menu1))
		{
			CreateMenu(m);
		}
		foreach (var m in NewStandardMenu(dt, "Menü 1 Klein", menu1))
		{
			CreateMenu(m);
		}
		foreach (var m in NewStandardMenu(dt, "Menü 2", menu2))
		{
			CreateMenu(m);
		}
		foreach (var m in NewStandardMenu(dt, "Menü 2 Klein", menu2))
		{
			CreateMenu(m);
		}
		foreach (var m in NewStandardMenu(dt, "Vitalmenü", vitalMenu))
		{
			CreateMenu(m);
		}
		foreach (var m in NewStandardMenu(dt, "Vitalmenü Klein", vitalMenu))
		{
			CreateMenu(m);
		}
		foreach (var m in NewStandardMenu(dt, "Salatteller", null))
		{
			CreateMenu(m);
		}
		foreach (var m in NewStandardMenu(dt, "Gebäck", null))
		{
			CreateMenu(m);
		}


	}

	public IEnumerable<Menu> NewStandardMenu(DateTime dt, string menu, string content)
	{

		var c = GetMenuConfig();


		foreach (var location in c.Locations)
		{

			var standard = c.Items.FirstOrDefault(w => w.Name == menu);
			if (standard != null)
			{
				yield return new() { Date = dt, Location = location, MenuType = standard.Name, Price = standard.Price, Content = content, MenuGroupName = standard.MenuGroupName };
			}


			//foreach (var element in c.Items)
			//{
			//	
			//}
			//foreach (var element in c.Items)
			//{
			//	yield return new Menu() { Date = dt, Location = location, MenuType = element.Name, Price = element.Price, Content = menu2, MenuGroupName = element.MenuGroupName };
			//}
			//foreach (var element in c.Items)
			//{
			//	yield return new Menu() { Date = dt, Location = location, MenuType = element.Name, Price = element.Price, Content = vitalMenu, MenuGroupName = element.MenuGroupName };
			//}

		}
	}

	Random randomizer = new();
	public Menu GetRandomFutureMenu()
	{
		return GetFutureMenus().OrderBy(it => randomizer.Next()).First();
	}

	public IEnumerable<Menu> GetFutureMenus()
	{
		return this.GetAllMenus().Where(t => t.Date >= DateTime.Now.Date);
	}

	public IEnumerable<Menu> GetPastMenus()
	{
		return this.GetAllMenus().Where(t => t.Date < DateTime.Now.Date);
	}

	public IEnumerable<Menu> GetMenusOnDate(DateTime dt)
	{
		return GetFutureMenus().Where(r => r.Date == dt.Date);
	}

    private StandardMenuConfig GetMenuConfig()
    {
        if (this.cachedMenuConfig != null)
        {
            return this.cachedMenuConfig;
        }

        var file = $"{FullMockRunner.Directory}\\menuConfig.json";
        if (File.Exists(file))
        {
            this.cachedMenuConfig = JsonConvert.DeserializeObject<StandardMenuConfig>(File.ReadAllText(file));
            return this.cachedMenuConfig;
        }

        var n = new StandardMenuConfig
        {
            Items = new()
            {
                new() { Name = "Menü 1", MenuGroupName = "Menü 1", Price = 4.5M },
                new() { Name = "Menü 1 Klein", MenuGroupName = "Menü 1", Price = 4.0M },
                new() { Name = "Menü 2", MenuGroupName = "Menü 2", Price = 4.5M },
                new() { Name = "Menü 2 Klein", MenuGroupName = "Menü 2", Price = 4.0M },
                new() { Name = "Vitalmenü", MenuGroupName = "Vitalmenü", Price = 4.5M },
                new() { Name = "Vitalmenü Klein", MenuGroupName = "Vitalmenü", Price = 4.0M },
                new() { Name = "Salatteller", Price = 2M },
                new() { Name = "Gebäck", Price = 0.5M }
            },
            Locations = new[] { "Pichling", "Haid" }
        };
        this.cachedMenuConfig = n;
        File.WriteAllText(file, JsonConvert.SerializeObject(n, Formatting.Indented));
        return n;
    }
}