using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// Class for Track data
/// </summary>
namespace Me_Spotify_App.API_CLIENT.Spotify_Tracks
{
    public class TrackRepo : SpotifyTrack
    {
        public async Task<FullTrack> GetTrack(string id, ISpotifyClient client)
        {
            try
            {
                var track = client.Tracks.Get(id);

                return await track;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<FullAlbum> GetTrackAlbum(string albumId, ISpotifyClient client)
        {
            try
            {
                var trackAlbum = client.Albums.Get(albumId);

                return await trackAlbum;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}