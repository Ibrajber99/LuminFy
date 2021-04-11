using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Me_Spotify_App.API_CLIENT
{
    public interface SpotifyUser
    {
        Task<PrivateUser> GetAllInformation(ISpotifyClient client);

        Task<List<SimplePlaylist>> GetUsersPlayLists(ISpotifyClient client);

        Task<List<FullArtist>> GetUserTopArtists(ISpotifyClient client);

        Task<List<FullTrack>> GetUserTopTracks(ISpotifyClient client);

        Task<List<SimplePlaylist>> GetUserPlayLists(ISpotifyClient client);

        Task<PublicUser> GetPublicUser(string id, ISpotifyClient client);

        Task<List<SimplePlaylist>> GetPublicUserPlayLists(string id, ISpotifyClient client);
    }
}
