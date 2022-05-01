namespace RubbleMenuAPI.FullMock.Model;

internal class Reservation
{

    public DateTime Date { get; set; }
    public string ReservationText { get; set; }

    public Guid ReservationId { get; set; } = Guid.NewGuid();
    public string ForUser { get; set; } // lukas.dorn@rubblemaster.com
    public Guid MenuId { get; set; }
    public bool Canceled { get; set; }
    public string Comment { get; set; }

    public DateTime ReservationDate { get; set; }
    public string ReservationSource { get; set; }
}