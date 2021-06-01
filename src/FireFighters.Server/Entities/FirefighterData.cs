namespace FireFighters.Server.Entities
{
    public class FirefighterData
    {
        public FirefighterPlayer Player { get; }

        public FirefighterData(FirefighterPlayer player)
        {
            Player = player;
        }

        public bool WaterHoseActive
        {
            get => Player.GetStreamSyncedMetaData("activeWaterHose", out bool result) && result;
            set => Player.SetStreamSyncedMetaData("activeWaterHose", value);
        }
    }
}