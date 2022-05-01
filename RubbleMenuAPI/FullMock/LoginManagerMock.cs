using RubbleMenuAPI.FullMock.Model;

namespace RubbleMenuAPI.FullMock;

internal class LoginManagerMock
{

    Random randomizer = new();
    public string GetRandomUserName()
    {
        string[] items = {
            "lukas.dorn@rubblemaster.com",
            "thomas.schableger@rubblemaster.com",
            "Wolfgang.Sengstbratl@rubblemaster.com",
            "Armin.Gaberscek@rubblemaster.com",
            "praktikant-einkauf@rubblemaster.com",
            "Erika.Musterfrau@rubblemaster.com",
            "John.Doe@rubblemaster.com",
            "HealthMailbox14b9a7b3b7d143cfbf50fbba06a2caa2@pichling.hmh.at",
            "HealthMailbox342680a00db54c83940efa0a414583e5@pichling.hmh.at",
            "HealthMailboxf7f0180d46b24a618be71e4c47f60778@pichling.hmh.at",
            "HealthMailboxdc6663bfa4fd4a19891b8fc3d12383a2@pichling.hmh.at",
            "HealthMailbox5cfbb6d21a6643369a9ff6f507dfa2dc@pichling.hmh.at",
            "HealthMailbox1d56221921374349887c2219ac424e73@pichling.hmh.at",
            "HealthMailbox6072684606eb4e3fb1b28887d00b2276@pichling.hmh.at",
            "HealthMailboxfd0ef23b6b5f4e72813be15f87652bbb@pichling.hmh.at"
        };
        return items.OrderBy(r => randomizer.Next()).First();
    }

    public string GetCurrentUserName()
    {
        return "Max.Mustermann@Rubblemaster.com";
    }
    public string GetCurrentName()
    {
        return "Max Mustermann";
    }

    public UserRole UserRole { get; set; } = UserRole.Owner;

}