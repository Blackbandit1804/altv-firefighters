using System.Numerics;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using FireFighters.Server.Entities;
using FireFighters.Server.Managers;

namespace FireFighters.Server.Modules
{
    public class FireTruckModule
        : IScript
    {
        private const int WaterHoseLength = 30;

        public FireTruckModule()
        {
            Alt.OnClient("FireFighters:Item:WaterHose", OnItemWaterHose);
            Alt.OnClient<FirefighterVehicle>("FireFighters:Item:WaterHose:VehicleToPlayer", OnItemWaterHoseVehicleToPlayer);
            Alt.OnClient<FirefighterVehicle, Position>("FireFighters:Item:WaterHose:VehicleToHydrant", OnItemWaterHoseVehicleToHydrant);
            Alt.OnClient<FirefighterVehicle, FirefighterVehicle>("FireFighters:Item:WaterHose:VehicleToVehicle", OnItemWaterHoseVehicleToVehicle);
        }

        private void OnItemWaterHose(IPlayer player)
        {
            // just demo code for what to do in your item system
            
            // notify: "Please select a hydrant to build up a water supply to a firetruck or a firetruck to prepare the water hose for extinguishing work"
            
            player.Emit("FireFighters:Item:WaterHose:Used");
        }

        private void OnItemWaterHoseVehicleToPlayer(IPlayer player, FirefighterVehicle vehicle)
        {
            if (!vehicle.FireTruckData.IsFireTruck)
            {
                // notify: that is not possible
                return;
            }

            if (Vector3.Distance(vehicle.Position, player.Position) > 2)
            {
                // notify: you are too far away from the firetruck
                return;
            }
            
            if (vehicle.FireTruckData.WaterHoseSocketUser1 == null)
            {
                vehicle.FireTruckData.WaterHoseSocketUser1 = player;
                return;
            }

            if (vehicle.FireTruckData.WaterHoseSocketUser2 == null)
            {
                vehicle.FireTruckData.WaterHoseSocketUser2 = player;
                return;
            }

            if (vehicle.FireTruckData.WaterHoseSocketUser3 == null)
            {
                vehicle.FireTruckData.WaterHoseSocketUser3 = player;
                return;
            }

            // notify: the water supply of this firetruck is already occupied
        }

        private void OnItemWaterHoseVehicleToHydrant(IPlayer player, FirefighterVehicle vehicle, Position hydrantPosition)
        {
            if (!vehicle.FireTruckData.IsFireTruck)
            {
                // notify: that is not possible
                return;
            }

            if (vehicle.FireTruckData.ConnectedHydrantPosition != null)
            {
                // notify: this firetruck is already connected to a hydrant
                return;
            }

            if (Vector3.Distance(vehicle.Position, player.Position) > 2)
            {
                // notify: you are too far away from the firetruck
                return;
            }

            if (Vector3.Distance(vehicle.Position, hydrantPosition) > WaterHoseLength)
            {
                // notify: the hydrant is too far away from the firetruck
                return;
            }

            if (vehicle.FireTruckData.IncomingVehicleSocket != null)
            {
                // notify: you already have a water supply by another firetruck
                return;
            }

            vehicle.FireTruckData.ConnectedHydrantPosition = hydrantPosition;
            
            player.Emit("FireFighters:FireTruck:ConnectHydrant", vehicle, hydrantPosition);
        }
        
        private void OnItemWaterHoseVehicleToVehicle(IPlayer player, FirefighterVehicle supplyingVehicle, FirefighterVehicle consumingVehicle)
        {
            if (!supplyingVehicle.FireTruckData.IsFireTruck || !consumingVehicle.FireTruckData.IsFireTruck)
            {
                // notify: that is not possible
                return;
            }

            if (Vector3.Distance(supplyingVehicle.Position, consumingVehicle.Position) > WaterHoseLength)
            {
                // notify: you are too far away from the vehicle
                return;
            }

            if (supplyingVehicle.FireTruckData.OutgoingVehicleSocket != null ||
                consumingVehicle.FireTruckData.IncomingVehicleSocket != null)
            {
                // notify: only one vehicle connection per vehicle is possible
                return;
            }

            if (consumingVehicle.FireTruckData.ConnectedHydrantPosition != null)
            {
                // notify: you already have a water supply by a hydrant
                return;
            }

            supplyingVehicle.FireTruckData.OutgoingVehicleSocket = consumingVehicle;
            consumingVehicle.FireTruckData.IncomingVehicleSocket = supplyingVehicle;
            
            player.Emit("FireFighters:FireTruck:ConnectFireTruck", supplyingVehicle, consumingVehicle);
        }
    }
}