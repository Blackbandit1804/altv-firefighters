using System;
using AltV.Net.Elements.Entities;

namespace FireFighters.Server.Entities
{
    public class FirefighterPlayer
        : Player
    {
        public FirefighterPlayer(IntPtr nativePointer, ushort id) 
            : base(nativePointer, id)
        {
            FirefighterData = new FirefighterData(this);
        }
        
        public FirefighterData FirefighterData { get; set; }
    }
}