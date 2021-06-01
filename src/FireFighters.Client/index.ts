import * as alt from 'alt-client'

import './EntitySync/FireStreamer'
import './EntitySync/FlameStreamer'

import './Handlers/BigWetHoseHandler'

alt.log('Resource has been loaded.')




// todo

/*

from server:
FireFighters:Item:WaterHose:Used
FireFighters:FireTruck:ConnectHydrant
FireFighters:FireTruck:ConnectFireTruck
FireFighters:FireTruck:DisconnectPlayer
FireFighters:FireTruck:ConnectPlayer

to server:
FireFighters:Item:WaterHose
FireFighters:Item:WaterHose:VehicleToPlayer
FireFighters:Item:WaterHose:VehicleToHydrant
FireFighters:Item:WaterHose:VehicleToVehicle

player streamSynced:
activeWaterHose

 */