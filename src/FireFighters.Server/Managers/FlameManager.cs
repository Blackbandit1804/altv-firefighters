using System;
using System.Numerics;
using AltV.Net.EntitySync;
using FireFighters.Models;
using FireFighters.Server.Builders;

namespace FireFighters.Server.Managers
{
    public class FlameManager
    {
        private readonly Random _random;

        public FlameManager()
        {
            _random = new Random();
        }

        public void OnTick(Flame flame)
        {
            if (!flame.IsPositionGroundValidated || !flame.Active)
            {
                return;
            }

            if (flame.Extinguished)
            {
                // to be sure: stop and remove all children
                flame.Children.ForEach(e =>
                {
                    e.Extinguished = true;
                    AltEntitySync.RemoveEntity(e);
                });

                AltEntitySync.RemoveEntity(flame);

                flame.Active = false;

                return;
            }

            // remove inactive flames and execute tick on every child
            flame.Children.RemoveAll(e => !e.Active);
            flame.Children.ForEach(OnTick);

            // check if the flames limit is reached
            if (flame.Fire.MaxFlames >= flame.Fire.CurrentFlames)
            {
                return;
            }

            // logical tick only every 5 seconds
            if (DateTime.Now < flame.LastManagedTick + TimeSpan.FromSeconds(5d))
            {
                return;
            }

            flame.LastManagedTick = DateTime.Now;

            if (_random.Next(0, 100) <= Math.Max(30 + flame.Level, 50))
            {
                flame.Level += 1;
            }

            // create new children
            if (flame.Level > 10 && flame.Level % 5 == 0 && _random.Next(0, 100) <= Math.Max(10 - flame.Level, 30))
            {
                var flamesToSpawn = _random.Next(1, Convert.ToInt32(Math.Floor(flame.Level / 7d)));

                for (var i = 0; i < flamesToSpawn; i++)
                {
                    var childFlamePos = flame.Position + new Vector3(_random.Next(0, 50) / 10f, _random.Next(0, 50) / 10f, 0);

                    if (Vector3.Distance(flame.Fire.Position, childFlamePos) > flame.Fire.MaxSpreadDistance)
                    {
                        i--;
                        continue;
                    }

                    var childFlame = new FlameBuilder().SetPosition(childFlamePos).Build(flame.Fire);
                    flame.Children.Add(childFlame);

                    Console.WriteLine($"Flame {flame.Id} generated child {childFlame.Id}");
                }
            }
        }
    }
}
