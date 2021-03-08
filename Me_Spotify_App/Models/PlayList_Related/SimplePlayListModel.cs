using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models.PlayList_Related
{
    public class SimplePlayListModel
    {
        public bool Collaborative { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> ExternalUrls { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public List<ImageModel> Images { get; set; }
        public string Name { get; set; }
        public PublicUserModel Owner { get; set; }
        public bool? Public { get; set; }
        public string SnapshotId { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }
}