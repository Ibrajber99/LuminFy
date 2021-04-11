using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Me_Spotify_App.API_CLIENT.Spotify_Player
{
    public interface SpotifyPlayer
    {
        Task<CurrentlyPlaying> GetCurrentlyPlayingItem
            (ISpotifyClient client, PlayerCurrentlyPlayingRequest request);

        Task<DeviceResponse> GetAvailableDevices(ISpotifyClient client);

        Task<bool> PauseCurrentPlayBack(ISpotifyClient client);

        Task<bool> ResumeCurrentPlayBack(ISpotifyClient client);

        Task<bool> SkipNextItem(ISpotifyClient client);

        Task<bool> SkipPreviousItem(ISpotifyClient client);

        Task<bool> AddToQueue
            (ISpotifyClient client, PlayerAddToQueueRequest request);
    }
}
