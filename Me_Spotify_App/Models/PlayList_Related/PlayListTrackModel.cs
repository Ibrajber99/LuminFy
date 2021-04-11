using Me_Spotify_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models.PlayList_Related
{
    public class PlayListTrackModel<T>
    {
        public DateTime? AddedAt { get; set; }

        public PublicUserModel AddedBy { get; set; }

        public bool IsLocal { get; set; }

        public T Track { get; set; }
    }
}