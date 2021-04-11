using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Me_Spotify_App.API_CLIENT.Spotify_Playlist
{
    public interface SpotifyPlayList
    {
        Task<FullPlaylist> GetPlayList(string id, ISpotifyClient client);

    }
}
