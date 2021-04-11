using AutoMapper;
using Me_Spotify_App.API_CLIENT;
using Me_Spotify_App.API_CLIENT.Spotify_Artist;
using Me_Spotify_App.CookieManager;
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
        private SpotifyClient client;
        private IMapper _mapper;
        private SpotifyArtist _artistRepo;
        private FullArtistViewModel _model;
        private ICookieManager _cookiesManager;

        public ArtistController
            (IMapper mapper, SpotifyArtist artistRepo,
            ICookieManager cookiesManager, FullArtistViewModel model )
        {
            _mapper = mapper;
            _artistRepo = artistRepo;
            _model = model;
            _cookiesManager = cookiesManager;
        }

        public async Task<ActionResult> Details(string artistId)
        {
            try
            {
                var tokenGiven = _cookiesManager.GetCookie("TokenGiven", Request);
                var tokenTimes = _cookiesManager.GetCookie("TokenTime", Request);

                if (tokenGiven != null && tokenTimes != null)
                {
                    if (ApiClientConfig.IsIfTokenExpired(Convert.ToDateTime(tokenTimes.Value), tokenGiven.Value))
                        return RedirectToAction("LoginUser", "SpotifyUser");
                }
                else
                {
                    return RedirectToAction("LoginUser", "SpotifyUser");
                }

                client = ApiClientConfig.GetClientInstance(tokenGiven.Value);

                var artistRes = _artistRepo.GetArtist(artistId, client);
                var artistAlbums = _artistRepo.GetArtistAlbums(artistId, client);
                var artistTopTracks = _artistRepo.GetArtistTopTracks(artistId, client);

                await Task.WhenAll(artistRes, artistAlbums, artistTopTracks);

                var artistmodel = _mapper.Map<FullArtistModel>(artistRes.Result);
                var artistAlbumsModel = _mapper.Map<List<SimpleAlbumModel>>(artistAlbums.Result);
                var artistTopTracksModel = _mapper.Map<List<FullTrackModel>>(artistTopTracks.Result);




                _model.ArtistModel =
                    artistmodel ?? new FullArtistModel();

                _model.ArtistAlbums =
                    artistAlbumsModel ?? new List<SimpleAlbumModel>();

                _model.ArtistTopTracks =
                    artistTopTracksModel ?? new List<FullTrackModel>();


                return View(_model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }
    }
}
