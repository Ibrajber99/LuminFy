using Me_Spotify_App.Models;
using Me_Spotify_App.Models.Artist_Related;
using Me_Spotify_App.Models.Track_Related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.ViewModels
{
    public class FullArtistViewModel
    {
        public FullArtistModel ArtistModel { get; set; }

        public List<SimpleAlbumModel> ArtistAlbums { get; set; }

        public List<FullTrackModel> ArtistTopTracks { get; set; }

        public AuthViewmodel AuthModel { get; set; }

    }
}