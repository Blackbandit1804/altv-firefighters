import * as alt from 'alt-client'

import './EntitySync/FireStreamer'
import './EntitySync/FlameStreamer'

import './Handlers/BigWetHoseHandler'

alt.log('Resource has been loaded.')




// todo

/*

from server:
FireFighters:Item:WaterHose:Used
FireFighters:FireTruck:AttachHydrant
FireFighters:FireTruck:AttachFireTruck
FireFighters:FireTruck:AttachPlayer
FireFighters:FireTruck:DetachPlayer

to server:
FireFighters:Item:WaterHose
FireFighters:Item:WaterHose:VehicleToPlayer
FireFighters:Item:WaterHose:VehicleToHydrant
FireFighters:Item:WaterHose:VehicleToVehicle
FireFighters:FireTruck:HydrantDetached
FireFighters:FireTruck:FireTruckDetached
FireFighters:FireTruck:PlayerDetached

player streamSynced:
activeWaterHose

 */