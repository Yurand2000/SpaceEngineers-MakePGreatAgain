using System;
using Sandbox.ModAPI;
using VRage.Utils;

namespace Yurand.MakePGreatAgain
{
    public class Config
    {
        public bool enable_wheel_parking;
        public bool enable_connector_parking;
        public bool enable_landing_gear_parking;
        public Config()
        {
            enable_wheel_parking = false;
            enable_connector_parking = false;
            enable_landing_gear_parking = false;
        }
        public static Config deserialize(string serialized_file)
        {
            Config read_config = null;
            try
            {
                var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(serialized_file, typeof(Config));
                string xml = reader.ReadToEnd();
                reader.Close();
                read_config = MyAPIGateway.Utilities.SerializeFromXML<Config>(xml);
                MyLog.Default.WriteLine("[MakePGreatAgain] Successfully deserialized the file: " + serialized_file);
            }
            catch(Exception e)
            {
                read_config = new Config();
                MyLog.Default.WriteLine("[MakePGreatAgain] Couldn't deserialize the file: " + serialized_file + " Exception: " + e.Message);
            }
            return read_config;
        }

        public static void serialize(Config config, string serialized_file)
        {
            try
            {
                string xml = MyAPIGateway.Utilities.SerializeToXML(config);
                var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage(serialized_file, typeof(Config));
                writer.Write(xml);
                writer.Close();
                MyLog.Default.WriteLine("[MakePGreatAgain] Successfully serialized the file: " + serialized_file);
            }
            catch (Exception e)
            {
                MyLog.Default.WriteLine("[MakePGreatAgain] Couldn't serialize the file: " + serialized_file + " Exception: " + e.Message);
            }
        }
    }
}
