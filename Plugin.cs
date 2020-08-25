using System;
using Exiled.API.Features;
using Exiled.API.Enums;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;
using Exiled.Events.Handlers;

namespace Replacer
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "DGvagabond";
        public override string Name { get; } = "Replacer";
        public override Version Version { get; } = new Version(1, 1, 1);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 1);

        private Handlers.Player PlayerEvents;
        public static Plugin Singleton;
        public override void OnEnabled()
        {
            try
            {
                base.OnEnabled();
                LoadEvents();
            }

            catch (Exception e)
            {
                Log.Error($"There was an error loading the plugin: {e}");
            }

        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            UnloadEvents();
        }

        private void LoadEvents()
        {
            Singleton = this;
            PlayerEvents = new Handlers.Player(this);

            Player.Left += PlayerEvents.OnLeave;
        }

        private void UnloadEvents()
        {
            Player.Left -= PlayerEvents.OnLeave;

            PlayerEvents = null;
        }
    }
}