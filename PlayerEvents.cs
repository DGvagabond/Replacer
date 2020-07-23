using System;
using System.Linq;
using System.Collections.Generic;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Permissions.Extensions;
using Exiled.Events.Handlers;
using MEC;
using UnityEngine;

namespace Replacer
{
    public class PlayerEvents
    {
        public Plugin plugin;
        public PlayerEvents(Plugin plugin) => this.plugin = plugin;
    
            public void OnPlayerLeave(LeftEventArgs ev)
        {
            if (ev.Player.Role.GetTeam() != Team.RIP)
            {
                bool scp079 = false;
                if (ev.Player.Role == RoleType.Scp079)
                    scp079 = true;

                Inventory.SyncListItemInfo items = ev.Player.Inventory.items;
                RoleType role = ev.Player.Role;
                Vector3 pos = ev.Player.Position;
                float health = ev.Player.Health;
                Dictionary<Exiled.API.Enums.AmmoType, uint> ammo = new Dictionary<Exiled.API.Enums.AmmoType, uint>();
                foreach (Exiled.API.Enums.AmmoType atype in (Exiled.API.Enums.AmmoType[])Enum.GetValues(typeof(Exiled.API.Enums.AmmoType)))
                {
                    ammo.Add(atype, ev.Player.GetAmmo(atype));
                }

                byte lvl079 = 0;
                float xp079 = 0f, ap079 = 0f;
                if (scp079)
                {
                    lvl079 = ev.Player.Level;
                    xp079 = ev.Player.Experience;
                    ap079 = ev.Player.Energy;
                }

                Exiled.API.Features.Player player = Exiled.API.Features.Player.List.FirstOrDefault(x => x.Role == RoleType.Spectator && x.UserId != string.Empty && !x.IsOverwatchEnabled && x != ev.Player);
                if (player != null)
                    player.SetRole(role);
                Timing.CallDelayed(0.3f, () =>
                {
                    player.Position = pos;
                    player.Inventory.Clear();
                    foreach (var item in items) player.Inventory.AddNewItem(item.id);
                    player.Health = health;

                    foreach (Exiled.API.Enums.AmmoType atype in (Exiled.API.Enums.AmmoType[])Enum.GetValues(typeof(Exiled.API.Enums.AmmoType)))
                    {
                        uint amount;
                        if (ammo.TryGetValue(atype, out amount))
                        {
                            player.SetAmmo(atype, amount);
                        }
                        else
                            Log.Error($"ERROR: Tried to get a value from dict that did not exist: (Ammo)");
                    }

                    if (scp079)
                    {
                        player.Level = lvl079;
                        player.Experience = xp079;
                        player.Energy = ap079;
                    }
                    player.Broadcast(10, $"You have replaced {ev.Player.Nickname}!");
                });
            }
        }
    }
}
