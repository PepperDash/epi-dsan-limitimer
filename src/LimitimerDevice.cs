using System;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Queues;

namespace PepperDash.Essentials.Plugins.Limitimer
{
	public class LimitimerDevice : EssentialsDevice, IOnline
    {
		/// <summary>
		/// It is often desirable to store the config
		/// </summary>

		// private EssentialsPluginTemplateConfigObject _config;

		private GenericQueue ReceiveQueue;

        #region IBasicCommunication Properties and Constructor.  Remove if not needed.

		private readonly IBasicCommunication _comms;
		private readonly GenericCommunicationMonitor _commsMonitor;

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
		private bool _beepState;

		/// Time string variables
		private string _totalTime;
		private string _sumUpTime;
		private string _remainingTime;

		#endregion

		public bool Connect
		{
			get { return _comms.IsConnected; }
			set
			{
				if (value)
				{
					_comms.Connect();
					_commsMonitor.Start();
				}
				else
				{
					_comms.Disconnect();
					_commsMonitor.Stop();
				}
			}
		}

		/// <summary>
		/// Reports connect feedback through the bridge
		/// </summary>
		public BoolFeedback ConnectFeedback { get; private set; }

		/// <summary>
		/// Reports online feedback through the bridge
		/// </summary>
		public BoolFeedback OnlineFeedback { get; private set; }

		/// <summary>
		/// Reports socket status feedback through the bridge
		/// </summary>
		public IntFeedback StatusFeedback { get; private set; }

        public BoolFeedback IsOnline => OnlineFeedback;

		#region Public State Properties

		/// LED state properties
		public LimitimerLedState Program1LedState => _program1LedState;
		public LimitimerLedState Program2LedState => _program2LedState;
		public LimitimerLedState Program3LedState => _program3LedState;
		public LimitimerLedState SessionLedState => _sessionLedState;
		public bool BeepLedState => _beepLedState;
		public bool BlinkLedState => _blinkLedState;
		public bool GreenLedState => _greenLedState;
		public bool RedLedState => _redLedState;
		public bool YellowLedState => _yellowLedState;
		public bool SecondsModeIndicatorState => _secondsModeIndicatorState;
		public bool BeepState => _beepState;

		/// Time string properties
		public string TotalTime => _totalTime;
		public string SumUpTime => _sumUpTime;
		public string RemainingTime => _remainingTime;

		#endregion

        public LimitimerDevice(string key, string name, LimitimerPropertiesConfig config, IBasicCommunication comms)
			: base(key, name)
		{
			Debug.LogMessage(Serilog.Events.LogEventLevel.Information, this, "Constructing new {0} instance", name);

			// TODO [ ] Update the constructor as needed for the plugin device being developed

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
			_beepState = false;
			_totalTime = "00:00";
			_sumUpTime = "00:00";
			_remainingTime = "00:00";

            ReceiveQueue = new GenericQueue(key + "-rxqueue");  // If you need to set the thread priority, use one of the available overloaded constructors.

			// TODO [ ] no polling for this device - anything needed to change here?
			_comms = comms;
			_commsMonitor = new GenericCommunicationMonitor(this, _comms, _config.PollTimeMs, _config.WarningTimeoutMs, _config.ErrorTimeoutMs, Poll);

			ConnectFeedback = new BoolFeedback(key, () => Connect);
			OnlineFeedback = new BoolFeedback(key, () => _commsMonitor.IsOnline);
			StatusFeedback = new IntFeedback(key, () => (int)_commsMonitor.Status);


			// TODO [ ] comms will always be rs-232 - anything to do here?
			var socket = _comms as ISocketStatus;
			if (socket != null)
			{
				// device comms is IP **ELSE** device comms is RS232
				socket.ConnectionChange += socket_ConnectionChange;
				Connect = true;
            }

            #region Communication data event handlers.  Comment out any that don't apply to the API type

            // _comms gather for any API that has a defined delimiter
			_commsGather = new CommunicationGather(_comms, CommsDelimiter);
			_commsGather.LineReceived += Handle_LineRecieved;

            #endregion
        }


		private void socket_ConnectionChange(object sender, GenericSocketStatusChageEventArgs args)
		{
			if (ConnectFeedback != null)
				ConnectFeedback.FireUpdate();

			if (StatusFeedback != null)
				StatusFeedback.FireUpdate();
		}

		/// <summary>
		/// Poll method for the communication monitor - currently not used for this device
		/// </summary>
		private void Poll()
		{
			// No polling needed for this device
		}

		// TODO [ ] If not using an API with a delimeter, delete the method below
		private void Handle_LineRecieved(object sender, GenericCommMethodReceiveTextArgs args)
		{
			// TODO [ ] Implement method 
			
            // Enqueues the message to be processed in a dedicated thread, but the specified method
            ReceiveQueue.Enqueue(new ProcessStringMessage(args.Text, ProcessFeedbackMessage));
		}

