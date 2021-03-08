using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Me_Spotify_App.API_CLIENT.Spotify_Tracks
{
    public interface SpotifyTrack
    {
        Task<FullTrack> GetTrack(string id, ISpotifyClient client);

        Task<FullAlbum> GetTrackAlbum(string albumId, ISpotifyClient client);
    }
}
