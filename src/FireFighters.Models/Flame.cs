using AltV.Net.EntitySync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FireFighters.Models
{
    public class Flame
        : Entity
    {
        public Flame(Vector3 position, Fire fire)
            : base((ulong)EntityTypes.Flame, position, 0, Constants.EntitySyncRange)
        {
            Fire = fire;
            IsGasFire = fire.IsGasFire;
            Level = 0;

            Active = true;

            Children = new List<Flame>();
        }

        public bool IsGasFire
        {
            get => TryGetData("isGasFire", out bool isGasFire) && isGasFire;
            set => SetData("isGasFire", value);
        }

        public uint Level
        {
            get => !TryGetData("level", out uint level) ? 0 : level;
            set => SetData("level", value);
        }

        public bool Extinguished
        {
            get => TryGetData("extinguished", out bool extinguished) && extinguished;
            set => SetData("extinguished", value);
        }

        public bool IsPositionGroundValidated
        {
            get => TryGetData("isPositionGroundValidated", out bool isPositionGroundValidated) && isPositionGroundValidated;
            set => SetData("isPositionGroundValidated", value);
        }

        public Fire Fire { get; }

        public List<Flame> Children { get; set; }

        public bool Active { get; set; }

        public DateTime LastManagedTick { get; set; }

        public int CountChildren()
        {
            return Children.Sum(e => e.CountChildren());
        }
    }
}