        // // TODO [ ] If not using an ASCII based API with no delimeter, delete the method below
        // void Handle_TextReceived(object sender, GenericCommMethodReceiveTextArgs e)
        // {
        //     // TODO [ ] Implement method 
        //     throw new System.NotImplementedException();
        // }

		void ProcessFeedbackMessage(string message)
		{
			Debug.LogMessage(Serilog.Events.LogEventLevel.Information, this, "Processing feedback message: {0}", message);
			
			if (string.IsNullOrEmpty(message))
				return;

			// Remove delimiter and trim whitespace
			var cleanMessage = message.Replace(CommsDelimiter, "").Trim();

			switch (cleanMessage)
			{
				// Program 1 LED states
				case "P1LEDON":
					_program1LedState = LimitimerLedState.on;
					break;
				case "P1LEDDM":
					_program1LedState = LimitimerLedState.dim;
					break;
				case "P1LEDOF":
					_program1LedState = LimitimerLedState.off;
					break;

				// Program 2 LED states
				case "P2LEDON":
					_program2LedState = LimitimerLedState.on;
					break;
				case "P2LEDDM":
					_program2LedState = LimitimerLedState.dim;
					break;
				case "P2LEDOF":
					_program2LedState = LimitimerLedState.off;
					break;

				// Program 3 LED states
				case "P3LEDON":
					_program3LedState = LimitimerLedState.on;
					break;
				case "P3LEDDM":
					_program3LedState = LimitimerLedState.dim;
					break;
				case "P3LEDOF":
					_program3LedState = LimitimerLedState.off;
					break;

				// Session LED states
				case "SESLEDON":
					_sessionLedState = LimitimerLedState.on;
					break;
				case "SESLEDDM":
					_sessionLedState = LimitimerLedState.dim;
					break;
				case "SESLEDOF":
					_sessionLedState = LimitimerLedState.off;
					break;

				// Beep LED states
				case "BPLEDON":
					_beepLedState = true;
					break;
				case "BPLEDOF":
					_beepLedState = false;
					break;

				// Blink LED states
				case "BKLEDON":
					_blinkLedState = true;
					break;
				case "BKLEDOF":
					_blinkLedState = false;
					break;

				// Green LED states
				case "GRNLEDON":
					_greenLedState = true;
					break;
				case "GRNLEDOF":
					_greenLedState = false;
					break;

				// Yellow LED states
				case "YELLEDON":
					_yellowLedState = true;
					break;
				case "YELLEDOF":
					_yellowLedState = false;
					break;

				// Red LED states
				case "REDLEDON":
					_redLedState = true;
					break;
				case "REDLEDOF":
					_redLedState = false;
					break;

				// Seconds Mode Indicator states
				case "SMON":
					_secondsModeIndicatorState = true;
					break;
				case "SMOF":
					_secondsModeIndicatorState = false;
					break;

				// Beep
				case "BEEP":
					_beepState = true;
					break;

				default:
					if (cleanMessage.StartsWith("TTSTR="))
					{
						// Total Time String (format: TTSTR=MM:SS)
						_totalTime = cleanMessage.Substring(6); // Remove "TTSTR=" prefix
					}
					else if (cleanMessage.StartsWith("STSTR="))
					{
						// Sum-Up Time String (format: STSTR=MM:SS)
						_sumUpTime = cleanMessage.Substring(6); // Remove "STSTR=" prefix
					}
					else if (cleanMessage.StartsWith("RTSTR="))
					{
						// Remaining Time String (format: RTSTR=MM:SS)
						_remainingTime = cleanMessage.Substring(6); // Remove "RTSTR=" prefix
					}
					else
					{
						Debug.LogMessage(Serilog.Events.LogEventLevel.Warning, this, "Unknown feedback message received: {0}", cleanMessage);
					}
					break;
			}

			// Trigger state change notification after processing any feedback
			OnStateChanged();
        }

		public event EventHandler StateChanged;

		private void OnStateChanged()
		{
			StateChanged?.Invoke(this, EventArgs.Empty);
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
			//SendText("BEEP1");
			throw new NotImplementedException();
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

        /// <summary>
        /// Links the plugin device to the EISC bridge
        /// </summary>
        /// <param name="trilist"></param>
        /// <param name="joinStart"></param>
        /// <param name="joinMapKey"></param>
        /// <param name="bridge"></param>
        /*public override void LinkToApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
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
        } */

        private void UpdateFeedbacks()
        {
            // TODO [ ] Update as needed for the plugin being developed
            ConnectFeedback.FireUpdate();
            OnlineFeedback.FireUpdate();
            StatusFeedback.FireUpdate();
        }

        #endregion

    }
}

