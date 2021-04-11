using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;

/// <summary>
/// Class for Playlist data
/// </summary>
namespace Me_Spotify_App.API_CLIENT.Spotify_Playlist
{
    public class PlayListRepo : SpotifyPlayList
    {
        public async Task<FullPlaylist> GetPlayList(string id, ISpotifyClient client)
        {
            try
            {
                var playList = await client.Playlists.Get(id);

                return playList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}