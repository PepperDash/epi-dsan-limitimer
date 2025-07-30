using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PepperDash.Essentials.AppServer.Messengers;
using PepperDash.Essentials.Core;

namespace PepperDash.Essentials.Plugins.Limitimer
{
    public class LimitimerMessenger : MessengerBase
    {
        private readonly LimitimerDevice _limitimerDevice;

        public LimitimerMessenger(string key, string path, LimitimerDevice device)
            : base(key, path)
        {
            _limitimerDevice = device;
            
            // Subscribe to device state changes for real-time updates
            _limitimerDevice.StateChanged += OnDeviceStateChanged;
        }

        private void OnDeviceStateChanged(object sender, EventArgs e)
        {
            // Send updated state to all connected clients when device state changes
            SendFullStatusUpdate();
        }

        private void SendFullStatusUpdate()
        {
            PostStatusMessage(new LimitimerStateMessage
            {
                Program1LedState = _limitimerDevice.Program1LedState,
                Program2LedState = _limitimerDevice.Program2LedState,
                Program3LedState = _limitimerDevice.Program3LedState,
                SessionLedState = _limitimerDevice.SessionLedState,
                BeepLedState = _limitimerDevice.BeepLedState,
                BlinkLedState = _limitimerDevice.BlinkLedState,
                GreenLedState = _limitimerDevice.GreenLedState,
                RedLedState = _limitimerDevice.RedLedState,
                YellowLedState = _limitimerDevice.YellowLedState,
                SecondsModeIndicatorState = _limitimerDevice.SecondsModeIndicatorState,
                Beep = _limitimerDevice.BeepState,
                TotalTime = _limitimerDevice.TotalTime,
                SumUpTime = _limitimerDevice.SumUpTime,
                RemainingTime = _limitimerDevice.RemainingTime
            });
        }

        protected override void RegisterActions()
        {
            base.RegisterActions();
            AddAction("/fullStatus", SendFullStatus);
        }

        private void SendFullStatus(string id, JToken content) //called once by front-end at start up, individual statuses will be sent as unsolicited feedback
        {
            SendFullStatusUpdate();
        }
    }

    public class LimitimerStateMessage : DeviceStateMessageBase
    {
        [JsonProperty("program1LedState")]
        public LimitimerLedState Program1LedState { get; set; }

        [JsonProperty("program2LedState")]
        public LimitimerLedState Program2LedState { get; set; }

        [JsonProperty("program3LedState")]
        public LimitimerLedState Program3LedState { get; set; }

        [JsonProperty("sessionLedState")]
        public LimitimerLedState SessionLedState { get; set; }

        [JsonProperty("beepLedState")]
        public bool BeepLedState { get; set; }

        [JsonProperty("blinkLedState")]
        public bool BlinkLedState { get; set; }

        [JsonProperty("greenLedState")]

        public bool GreenLedState { get; set; }

        [JsonProperty("redLedState")]
        public bool RedLedState { get; set; }

        [JsonProperty("yellowLedState")]
        public bool YellowLedState { get; set; }

        [JsonProperty("secondsModeIndicatorState")]

        public bool SecondsModeIndicatorState { get; set; }

        [JsonProperty("beep")]
        public bool Beep { get; set; }

        [JsonProperty("totalTime")]
        public string TotalTime { get; set; }

        [JsonProperty("sumUpTime")]
        public string SumUpTime { get; set; }

        [JsonProperty("remainingTime")]
        public string RemainingTime { get; set; }



    }

    public enum LimitimerLedState
    {
        on,
        dim,
        off
    }
}
