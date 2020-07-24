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
    
            public void OnPlayerKicking(KickingEventArgs ev)
            {
                if (!plugin.Config.ReplaceKicked) return;
                if (ev.Target.Role.GetTeam() != Team.RIP)
                {
                    Log.Info($"{ev.Target} has been kicked, searching for replacement...");
                    bool scp079 = false;
                    if (ev.Target.Role == RoleType.Scp079)
                        scp079 = true;

                    Inventory.SyncListItemInfo items = ev.Target.Inventory.items;
                    RoleType role = ev.Target.Role;
                    Vector3 pos = ev.Target.Position;
                    float health = ev.Target.Health, xp079 = 0f, ap079 = 0f;
                    byte lvl079 = 0;
                    Dictionary<Exiled.API.Enums.AmmoType, uint> ammo = new Dictionary<Exiled.API.Enums.AmmoType, uint>();
                    foreach (Exiled.API.Enums.AmmoType atype in (Exiled.API.Enums.AmmoType[])Enum.GetValues(typeof(Exiled.API.Enums.AmmoType)))
                    {
                        ammo.Add(atype, ev.Target.GetAmmo(atype));
                    }

                    if (scp079)
                    {
                        lvl079 = ev.Target.Level;
                        xp079 = ev.Target.Experience;
                        ap079 = ev.Target.Energy;
                    }

                    Exiled.API.Features.Player player = Exiled.API.Features.Player.List.FirstOrDefault(x => x.Role == RoleType.Spectator && x.UserId != string.Empty && !x.IsOverwatchEnabled && x != ev.Target);
                
                    if (player != null)
                        player.SetRole(role);
                        Log.Debug($"Replacement found! Transferring settings...");

                        Timing.CallDelayed(0.3f, () =>
                        {
                            player.Position = pos;
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
                                    Log.Error($"Tried to get a value from dict that did not exist: (Ammo)");
                            }

                            Log.Debug($"Applied basic player settings...");

                            if (scp079)
                            {
                                player.Level = lvl079;
                                player.Experience = xp079;
                                player.Energy = ap079;
                                Log.Debug($"Applied SCP-079 specfic settings...");
                            }
                        Log.Debug($"{player} has successfully replaced {ev.Target}.");
                        player.Broadcast(10, $"You have replaced {ev.Target.Nickname}!");
                    });
                }
                else
                {
                    Log.Info($"{ev.Target} was a SPECTATOR; ignoring replacement need...");
                }
            }

        public void OnPlayerBanning(BanningEventArgs ev)
        {
            if (!plugin.Config.ReplaceBanned) return;
            if (ev.Target.Role.GetTeam() != Team.RIP)
            {
                Log.Info($"{ev.Target} has been banned, searching for replacement...");
                bool scp079 = false;
                if (ev.Target.Role == RoleType.Scp079)
                    scp079 = true;

                Inventory.SyncListItemInfo items = ev.Target.Inventory.items;
                RoleType role = ev.Target.Role;
                Vector3 pos = ev.Target.Position;
                float health = ev.Target.Health, xp079 = 0f, ap079 = 0f;
                byte lvl079 = 0;
                Dictionary<Exiled.API.Enums.AmmoType, uint> ammo = new Dictionary<Exiled.API.Enums.AmmoType, uint>();
                foreach (Exiled.API.Enums.AmmoType atype in (Exiled.API.Enums.AmmoType[])Enum.GetValues(typeof(Exiled.API.Enums.AmmoType)))
                {
                    ammo.Add(atype, ev.Target.GetAmmo(atype));
                }

                if (scp079)
                {
                    lvl079 = ev.Target.Level;
                    xp079 = ev.Target.Experience;
                    ap079 = ev.Target.Energy;
                }

                Exiled.API.Features.Player player = Exiled.API.Features.Player.List.FirstOrDefault(x => x.Role == RoleType.Spectator && x.UserId != string.Empty && !x.IsOverwatchEnabled && x != ev.Target);

                if (player != null)
                    player.SetRole(role);
                Log.Debug($"Replacement found! Transferring settings...");

                Timing.CallDelayed(0.3f, () =>
                {
                    player.Position = pos;
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
                            Log.Error($"Tried to get a value from dict that did not exist: (Ammo)");
                    }

                    Log.Debug($"Applied basic player settings...");

                    if (scp079)
                    {
                        player.Level = lvl079;
                        player.Experience = xp079;
                        player.Energy = ap079;
                        Log.Debug($"Applied SCP-079 specfic settings...");
                    }
                    Log.Debug($"{player} has successfully replaced {ev.Target}.");
                    player.Broadcast(10, $"You have replaced {ev.Target.Nickname}!");
                });
            }
            else
            {
                Log.Info($"{ev.Target} was a SPECTATOR; ignoring replacement need...");
            }
        }
    }
}