using System.Collections.Generic;
using System.Linq;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;

namespace FireFighters.Server.Entities
{
    public class FirefighterVehicleData
    {
        private static readonly List<uint> FireTruckModels = new()
        {
            Alt.Hash("firetruk")
        };

        public FirefighterVehicle Vehicle { get; }

        public FirefighterVehicleData(FirefighterVehicle vehicle)
        {
            Vehicle = vehicle;
        }

        public bool IsFireTruck => FireTruckModels.Contains(Vehicle.Model);

        public int WaterTank { get; set; }
        public int WaterTankCapacity { get; set; }
        public FirefighterVehicle OutgoingVehicleSocket { get; set; }
        public FirefighterVehicle IncomingVehicleSocket { get; set; }
        public IPlayer WaterHoseSocketUsedBy { get; set; }
        public Position? ConnectedHydrantPosition { get; set; }
    }
}