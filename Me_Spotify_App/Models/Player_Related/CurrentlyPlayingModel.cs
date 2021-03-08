using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models.Player_Related
{
    public class CurrentlyPlayingModel
    {
        public ContextModel Context { get; set; }

        public string CurrentlyPlayingType { get; set; }
        public bool IsPlaying { get; set; }

        public int? ProgressMs { get; set; }
        public long Timestamp { get; set; }

        public FullTrackModel Item { get; set; }
    }
}