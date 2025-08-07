using System;
using System.Collections.Generic;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Config;

namespace PepperDash.Essentials.Plugins.Limitimer
{
	
    public class LimitimerFactory : EssentialsPluginDeviceFactory<LimitimerDevice>
    {

        public LimitimerFactory()
        {
			MinimumEssentialsFrameworkVersion = "2.8.0";

            TypeNames = new List<string>() { "limitimer" };
        }

        public override EssentialsDevice BuildDevice(DeviceConfig dc)
        {
            try {
                Debug.LogMessage(Serilog.Events.LogEventLevel.Information, "[{key}] Factory Attempting to create new device from type: {type}", this, dc.Key, dc.Type);

                // attempt build the plugin device comms device
                var comms = CommFactory.CreateCommForDevice(dc);
                if (comms == null)
                {
                    Debug.LogMessage(Serilog.Events.LogEventLevel.Information, "[{0}] Factory Notice: No control object present for device {1}", dc.Key, dc.Name);
                    return null;
                }

                // get the plugin device properties configuration object & check for null 
                var config = dc.Properties.ToObject<LimitimerPropertiesConfig>();
                if (config == null)
                {
                    Debug.LogMessage(Serilog.Events.LogEventLevel.Information, "[{0}] Factory: failed to read properties config for {1}", dc.Key, dc.Name);
                    return null;
                }

                return new LimitimerDevice(dc.Key, dc.Name, config, comms);
            } 
            catch (Exception e)
            {
                Debug.LogMessage(Serilog.Events.LogEventLevel.Error, "[{key}] Factory Error: Failed to create Limitimer device {name} of type {type}. Exception Type: {exceptionType}, Message: {message}, StackTrace: {stackTrace}", this, dc.Key, dc.Name, dc.Type, e.GetType().Name, e.Message, e.StackTrace);
                return null;
            }
        }
    }
}

          