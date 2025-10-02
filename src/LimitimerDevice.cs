using System;
using System.Linq;
using PepperDash.Core;
using PepperDash.Core.Logging;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.DeviceTypeInterfaces;
using PepperDash.Essentials.Core.Queues;

namespace PepperDash.Essentials.Plugins.Limitimer
{
	public class LimitimerDevice : EssentialsDevice, IOnline, ICommunicationMonitor
    {
		// private EssentialsPluginTemplateConfigObject _config;

		private GenericQueue ReceiveQueue;

        #region IBasicCommunication Properties and Constructor.  Remove if not needed.

		private readonly IBasicCommunication _comms;
		private readonly GenericCommunicationMonitor _commsMonitor;

		public StatusMonitorBase CommunicationMonitor => _commsMonitor;

        // _comms gather for ASCII based API's
        private readonly CommunicationGather _commsGather;

		private const string CommsDelimiter = "\r";

		private readonly LimitimerPropertiesConfig _config;

		#region Private State Variables

		/// LED state variables
		private LimitimerLedState _program1LedState;
		private LimitimerLedState _program2LedState;
		private LimitimerLedState _program3LedState;
		private LimitimerLedState _sessionLedState;
		private bool _beepLedState;
		private bool _blinkLedState;
		private bool _greenLedState;
		private bool _redLedState;
		private bool _yellowLedState;
		private bool _secondsModeIndicatorState;

		/// Time string variables
		private string _totalTime;
		private string _sumUpTime;
		private string _remainingTime;

		#endregion


		public IntFeedback StatusFeedback { get; private set; }

        public BoolFeedback IsOnline => CommunicationMonitor.IsOnlineFeedback;

		#region State Feedback Objects

		/// LED state feedbacks
		public IntFeedback Program1LedStateFeedback { get; private set; }
		public IntFeedback Program2LedStateFeedback { get; private set; }
		public IntFeedback Program3LedStateFeedback { get; private set; }
		public IntFeedback SessionLedStateFeedback { get; private set; }
		public BoolFeedback BeepLedStateFeedback { get; private set; }
		public BoolFeedback BlinkLedStateFeedback { get; private set; }
		public BoolFeedback GreenLedStateFeedback { get; private set; }
		public BoolFeedback RedLedStateFeedback { get; private set; }
		public BoolFeedback YellowLedStateFeedback { get; private set; }
		public BoolFeedback SecondsModeIndicatorStateFeedback { get; private set; }

		/// Time string feedbacks
		public StringFeedback TotalTimeFeedback { get; private set; }
		public StringFeedback SumUpTimeFeedback { get; private set; }
		public StringFeedback RemainingTimeFeedback { get; private set; }

		#endregion

		#region Public State Properties

		/// LED state properties
		public LimitimerLedState Program1LedState
		{
			get { return _program1LedState; }
			set
			{
				if (_program1LedState != value)
                {
                    _program1LedState = value;
                    Program1LedStateFeedback?.FireUpdate();
                }
            }
		}
		public LimitimerLedState Program2LedState
        {
            get { return _program2LedState; }
            set
            {
                if (_program2LedState != value)
                {
                    _program2LedState = value;
                    Program2LedStateFeedback?.FireUpdate();
                }
            }
        }
        public LimitimerLedState Program3LedState
        {
            get { return _program3LedState; }
            set
            {
                if (_program3LedState != value)
                {
                    _program3LedState = value;
                    Program3LedStateFeedback?.FireUpdate();
                }
            }
        }
        public LimitimerLedState SessionLedState
        {
            get { return _sessionLedState; }
            set
            {
                if (_sessionLedState != value)
                {
                    _sessionLedState = value;
                    SessionLedStateFeedback?.FireUpdate();
                }
            }
        }
        public bool BeepLedState
        {
            get { return _beepLedState; }
            set
            {
                if (_beepLedState != value)
                {
                    _beepLedState = value;
                    BeepLedStateFeedback?.FireUpdate();
                }
            }
        }
		public bool BlinkLedState
        {
            get { return _blinkLedState; }
            set
            {
                if (_blinkLedState != value)
                {
                    _blinkLedState = value;
                    BlinkLedStateFeedback?.FireUpdate();
                }
            }
        }
        public bool GreenLedState
        {
            get { return _greenLedState; }
            set
            {
                if (_greenLedState != value)
                {
                    _greenLedState = value;
                    GreenLedStateFeedback?.FireUpdate();
                }
            }
        }
        public bool RedLedState
        {
            get { return _redLedState; }
            set
            {
                if (_redLedState != value)
                {
                    _redLedState = value;
                    RedLedStateFeedback?.FireUpdate();
                }
            }
        }
        public bool YellowLedState
        {
            get { return _yellowLedState; }
            set
            {
                if (_yellowLedState != value)
                {
                    _yellowLedState = value;
                    YellowLedStateFeedback?.FireUpdate();
                }
            }
        }
        public bool SecondsModeIndicatorState
        {
            get { return _secondsModeIndicatorState; }
            set
            {
                if (_secondsModeIndicatorState != value)
                {
                    _secondsModeIndicatorState = value;
                    SecondsModeIndicatorStateFeedback?.FireUpdate();
                }
            }
        }

