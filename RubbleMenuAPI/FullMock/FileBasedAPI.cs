using System.Collections.Immutable;

namespace RubbleMenuAPI.FullMock;

public sealed class FileBasedAPI : IRubbleMenuAPI
{
    public Task<ImmutableList<RawRubbleMenu>> GetAllAvailableMenus()
    {
        throw new NotImplementedException();
    }

    public Task<ImmutableList<RawRubbleReservation>> GetAllStoredReservations()
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddReservation(RawRubbleReservation reservation)
    {
        throw new NotImplementedException();
    }
}