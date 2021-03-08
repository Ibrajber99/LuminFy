using Me_Spotify_App.Models;
using Me_Spotify_App.Models.Artist_Related;
using Me_Spotify_App.Models.Player_Related;
using Me_Spotify_App.Models.PlayList_Related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.ViewModels
{
    public class UserProfileViewModel
    {
        public AuthViewmodel AuthModel { get; set; }

        public SpotifyUserProfile UserProfile { get; set; }

        public List<FullTrackModel> UserTopTracks { get; set; }

        public List<FullArtistModel> UserTopArtists { get; set; }

        public List<SimplePlayListModel> UserPlayLists { get; set; }

        public CurrentlyPlayingModel CurrentlyPlaying { get; set; }

        public DeviceResponseModel CurrentDevices { get; set; }
    }
}