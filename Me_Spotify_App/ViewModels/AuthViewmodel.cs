using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.ViewModels
{
    public class AuthViewmodel
    {
        public string CallBackCode { get; set; }

        public string TokenGiven { get; set; }

        public int TimeLimit { get; set; }

        public DateTime TokenTimeGeneration { get; set; }
    }
}