        /// Time string properties
        public string TotalTime
        {
            get { return _totalTime; }
            set
            {
                if (_totalTime != value)
                {
                    _totalTime = value;
                    TotalTimeFeedback?.FireUpdate();
                }
            }
        }
        public string SumUpTime
        {
            get { return _sumUpTime; }
            set
            {
                if (_sumUpTime != value)
                {
                    _sumUpTime = value;
                    SumUpTimeFeedback?.FireUpdate();
                }
            }
        }
        public string RemainingTime
        {
            get { return _remainingTime; }
            set
            {
                if (_remainingTime != value)
                {
                    _remainingTime = value;
                    RemainingTimeFeedback?.FireUpdate();
                }
            }
        }

        #endregion

        protected override void CreateMobileControlMessengers()
		{
			var mc = DeviceManager.AllDevices.OfType<IMobileControl>().FirstOrDefault();

			if (mc == null)
			{
				this.LogInformation("Mobile Control not found");
				return;
			}

			var limtimerMessenger = new LimitimerMessenger($"{Key}", $"/device/{Key}", this);

			mc.AddDeviceMessenger(limtimerMessenger);

		} 


		public LimitimerDevice(string key, string name, LimitimerPropertiesConfig config, IBasicCommunication comms)
			: base(key, name)
		{
			_config = config;

			// Initialize state variables with default values
			_program1LedState = LimitimerLedState.off;
			_program2LedState = LimitimerLedState.off;
			_program3LedState = LimitimerLedState.off;
			_sessionLedState = LimitimerLedState.off;
			_beepLedState = false;
			_blinkLedState = false;
			_greenLedState = false;
			_redLedState = false;
			_yellowLedState = false;
			_secondsModeIndicatorState = false;
			_totalTime = "00:00";
			_sumUpTime = "00:00";
			_remainingTime = "00:00";

			ReceiveQueue = new GenericQueue(key + "-rxqueue");  // If you need to set the thread priority, use one of the available overloaded constructors.

			// TODO [ ] no polling for this device - anything needed to change here?
			_comms = comms;
			_commsMonitor = new GenericCommunicationMonitor(this, _comms, _config.PollTimeMs, _config.WarningTimeoutMs, _config.ErrorTimeoutMs, Poll);

			StatusFeedback = new IntFeedback(key, () => (int)_commsMonitor.Status);

			// Initialize state feedback objects - following NVX pattern
			Program1LedStateFeedback = new IntFeedback($"program1LedState", () => (int)_program1LedState);
			Program2LedStateFeedback = new IntFeedback($"program2LedState", () => (int)_program2LedState);
			Program3LedStateFeedback = new IntFeedback($"program3LedState", () => (int)_program3LedState);
			SessionLedStateFeedback = new IntFeedback($"sessionLedState", () => (int)_sessionLedState);
			BeepLedStateFeedback = new BoolFeedback($"beepLedState", () => _beepLedState);
			BlinkLedStateFeedback = new BoolFeedback($"blinkLedState", () => _blinkLedState);
			GreenLedStateFeedback = new BoolFeedback($"greenLedState", () => _greenLedState);
			RedLedStateFeedback = new BoolFeedback($"redLedState", () => _redLedState);
			YellowLedStateFeedback = new BoolFeedback($"yellowLedState", () => _yellowLedState);
			SecondsModeIndicatorStateFeedback = new BoolFeedback($"secondsModeIndicatorState", () => _secondsModeIndicatorState);
			TotalTimeFeedback = new StringFeedback($"totalTime", () => _totalTime);
			SumUpTimeFeedback = new StringFeedback($"sumUpTime", () => _sumUpTime);
			RemainingTimeFeedback = new StringFeedback($"remainingTime", () => _remainingTime);


			#region Communication data event handlers.  Comment out any that don't apply to the API type

			// _comms gather for any API that has a defined delimiter
			_commsGather = new CommunicationGather(_comms, CommsDelimiter);
			_commsGather.LineReceived += Handle_LineRecieved;

			#endregion
		}

        public override void Initialize()
        {
            base.Initialize();

			_commsMonitor.Start();
        }


        /// <summary>
        /// Poll method for the communication monitor - currently not used for this device
        /// </summary>
        private void Poll()
		{
			
		}

