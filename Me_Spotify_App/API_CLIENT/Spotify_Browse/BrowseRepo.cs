using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


/// <summary>
/// Class for Browsing data
/// </summary>
namespace Me_Spotify_App.API_CLIENT.Spotify_Browse
{
    public class BrowseRepo : SpotifyBrowse
    {
        public async Task<List<Category>> GetCategories(ISpotifyClient client)
        {
            try
            {
                var categories = await client.Browse.GetCategories();
                var res = categories.Categories.Items.ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SimplePlaylist>> GetCategoryPlayLists(string id, ISpotifyClient client)
        {
            try
            {
                var resultRequest = new CategoriesPlaylistsRequest();
                resultRequest.Limit = 50;

                var categoryPlayLists = await client.Browse.GetCategoryPlaylists(id, resultRequest);
                var res = categoryPlayLists.Playlists.Items.ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SimpleAlbum>> GetNewReleases(ISpotifyClient client)
        {
            try
            {
                var newReleases = await client.Browse.GetNewReleases();
                var res = newReleases.Albums.Items.ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SearchResponse> GetSearchResult
           (SearchRequest request, ISpotifyClient client)
        {
            try
            {
                request.Limit = 50;

                var searchRes = await client.Search.Item(request);

                return searchRes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
