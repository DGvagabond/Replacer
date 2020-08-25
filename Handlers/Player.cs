using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;
using Exiled.Loader;

namespace Replacer.Handlers
{
    public class Player
    {
        public Plugin plugin;
        public Player(Plugin plugin) => this.plugin = plugin;

        public void OnLeave(LeftEventArgs ev)
        {
            if (ev.Player.Role.GetTeam() != Team.RIP)
            {
                Log.Debug($"{ev.Player} has left the server, searching for replacement...", Loader.ShouldDebugBeShown);
                bool scp079 = false;
                if (ev.Player.Role == RoleType.Scp079)
                    scp079 = true;

                Inventory.SyncListItemInfo items = ev.Player.Inventory.items;
                RoleType role = ev.Player.Role;
                Vector3 pos = ev.Player.Position;
                float health = ev.Player.Health, xp079 = 0f, ap079 = 0f;
                byte lvl079 = 0;

                if (scp079)
                {
                    lvl079 = ev.Player.Level;
                    xp079 = ev.Player.Experience;
                    ap079 = ev.Player.Energy;
                }

                Exiled.API.Features.Player player = Exiled.API.Features.Player.List.FirstOrDefault(x => x.Role == RoleType.Spectator && x.UserId != string.Empty && !x.IsOverwatchEnabled && x != ev.Player);

                if (player != null)
                    player.SetRole(role);
                Log.Debug($"Replacement found! Transferring settings...", Loader.ShouldDebugBeShown);

                Timing.CallDelayed(0.3f, () =>
                {
                    player.Position = pos;
                    foreach (var item in items) player.Inventory.AddNewItem(item.id);
                    player.Health = health;

                    Log.Debug($"Applied basic player settings...", Loader.ShouldDebugBeShown);

                    if (scp079)
                    {
                        player.Level = lvl079;
                        player.Experience = xp079;
                        player.Energy = ap079;
                        Log.Debug($"Applied SCP-079 specfic settings...");
                    }
                    Log.Debug($"{player} has successfully replaced {ev.Player.Nickname}.", Loader.ShouldDebugBeShown);
                    player.ShowHint($"<color=yellow>You replaced {ev.Player.Nickname}!</color>");
                });
            }
            else
            {
                Log.Debug($"{ev.Player.Nickname} was a SPECTATOR; ignoring replacement need...", Loader.ShouldDebugBeShown);
            }
        }
    }
}
