using AltV.Net;
using AltV.Net.EntitySync;
using FireFighters.Models;
using FireFighters.Server.Builders;
using System;
using System.Threading.Tasks;
using FireFighters.Server.Callbacks;
using System.Collections.Generic;
using System.Linq;
using FireFighters.Server.Extensions;

namespace FireFighters.Server.Managers
{
    public class FireManager
    {
        private readonly FlameManager _flameManager;
        private readonly SynchronizedCollection<Fire> _activeFires;

        public FireManager()
        {
            _activeFires = new SynchronizedCollection<Fire>();
            _flameManager = new FlameManager();

            Task
                .Factory
                .StartNew(ManageFires, null, TaskCreationOptions.LongRunning)
                .PerformAsyncTaskWithoutAwait();
        }

        private async Task ManageFires(object state)
        {
            var firesToRemove = new List<Fire>();

            foreach (var fire in _activeFires)
            {
                _flameManager.OnTick(fire.MainFlame);

                if (!fire.MainFlame.Extinguished)
                {
                    continue;
                }

                // main flame of the fire is extinguished -> remove fire from entity sync
                InternalStopFire(fire);
                firesToRemove.Add(fire);
            }

            foreach (var fire in firesToRemove)
            {
                _activeFires.Remove(fire);
            }

            await Task.Delay(100);
        }

        public async Task StartNewFire(Fire fire)
        {
            AltEntitySync.AddEntity(fire);

            // note: is fire.Range already defined by entitysync at this position? should be..

            var emitCallback = new EmitInRangeCallback("FireFighters:Fire:NewStarted", fire.Position, fire.Range, new object[] { fire.Id });
            await Alt.ForEachPlayers(emitCallback);

            await Task.Delay(fire.FlameSpawnDelay);

            // render big smoke
            fire.DisplayFlameSmoke = true;

            // build the fire object
            var mainFlameBuilder = new FlameBuilder()
                .SetPosition(fire.Position)
                .SetLevel(10);

            fire.MainFlame = mainFlameBuilder.Build(fire);

            // add to tick manager
            _activeFires.Add(fire);
        }

        public bool StopFire(ulong fireId)
        {
            var fire = _activeFires.SingleOrDefault(e => e.Id == fireId);

            if (fire == null)
            {
                Console.WriteLine($"Warning: Fire Id {fireId} not found!");
                return false;
            }

            InternalStopFire(fire);
            _activeFires.Remove(fire);

            return true;
        }

        private void InternalStopFire(Fire fire)
        {
            if (!AltEntitySync.TryGetEntity(fire.Id, (ulong)EntityTypes.Fire, out _))
            {
                Console.WriteLine($"Warning: Fire Id {fire.Id} was not synchronized via EntitySync!");
                return;
            }
            
            AltEntitySync.RemoveEntity(fire);
            Console.WriteLine($"Fire {fire.Id} removed from EntitySync");
        }
    }
}
