using PepperDash.Essentials.Core;

namespace PepperDash.Essentials.Plugins
{
	/// <summary>
	/// Plugin device Bridge Join Map
	/// </summary>
	/// <remarks>
	/// Rename the class to match the device plugin being developed.  Reference Essentials JoinMaps, if one exists for the device plugin being developed
	/// </remarks>
	/// <see cref="PepperDash.Essentials.Core.Bridges"/>
	/// <example>
	/// "EssentialsPluginBridgeJoinMapTemplate" renamed to "SamsungMdcBridgeJoinMap"
	/// </example>
	public class LimitimerBridgeJoinMap : JoinMapBaseAdvanced
	{
		#region Digital

		[JoinName("IsOnline")]
		public JoinDataComplete IsOnline = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 1,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Is Online",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("Program1")]
		public JoinDataComplete Program1 = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 11,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Program 1 Press / Program 1 LED On Feedback",
				JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("Program1LedDim")]
		public JoinDataComplete Program1LedDim = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 12,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Program 1 LED Dim Feedback",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("Program2")]
		public JoinDataComplete Program2 = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 13,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Program 2 Press / Program 2 LED On Feedback",
				JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("Program2LedDim")]
		public JoinDataComplete Program2LedDim = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 14,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Program 2 LED Dim Feedback",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Digital
			});



		[JoinName("Program3")]
		public JoinDataComplete Program3 = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 15,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Program 3 Press / Program 3 LED On Feedback",
				JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("Program3LedDim")]
		public JoinDataComplete Program3LedDim = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 16,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Program 3 LED Dim Feedback",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("Session")]
		public JoinDataComplete Session = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 17,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Session Press / Session LED On Feedback",
				JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("SessionLedDim")]
		public JoinDataComplete SessionLedDim = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 18,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Session LED Dim Feedback",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("Beep")]
		public JoinDataComplete Beep = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 21,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Beep Press / Beep LED On Feedback",
				JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("Blink")]
		public JoinDataComplete Blink = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 22,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Blink Press / Blink LED On Feedback",
				JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("SecondsMode")]
		public JoinDataComplete SecondsMode = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 23,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Seconds Mode Press / Seconds Mode Indicator Feedback",
				JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("GreenLed")]
		public JoinDataComplete GreenLed = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 24,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Green LED On Feedback",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("RedLed")]
		public JoinDataComplete RedLed = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 25,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Red LED On Feedback",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("YellowLed")]
		public JoinDataComplete YellowLed = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 26,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Yellow LED On Feedback",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("StartStop")]
		public JoinDataComplete StartStop = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 27,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Start/Stop Press",
				JoinCapabilities = eJoinCapabilities.FromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("Repeat")]
		public JoinDataComplete Repeat = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 28,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Repeat Press",
				JoinCapabilities = eJoinCapabilities.FromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("Clear")]
		public JoinDataComplete Clear = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 29,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Clear Press",
				JoinCapabilities = eJoinCapabilities.FromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("TotalTimePlus")]
		public JoinDataComplete TotalTimePlus = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 30,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Total Time Plus Press",
				JoinCapabilities = eJoinCapabilities.FromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("TotalTimeMinus")]
		public JoinDataComplete TotalTimeMinus = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 31,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Total Time Minus Press",
				JoinCapabilities = eJoinCapabilities.FromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("SumTimePlus")]
		public JoinDataComplete SumTimePlus = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 32,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Sum Time Plus Press",
				JoinCapabilities = eJoinCapabilities.FromSIMPL,
				JoinType = eJoinType.Digital
			});

		[JoinName("SumTimeMinus")]
		public JoinDataComplete SumTimeMinus = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 33,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Sum Time Minus Press",
				JoinCapabilities = eJoinCapabilities.FromSIMPL,
				JoinType = eJoinType.Digital
			});

		#endregion


		#region Analog

		[JoinName("Status")]
		public JoinDataComplete Status = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 1,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Socket Status (0=IsOk, 1=CompromisedCommunication, 2=CommunicationError)",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Analog
			});

		[JoinName("Program1LedState")]
		public JoinDataComplete Program1LedState = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 2,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Program 1 LED State (0=off, 1=on, 2=dim)",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Analog
			});

		[JoinName("Program2LedState")]
		public JoinDataComplete Program2LedState = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 3,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Program 2 LED State (0=off, 1=on, 2=dim)",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Analog
			});

		[JoinName("Program3LedState")]
		public JoinDataComplete Program3LedState = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 4,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Program 3 LED State (0=off, 1=on, 2=dim)",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Analog
			});

		[JoinName("SessionLedState")]
		public JoinDataComplete SessionLedState = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 5,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Session LED State (0=off, 1=on, 2=dim)",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Analog
			});

		#endregion


		#region Serial

		[JoinName("DeviceName")]
		public JoinDataComplete DeviceName = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 1,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Device Name",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Serial
			});

		[JoinName("TotalTime")]
		public JoinDataComplete TotalTime = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 2,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Total Time String (MM:SS format)",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Serial
			});

		[JoinName("SumUpTime")]
		public JoinDataComplete SumUpTime = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 3,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Sum-Up Time String (MM:SS format)",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Serial
			});

		[JoinName("RemainingTime")]
		public JoinDataComplete RemainingTime = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 4,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Remaining Time String (MM:SS format)",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Serial
			});

		[JoinName("RemainingTimeSZ")]
		public JoinDataComplete RemainingTimeSZ = new JoinDataComplete(
			new JoinData
			{
				JoinNumber = 5,
				JoinSpan = 1
			},
			new JoinMetadata
			{
				Description = "Remaining Time String Stop at Zero (MM:SS format)",
				JoinCapabilities = eJoinCapabilities.ToSIMPL,
				JoinType = eJoinType.Serial
			});

		#endregion

		/// <summary>
		/// Plugin device BridgeJoinMap constructor
		/// </summary>
		/// <param name="joinStart">This will be the join it starts on the EISC bridge</param>
        public LimitimerBridgeJoinMap(uint joinStart)
            : base(joinStart, typeof(LimitimerBridgeJoinMap))
		{
		}
	}
}