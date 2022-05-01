namespace RubbleMenuAPI.FullMock;

internal static class Extensions
{
    /// <summary>
    ///     TODO !Assumption! MH: Extension method was missing from code dump
    ///     Added implementation to allow for compilation
    /// </summary>
    public static void Dump(this IEnumerable<ReportManager.ReservationReportLine> self)
    {
        var lines = self.Select(l => $"[{l.Date}] {l.Menu}: {l.Count}");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }
}