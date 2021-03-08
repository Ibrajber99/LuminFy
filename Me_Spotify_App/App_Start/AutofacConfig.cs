using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Me_Spotify_App.API_CLIENT;
using Me_Spotify_App.API_CLIENT.Spotify_Artist;
using Me_Spotify_App.API_CLIENT.Spotify_Browse;
using Me_Spotify_App.API_CLIENT.Spotify_Player;
using Me_Spotify_App.API_CLIENT.Spotify_Playlist;
using Me_Spotify_App.API_CLIENT.Spotify_Tracks;
using Me_Spotify_App.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Me_Spotify_App.App_Start
{
    public static class AutofacConfig
    {
        public static void Register()
        {
            var bldr = new ContainerBuilder();
            bldr.RegisterControllers(typeof(MvcApplication).Assembly);
            bldr.RegisterModelBinders(typeof(MvcApplication).Assembly);
            bldr.RegisterModule<AutofacWebTypesModule>();

            RegisterServices(bldr);

            var container = bldr.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterServices(ContainerBuilder bldr)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SpotifyMappingProfile());
            });

            bldr.RegisterInstance(config.CreateMapper())
                .As<IMapper>()
                .SingleInstance();

            bldr.RegisterType<SpotifyUserRepo>()
                .As<SpotifyUser>()
                .InstancePerRequest();

            bldr.RegisterType<TrackRepo>()
                .As<SpotifyTrack>()
                .InstancePerRequest();

            bldr.RegisterType<ArtistRepo>()
                .As<SpotifyArtist>()
                .InstancePerRequest();

            bldr.RegisterType<BrowseRepo>()
                .As<SpotifyBrowse>()
                .InstancePerRequest();

            bldr.RegisterType<PlayListRepo>()
                .As<SpotifyPlayList>()
                .InstancePerRequest();

            bldr.RegisterType<PlayerRepo>()
                .As<SpotifyPlayer>()
                .InstancePerRequest();
        }


    }
}