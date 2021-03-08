using AutoMapper;
using Me_Spotify_App.API_CLIENT;
using Me_Spotify_App.API_CLIENT.Spotify_Browse;
using Me_Spotify_App.Models.Browse_Related;
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
/// Controller for Browsing/Searching
/// </summary>
namespace Me_Spotify_App.Controllers
{
    public class BrowseController : Controller
    {
        private const string ERROR_MESSAGE_PATH = "ErrorMessage";
        private AuthViewmodel authCodeModel; //---> Holds the token and time when token got accquired
        private SpotifyBrowse _browseRepo;
        private SpotifyClient client;
        private IMapper _mapper;
        private BrowseViewModel model;


        public BrowseController(SpotifyBrowse browseRepo, IMapper mapper)
        {
            _mapper = mapper;
            _browseRepo = browseRepo;
            authCodeModel = new AuthViewmodel();
            model = new BrowseViewModel();
        }

        // GET: Browse
        public async Task<ActionResult> Index(DateTime? tokenTime, string TokenGiven = null)
        {
            try
            {
                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser", "SpotifyUser");


                authCodeModel.TokenGiven = TokenGiven;
                authCodeModel.TokenTimeGeneration = tokenTime.Value;


                client = new SpotifyClient(TokenGiven);

                var categoriesRes = await _browseRepo.GetCategories(client);


                var categoyListModel = _mapper.Map<List<CategoryModel>>(categoriesRes);

                model.Categories = categoyListModel;
                model.AuthModel = authCodeModel;


                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);

            }

        }

        public async Task<ActionResult> CategoryPlayList
            (string categoryId, DateTime? tokenTime, string TokenGiven = null)
        {
            try
            {
                if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                    return RedirectToAction("LoginUser", "SpotifyUser");


                authCodeModel.TokenGiven = TokenGiven;
                authCodeModel.TokenTimeGeneration = tokenTime.Value;


                client = new SpotifyClient(TokenGiven);

                var categoryPlayLists = await _browseRepo
                    .GetCategoryPlayLists(categoryId, client);

                var categoryPlayListsModel = _mapper
                    .Map<List<SimplePlayListModel>>(categoryPlayLists);



                if (categoryPlayListsModel != null)
                    model.CategoryPlayLists = categoryPlayListsModel;

                model.AuthModel = authCodeModel;

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);
            }

        }


        public async Task<ActionResult> GetSearchedValues
            (BrowseViewModel inputModel)
        {

            var TokenGiven = inputModel.AuthModel.TokenGiven;
            DateTime? tokenTime = inputModel.AuthModel.TokenTimeGeneration;


            if (ApiClientConfig.IsIfTokenExpired(tokenTime, TokenGiven))
                return RedirectToAction("LoginUser", "SpotifyUser");

            authCodeModel.TokenGiven = TokenGiven;
            authCodeModel.TokenTimeGeneration = tokenTime.Value;

            if (ModelState.IsValid)
            {
                try
                {
                    var searchRequestSpotify = _mapper.Map<SearchRequest>(inputModel.SearchModel);

                    client = new SpotifyClient(TokenGiven);

                    var searchRes = await _browseRepo.GetSearchResult(searchRequestSpotify, client);

                    var searchResModel = _mapper.Map<SearchResponseModel>(searchRes);


                    model.SearchResponseModel = searchResModel;
                    model.AuthModel = authCodeModel;


                    return View(model);
                }
                catch (Exception ex)
                 {

                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(ERROR_MESSAGE_PATH);
                }            
            }

            return View(model);
        }
    }
}
