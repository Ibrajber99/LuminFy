using System.ComponentModel.DataAnnotations;

namespace Me_Spotify_App.Models
{
    public class FollowersModel
    {
        public string Href { get; set; }

        [DisplayFormat(DataFormatString = "{0:n}")]
        public int Total { get; set; }
    }
}