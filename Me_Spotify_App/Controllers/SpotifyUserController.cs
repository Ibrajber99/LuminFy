using AutoMapper;
using Me_Spotify_App.API_CLIENT;
using Me_Spotify_App.API_CLIENT.Spotify_Player;
using Me_Spotify_App.CookieManager;
using Me_Spotify_App.Models;
using Me_Spotify_App.Models.Artist_Related;
using Me_Spotify_App.Models.Player_Related;
using Me_Spotify_App.Models.PlayList_Related;
using Me_Spotify_App.ViewModels;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static SpotifyAPI.Web.PlayerCurrentlyPlayingRequest;

/// <summary>
/// Controller for logged in user data
/// Control player(PAUSE,PLAY,NEXT,PREV)
/// </summary>
namespace Me_Spotify_App.Controllers
{
    public class SpotifyUserController : Controller
    {
        private const string ERROR_MESSAGE_PATH = "ErrorMessage";
        private UserProfileViewModel _model;
        private PublicUserViewModel _userModel;
        private SpotifyClient client;
        private SpotifyUser _userRepo;
        private SpotifyPlayer _playerRepo;
        private IMapper _mapper;
        private ICookieManager _cookiesManager;

        public SpotifyUserController
            (SpotifyUser userRepo,
            IMapper mapper,
            SpotifyPlayer playerRepo,
            ICookieManager cookiesManager,
            PublicUserViewModel userModel,
            UserProfileViewModel model)
        {

            _model = model;
            _userModel = userModel;
            _userRepo = userRepo;
            _mapper = mapper;
            _playerRepo = playerRepo;
            _cookiesManager = cookiesManager;

        }


        public async Task<UserProfileViewModel> GetPrivateUserDefaultModel
            (ISpotifyClient client)
        {

            var userProfileRes = _userRepo.GetAllInformation(client);
            var userTopTracksRes = _userRepo.GetUserTopTracks(client);
            var userTopArtistsRes = _userRepo.GetUserTopArtists(client);
            var userPlayListsRes = _userRepo.GetUserPlayLists(client);
            var availableDevices = _playerRepo.GetAvailableDevices(client);
            var currentlyPlaying = _playerRepo.GetCurrentlyPlayingItem(client,
                new PlayerCurrentlyPlayingRequest(AdditionalTypes.Track));


            await Task.WhenAll(userProfileRes, userTopTracksRes,
                userTopArtistsRes, userPlayListsRes, availableDevices, currentlyPlaying);

            var userModel = _mapper.Map<SpotifyUserProfile>(userProfileRes.Result);
            var listofTracks = _mapper.Map<List<FullTrackModel>>(userTopTracksRes.Result);
            var listofArtists = _mapper.Map<List<FullArtistModel>>(userTopArtistsRes.Result);
            var listOfPlayLists = _mapper.Map<List<SimplePlayListModel>>(userPlayListsRes.Result);
            var listOfDevices = _mapper.Map<DeviceResponseModel>(availableDevices.Result);

            if (currentlyPlaying.Result != null)
            {
                if (currentlyPlaying.Result.Item != null)
                {
                    if (currentlyPlaying.Result.Item.Type == ItemType.Track)
                    {
                        var currentPlayingItem = _mapper.Map<CurrentlyPlayingModel>(currentlyPlaying.Result);
                        if (currentPlayingItem != null)
                        {
                            _model.CurrentlyPlaying = currentPlayingItem;

                            _model.CurrentlyPlaying.Item =
                                _mapper.Map<FullTrackModel>(currentlyPlaying.Result.Item);
                        }
                    }
                }
            }


            _model.UserProfile = userModel;
            _model.UserTopTracks = listofTracks;
            _model.UserTopArtists = listofArtists;
            _model.UserPlayLists = listOfPlayLists;
            _model.CurrentDevices = listOfDevices;

            return _model;
        }


        public async Task<ActionResult> Index()
        {
            try
            {

                var tokenGiven = _cookiesManager.GetCookie("TokenGiven", Request);
                var tokenTimes = _cookiesManager.GetCookie("TokenTime", Request);

                if (tokenGiven != null && tokenTimes != null)
                {
                    if (ApiClientConfig.IsIfTokenExpired(Convert.ToDateTime(tokenTimes.Value), tokenGiven.Value))
                        return RedirectToAction("LoginUser");
                }
                else
                {
                    return RedirectToAction("LoginUser");
                }


                client = ApiClientConfig.GetClientInstance(tokenGiven.Value);

                _model = await GetPrivateUserDefaultModel(client);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }

            return View(_model);
        }

