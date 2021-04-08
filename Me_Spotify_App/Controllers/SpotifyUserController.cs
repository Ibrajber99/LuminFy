using AutoMapper;
using Me_Spotify_App.API_CLIENT;
using Me_Spotify_App.API_CLIENT.Spotify_Player;
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
        private AuthViewmodel authCodeModel; //---> Holds the token and time when token got accquired
        private UserProfileViewModel model;
        private SpotifyClient client;
        private SpotifyUser _userRepo;
        private SpotifyPlayer _playerRepo;
        private IMapper _mapper;

        public SpotifyUserController
            (SpotifyUser userRepo, IMapper mapper, SpotifyPlayer playerRepo)
        {
            authCodeModel = new AuthViewmodel();

            model = new UserProfileViewModel();

            _userRepo = userRepo;
            _mapper = mapper;
            _playerRepo = playerRepo;
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
                            model.CurrentlyPlaying = currentPlayingItem;

                            model.CurrentlyPlaying.Item =
                                _mapper.Map<FullTrackModel>(currentlyPlaying.Result.Item);
                        }
                    }
                }
            }


            model.UserProfile = userModel;
            model.UserTopTracks = listofTracks;
            model.UserTopArtists = listofArtists;
            model.UserPlayLists = listOfPlayLists;
            model.AuthModel = authCodeModel;
            model.CurrentDevices = listOfDevices;

            return model;
        }


        public async Task<ActionResult> Index
            (DateTime? tokenTime,string TokenGiven = null)
        {
            try
            {
                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser");



                authCodeModel.TokenGiven = TokenGiven;
                authCodeModel.TokenTimeGeneration = tokenTime.Value;


                client = new SpotifyClient(TokenGiven);

                model = await GetPrivateUserDefaultModel(client);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }

            return View(model);
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


                authCodeModel.TokenGiven = responseConfig.AccessToken;
                authCodeModel.TimeLimit = responseConfig.ExpiresIn;
                authCodeModel.TokenTimeGeneration = DateTime.Now;
                authCodeModel.CallBackCode = code;

                client = new SpotifyClient(authCodeModel.TokenGiven);

                model = await GetPrivateUserDefaultModel(client);

                return RedirectToAction("Index", new { tokenTime = authCodeModel.TokenTimeGeneration,
                    TokenGiven = authCodeModel.TokenGiven });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }


        public async Task<ActionResult> GetPublicUserProfile
            (string userid, DateTime? tokenTime, string TokenGiven = null)
        {
            try
            {
                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser");


                authCodeModel.TokenGiven = TokenGiven;
                authCodeModel.TokenTimeGeneration = tokenTime.Value;


                client = new SpotifyClient(TokenGiven);

                var userProfile = _userRepo.GetPublicUser(userid, client);
                var userPlayList = _userRepo.GetPublicUserPlayLists(userid, client);

                await Task.WhenAll(userProfile, userPlayList);

                var userProfileModel = _mapper.Map<PublicUserModel>(userProfile.Result);
                var userPlayListsModel = _mapper.Map<List<SimplePlayListModel>>(userPlayList.Result);


                var model = new PublicUserViewModel()
                {
                    UserModel = userProfileModel,
                    UserPublicPlayLists = userPlayListsModel,
                    Authmodel = authCodeModel
                };

                return View(model);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }


        //Player Methods
        public async Task<ActionResult> PlayNextTrack
            (DateTime? tokenTime, string TokenGiven = null)
        {
            try
            {
                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser");

                client = new SpotifyClient(TokenGiven);

                var trySkipNext = await _playerRepo.SkipNextItem(client);
                var modelValues = await GetPrivateUserDefaultModel(client);

                var m = trySkipNext;
                model = modelValues;

                return
                    RedirectToAction
                    ("Index", new
                    {
                        tokenTime = tokenTime,
                        TokenGiven = TokenGiven
                    });
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }

        public async Task<ActionResult>PlayPrevTrack
            (DateTime? tokenTime, string TokenGiven = null)
        {
            try
            {
                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser");

                client = new SpotifyClient(TokenGiven);

                var trySkipPrev = await _playerRepo.SkipPreviousItem(client);
                var modelValues = await GetPrivateUserDefaultModel(client);


                var m = trySkipPrev;
                model = modelValues;

                return
                    RedirectToAction
                    ("Index", new
                    {
                        tokenTime = tokenTime,
                        TokenGiven = TokenGiven
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }

        public async Task<ActionResult>PauseCurrentTrack
            (DateTime? tokenTime, string TokenGiven = null)
        {
            try
            {
                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser");

                client = new SpotifyClient(TokenGiven);

                var tryPauseTrack = await _playerRepo.PauseCurrentPlayBack(client);
                var modelValues = await GetPrivateUserDefaultModel(client);

                model = modelValues;


                return
                    RedirectToAction
                    ("Index", new
                    {
                        tokenTime = tokenTime,
                        TokenGiven = TokenGiven
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH); throw;
            }
        }

        public async Task<ActionResult> ResumeCurrentTrack
            (DateTime? tokenTime, string TokenGiven = null)
        {
            try
            {
                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser");

                client = new SpotifyClient(TokenGiven);

                var tryResumeTrack = await _playerRepo.ResumeCurrentPlayBack(client);
                var modelValues = await GetPrivateUserDefaultModel(client);

                model = modelValues;


                return
                    RedirectToAction
                    ("Index", new
                    {
                        tokenTime = tokenTime,
                        TokenGiven = TokenGiven
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }

        public async Task<ActionResult> AddToQueue
            (string itemUri, DateTime? tokenTime, string TokenGiven = null)
        {
            try
            {
                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser");

                client = new SpotifyClient(TokenGiven);

                var addToQueue = await _playerRepo.AddToQueue
                    (client, new PlayerAddToQueueRequest(itemUri));

                var modelValues = await GetPrivateUserDefaultModel(client);

                model = modelValues;

                return
                    RedirectToAction
                    ("Index", new
                    {
                        tokenTime = tokenTime,
                        TokenGiven = TokenGiven
                    });

            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }
        }
    }
}
