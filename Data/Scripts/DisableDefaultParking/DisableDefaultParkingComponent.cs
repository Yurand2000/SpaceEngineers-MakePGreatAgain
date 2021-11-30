using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using Yurand.MakePGreatAgain;

namespace Yurand.MakePGreatAgain
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class DisableDefaultParkingComponent : MySessionComponentBase
    {
        private static string default_configuration_file = "disable_default_parking_config.xml";
        private Config configuration;
        public override void LoadData()
        {
            if(MyAPIGateway.Multiplayer.IsServer)
            {
                MyAPIGateway.Entities.OnEntityAdd += EntityAdded;
            }

            configuration = Config.deserialize(default_configuration_file);
            Config.serialize(configuration, default_configuration_file);
        }

        protected override void UnloadData()
        {
            if (MyAPIGateway.Multiplayer.IsServer)
            {
                MyAPIGateway.Entities.OnEntityAdd -= EntityAdded;
            }
        }

        private void EntityAdded(IMyEntity ent)
        {
            var grid = ent as IMyCubeGrid;

            if (grid != null)
            {
                grid.OnBlockAdded += OnBlockAdded;
                grid.OnMarkForClose += OnMarkForClose;
            }
        }

        private void OnMarkForClose(IMyEntity obj)
        {
            var grid = obj as IMyCubeGrid;
            
            if(grid != null)
            {
                grid.OnBlockAdded -= OnBlockAdded;
                grid.OnMarkForClose -= OnMarkForClose;
            }
        }

        private void OnBlockAdded(IMySlimBlock obj)
        {
            IMyCubeBlock block = obj.FatBlock;
            if (block != null)
            {
                if (block is IMyMotorSuspension)
                {
                    disableWheelParking(block as IMyMotorSuspension);
                }
                else if(block is IMyShipConnector)
                {
                    disableConnectorParking(block as IMyShipConnector);
                }
                else if (block is IMyLandingGear)
                {
                    disableLandingGearParking(block as IMyLandingGear);
                }
            }
        }

        private void disableWheelParking(IMyMotorSuspension block)
        {
            block.IsParkingEnabled = configuration.enable_wheel_parking;
        }
        private void disableConnectorParking(IMyShipConnector block)
        {
            if(block.OtherConnector == null)
            {
                block.IsParkingEnabled = configuration.enable_connector_parking;
            }
        }
        private void disableLandingGearParking(IMyLandingGear block)
        {
            block.IsParkingEnabled = configuration.enable_landing_gear_parking;
        }
    }
}
