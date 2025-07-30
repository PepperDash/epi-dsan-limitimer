using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PepperDash.Essentials.AppServer.Messengers;

namespace PepperDash.Essentials.Plugins.Limitimer
{
    public class LimitimerMessenger : MessengerBase
    {
        private readonly LimitimerDevice _limitimerDevice;

        public LimitimerMessenger(string key, string path, LimitimerDevice device)
            : base(key, path)
        {
            _limitimerDevice = device;
            _limitimerDevice.StateChanged += OnDeviceStateChanged;
        }

        private void OnDeviceStateChanged(object sender, EventArgs e)
        {
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

            // Device control actions
            AddAction("/program1", Program1Action);
            AddAction("/program2", Program2Action);
            AddAction("/program3", Program3Action);
            AddAction("/session4", Session4Action);
            AddAction("/beep", BeepAction);
            AddAction("/beep1", Beep1Action);
            AddAction("/blink", BlinkAction);
            AddAction("/startStop", StartStopAction);
            AddAction("/repeat", RepeatAction);
            AddAction("/clear", ClearAction);
            AddAction("/totalTimePlus", TotalTimePlusAction);
            AddAction("/totalTimeMinus", TotalTimeMinusAction);
            AddAction("/sumTimePlus", SumTimePlusAction);
            AddAction("/sumTimeMinus", SumTimeMinusAction);
            AddAction("/setSeconds", SetSecondsAction);
        }

        private void SendFullStatus(string id, JToken content)
        {
            SendFullStatusUpdate();
        }

        #region Device Action Methods

        private void Program1Action(string id, JToken content)
        {
            _limitimerDevice.Program1();
        }

        private void Program2Action(string id, JToken content)
        {
            _limitimerDevice.Program2();
        }

        private void Program3Action(string id, JToken content)
        {
            _limitimerDevice.Program3();
        }

        private void Session4Action(string id, JToken content)
        {
            _limitimerDevice.Session4();
        }

        private void BeepAction(string id, JToken content)
        {
            _limitimerDevice.Beep();
        }

        private void Beep1Action(string id, JToken content)
        {
            _limitimerDevice.Beep1();
        }

        private void BlinkAction(string id, JToken content)
        {
            _limitimerDevice.Blink();
        }

        private void StartStopAction(string id, JToken content)
        {
            _limitimerDevice.StartStop();
        }

        private void RepeatAction(string id, JToken content)
        {
            _limitimerDevice.Repeat();
        }

        private void ClearAction(string id, JToken content)
        {
            _limitimerDevice.Clear();
        }

        private void TotalTimePlusAction(string id, JToken content)
        {
            _limitimerDevice.TotalTimePlus();
        }

        private void TotalTimeMinusAction(string id, JToken content)
        {
            _limitimerDevice.TotalTimeMinus();
        }

        private void SumTimePlusAction(string id, JToken content)
        {
            _limitimerDevice.SumTimePlus();
        }

        private void SumTimeMinusAction(string id, JToken content)
        {
            _limitimerDevice.SumTimeMinus();
        }

        private void SetSecondsAction(string id, JToken content)
        {
            _limitimerDevice.SetSeconds();
        }

        #endregion
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
