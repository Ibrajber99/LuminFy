using Me_Spotify_App.Models.PlayList_Related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.ViewModels
{
    public class FullPlayListViewModel
    {
        public FullPlayListModel PlayList { get; set; }

        public AuthViewmodel AuthModel { get; set; }

    }
}