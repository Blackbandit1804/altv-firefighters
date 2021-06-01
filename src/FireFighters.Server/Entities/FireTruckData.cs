using System.Collections.Generic;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;

namespace FireFighters.Server.Entities
{
    public class FireTruckData
    {
        private static readonly List<uint> FireTruckModels = new()
        {
            Alt.Hash("firetruk")
        };

        public FirefighterVehicle Vehicle { get; }

        public FireTruckData(FirefighterVehicle vehicle)
        {
            Vehicle = vehicle;
        }

        public bool IsFireTruck => FireTruckModels.Contains(Vehicle.Model);

        public int WaterTank { get; set; }
        public int WaterTankCapacity { get; set; }
        public FirefighterVehicle OutgoingVehicleSocket { get; set; }
        public FirefighterVehicle IncomingVehicleSocket { get; set; }

        private IPlayer _waterHoseSocketUser1;
        public IPlayer WaterHoseSocketUser1
        {
            get
            {
                if (_waterHoseSocketUser1 is {Exists: true})
                {
                    return _waterHoseSocketUser1;
                }

                _waterHoseSocketUser1 = null;
                return _waterHoseSocketUser1;
            }
            set
            {
                _waterHoseSocketUser1?.Emit("FireFighters:FireTruck:DetachPlayer", Vehicle);
                _waterHoseSocketUser1 = value;
                _waterHoseSocketUser1.Emit("FireFighters:FireTruck:AttachPlayer", Vehicle);
            }
        }
        
        private IPlayer _waterHoseSocketUser2;
        public IPlayer WaterHoseSocketUser2
        {
            get
            {
                if (_waterHoseSocketUser2 is {Exists: true})
                {
                    return _waterHoseSocketUser2;
                }

                _waterHoseSocketUser2 = null;
                return _waterHoseSocketUser2;
            }
            set
            {
                _waterHoseSocketUser2?.Emit("FireFighters:FireTruck:DetachPlayer", Vehicle);
                _waterHoseSocketUser2 = value;
                _waterHoseSocketUser2.Emit("FireFighters:FireTruck:AttachPlayer", Vehicle);
            }
        }
        
        private IPlayer _waterHoseSocketUser3;
        public IPlayer WaterHoseSocketUser3
        {
            get
            {
                if (_waterHoseSocketUser3 is {Exists: true})
                {
                    return _waterHoseSocketUser3;
                }

                _waterHoseSocketUser3 = null;
                return _waterHoseSocketUser3;
            }
            set
            {
                _waterHoseSocketUser3?.Emit("FireFighters:FireTruck:DetachPlayer", Vehicle);
               _waterHoseSocketUser3 = value;
                _waterHoseSocketUser3.Emit("FireFighters:FireTruck:AttachPlayer", Vehicle);
            }
        }
        
        public Position? ConnectedHydrantPosition { get; set; }
    }
}