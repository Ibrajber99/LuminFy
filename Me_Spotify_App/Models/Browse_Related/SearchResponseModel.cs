using Me_Spotify_App.Models.Artist_Related;
using Me_Spotify_App.Models.PlayList_Related;
using Me_Spotify_App.Models.Track_Related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models.Browse_Related
{
    public class SearchResponseModel
    {

        public SearchResponseModel()
        {
            Artists = new List<FullArtistModel>();
            Albums = new List<SimpleAlbumModel>();
            Tracks = new List<FullTrackModel>();
            Playlists = new List<SimplePlayListModel>();
        }


        public List<FullArtistModel> Artists { get; set; }
        public List<SimpleAlbumModel> Albums { get; set; }
        public List<FullTrackModel> Tracks { get; set; }
        public List<SimplePlayListModel> Playlists { get; set; }
    }
}