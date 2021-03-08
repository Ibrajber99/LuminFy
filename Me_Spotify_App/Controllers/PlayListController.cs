using AutoMapper;
using Me_Spotify_App.API_CLIENT;
using Me_Spotify_App.API_CLIENT.Spotify_Playlist;
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
        private AuthViewmodel authCodeModel; //---> Holds the token and time when token got accquired
        private SpotifyClient client;
        private SpotifyPlayList _playListRepo;
        private IMapper _mapper;

        public PlayListController(IMapper mapper, SpotifyPlayList playListRepo)
        {
            _mapper = mapper;
            _playListRepo = playListRepo;
            authCodeModel = new AuthViewmodel();
        }

        public async Task<ActionResult> Index
            (string playListId, DateTime? tokenTime, string TokenGiven = null)
        {
            try
            {
                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser", "SpotifyUser");

                client = new SpotifyClient(TokenGiven);

                var playList = await _playListRepo.GetPlayList(playListId, client);

                var playListModel = _mapper.Map<FullPlayListModel>(playList);


                authCodeModel.TokenGiven = TokenGiven;
                authCodeModel.TokenTimeGeneration = tokenTime.Value;

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

                var model = new FullPlayListViewModel
                {
                    PlayList = playListModel,
                    AuthModel = authCodeModel
                };

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
