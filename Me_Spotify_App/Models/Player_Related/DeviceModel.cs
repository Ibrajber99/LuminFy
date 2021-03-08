using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.Models.Player_Related
{
    public class DeviceModel
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrivateSession { get; set; }
        public bool IsRestricted { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? VolumePercent { get; set; }
    }
}