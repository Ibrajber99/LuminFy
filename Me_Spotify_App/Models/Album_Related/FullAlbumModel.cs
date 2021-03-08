using Me_Spotify_App.Models.Track_Related;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models
{
    public class FullAlbumModel
    {
        public List<SimpleTrackModel> Tracks { get; set; }
        public Dictionary<string, string> Restrictions { get; set; }
        public string ReleaseDatePrecision { get; set; }
        public string ReleaseDate { get; set; }
        public int Popularity { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public List<ImageModel> Images { get; set; }
        public string Id { get; set; }
        public string Href { get; set; }
        public List<string> Genres { get; set; }
        public Dictionary<string, string> ExternalUrls { get; set; }
        public Dictionary<string, string> ExternalIds { get; set; }

        public List<CopyrightModel> Copyrights { get; set; }
        public List<string> AvailableMarkets { get; set; }
        public List<SimpleArtistModel> Artists { get; set; }
        public string AlbumType { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }
}