        public ActionResult LoginUser()
        {
            try
            {
                var urlRedirection = ApiClientConfig.GetRedirectionUriPath();

                return Redirect(urlRedirection.ToString());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }

        public async Task<ActionResult> Callback(string code)
        {
            try
            {
                var responseConfig = await ApiClientConfig.GetClientReponse(code);

                var tokenCookie = new HttpCookie("TokenGiven", responseConfig.AccessToken);
                var tokentime = new HttpCookie("TokenTime", DateTime.Now.ToString());

                _cookiesManager.AddCookie(tokenCookie, Response);
                _cookiesManager.AddCookie(tokentime, Response);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }


        public async Task<ActionResult> GetPublicUserProfile(string userid)
        {
            try
            {
                var tokenGiven = _cookiesManager.GetCookie("TokenGiven", Request);
                var tokenTimes = _cookiesManager.GetCookie("TokenTime", Request);

                if (tokenGiven != null && tokenTimes != null)
                {
                    if (ApiClientConfig.IsIfTokenExpired(Convert.ToDateTime(tokenTimes.Value), tokenGiven.Value))
                        return RedirectToAction("LoginUser");
                }
                else
                {
                    return RedirectToAction("LoginUser");
                }


                client = ApiClientConfig.GetClientInstance(tokenGiven.Value);

                var userProfile = _userRepo.GetPublicUser(userid, client);
                var userPlayList = _userRepo.GetPublicUserPlayLists(userid, client);

                await Task.WhenAll(userProfile, userPlayList);

                var userProfileModel = _mapper.Map<PublicUserModel>(userProfile.Result);
                var userPlayListsModel = _mapper.Map<List<SimplePlayListModel>>(userPlayList.Result);

                _userModel.UserModel = userProfileModel;
                _userModel.UserPublicPlayLists = userPlayListsModel;


                return View(_userModel);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }


        //Player Methods
        public async Task<ActionResult> PlayNextTrack()
        {
            try
            {
                var tokenGiven = _cookiesManager.GetCookie("TokenGiven", Request);
                var tokenTimes = _cookiesManager.GetCookie("TokenTime", Request);

                if (tokenGiven != null && tokenTimes != null)
                {
                    if (ApiClientConfig.IsIfTokenExpired(Convert.ToDateTime(tokenTimes.Value), tokenGiven.Value))
                        return RedirectToAction("LoginUser");
                }
                else
                {
                    return RedirectToAction("LoginUser");
                }

                client = ApiClientConfig.GetClientInstance(tokenGiven.Value);

                var trySkipNext = await _playerRepo.SkipNextItem(client);
                var modelValues = await GetPrivateUserDefaultModel(client);

                var m = trySkipNext;
                _model = modelValues;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }

        public async Task<ActionResult> PlayPrevTrack()
        {
            try
            {
                var tokenGiven = _cookiesManager.GetCookie("TokenGiven", Request);
                var tokenTimes = _cookiesManager.GetCookie("TokenTime", Request);

                if (tokenGiven != null && tokenTimes != null)
                {
                    if (ApiClientConfig.IsIfTokenExpired(Convert.ToDateTime(tokenTimes.Value), tokenGiven.Value))
                        return RedirectToAction("LoginUser");
                }
                else
                {
                    return RedirectToAction("LoginUser");
                }

                client = ApiClientConfig.GetClientInstance(tokenGiven.Value);

                var trySkipPrev = await _playerRepo.SkipPreviousItem(client);
                var modelValues = await GetPrivateUserDefaultModel(client);


                var m = trySkipPrev;
                _model = modelValues;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }

        public async Task<ActionResult> PauseCurrentTrack()
        {
            try
            {
                var tokenGiven = _cookiesManager.GetCookie("TokenGiven", Request);
                var tokenTimes = _cookiesManager.GetCookie("TokenTime", Request);

                if (tokenGiven != null && tokenTimes != null)
                {
                    if (ApiClientConfig.IsIfTokenExpired(Convert.ToDateTime(tokenTimes.Value), tokenGiven.Value))
                        return RedirectToAction("LoginUser");
                }
                else
                {
                    return RedirectToAction("LoginUser");
                }

                client = ApiClientConfig.GetClientInstance(tokenGiven.Value);

                var tryPauseTrack = await _playerRepo.PauseCurrentPlayBack(client);
                var modelValues = await GetPrivateUserDefaultModel(client);

                _model = modelValues;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH); throw;
            }
        }

        public async Task<ActionResult> ResumeCurrentTrack()
        {
            try
            {
                var tokenGiven = _cookiesManager.GetCookie("TokenGiven", Request);
                var tokenTimes = _cookiesManager.GetCookie("TokenTime", Request);

                if (tokenGiven != null && tokenTimes != null)
                {
                    if (ApiClientConfig.IsIfTokenExpired(Convert.ToDateTime(tokenTimes.Value), tokenGiven.Value))
                        return RedirectToAction("LoginUser");
                }
                else
                {
                    return RedirectToAction("LoginUser");
                }

                client = ApiClientConfig.GetClientInstance(tokenGiven.Value);

                var tryResumeTrack = await _playerRepo.ResumeCurrentPlayBack(client);
                var modelValues = await GetPrivateUserDefaultModel(client);

                _model = modelValues;


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }

        public async Task<ActionResult> AddToQueue(string itemUri)
        {
            try
            {
                var tokenGiven = _cookiesManager.GetCookie("TokenGiven", Request);
                var tokenTimes = _cookiesManager.GetCookie("TokenTime", Request);

                if (tokenGiven != null && tokenTimes != null)
                {
                    if (ApiClientConfig.IsIfTokenExpired(Convert.ToDateTime(tokenTimes.Value), tokenGiven.Value))
                        return RedirectToAction("LoginUser");
                }
                else
                {
                    return RedirectToAction("LoginUser");
                }


                client = ApiClientConfig.GetClientInstance(tokenGiven.Value);

                var addToQueue = await _playerRepo.AddToQueue
                    (client, new PlayerAddToQueueRequest(itemUri));

                var modelValues = await GetPrivateUserDefaultModel(client);

                _model = modelValues;


                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }
    }
}
