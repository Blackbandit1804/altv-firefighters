using AltV.Net.Data;
using FireFighters.Models;
using FireFighters.Server.Managers;
using System;
using System.Numerics;

namespace FireFighters.Server.Builders
{
    public class FireBuilder
    {
        private Random _random;

        private bool _explosionOnStart;
        private int _maxFlames;
        private int _maxSpreadDistance;
        private int _flameSpawnDelay;
        private bool _isGasFire;
        private Vector3 _position;

        public FireBuilder()
        {
            Reset();
        }

        public FireBuilder Reset()
        {
            _random = new Random();

            _explosionOnStart = false;
            _flameSpawnDelay = 0;
            _maxFlames = 20;
            _maxSpreadDistance = 80;
            _isGasFire = false;
            _position = Vector3.Zero;

            return this;
        }

        public FireBuilder ExplodesOnStartup(bool value)
        {
            _explosionOnStart = value;
            return this;
        }

        public FireBuilder SetMaxFlames(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            _maxFlames = count;
            return this;
        }

        public FireBuilder InstantlySpawnFlames()
        {
            _flameSpawnDelay = 0;
            return this;
        }

        public FireBuilder SetFlameSpawnDelay(int ms)
        {
            if (ms < 0) throw new ArgumentOutOfRangeException(nameof(ms));
            if (ms == 0) return InstantlySpawnFlames();
            _flameSpawnDelay = ms;
            return this;
        }

        public FireBuilder SetFlameSpawnDelay(int randomMin, int randomMax)
        {
            if (randomMin <= 0) throw new ArgumentOutOfRangeException(nameof(randomMin));
            if (randomMax <= randomMin) throw new ArgumentOutOfRangeException(nameof(randomMax));
            _flameSpawnDelay = _random.Next(randomMin, randomMax);
            return this;
        }

        public FireBuilder SetMaxSpreadDistance(int distance)
        {
            _maxSpreadDistance = distance;
            return this;
        }

        public FireBuilder IsGasFire()
        {
            _isGasFire = true;
            return this;
        }

        public FireBuilder SetPosition(Position position)
        {
            _position = position;
            return this;
        }

        public Fire Build()
        {
            var fire = new Fire(_position)
            {
                ExplosionOnStart = _explosionOnStart,
                MaxFlames = _maxFlames,
                MaxSpreadDistance = _maxSpreadDistance,
                FlameSpawnDelay = _flameSpawnDelay,
                IsGasFire = _isGasFire
            };

            return fire;
        }
    }
}
