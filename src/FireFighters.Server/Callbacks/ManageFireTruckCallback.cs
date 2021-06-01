using System;
using System.Threading.Tasks;
using AltV.Net.Elements.Entities;
using AltV.Net.Elements.Pools;
using FireFighters.Server.Entities;

namespace FireFighters.Server.Callbacks
{
    public class ManageFireTruckCallback
        : IAsyncBaseObjectCallback<IVehicle>
    {
        public async Task OnBaseObject(IVehicle vehicle)
        {
            if (vehicle is not FirefighterVehicle fireTruck)
            {
                return;
            }

            var data = fireTruck.FireTruckData;

            if (!data.IsFireTruck)
            {
                return;
            }

            if (data.ConnectedHydrantPosition != null)
            {
                // refill water tank
                const int amount = 10;
                
                data.WaterTank = Math.Min(data.WaterTank + amount, data.WaterTankCapacity);

                if (data.WaterTank > data.WaterTankCapacity)
                {
                    data.WaterTank = data.WaterTankCapacity;
                }
            }

            if (data.IncomingVehicleSocket != null && Equals(data.IncomingVehicleSocket.FireTruckData.OutgoingVehicleSocket, fireTruck))
            {
                const int amount = 10;

                // check available space in water tank
                var consume = Math.Min(data.WaterTankCapacity - data.WaterTank, amount);

                // check available water of the other firetruck
                consume = Math.Min(consume, data.IncomingVehicleSocket.FireTruckData.WaterTank);
                
                if (consume > 0)
                {
                    data.WaterTank += consume;
                    data.IncomingVehicleSocket.FireTruckData.WaterTank -= consume;
                }
            }

            if (data.WaterHoseSocketUser1 is FirefighterPlayer f1 && f1.FirefighterData.WaterHoseActive)
            {
                OperateWaterHoseUser(f1, data);
            }
            
            if (data.WaterHoseSocketUser2 is FirefighterPlayer f2 && f2.FirefighterData.WaterHoseActive)
            {
                OperateWaterHoseUser(f2, data);
            }

            if (data.WaterHoseSocketUser3 is FirefighterPlayer f3 && f3.FirefighterData.WaterHoseActive)
            {
                OperateWaterHoseUser(f3, data);
            }

            await Task.CompletedTask;
        }
        
        private void OperateWaterHoseUser(FirefighterPlayer firefighter, FireTruckData data)
        {
            lock (firefighter)
            {
                if (!firefighter.Exists)
                {
                    return;
                }
                
                const int amount = 5;

                if (data.WaterTank < amount)
                {
                    firefighter.FirefighterData.WaterHoseActive = false;
                }
                else
                {
                    data.WaterTank -= amount;
                }
            }
        }
    }
}