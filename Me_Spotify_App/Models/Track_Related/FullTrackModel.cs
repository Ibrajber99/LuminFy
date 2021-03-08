using Me_Spotify_App.Models.Track_Related;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models
{
    public class FullTrackModel
    {
        [Display(Name="Track number")]
        public int TrackNumber { get; set; }

        [Display(Name="URL preview")]
        public string PreviewUrl { get; set; }

        public int Popularity { get; set; }
        public string Name { get; set; }

        public Dictionary<string, string> Restrictions { get; set; }

        public LinkedTrackModel LinkedFrom { get; set; }

        public bool IsPlayable { get; set; }

        public string Uri { get; set; }

        public string Id { get; set; }

        public Dictionary<string, string> ExternalUrls { get; set; }

        public Dictionary<string, string> ExternalIds { get; set; }

        public bool Explicit { get; set; }

        public int DurationMs { get; set; }

        public int DiscNumber { get; set; }

        public List<string> AvailableMarkets { get; set; }

        public List<SimpleArtistModel> Artists { get; set; }

        public SimpleAlbumModel Album { get; set; }

        public string Href { get; set; }

        public bool IsLocal { get; set; }


        [Display(Name="Duration in minutes")]
        public int DurationMinutes { get { return DurationMs / 60000; } private set { }}

    }
}