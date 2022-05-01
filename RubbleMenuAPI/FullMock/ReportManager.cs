namespace RubbleMenuAPI.FullMock;

internal class ReportManager
{
    public IEnumerable<ReservationReportLine> GetReservationReport() => GetReservationReport(DateTime.Now.Date);
    public IEnumerable<ReservationReportLine> GetReservationReport(DateTime forDate)
    {
        MenuManagerMock m = new MenuManagerMock();
        ReservationManagerMock r = new ReservationManagerMock();

        var menus = m.GetFutureMenus();
        var reservations = r.GetFutureReservations(m);

        foreach (var menu in menus.Where(w => w.Date.Date == forDate.Date))
        {
            var items = reservations.Where(re => re.MenuId == menu.MenuId);
            yield return new() { Date = menu.Date, Count = items.Count(), Menu = menu.MenuType };
        }
    }


    public IEnumerable<UserReportLine> GetUserReport(DateTime dtStart, DateTime dtEnd)
    {
        MenuManagerMock m = new MenuManagerMock();
        ReservationManagerMock r = new ReservationManagerMock();
        var menus = m.GetPastMenus().Where(re => re.Date >= dtStart && re.Date <= dtEnd);
        var reservations = r.GetPastReservations(m).Where(re => re.Date >= dtStart && re.Date <= dtEnd);

        foreach (var u in reservations.GroupBy(re => new { Date = re.Date, User = re.ForUser }))
        {
            var referencedMenus = u.SelectMany(re1 => m.GetPastMenus().Where(re => re.MenuId == re1.MenuId));
            yield return new() { Date = u.Key.Date, User = u.Key.User, Sum = referencedMenus.Sum(s => s.Price) };
        }
    }

    public class ReservationReportLine
    {
        public DateTime Date { get; set; }
        public string Menu { get; set; }
        public int Count { get; set; }
    }

    public class UserReportLine
    {
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Details { get; set; }
        public decimal Sum { get; set; }
    }
}