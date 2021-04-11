using AutoMapper;
using Me_Spotify_App.API_CLIENT;
using Me_Spotify_App.API_CLIENT.Spotify_Browse;
using Me_Spotify_App.CookieManager;
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
        private SpotifyBrowse _browseRepo;
        private SpotifyClient client;
        private IMapper _mapper;
        private BrowseViewModel _model;
        private ICookieManager _cookiesManager;

        public BrowseController(SpotifyBrowse browseRepo, IMapper mapper,
            ICookieManager cookiesManager, BrowseViewModel model)
        {
            _mapper = mapper;
            _browseRepo = browseRepo;
            _model = model;
            _cookiesManager = cookiesManager;
        }

        // GET: Browse
        public async Task<ActionResult> Index()
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

                var categoriesRes = await _browseRepo.GetCategories(client);


                var categoyListModel = _mapper.Map<List<CategoryModel>>(categoriesRes);

                _model.Categories = categoyListModel;



                return View(_model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ERROR_MESSAGE_PATH);

            }

        }

        public async Task<ActionResult> CategoryPlayList
            (string categoryId)
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

                var categoryPlayLists = await _browseRepo
                    .GetCategoryPlayLists(categoryId, client);

                var categoryPlayListsModel = _mapper
                    .Map<List<SimplePlayListModel>>(categoryPlayLists);



                if (categoryPlayListsModel != null)
                    _model.CategoryPlayLists = categoryPlayListsModel;



                return View(_model);
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

            if (ModelState.IsValid)
            {
                try
                {
                    var searchRequestSpotify = _mapper.Map<SearchRequest>(inputModel.SearchModel);

                    client = ApiClientConfig.GetClientInstance(tokenGiven.Value);

                    var searchRes = await _browseRepo.GetSearchResult(searchRequestSpotify, client);

                    var searchResModel = _mapper.Map<SearchResponseModel>(searchRes);


                    _model.SearchResponseModel = searchResModel;



                    return View(_model);
                }
                catch (Exception ex)
                 {

                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(ERROR_MESSAGE_PATH);
                }            
            }

            return View(_model);
        }
    }
}
