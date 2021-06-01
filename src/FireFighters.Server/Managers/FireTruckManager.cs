using System;
using System.Threading;
using System.Threading.Tasks;
using AltV.Net;
using FireFighters.Server.Callbacks;
using FireFighters.Server.Extensions;

namespace FireFighters.Server.Managers
{
    public class FireTruckManager
    {
        private Timer _timer;
        
        public async Task Start()
        {
            if (_timer != null)
            {
                await _timer.DisposeAsync();
            }
            
            _timer = new Timer(ManageFireTrucks, null, TimeSpan.FromMinutes(1d), TimeSpan.FromSeconds(2d));
        }

        public async Task Stop()
        {
            await _timer.DisposeAsync();
            _timer = null;
        }

        private void ManageFireTrucks(object state)
        {
            var operation = new ManageFireTruckCallback();

            Alt.ForEachVehicles(operation)
                .BlockExecutionUntilTaskFinished();
        }
    }
}