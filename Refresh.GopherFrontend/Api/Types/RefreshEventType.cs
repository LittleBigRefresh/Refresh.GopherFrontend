namespace Refresh.GopherFrontend.Api.Types;

public enum RefreshEventType
{
    UploadLevel = 0,
    FavouriteLevel = 1,
    UnfavouriteLevel = 2,
    FavouriteUser = 3,
    UnfavouriteUser = 4,
    PlayLevel = 5,
    TagLevel = 6,
    TeamPickLevel = 13,
    DpadRateLevel = 14,
    ReviewLevel = 15,
    SubmittedScore = 20,
    FirstLogin = 127,
}