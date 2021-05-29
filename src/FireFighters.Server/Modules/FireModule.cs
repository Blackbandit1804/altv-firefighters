using System;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.EntitySync;
using FireFighters.Models;
using FireFighters.Server.Builders;
using System.Linq;
using System.Numerics;
using AltV.Net.Async;
using FireFighters.Server.Managers;

namespace FireFighters.Server.Modules
{
    public class FireModule
        : IScript
    {
        private readonly FireManager _fireManager;

        public FireModule()
        {
            _fireManager = new FireManager();

            AltAsync.OnServer<Position, int, int, int, bool>("FireFighters:Fire:Create", OnServerFireCreate);
            Alt.OnServer<ulong>("FireFighters:Fire:Remove", OnServerFireRemove);

            Alt.OnClient<ulong>("FireFighters:Flame:ScriptFireExtinguished", OnClientFlameScriptFireExtinguished);
            Alt.OnClient<ulong, float>("FireFighters:Flame:FoundPositionGround", OnClientFlameFoundPositionGround);
        }

        private async void OnServerFireCreate(Position position, int maxFlames, int maxRange, int spawnDelay, bool explosion)
        {
            if (maxFlames <= 0) throw new ArgumentOutOfRangeException(nameof(maxFlames));
            if (maxRange <= 0) throw new ArgumentOutOfRangeException(nameof(maxRange));
            if (spawnDelay <= 0) throw new ArgumentOutOfRangeException(nameof(spawnDelay));

            var builder = new FireBuilder()
                .SetPosition(position)
                .SetMaxFlames(maxFlames)
                .SetMaxSpreadDistance(maxRange)
                .SetFlameSpawnDelay(spawnDelay)
                .ExplodesOnStartup(explosion);

            var fire = builder.Build();

            await _fireManager.StartNewFire(fire);
        }

        private void OnServerFireRemove(ulong fireId)
        {
            _fireManager.StopFire(fireId);
        }

        private void OnClientFlameScriptFireExtinguished(IPlayer player, ulong entityId)
        {
            if (!AltEntitySync.TryGetEntity(entityId, (ulong)EntityTypes.Flame, out AltV.Net.EntitySync.IEntity entity))
            {
                return;
            }

            if (entity is not Flame flame)
            {
                return;
            }

            if (flame.Children.Any())
            {
                player.Emit("FireFighters:Flame:RespawnScriptFire", entityId);
                return;
            }

            flame.Extinguished = true;
        }

        private void OnClientFlameFoundPositionGround(IPlayer player, ulong entityId, float ground)
        {
            if (!AltEntitySync.TryGetEntity(entityId, (ulong)EntityTypes.Flame, out AltV.Net.EntitySync.IEntity entity))
            {
                return;
            }

            if (entity is not Flame flame)
            {
                return;
            }

            flame.Position = new Vector3(flame.Position.X, flame.Position.Y, ground);
            flame.IsPositionGroundValidated = true;
        }
    }
}
