using RubbleMenuAPI;

namespace MinimalBackend.Rubble;

public sealed class RubbleMenuService
{
    // TODO (Jonas) implement RubbleMenu Backend Logic here

    private readonly IRubbleMenuAPI _api;

    // proper mock will be injected automatically and can be used
    public RubbleMenuService(IRubbleMenuAPI api)
    {
        this._api = api;
    }

    public async Task<List<RawRubbleMenu>> GetMenusForDay(DateTime day)
    {
        // TODO (Jonas) consider storing data from the API in the database and serving from there (which would be much better)
        // this is just an example on how to use the (mock) API
        // also the data type is the "raw" type from the API, not the usual one

        return (await this._api.GetAllAvailableMenus())
            .Where(rm => rm.Date == day).ToList();
    }

    public async Task<List<RawRubbleReservation>> GetReservations()
    {
        // TODO (Jonas) see above
        return (await this._api.GetAllStoredReservations()).ToList();
    }
}