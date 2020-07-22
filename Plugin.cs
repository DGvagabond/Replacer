using System;
using System.Collections.Generic;
using System.IO;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events;
using Handlers = Exiled.Events.Handlers;

namespace Replacer
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    public class Plugin : Exiled.API.Features.Plugin<Config>
    {
        public override string Author { get; } = "DGvagabond";
        public override string Name { get; } = "Replacer";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 0, 5);

        public PlayerEvents PlayerEvents;
        public static Plugin Singleton;
        public override void OnEnabled()
        {
            Singleton = this;
            PlayerEvents = new PlayerEvents(this);
            Handlers.Player.Left += PlayerEvents.OnPlayerLeave;
        }

        public override void OnDisabled()
        {
            Handlers.Player.Left -= PlayerEvents.OnPlayerLeave;
            PlayerEvents = null;
        }
    }
}