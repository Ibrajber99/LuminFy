using Me_Spotify_App.Models.Browse_Related;
using Me_Spotify_App.Models.PlayList_Related;
using Me_Spotify_App.Models.Track_Related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Me_Spotify_App.ViewModels
{
    public class BrowseViewModel
    {
        public BrowseViewModel()
        {
            SearchModel = new SearchRequestModel();
            AuthModel = new AuthViewmodel();
            Categories = new List<CategoryModel>();
            CategoryPlayLists = new List<SimplePlayListModel>();
            NewReleasesAlbums = new List<SimpleAlbumModel>();
        }

        public SearchRequestModel SearchModel { get; set; }

        public SearchResponseModel SearchResponseModel { get; set; }

        public AuthViewmodel AuthModel { get; set; }

        public List<CategoryModel> Categories { get; set; }

        public List<SimplePlayListModel> CategoryPlayLists { get; set; }

        public List<SimpleAlbumModel> NewReleasesAlbums { get; set; }
    }
}