using System;
using AltV.Net;
using AltV.Net.Elements.Entities;

namespace FireFighters.Server.Entities.Factories
{
    public class FirefighterVehicleFactory
        : IEntityFactory<IVehicle>
    {
        public IVehicle Create(IntPtr entityPointer, ushort id)
        {
            return new FirefighterVehicle(entityPointer, id);
        }
    }
}