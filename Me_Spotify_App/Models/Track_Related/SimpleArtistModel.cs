using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models.Track_Related
{
    public class SimpleArtistModel
    {
        public Dictionary<string, string> ExternalUrls { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }
}