		// TODO [ ] If not using an API with a delimeter, delete the method below
		private void Handle_LineRecieved(object sender, GenericCommMethodReceiveTextArgs args)
		{
            // Enqueues the message to be processed in a dedicated thread, but the specified method
            ReceiveQueue.Enqueue(new ProcessStringMessage(args.Text, ProcessFeedbackMessage));
		}


		public void ProcessFeedbackMessage(string message)
		{
            this.LogVerbose("Processing feedback message: {0}", message);
			
			if (string.IsNullOrEmpty(message))
				return;

			// Remove delimiter and trim whitespace
			var cleanMessage = message.Replace(CommsDelimiter, "").Trim();

			switch (cleanMessage)
			{
				// Program 1 LED states
				case "P1LEDON":
                    this.LogVerbose("Processing feedback message: case-> P1LEDON  currentValue = {0}", _program1LedState);
					_program1LedState = LimitimerLedState.on;
                    Program1LedStateFeedback?.FireUpdate();
                    this.LogVerbose("Processing feedback message: case-> P1LEDON  newValue = {0}", _program1LedState);
					break;
				case "P1LEDDM":
					_program1LedState = LimitimerLedState.dim;
                    Program1LedStateFeedback?.FireUpdate();
                    break;
				case "P1LEDOF":
					_program1LedState = LimitimerLedState.off;
                    Program1LedStateFeedback?.FireUpdate();
                    break;

				// Program 2 LED states
				case "P2LEDON":
					_program2LedState = LimitimerLedState.on;
                    Program2LedStateFeedback?.FireUpdate();

                    break;
				case "P2LEDDM":
					_program2LedState = LimitimerLedState.dim;
                    Program2LedStateFeedback?.FireUpdate();

                    break;
				case "P2LEDOF":
					_program2LedState = LimitimerLedState.off;
                    Program2LedStateFeedback?.FireUpdate();

                    break;

				// Program 3 LED states
				case "P3LEDON":
					_program3LedState = LimitimerLedState.on;
                    Program3LedStateFeedback?.FireUpdate();
                    break;
				case "P3LEDDM":
					_program3LedState = LimitimerLedState.dim;
                    Program3LedStateFeedback?.FireUpdate();
                    break;
				case "P3LEDOF":
					_program3LedState = LimitimerLedState.off;
                    Program3LedStateFeedback?.FireUpdate();
                    break;

				// Session LED states
				case "SESLEDON":
					_sessionLedState = LimitimerLedState.on;
                    SessionLedStateFeedback?.FireUpdate();

                    break;
				case "SESLEDDM":
					_sessionLedState = LimitimerLedState.dim;
                    SessionLedStateFeedback?.FireUpdate();
                    break;
				case "SESLEDOF":
					_sessionLedState = LimitimerLedState.off;
                    SessionLedStateFeedback?.FireUpdate();
                    break;

				// Beep LED states
				case "BPLEDON":
					_beepLedState = true;
                    BeepLedStateFeedback?.FireUpdate();

                    break;
				case "BPLEDOF":
					_beepLedState = false;
                    BeepLedStateFeedback?.FireUpdate();

                    break;

				// Blink LED states
				case "BKLEDON":
					_blinkLedState = true;
                    BlinkLedStateFeedback?.FireUpdate();

                    break;
				case "BKLEDOF":
					_blinkLedState = false;
                    BlinkLedStateFeedback?.FireUpdate();

                    break;

				// Green LED states
				case "GRNLEDON":
					_greenLedState = true;
                    GreenLedStateFeedback?.FireUpdate();

                    break;
				case "GRNLEDOF":
					_greenLedState = false;
                    GreenLedStateFeedback?.FireUpdate();

                    break;

				// Yellow LED states
				case "YELLEDON":
					_yellowLedState = true;
                    YellowLedStateFeedback?.FireUpdate();

                    break;
				case "YELLEDOF":
					_yellowLedState = false;
                    YellowLedStateFeedback?.FireUpdate();

                    break;

				// Red LED states
				case "REDLEDON":
					_redLedState = true;
                    RedLedStateFeedback?.FireUpdate();

                    break;
				case "REDLEDOF":
					_redLedState = false;
                    RedLedStateFeedback?.FireUpdate();

                    break;

				// Seconds Mode Indicator states
				case "SMON":
					_secondsModeIndicatorState = true;
                    SecondsModeIndicatorStateFeedback?.FireUpdate();

                    break;
				case "SMOF":
					_secondsModeIndicatorState = false;
                    SecondsModeIndicatorStateFeedback?.FireUpdate();

                    break;

				// Beep event
				case "BEEP":
					OnBeepEvent();
					break;

				default:
					if (cleanMessage.StartsWith("TTSTR="))
					{
						// Total Time String (format: TTSTR=MM:SS)
						_totalTime = cleanMessage.Substring(6); // Remove "TTSTR=" prefix
                        TotalTimeFeedback?.FireUpdate();

                    }
                    else if (cleanMessage.StartsWith("STSTR="))
					{
						// Sum-Up Time String (format: STSTR=MM:SS)
						_sumUpTime = cleanMessage.Substring(6); // Remove "STSTR=" prefix
                        SumUpTimeFeedback?.FireUpdate();
                    }
					else if (cleanMessage.StartsWith("RTSTR="))
					{
						// Remaining Time String (format: RTSTR=MM:SS)
						_remainingTime = cleanMessage.Substring(6); // Remove "RTSTR=" prefix
                        RemainingTimeFeedback?.FireUpdate();
                    }
					else
					{
						this.LogWarning("Unknown feedback message received: {0}", cleanMessage);
					}
					break;
			}

        }

