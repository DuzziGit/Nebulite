using Dan.Enums;

namespace Dan
{
    public static class ConstantVariables
    {
        internal const string GUID_KEY = "LEADERBOARD_CREATOR___LOCAL_GUID";
        
        public static string GetServerURL(Routes route = Routes.None, string extra = "")
        {
            return SERVER_URL + (route == Routes.Authorize ? "/authorize" :
                route == Routes.Get ? "/get" :
                route == Routes.GetLeaderboard ? "/leaderboard" :
                route == Routes.Test ? "/test" :
                route == Routes.Upload ? "/entry/upload" :
                route == Routes.UpdateUsername ? "/entry/update-username" :
                route == Routes.DeleteEntry ? "/entry/delete" :
                route == Routes.GetPersonalEntry ? "/entry/get" :
                route == Routes.GetEntryCount ? "/entry/count" : "/") + extra;
        }

        private const string SERVER_URL = "https://fp5xe1hop3.execute-api.us-east-1.amazonaws.com/production";
    }
}
