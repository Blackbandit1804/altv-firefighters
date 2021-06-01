using System;
using AltV.Net;
using AltV.Net.Elements.Entities;

namespace FireFighters.Server.Entities.Factories
{
    public class FirefighterPlayerFactory
        : IEntityFactory<IPlayer>
    {
        public IPlayer Create(IntPtr entityPointer, ushort id)
        {
            return new FirefighterPlayer(entityPointer, id);
        }
    }
}