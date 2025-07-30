using System;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Bridges;
using PepperDash.Essentials.Core.Queues;

namespace PepperDash.Essentials.Plugins.Limitimer
{
	/// <summary>
	/// Plugin device template for third party devices that use IBasicCommunication
	/// </summary>
	/// <remarks>
	/// Rename the class to match the device plugin being developed.
	/// </remarks>
	/// <example>
	/// "EssentialsPluginDeviceTemplate" renamed to "SamsungMdcDevice"
	/// </example>
	public class LimitimerDevice : EssentialsDevice, IOnline
    {
		/// <summary>
		/// It is often desirable to store the config
		/// </summary>

		// private EssentialsPluginTemplateConfigObject _config;

		/// <summary>
		/// Provides a queue and dedicated worker thread for processing feedback messages from a device.
		/// </summary>
		private GenericQueue ReceiveQueue;

        #region IBasicCommunication Properties and Constructor.  Remove if not needed.

        // TODO [ ] Add, modify, remove properties and fields as needed for the plugin being developed
		private readonly IBasicCommunication _comms;
		private readonly GenericCommunicationMonitor _commsMonitor;

		// _comms gather for ASCII based API's
		// TODO [ ] If not using an ASCII based API, delete the properties below
		private readonly CommunicationGather _commsGather;

        /// <summary>
        /// Set this value to that of the delimiter used by the API (if applicable)
        /// </summary>
		private const string CommsDelimiter = "\r";

		/// <summary>
		/// The properties configuration object for the plugin device
		/// </summary>
		private readonly LimitimerPropertiesConfig _config;

		#region Private State Variables

		/// <summary>
		/// LED state variables
		/// </summary>
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

		/// <summary>
		/// Time string variables
		/// </summary>
		private string _totalTime;
		private string _sumUpTime;
		private string _remainingTime;

		#endregion

		/// <summary>
		/// Connects/disconnects the comms of the plugin device
		/// </summary>
		/// <remarks>
		/// triggers the _comms.Connect/Disconnect as well as thee comms monitor start/stop
		/// </remarks>
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

		/// <summary>
		/// LED state properties
		/// </summary>
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

		/// <summary>
		/// Time string properties
		/// </summary>
		public string TotalTime => _totalTime;
		public string SumUpTime => _sumUpTime;
		public string RemainingTime => _remainingTime;

		#endregion

        /// <summary>
        /// Plugin device constructor for devices that need IBasicCommunication
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="config"></param>
        /// <param name="comms"></param>
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

			ConnectFeedback = new BoolFeedback(key, () => Connect);
			OnlineFeedback = new BoolFeedback(key, () => _commsMonitor.IsOnline);
			StatusFeedback = new IntFeedback(key, () => (int)_commsMonitor.Status);

			// TODO [ ] no polling for this device - anything needed to change here?
			_comms = comms;
			_commsMonitor = new GenericCommunicationMonitor(this, _comms, _config.PollTimeMs, _config.WarningTimeoutMs, _config.ErrorTimeoutMs, Poll);

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

        // TODO [ ] If not using an ASCII based API with no delimeter, delete the method below
        void Handle_TextReceived(object sender, GenericCommMethodReceiveTextArgs e)
        {
            // TODO [ ] Implement method 
            throw new System.NotImplementedException();
        }

		/// <summary>
		/// This method should perform any necessary parsing of feedback messages from the device
		/// </summary>
		/// <param name="message"></param>
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
					// Check for time string patterns
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

		/// <summary>
		/// Event that fires when device state changes - can be used by messenger for notifications
		/// </summary>
		public event EventHandler StateChanged;

		/// <summary>
		/// Triggers the StateChanged event
		/// </summary>
		private void OnStateChanged()
		{
			StateChanged?.Invoke(this, EventArgs.Empty);
		}


		// TODO [ ] If not using an ACII based API, delete the properties below
		/// <summary>
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

