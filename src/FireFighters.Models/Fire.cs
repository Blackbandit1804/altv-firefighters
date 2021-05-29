using System.Linq;
using AltV.Net.EntitySync;
using System.Numerics;

namespace FireFighters.Models
{
    public class Fire
        : Entity
    {
        public Fire(Vector3 position)
            : base((ulong)EntityTypes.Fire, position, 0, Constants.EntitySyncRange)
        {
        }

        public bool ExplosionOnStart
        {
            get => TryGetData("explosionOnStart", out bool explosionOnStart) && explosionOnStart;
            set => SetData("explosionOnStart", value);
        }

        public bool DisplayFlameSmoke
        {
            get => TryGetData("flamesSpawned", out bool flamesSpawned) && flamesSpawned;
            set => SetData("flamesSpawned", value);
        }
        
        public bool IsGasFire
        {
            get => TryGetData("isGasFire", out bool isGasFire) && isGasFire;
            set => SetData("isGasFire", value);
        }

        public int MaxSpreadDistance { get; set; }

        public int MaxFlames { get; set; }

        public int FlameSpawnDelay { get; set; }

        public Flame MainFlame { get; set; }

        public int CurrentFlames => GetCountOfFlamesRecursive(MainFlame);

        private int GetCountOfFlamesRecursive(Flame flame)
        {
            return flame?.Children.Sum(GetCountOfFlamesRecursive) ?? 0;
        }
    }
}
