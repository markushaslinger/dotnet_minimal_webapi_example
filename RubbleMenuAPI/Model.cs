namespace RubbleMenuAPI;

public record RawRubbleMenu(Guid MenuId, string MenuGroupName, decimal Price, DateTime Date,
    string MenuType, string Content, string Location);
public record RawRubbleReservation(Guid ReservationId, Guid MenuId, DateTime Date, string ForUser,
    bool Canceled, DateTime ReservationDate, string ReservationSource, 
    string? ReservationText, string? Comment);