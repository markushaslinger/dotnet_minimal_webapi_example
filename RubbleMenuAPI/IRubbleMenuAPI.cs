using System.Collections.Immutable;

namespace RubbleMenuAPI;

public interface IRubbleMenuAPI
{
    public Task<ImmutableList<RawRubbleMenu>> GetAllAvailableMenus();
    public Task<ImmutableList<RawRubbleReservation>> GetAllStoredReservations();
    public Task<bool> AddReservation(RawRubbleReservation reservation);
}