using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models.Browse_Related
{
    public class SearchRequestModel
    {

        public SearchRequestModel()
        {

            Type = Types.All;
            TypesList = new List<Types> 
            { Types.Album, Types.Track, Types.Playlist, Types.Artist,Types.All};
        }

        public List<Types> TypesList { get; private set; }

       
        public Types Type { get; set; }

        [Required]
        [Display(Name ="Search ")]
        public string Query { get; set; }

        public int? Limit { get; set; }

        public IncludeExternals? IncludeExternal { get; set; }

        public enum IncludeExternals
        {
            Audio = 1
        }

        public enum Types
        {
            Album = 1,
            Artist = 2,
            Playlist = 4,
            Track = 8,
            Show = 16,
            Episode = 32,
            All = 63
        }
    }
}