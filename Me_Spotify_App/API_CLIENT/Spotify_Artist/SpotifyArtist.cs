using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Me_Spotify_App.API_CLIENT.Spotify_Artist
{
    public interface SpotifyArtist
    {
        Task<FullArtist> GetArtist(string id,ISpotifyClient client);

        Task<List<SimpleAlbum>> GetArtistAlbums(string id,ISpotifyClient client);

        Task<List<FullTrack>> GetArtistTopTracks(string id, ISpotifyClient client,string market = "US");

    }
}
