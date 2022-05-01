using System.Collections.Immutable;

namespace RubbleMenuAPI.SimpleMock;

public sealed class InMemoryAPI : IRubbleMenuAPI
{
    public Task<ImmutableList<RawRubbleMenu>> GetAllAvailableMenus()
    {
        return Task.FromResult(Data.Menus.ToImmutableList());
    }

    public Task<ImmutableList<RawRubbleReservation>> GetAllStoredReservations()
    {
        return Task.FromResult(Data.Reservations.ToImmutableList());
    }

    public Task<bool> AddReservation(RawRubbleReservation reservation)
    {
        var menu = Data.Menus.FirstOrDefault(m => m.MenuId == reservation.MenuId);
        if (menu == null)
        {
            return Task.FromResult(false);
        }
        Data.Reservations.Add(reservation);
        return Task.FromResult(true);
    }

    private static class Data
    {
        public static readonly RawRubbleMenu[] Menus;
        public static readonly List<RawRubbleReservation> Reservations;

        static Data()
        {
            const string M_1 = "Menü 1";
            const string M_2 = "Menü 2";
            const string M_V = "Vitalmenü";
            const string PICHLING = "Pichling";
            const string HAID = "Haid";

            Menus = new RawRubbleMenu[]
            {
                new(Guid.NewGuid(), M_1, 6.29M, Yesterday,
                    M_1, "Jägerschnitzel", PICHLING),
                new(Guid.NewGuid(), M_2, 6.49M, Yesterday,
                    M_2, "Lasagne", PICHLING),
                new(Guid.NewGuid(), M_V, 7.09M, Yesterday,
                    M_V, "Kürbisgulasch", PICHLING),
                new(Guid.NewGuid(), M_1, 6.29M, Yesterday,
                    M_1, "Jägerschnitzel", HAID),
                new(Guid.NewGuid(), M_2, 5.49M, Yesterday,
                    M_2, "Tortellini", HAID),
                new(Guid.NewGuid(), M_V, 7.09M, Yesterday,
                    M_V, "Kürbisgulasch", HAID),

                new(Guid.NewGuid(), M_1, 4.99M, Today,
                    M_1, "Spaghetti", PICHLING),
                new(Guid.NewGuid(), M_2, 6.49M, Today,
                    M_2, "Gulasch", PICHLING),
                new(Guid.NewGuid(), M_V, 7.99M, Today,
                    M_V, "Gemüseauflauf", PICHLING),
                new(Guid.NewGuid(), M_1, 4.99M, Today,
                    M_1, "Spaghetti", HAID),
                new(Guid.NewGuid(), M_2, 6.49M, Today,
                    M_2, "Gulasch", HAID),

                new(Guid.NewGuid(), M_1, 8.99M, Tomorrow,
                    M_1, "Lachsspaghetti", PICHLING),
                new(Guid.NewGuid(), M_2, 7.49M, Tomorrow,
                    M_2, "Chilli con Carne", PICHLING),
                new(Guid.NewGuid(), M_V, 6.99M, Tomorrow,
                    M_V, "Zucchininudeln mit Sauce", PICHLING),
                new(Guid.NewGuid(), M_1, 8.99M, Tomorrow,
                    M_1, "Lachsspaghetti", HAID),
                new(Guid.NewGuid(), M_2, 7.49M, Tomorrow,
                    M_2, "Chilli con Carne", HAID),

                new(Guid.NewGuid(), M_1, 5.99M, DayAfterTomorrow,
                    M_1, "Fleischlaibchen", PICHLING),
                new(Guid.NewGuid(), M_2, 8.49M, DayAfterTomorrow,
                    M_2, "Putenspieß", PICHLING),
                new(Guid.NewGuid(), M_V, 4.99M, DayAfterTomorrow,
                    M_V, "Linseneintopf", PICHLING),
                new(Guid.NewGuid(), M_1, 5.99M, DayAfterTomorrow,
                    M_1, "Fleischlaibchen", HAID),
                new(Guid.NewGuid(), M_V, 4.99M, DayAfterTomorrow,
                    M_V, "Linseneintopf", HAID)
            };

            static RawRubbleReservation CreateReservation(int menuIdx, string forUser, bool canceled,
                DateTime reservationDate, string reservationSource = "Mock", 
                string? reservationText = null, string? comment = null)
            {
                var menu = Menus[menuIdx];
                return new (Guid.NewGuid(), menu.MenuId, menu.Date,
                    forUser, canceled, reservationDate, reservationSource, 
                    reservationText, comment);
            }

            Reservations = new(new[]
            {
                CreateReservation(0, "h.fuchs@rubblemaster.com", false, TwoDaysAgo),
                CreateReservation(1, "h.fuchs@rubblemaster.com", true, TwoDaysAgo),
                CreateReservation(1, "s.schwammal@rubblemaster.com", false, TwoDaysAgo),
                CreateReservation(2, "j.pamm@rubblemaster.com", false, TwoDaysAgo),
                CreateReservation(4, "f.dorn@rubblemaster.com", false, TwoDaysAgo),
                CreateReservation(10, "j.macho@rubblemaster.com", false, Today),
                CreateReservation(11, "j.macho@rubblemaster.com", true, Tomorrow),
                CreateReservation(15, "j.macho@rubblemaster.com", false, Tomorrow),
                // TODO (Jonas) extend
            });
        }

        private static DateTime Today => DateTime.Today;
        private static DateTime Tomorrow => Today.AddDays(1);
        private static DateTime Yesterday => Today.AddDays(-1);
        private static DateTime TwoDaysAgo => Yesterday.AddDays(-1);
        private static DateTime DayAfterTomorrow => Tomorrow.AddDays(1);
    }
}