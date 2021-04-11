using Me_Spotify_App.Models;
using Me_Spotify_App.Models.PlayList_Related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.ViewModels
{
    public class PublicUserViewModel
    {
        public PublicUserModel UserModel { get; set; }

        public List<SimplePlayListModel> UserPublicPlayLists { get; set; }

    }
}