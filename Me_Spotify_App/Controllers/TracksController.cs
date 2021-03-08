using AutoMapper;
using Me_Spotify_App.API_CLIENT;
using Me_Spotify_App.API_CLIENT.Spotify_Tracks;
using Me_Spotify_App.Models;
using Me_Spotify_App.ViewModels;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// Controller for Track details
/// </summary>
namespace Me_Spotify_App.Controllers
{
    public class TracksController : Controller
    {
        private const string ERROR_MESSAGE_PATH = "ErrorMessage";

        private AuthViewmodel authCodeModel;//---> Holds the token and time when token got accquired
        private FullTrackViewModel model;
        private SpotifyClient client;
        private IMapper _mapper;
        private SpotifyTrack _trackrepo;


        public TracksController(IMapper mapper,SpotifyTrack trackrepo)
        {
            _mapper = mapper;
            _trackrepo = trackrepo;

            authCodeModel = new AuthViewmodel();
            model = new FullTrackViewModel();
        }

        public async Task<ActionResult> Details
         (string trackId, DateTime? tokenTime, string TokenGiven = null)
        {
            try
            {

                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser", "SpotifyUser");


                authCodeModel.TokenGiven = TokenGiven;
                authCodeModel.TokenTimeGeneration = tokenTime.Value;


                client = new SpotifyClient(TokenGiven);

                var trackRes = await _trackrepo.GetTrack(trackId, client);
                var albumRes = await _trackrepo.GetTrackAlbum(trackRes.Album.Id, client);

                var trackmodel = _mapper.Map<FullTrackModel>(trackRes);
                var albumModel = _mapper.Map<FullAlbumModel>(albumRes);



                model.AlbumModel = albumModel;
                model.TrackModel = trackmodel;
                model.AuthModel = authCodeModel;

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
