using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Class for artist related data
/// </summary>
namespace Me_Spotify_App.API_CLIENT.Spotify_Artist
{
    public class ArtistRepo : SpotifyArtist
    {
        public async Task<FullArtist> GetArtist(string id,ISpotifyClient client)
        {
            try
            {
                var artist = client.Artists.Get(id);

                return await artist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SimpleAlbum>> GetArtistAlbums(string id, ISpotifyClient client)
        {
            try
            {
                var artistAlbums =  await client.Artists.GetAlbums(id);

                return artistAlbums.Items.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FullTrack>> GetArtistTopTracks(string id, ISpotifyClient client,
            string market = "US")
        {
            try
            {
                var topTrackRequest = new ArtistsTopTracksRequest(market);

                var artistTopTracks = await client.Artists.GetTopTracks(id, topTrackRequest);

                return artistTopTracks.Tracks;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }
    }
}
