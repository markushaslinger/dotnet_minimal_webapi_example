using Newtonsoft.Json;
using RubbleMenuAPI.FullMock.Model;

namespace RubbleMenuAPI.FullMock;

internal class ReservationManagerMock
{


    // SELECT * FROM Reservaton WHERE Date > getdate()
    private IEnumerable<Reservation> GetAllReservations()
    {
        var files = new DirectoryInfo(FullMockRunner.Directory).GetFiles("*.reservation");
        foreach (var f in files)
        {
            yield return JsonConvert.DeserializeObject<Reservation>(File.ReadAllText(f.FullName));
        }
    }

    public IEnumerable<Reservation> GetFutureReservations(MenuManagerMock menuManager)
    {
        var menus = menuManager.GetFutureMenus().ToArray();
        foreach (var r in GetAllReservations())
        {
            var correspondingMenu = menus.First(w => w.MenuId == r.MenuId);
            if (correspondingMenu.Date >= DateTime.Now.Date)
            {
                yield return r;
            }
        }
    }

    public IEnumerable<Reservation> GetPastReservations(MenuManagerMock menuManager)
    {
        var menus = menuManager.GetFutureMenus().ToArray();
        foreach (var r in GetAllReservations())
        {
            var correspondingMenu = menus.First(w => w.MenuId == r.MenuId);
            if (correspondingMenu.Date < DateTime.Now.Date)
            {
                yield return r;
            }
        }
    }

    public Guid CreateReservation(Menu mnu, string forName)
    {
        return CreateReservation(mnu.Date, mnu.MenuId, forName);
    }

    public Guid CreateReservation(DateTime dt, Guid menuId, string forName, string comment = null)
    {
        if (dt.Date < DateTime.Now.Date)
        {
            throw new ArgumentOutOfRangeException("Reservation must not be in the past");
        }

        var m = new MenuManagerMock();
        var menu = m.GetFutureMenus().First(r => r.MenuId == menuId);

        var r = new Reservation
        {
            Date = dt.Date, MenuId = menuId, ForUser = forName, ReservationDate = DateTime.Now,
            ReservationSource = "Mock", Comment = comment
        };
        CreateReservation(r);
        return r.ReservationId;
    }

    // INSERT INTO
    public void CreateReservation(Reservation r)
    {
        File.WriteAllText($"{FullMockRunner.Directory}\\{r.ReservationId}.reservation",
            JsonConvert.SerializeObject(r, Formatting.Indented));
    }

    // DELETE FROM
    public void DeleteReservation(Reservation r)
    {
        File.Delete($"{FullMockRunner.Directory}\\{r.ReservationId}.reservation");
    }

    // UPDATE
    public void UpdateReservation(Reservation r)
    {
        File.WriteAllText($"{FullMockRunner.Directory}\\{r.ReservationId}.reservation",
            JsonConvert.SerializeObject(r, Formatting.Indented));
    }

    private IEnumerable<Reservation> GetReservationsForName(string forName)
    {
        return GetAllReservations().Where(t => t.ForUser == forName);
    }

    
}