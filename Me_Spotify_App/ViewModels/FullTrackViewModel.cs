using Me_Spotify_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.ViewModels
{
    public class FullTrackViewModel
    {
        public FullTrackModel TrackModel { get; set; }

        public FullAlbumModel AlbumModel { get; set; }

        public AuthViewmodel AuthModel { get; set; }
    }
}