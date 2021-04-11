using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;


/// <summary>
/// Class for basic Player management
/// </summary>
namespace Me_Spotify_App.API_CLIENT.Spotify_Player
{
    public class PlayerRepo : SpotifyPlayer
    {
        public async Task<bool> AddToQueue
            (ISpotifyClient client, PlayerAddToQueueRequest request)
        {
            try
            {
                var addToQueue = await client.Player.AddToQueue(request);
                return addToQueue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DeviceResponse> GetAvailableDevices(ISpotifyClient client)
        {
            try
            {
                var device = await client.Player.GetAvailableDevices();
                return device;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CurrentlyPlaying> GetCurrentlyPlayingItem
            (ISpotifyClient client, PlayerCurrentlyPlayingRequest request)
        {
            try
            {
                var currentPlaying =await client.Player.GetCurrentlyPlaying(request);

                return currentPlaying;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> PauseCurrentPlayBack(ISpotifyClient client)
        {
            try
            {
                var pauseCurrent = await client.Player.PausePlayback();
                return pauseCurrent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ResumeCurrentPlayBack(ISpotifyClient client)
        {
            try
            {
                var resumeCurrent = await client.Player.ResumePlayback();
                return resumeCurrent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SkipNextItem(ISpotifyClient client)
        {
            try
            {
                var skipNext = await client.Player.SkipNext();
                return skipNext;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SkipPreviousItem(ISpotifyClient client)
        {
            try
            {
                var skipPrev = await client.Player.SkipPrevious();
                return skipPrev;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}