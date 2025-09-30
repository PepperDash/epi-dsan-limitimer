using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PepperDash.Essentials.AppServer.Messengers;
using PepperDash.Core;

namespace PepperDash.Essentials.Plugins.Limitimer
{
    public class LimitimerMessenger : MessengerBase
    {
        private readonly LimitimerDevice _limitimerDevice;

        public LimitimerMessenger(string key, string path, LimitimerDevice device)
            : base(key, path, device)
        {
            _limitimerDevice = device;

            SubscribeToDeviceFeedbacks();
            
            _limitimerDevice.BeepEvent += OnDeviceBeepEvent;
        }

        private void SubscribeToDeviceFeedbacks()
        {
            // LED state feedbacks
            _limitimerDevice.Program1LedStateFeedback.OutputChange += (o, a) => 
                SendLedUpdate("program1LedState", _limitimerDevice.Program1LedState);
            _limitimerDevice.Program2LedStateFeedback.OutputChange += (o, a) => 
                SendLedUpdate("program2LedState", _limitimerDevice.Program2LedState);
            _limitimerDevice.Program3LedStateFeedback.OutputChange += (o, a) => 
                SendLedUpdate("program3LedState", _limitimerDevice.Program3LedState);
            _limitimerDevice.SessionLedStateFeedback.OutputChange += (o, a) => 
                SendLedUpdate("sessionLedState", _limitimerDevice.SessionLedState);
            
            // Boolean LED states
            _limitimerDevice.BeepLedStateFeedback.OutputChange += (o, a) => 
                SendBoolUpdate("beepLedState", _limitimerDevice.BeepLedState);
            _limitimerDevice.BlinkLedStateFeedback.OutputChange += (o, a) => 
                SendBoolUpdate("blinkLedState", _limitimerDevice.BlinkLedState);
            _limitimerDevice.GreenLedStateFeedback.OutputChange += (o, a) => 
                SendBoolUpdate("greenLedState", _limitimerDevice.GreenLedState);
            _limitimerDevice.RedLedStateFeedback.OutputChange += (o, a) => 
                SendBoolUpdate("redLedState", _limitimerDevice.RedLedState);
            _limitimerDevice.YellowLedStateFeedback.OutputChange += (o, a) => 
                SendBoolUpdate("yellowLedState", _limitimerDevice.YellowLedState);
            _limitimerDevice.SecondsModeIndicatorStateFeedback.OutputChange += (o, a) => 
                SendBoolUpdate("secondsModeIndicatorState", _limitimerDevice.SecondsModeIndicatorState);
            
            // Time string feedbacks
            _limitimerDevice.TotalTimeFeedback.OutputChange += (o, a) => 
                SendTimeUpdate("totalTime", _limitimerDevice.TotalTime);
            _limitimerDevice.SumUpTimeFeedback.OutputChange += (o, a) => 
                SendTimeUpdate("sumUpTime", _limitimerDevice.SumUpTime);
            _limitimerDevice.RemainingTimeFeedback.OutputChange += (o, a) => 
                SendTimeUpdate("remainingTime", _limitimerDevice.RemainingTime);
        }

        private void SendLedUpdate(string propertyName, LimitimerLedState state)
        {
            Debug.LogMessage(Serilog.Events.LogEventLevel.Information, "SendLedUpdate: propertyName={0}", propertyName);
            Debug.LogMessage(Serilog.Events.LogEventLevel.Information, "SendLedUpdate: state={0}", state);
            var updateObject = new JObject();
            updateObject[propertyName] = JToken.FromObject(state);
            PostStatusMessage(updateObject);
        }

        private void SendBoolUpdate(string propertyName, bool value)
        {
            var updateObject = new JObject();
            updateObject[propertyName] = value;
            PostStatusMessage(updateObject);
            Debug.LogMessage(Serilog.Events.LogEventLevel.Information, "SendBoolUpdate: object={0}", updateObject);
        }

        private void SendTimeUpdate(string propertyName, string timeValue)
        {
            var updateObject = new JObject();
            updateObject[propertyName] = timeValue;
            PostStatusMessage(updateObject);
            Debug.LogMessage(Serilog.Events.LogEventLevel.Information, "SendTimeUpdate: object={0}", updateObject);
        }

        private void OnDeviceBeepEvent(object sender, EventArgs e)
        {
            // Send a separate beep event message
            PostStatusMessage(new LimitimerBeepEventMessage());
        }

        private void SendFullStatusUpdate(string id)
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
                TotalTime = _limitimerDevice.TotalTime,
                SumUpTime = _limitimerDevice.SumUpTime,
                RemainingTime = _limitimerDevice.RemainingTime
            }, id);
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
            SendFullStatusUpdate(id);
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

        [JsonProperty("totalTime")]
        public string TotalTime { get; set; }

        [JsonProperty("sumUpTime")]
        public string SumUpTime { get; set; }

        [JsonProperty("remainingTime")]
        public string RemainingTime { get; set; }



    }

    public class LimitimerBeepEventMessage : DeviceStateMessageBase
    {
        [JsonProperty("eventType")]
        public string EventType { get; set; } = "beep";
    }

    public enum LimitimerLedState
    {
        off,
        on,
        dim
    }
}
