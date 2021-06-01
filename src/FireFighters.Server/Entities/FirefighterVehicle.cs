using System;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;

namespace FireFighters.Server.Entities
{
    public class FirefighterVehicle
        : Vehicle
    {
        public FirefighterVehicle(uint model, Position position, Rotation rotation)
            : base(model, position, rotation)
        {
            FireTruckData = new FireTruckData(this);
        }

        public FirefighterVehicle(IntPtr nativePointer, ushort id)
            : base(nativePointer, id)
        {
            FireTruckData = new FireTruckData(this);
        }
        
        public FireTruckData FireTruckData { get; }
    }
}