using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PepperDash.Essentials.AppServer.Messengers;
using PepperDash.Essentials.Core;

namespace PepperDash.Essentials.Plugin
{
    public class LimitimerMessenger : MessengerBase
    {

        public LimitimerMessenger(string key, string path) //add reference to this devie type when defined
            : base(key, path)
        {

        }

        protected override void RegisterActions()
        {
            base.RegisterActions();
            AddAction("/fullStatus", SendFullStatus);
        }

        private void SendFullStatus(string id, JToken content) 
        {
            PostStatusMessage(new LimitimerStateMessage
            {
                //Define Getter Property methods
                /*Program1LedState = GetProgram1LedState()
                Program2LedState = GetProgram2LedState(),
                Program3LedState = GetProgram3LedState(),
                BeepLedState = GetBeepLedState(),
                BlinkLedState = GetBlinkLedState(),
                GreenLedState = GetGreenLedState(),
                RedLedState = GetRedLedState(),
                YellowLedState = GetYellowLedState(),
                SecondsModeIndicatorState = GetSecondsModeIndicatorState(),
                Beep = GetBeepState(),
                TotalTime = GetTotalTime(),
                SumUpTime = GetSumUpTime(),
                RemainingTime = GetRemainingTime()*/
            });
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
