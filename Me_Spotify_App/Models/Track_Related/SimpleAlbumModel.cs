using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models.Track_Related
{
    public class SimpleAlbumModel
    {
        public string AlbumGroup { get; set; }
        public string AlbumType { get; set; }
        public List<SimpleArtistModel> Artists { get; set; }
        public List<string> AvailableMarkets { get; set; }
        public Dictionary<string, string> ExternalUrls { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public List<ImageModel> Images { get; set; }

        [Display(Name="Album Name")]
        public string Name { get; set; }

        [Display(Name ="Release Date")]
        public string ReleaseDate { get; set; }
        public string ReleaseDatePrecision { get; set; }
        public Dictionary<string, string> Restrictions { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }
}