		public event EventHandler BeepEvent;

		private void OnBeepEvent()
		{
			// TODO: Implement BeepEvent functionality
			this.LogInformation("Beep event triggered"); ;
		}


		/// Sends text to the device plugin comms
		/// </summary>
		/// <remarks>
		/// Can be used to test commands with the device plugin using the DEVPROPS and DEVJSON console commands
		/// </remarks>
		/// <param name="text">Command to be sent</param>		
		public void SendText(string text)
		{
			if (string.IsNullOrEmpty(text)) return;

			_comms.SendText(string.Format("{0}{1}", text, CommsDelimiter));
		}

		#region Action Methods

		public void Program1()
		{
			SendText("PRG1");
		}

		public void Program2()
		{
			SendText("PRG2");
		}

		public void Program3()
		{
			SendText("PRG3");
		}

		public void Session4()
		{
			SendText("SESS");
		}

		public void Beep()
		{
			SendText("BEEP");
		}

		/// TODO [] determine how/if we can reproduce this function (simulates press/hold of panel beep button to elicit a single beep)
		public void Beep1()
		{
			
		}

		public void Blink()
		{
			SendText("BLNK");
		}

		public void StartStop()
		{
			SendText("STOP");
		}

		public void Repeat()
		{
			SendText("REPT");
		}

		public void Clear()
		{
			SendText("CLR");
		}

		public void TotalTimePlus()
		{
			SendText("TTUP");
		}

		public void TotalTimeMinus()
		{
			SendText("TTDN");
		}

		public void SumTimePlus()
		{
			SendText("STUP");
		}

		public void SumTimeMinus()
		{
			SendText("STDN");
		}

		public void SetSeconds()
		{
			SendText("SSEC");
		}

		#endregion

        #endregion


        #region Overrides of EssentialsBridgeableDevice
/*
        /// <summary>
        /// Links the plugin device to the EISC bridge
        /// </summary>
        /// <param name="trilist"></param>
        /// <param name="joinStart"></param>
        /// <param name="joinMapKey"></param>
        /// <param name="bridge"></param>
        public override void LinkToApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
        {
            var joinMap = new EssentialsPluginTemplateBridgeJoinMap(joinStart);

            // This adds the join map to the collection on the bridge
            if (bridge != null)
            {
                bridge.AddJoinMap(Key, joinMap);
            }

            var customJoins = JoinMapHelper.TryGetJoinMapAdvancedForDevice(joinMapKey);

            if (customJoins != null)
            {
                joinMap.SetCustomJoinData(customJoins);
            }

            Debug.Console(1, "Linking to Trilist '{0}'", trilist.ID.ToString("X"));
            Debug.Console(0, "Linking to Bridge Type {0}", GetType().Name);

            // TODO [ ] Implement bridge links as needed

            // links to bridge
            trilist.SetString(joinMap.DeviceName.JoinNumber, Name);

            trilist.SetBoolSigAction(joinMap.Connect.JoinNumber, sig => Connect = sig);
            ConnectFeedback.LinkInputSig(trilist.BooleanInput[joinMap.Connect.JoinNumber]);

            StatusFeedback.LinkInputSig(trilist.UShortInput[joinMap.Status.JoinNumber]);
            OnlineFeedback.LinkInputSig(trilist.BooleanInput[joinMap.IsOnline.JoinNumber]);

            UpdateFeedbacks();

            trilist.OnlineStatusChange += (o, a) =>
            {
                if (!a.DeviceOnLine) return;

                trilist.SetString(joinMap.DeviceName.JoinNumber, Name);
                UpdateFeedbacks();
            };
        } 

        private void UpdateFeedbacks()
        {
            // TODO [ ] Update as needed for the plugin being developed
            ConnectFeedback.FireUpdate();
            OnlineFeedback.FireUpdate();
            StatusFeedback.FireUpdate();
        }
 */
        #endregion

    }
}

