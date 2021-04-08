using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// Class for managing access to Spotify API
/// </summary>
namespace Me_Spotify_App.API_CLIENT
{

    public static class ApiClientConfig
    {
        //Client ID code goes here
        public const string CLIENT_ID = "bb36044f469e4e9c85781ac576e79473";

        //Client secret code goes here
        public const string CLIENT_SECRET = "fdafbcd6ed6b4d1dbf3aeeda774f9ba1";

        //Application URL path + /callback/
        public const string CallBackURI = "https://localhost:44384/SpotifyUser/callback/";


        public static Uri GetRedirectionUriPath()
        {
            var loginRequest = new LoginRequest(
             new Uri(CallBackURI),
             CLIENT_ID,
             LoginRequest.ResponseType.Code)

            {
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative,Scopes.UgcImageUpload,
                    Scopes.UserReadRecentlyPlayed,Scopes.UserReadPlaybackPosition
                ,Scopes.UserTopRead,Scopes.UserLibraryRead,Scopes.UserLibraryModify,
                    Scopes.PlaylistModifyPrivate,Scopes.UserFollowRead,Scopes.PlaylistModifyPublic,
                Scopes.UserReadPrivate,Scopes.UserReadEmail,Scopes.AppRemoteControl,
                    Scopes.Streaming,Scopes.UserReadCurrentlyPlaying,Scopes.UserModifyPlaybackState
                ,Scopes.UserReadPlaybackState,Scopes.UserFollowModify}
            };

            return loginRequest.ToUri();
        }


        public static async Task<AuthorizationCodeTokenResponse> GetClientReponse
            (string callBackCode)
        {
            try
            {
                var res = await new OAuthClient().RequestToken(
                    new AuthorizationCodeTokenRequest(CLIENT_ID, CLIENT_SECRET,
                    callBackCode, new Uri(CallBackURI)));

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<SpotifyClient> GetValidClient
            (string callBackCode)
        {
            try
            {
                var config = await GetClientReponse(callBackCode);

                var client = new SpotifyClient(config);

                return client;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static bool IsIfTokenExpired
            (DateTime? tokenTime, string TokenGiven = null)
        {
            if (string.IsNullOrEmpty(TokenGiven)
            || tokenTime == null)
                return true;


            var expiryDate = tokenTime?.AddMinutes(60);


            if (expiryDate <= DateTime.Now)
                return true;

            return false;
        }

    }
}