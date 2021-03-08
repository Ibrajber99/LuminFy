using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models.Artist_Related
{
    public class FullArtistModel
    {
        public Dictionary<string, string> ExternalUrls { get; set; }
        public FollowersModel Followers { get; set; }
        public List<string> Genres { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public List<ImageModel> Images { get; set; }
        public string Name { get; set; }
        public int Popularity { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }

    }
}