namespace RubbleMenuAPI.FullMock;

public class FullMockRunner
{
    public static readonly string Directory = "C:\\Temp\\rmLunch";

	public static void Main(string[] args)
    {
		//var mtest = m.GetFutureMenus();
		//
		////m.CreateStandardMenu(DateTime.Now.Date.AddDays(0), "Spagetti", "Gulasch", "Gemüseauflauf");
		////m.CreateStandardMenu(DateTime.Now.Date.AddDays(1), "Schnitzel", "Fisch", "Gemüselaibchen");
		////m.CreateStandardMenu(DateTime.Now.Date.AddDays(2), "Lachsspagetti", "Putenfilet", "Steirischer Salat");
		//
		//foreach (var menus in m.GetFutureMenus().Where(re => re.MenuType == "Salatteller"))
		//{
		//	r.CreateReservation(menus.Date, menus.MenuId, l.GetCurrentUserName(), "Kommentar :-)");
		//}

        CreateDemoData();
	}

	private static void CreateDemoData()
	{
		MenuManagerMock m = new MenuManagerMock();
		ReservationManagerMock r = new ReservationManagerMock();
		LoginManagerMock l = new LoginManagerMock();
		ReportManager rp = new ReportManager();

		m.CreateStandardMenu(DateTime.Now.Date.AddDays(0), "Spagetti", "Gulasch", "Gemüseauflauf");
		m.CreateStandardMenu(DateTime.Now.Date.AddDays(1), "Schnitzel", "Fisch", "Gemüselaibchen");
		m.CreateStandardMenu(DateTime.Now.Date.AddDays(2), "Lachsspagetti", "Zuchiniauflauf", "Zuchininudeln mit Sauce");
		m.CreateStandardMenu(DateTime.Now.Date.AddDays(3), "Fleischleibchen", "Nudelauflauf", "Linseneintopf");
		m.CreateStandardMenu(DateTime.Now.Date.AddDays(4), "Putenspieß", "Spagetti Carbonara", "Chilli sin Carne");
		m.CreateStandardMenu(DateTime.Now.Date.AddDays(5), "Jägerschnitzel", "Lasagne", "Kürbisgulasch");
		m.CreateStandardMenu(DateTime.Now.Date.AddDays(6), "Tortellini", "Macaroni", "Gemüsesuppe");

		var user = l.GetCurrentUserName();
		r.CreateReservation(m.GetFutureMenus().FirstOrDefault(x => x.Content == "Spagetti" && x.MenuType == "Menü 1"), l.GetCurrentUserName());
		r.CreateReservation(m.GetFutureMenus().FirstOrDefault(x => x.Content == "Fisch" && x.MenuType == "Menü 2"), l.GetCurrentUserName());
		r.CreateReservation(m.GetFutureMenus().FirstOrDefault(x => x.Content == "Zuchiniauflauf" && x.MenuType == "Menü 2 Klein"), l.GetCurrentUserName());
		r.CreateReservation(m.GetFutureMenus().FirstOrDefault(x => x.Content == "Fleischleibchen" && x.MenuType == "Menü 1 Klein"), l.GetCurrentUserName());
		r.CreateReservation(m.GetFutureMenus().FirstOrDefault(x => x.Content == "Chilli sin Carne" && x.MenuType == "Vitalmenü"), l.GetCurrentUserName());
		r.CreateReservation(m.GetFutureMenus().FirstOrDefault(x => x.Content == "Kürbisgulasch" && x.MenuType == "Vitalmenü"), l.GetCurrentUserName());
		r.CreateReservation(m.GetFutureMenus().FirstOrDefault(x => x.Content == "Macaroni" && x.MenuType == "Menü 2"), l.GetCurrentUserName());

		for (int i = 0; i < 100; i++)
		{
			r.CreateReservation(m.GetRandomFutureMenu(), l.GetRandomUserName());
		}

		rp.GetReservationReport(DateTime.Now.Date.AddDays(0)).Dump();
		rp.GetReservationReport(DateTime.Now.Date.AddDays(1)).Dump();
		rp.GetReservationReport(DateTime.Now.Date.AddDays(2)).Dump();
	}
}











