using AutoMapper;
using Me_Spotify_App.API_CLIENT;
using Me_Spotify_App.API_CLIENT.Spotify_Playlist;
using Me_Spotify_App.CookieManager;
using Me_Spotify_App.Models;
using Me_Spotify_App.Models.PlayList_Related;
using Me_Spotify_App.ViewModels;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// Controller for PlayList details
/// </summary>
namespace Me_Spotify_App.Controllers
{
    public class PlayListController : Controller
    {
        private const string ERROR_MESSAGE_PATH = "ErrorMessage";
        private SpotifyClient client;
        private FullPlayListViewModel _model;
        private SpotifyPlayList _playListRepo;
        private IMapper _mapper;
        private ICookieManager _cookiesManager;

        public PlayListController(IMapper mapper, SpotifyPlayList playListRepo,
            ICookieManager cookiesManager, FullPlayListViewModel model)
        {
            _mapper = mapper;
            _playListRepo = playListRepo;
            _cookiesManager = cookiesManager;
            _model = model;
        }

        public async Task<ActionResult> Index(string playListId)
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

                var playList = await _playListRepo.GetPlayList(playListId, client);

                var playListModel = _mapper.Map<FullPlayListModel>(playList);


                foreach (var pList in playList.Tracks.Items.ToList())
                {
                    if (pList?.Track?.Type == ItemType.Track)
                    {
                        var trackModel = new PlayListTrackModel<FullTrackModel>
                        {
                            AddedAt = pList.AddedAt,
                            AddedBy = _mapper.Map<PublicUserModel>(pList.AddedBy),
                            IsLocal = pList.IsLocal,
                            Track = _mapper.Map<FullTrackModel>(pList.Track)
                        };

                        playListModel.Tracks.Add(trackModel);
                    }
                }

                _model.PlayList = playListModel;

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
