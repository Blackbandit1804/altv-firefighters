using AltV.Net.Async;
using AltV.Net.EntitySync;
using AltV.Net.EntitySync.ServerEvent;
using AltV.Net.EntitySync.SpatialPartitions;

namespace FireFighters.Server
{
    public class FirefighterResource
        : AsyncResource
    {
        public override void OnStart()
        {
            AltEntitySync.Init(1, 100,
                (threadId) => false,
                (threadCount, repository) => new ServerEventNetworkLayer(threadCount, repository),
                (entity, threadCount) => 0,
                (entityId, entityType, threadCount) => 0,
                (threadId) => new LimitedGrid3(50_000, 50_000, 100, 10_000, 10_000, 300),
                new IdProvider());
        }

        public override void OnStop()
        {
            AltEntitySync.Stop();
        }
    }
}