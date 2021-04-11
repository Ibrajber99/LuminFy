using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models
{
    public class PublicUserModel
    {
        [Display(Name ="Owner name")]
        public string DisplayName { get; set; }
        public Dictionary<string, string> ExternalUrls { get; set; }
        public FollowersModel Followers { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public List<ImageModel> Images { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }

    }
}