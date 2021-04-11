using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models.Browse_Related
{
    public class CategoryModel
    {
        public string Href { get; set; }
        public List<ImageModel> Icons { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}