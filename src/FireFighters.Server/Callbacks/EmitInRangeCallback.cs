using System.Threading.Tasks;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Elements.Pools;

namespace FireFighters.Server.Callbacks
{
    public class EmitInRangeCallback
        : IAsyncBaseObjectCallback<IPlayer>
    {
        private readonly string _eventName;
        private readonly Position _origin;
        private readonly float _distance;
        private readonly object[] _args;

        public EmitInRangeCallback(string eventName, Position origin, float distance, object[] args)
        {
            _eventName = eventName;
            _origin = origin;
            _distance = distance;
            _args = args;
        }

        public async Task OnBaseObject(IPlayer baseObject)
        {
            if (baseObject.Position.Distance(_origin) > _distance)
            {
                return;
            }

            baseObject.Emit(_eventName, _args);

            await Task.CompletedTask;
        }
    }
}
