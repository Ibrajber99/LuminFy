using Me_Spotify_App.Models.Track_Related;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Me_Spotify_App.Models
{
    public class SimpleTrackModel
    {
        public List<string> AvailableMarkets { get; set; }
        public int DiscNumber { get; set; }
        public int DurationMs { get; set; }
        public bool Explicit { get; set; }
        public Dictionary<string, string> ExternalUrls { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public bool IsPlayable { get; set; }
        public LinkedTrackModel LinkedFrom { get; set; }
        public string Name { get; set; }
        public string PreviewUrl { get; set; }
        public int TrackNumber { get; set; }

        public string Uri { get; set; }


        [Display(Name = "Duration in minutes")]
        public int DurationMinutes { get { return DurationMs / 60000; }
            private set { } }
    }
}