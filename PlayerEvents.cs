using UnityEngine;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Permissions.Extensions;
using System.Linq;
using MEC;
using System;
using System.Collections.Generic;

namespace Replacer
{
    public class PlayerEvents
    {
        public Plugin plugin;
        Exiled.API.Features.Player ply;

        public void Awake()
        {
            ReferenceHub gameObject = null;
            ply = Player.Get(gameObject);
        }
        public PlayerEvents(Plugin plugin) => this.plugin = plugin;
    
            public void OnPlayerLeave(LeftEventArgs ev)
        {
            if (this.ply.Team != Team.RIP)
            {
                Inventory.SyncListItemInfo items = this.ply.Inventory.items;
                RoleType role = this.ply.Role;
                Vector3 pos = this.ply.Position;
                float health = this.ply.Health;
                Dictionary<Exiled.API.Enums.AmmoType, uint> ammo = new Dictionary<Exiled.API.Enums.AmmoType, uint>();
                foreach (Exiled.API.Enums.AmmoType atype in (Exiled.API.Enums.AmmoType[])Enum.GetValues(typeof(Exiled.API.Enums.AmmoType)))
                {
                    ammo.Add(atype, this.ply.GetAmmo(atype));
                }
                Player player = Player.List.FirstOrDefault(x => x.Role == RoleType.Spectator && x.UserId != string.Empty && !x.IsOverwatchEnabled && x != this.ply);
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
                            this.ply.SetAmmo(atype, amount);
                        }
                        else
                            Log.Error($"[Replacer] ERROR: Tried to get a value from dict that did not exist! (Ammo)");
                    }
                });
            }
        }
    }
}