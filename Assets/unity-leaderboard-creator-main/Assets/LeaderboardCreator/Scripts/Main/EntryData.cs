using System;

namespace Dan.Models
{
    [Serializable]
    public class EntryData
    {
        public string publicKey;
        public string username;
        public string score;
        public string extra;
        public string userGuid;

        public EntryData(string publicKey, string username, string score, string extra, string userGuid = "")
        {
            this.publicKey = publicKey;
            this.username = username;
            this.score = score;
            this.extra = extra;
            this.userGuid = userGuid;
        }
    }
}