using System.Collections.Generic;
using Newtonsoft.Json;
using PepperDash.Essentials.Core;

namespace PepperDash.Essentials.Plugins.Limitimer
{
	public class LimitimerPropertiesConfig
	{
		[JsonProperty("control")]
		public EssentialsControlPropertiesConfig Control { get; set; }

		[JsonProperty("pollTimeMs")]
		public long PollTimeMs { get; set; }

		[JsonProperty("warningTimeoutMs")]
		public long WarningTimeoutMs { get; set; }

		[JsonProperty("errorTimeoutMs")]
		public long ErrorTimeoutMs { get; set; }
	}
}