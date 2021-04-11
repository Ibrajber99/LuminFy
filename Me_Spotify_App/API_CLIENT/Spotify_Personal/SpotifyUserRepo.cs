using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


/// <summary>
/// Class for User data
/// </summary>
namespace Me_Spotify_App.API_CLIENT
{
    public class SpotifyUserRepo : SpotifyUser
    {
        public SpotifyUserRepo()
        {

        }

        public async Task<PrivateUser> GetAllInformation(ISpotifyClient client)
        {
            try
            {
                var userInfo = client.UserProfile.Current();
                return await userInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PublicUser> GetPublicUser(string id, ISpotifyClient client)
        {
            try
            {
                var publicUserProfile = client.UserProfile.Get(id);
                return await publicUserProfile;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SimplePlaylist>> GetPublicUserPlayLists(string id, ISpotifyClient client)
        {
            try
            {
                var publicUserPlayLists = await client.Playlists.GetUsers(id);

                return  publicUserPlayLists.Items.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SimplePlaylist>> GetUserPlayLists(ISpotifyClient client)
        {
            try
            {
                var playLists = await client.Playlists.CurrentUsers();

                return playLists.Items.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SimplePlaylist>> GetUsersPlayLists(ISpotifyClient client)
        {
            try
            {
                var userPlayLists = await client.Playlists.CurrentUsers();

                var userPlayListsList = userPlayLists.Items.ToList();

                return userPlayListsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<FullArtist>> GetUserTopArtists(ISpotifyClient client)
        {
            try
            {
                var userTopArtists = await client.Personalization.GetTopArtists();

                var userTopArtistsList = userTopArtists.Items.ToList();

                return userTopArtistsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FullTrack>> GetUserTopTracks(ISpotifyClient client)
        {
            try
            {
                var userTopTracks = await client.Personalization.GetTopTracks();

                 var tracksList = userTopTracks.Items.ToList();

                return tracksList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}