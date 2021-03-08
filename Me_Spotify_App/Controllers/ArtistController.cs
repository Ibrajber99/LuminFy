using AutoMapper;
using Me_Spotify_App.API_CLIENT;
using Me_Spotify_App.API_CLIENT.Spotify_Artist;
using Me_Spotify_App.Models;
using Me_Spotify_App.Models.Artist_Related;
using Me_Spotify_App.Models.Track_Related;
using Me_Spotify_App.ViewModels;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// Controller Fro artist details
/// </summary>
namespace Me_Spotify_App.Controllers
{
    public class ArtistController : Controller
    {
        private const string ERROR_MESSAGE_PATH = "ErrorMessage";
        private AuthViewmodel authCodeModel;//---> Holds the token and time when token got accquired
        private SpotifyClient client;
        private IMapper _mapper;
        private SpotifyArtist _artistRepo;
        private FullArtistViewModel model;


        public ArtistController(IMapper mapper, SpotifyArtist artistRepo )
        {
            _mapper = mapper;
            _artistRepo = artistRepo;
            authCodeModel = new AuthViewmodel();
            model = new FullArtistViewModel();
        }

        public async Task<ActionResult> Details
            (string artistId, DateTime? tokenTime, string TokenGiven = null)
        {
            try
            {
                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser", "SpotifyUser");



                authCodeModel.TokenGiven = TokenGiven;
                authCodeModel.TokenTimeGeneration = tokenTime.Value;

                client = new SpotifyClient(TokenGiven);

                var artistRes = _artistRepo.GetArtist(artistId, client);
                var artistAlbums = _artistRepo.GetArtistAlbums(artistId, client);
                var artistTopTracks = _artistRepo.GetArtistTopTracks(artistId, client);

                await Task.WhenAll(artistRes, artistAlbums, artistTopTracks);

                var artistmodel = _mapper.Map<FullArtistModel>(artistRes.Result);
                var artistAlbumsModel = _mapper.Map<List<SimpleAlbumModel>>(artistAlbums.Result);
                var artistTopTracksModel = _mapper.Map<List<FullTrackModel>>(artistTopTracks.Result);

                model.AuthModel = authCodeModel;

                model.ArtistModel =
                    artistmodel ?? new FullArtistModel();

                model.ArtistAlbums =
                    artistAlbumsModel ?? new List<SimpleAlbumModel>();

                model.ArtistTopTracks =
                    artistTopTracksModel ?? new List<FullTrackModel>();


                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }
    }
}
