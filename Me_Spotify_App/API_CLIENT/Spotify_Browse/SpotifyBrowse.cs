using Me_Spotify_App.Models.Browse_Related;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Me_Spotify_App.API_CLIENT.Spotify_Browse
{
    public interface SpotifyBrowse
    {
        Task<SearchResponse> GetSearchResult(SearchRequest request,ISpotifyClient client);

        Task<List<Category>> GetCategories(ISpotifyClient client);

        Task<List<SimplePlaylist>> GetCategoryPlayLists(string id, ISpotifyClient client);

        Task<List<SimpleAlbum>> GetNewReleases(ISpotifyClient client);
    }
}
