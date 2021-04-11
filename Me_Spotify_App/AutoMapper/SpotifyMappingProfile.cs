using AutoMapper;
using Me_Spotify_App.Models;
using Me_Spotify_App.Models.Artist_Related;
using Me_Spotify_App.Models.Browse_Related;
using Me_Spotify_App.Models.Player_Related;
using Me_Spotify_App.Models.PlayList_Related;
using Me_Spotify_App.Models.Track_Related;
using SpotifyAPI.Web;
using System.Linq;

/// <summary>
/// Mapping the SPotfiy client library with the application domain models
/// </summary>
namespace Me_Spotify_App.AutoMapper
{
    public class SpotifyMappingProfile : Profile
    {
        public SpotifyMappingProfile()
        {
            CreateMap<SpotifyUserProfile, PrivateUser>()
                .ReverseMap();

            CreateMap<FollowersModel, Followers>()
                .ReverseMap();

            CreateMap<ImageModel, Image>()
                .ReverseMap();

            CreateMap<FullArtistModel, FullArtist>()
                .ReverseMap();

            CreateMap<FullTrackModel, FullTrack>()
                .ReverseMap()
                .ForMember(t => t.DurationMinutes,opt => opt.Ignore());

            CreateMap<LinkedTrackModel, LinkedTrack>()
                .ReverseMap();

            CreateMap<SimpleArtistModel, SimpleArtist>()
                .ReverseMap();

            CreateMap<SimpleAlbumModel, SimpleAlbum>()
                .ReverseMap();

            CreateMap<SimpleTrack, SimpleTrackModel>()
                .ReverseMap()
                .ForMember(m => m.Artists, opt => opt.Ignore())
                .ForMember(m => m.Type, opt => opt.Ignore());

            CreateMap<FullAlbumModel, FullAlbum>()
                .ReverseMap()
                .ForMember(m => m.Tracks, 
                opt => opt.MapFrom(c => c.Tracks.Items.ToList()));

            CreateMap<CopyrightModel, Copyright>()
                .ReverseMap();

            CreateMap<PublicUserModel, PublicUser>()
                .ReverseMap();

            CreateMap<SimplePlaylist, SimplePlayListModel>()
                .ReverseMap()
                .ForMember(m => m.Tracks, opt => opt.Ignore());

            CreateMap<SearchRequest, SearchRequestModel>()
                .ReverseMap()
                .ForMember(m => m.Market, opt => opt.Ignore())
                .ForMember(m => m.Offset, opt => opt.Ignore());


            CreateMap<FullPlaylist, FullPlayListModel>()
                .ForMember(t => t.Tracks, opt => opt.Ignore());
           
            CreateMap<PlaylistTrack<FullTrack>, PlayListTrackModel<FullTrackModel>>()
                .ReverseMap();
  
            CreateMap<SearchRequestModel.Types, SearchRequest.Types>();

            CreateMap<Category, CategoryModel>()
                .ReverseMap();

            CreateMap<SearchResponseModel, SearchResponse>()
                .ReverseMap()
                .ForMember(m => m.Artists, opt => opt.MapFrom(c => c.Artists.Items.ToList()))
                .ForMember(m => m.Albums, opt => opt.MapFrom(c => c.Albums.Items.ToList()))
                .ForMember(m => m.Tracks, opt => opt.MapFrom(c => c.Tracks.Items.ToList()))
                .ForMember(m => m.Playlists, opt => opt.MapFrom(c => c.Playlists.Items.ToList()))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Device, DeviceModel>()
                .ReverseMap();

            CreateMap<DeviceResponse, DeviceResponseModel>()
                .ReverseMap();


            CreateMap<Context, ContextModel>()
                .ReverseMap();

            CreateMap<CurrentlyPlayingModel, CurrentlyPlaying>()
                .ReverseMap()
                .ForMember(i => i.Item, 
                opt => opt.Ignore());

        }
    }
}