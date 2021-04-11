using AutoMapper;
using Me_Spotify_App.API_CLIENT;
using Me_Spotify_App.API_CLIENT.Spotify_Tracks;
using Me_Spotify_App.CookieManager;
using Me_Spotify_App.Models;
using Me_Spotify_App.ViewModels;
using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

/// <summary>
/// Controller for Track details
/// </summary>
namespace Me_Spotify_App.Controllers
{
    public class TracksController : Controller
    {
        private const string ERROR_MESSAGE_PATH = "ErrorMessage";
        private FullTrackViewModel _model;
        private SpotifyClient client;
        private IMapper _mapper;
        private SpotifyTrack _trackrepo;
        private ICookieManager _cookiesManager;


        public TracksController(IMapper mapper,SpotifyTrack trackrepo,
            ICookieManager cookiesManager, FullTrackViewModel model)
        {
            _mapper = mapper;
            _trackrepo = trackrepo;
            _model = model;
            _cookiesManager = cookiesManager;
        }

        public async Task<ActionResult> Details(string trackId)
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

                var trackRes = await _trackrepo.GetTrack(trackId, client);
                var albumRes = await _trackrepo.GetTrackAlbum(trackRes.Album.Id, client);

                var trackmodel = _mapper.Map<FullTrackModel>(trackRes);
                var albumModel = _mapper.Map<FullAlbumModel>(albumRes);



                _model.AlbumModel = albumModel;
                _model.TrackModel = trackmodel;

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
