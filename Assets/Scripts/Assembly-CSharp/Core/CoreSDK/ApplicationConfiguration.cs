using System;
using System.Collections.Generic;
using System.Linq;

namespace LEGO.CoreSDK
{
	// Token: 0x02000073 RID: 115
	public struct ApplicationConfiguration : IApplicationConfiguration
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x00007DEC File Offset: 0x00005FEC
		public ApplicationConfiguration(string jsonString)
		{
			this.RawJson = new JSONObject(jsonString, -2, false, false);
			if (this.RawJson == null || this.RawJson.type == JSONObject.Type.NULL)
			{
				throw new ConfigurationParsingException("Received invalid JSON to application configuration: " + jsonString);
			}
			this.Name = this.RawJson.StringForKey("configuration.@name", true);
			if (string.IsNullOrEmpty(this.Name))
			{
				throw new ConfigurationParsingException("Failed to find required field 'configuration.@name' in application configuration JSON: " + jsonString);
			}
			if (this.RawJson.StringForKey("configuration.killswitch", true) == "1")
			{
				this.KillSwitch = true;
			}
			else
			{
				this.KillSwitch = false;
			}
			this.ContentLanguage = this.RawJson.StringForKey("configuration.@contentlanguage", true);
			if (this.ContentLanguage == null)
			{
				throw new ConfigurationParsingException("Failed to find or parse required field 'configuration.@contentlanguage' in application configuration JSON: " + jsonString);
			}
			this.Version = this.RawJson.VersionForKey("configuration.@version");
			if (this.Version == null)
			{
				throw new ConfigurationParsingException("Failed to find or parse required field 'configuration.@version' in application configuration JSON: " + jsonString);
			}
			this.Experience = this.RawJson.StringForKey("configuration.experience", true);
			if (this.Experience == null)
			{
				throw new ConfigurationParsingException("Failed to find or parse required field 'configuration.experience' in application configuration JSON: " + jsonString);
			}
			this.MinimumVersion = this.RawJson.VersionForKey("configuration.minimumversion");
			if (this.MinimumVersion == null)
			{
				throw new ConfigurationParsingException("Failed to find or parse required field 'configuration.minimumversion' in application configuration JSON: " + jsonString);
			}
			if (this.RawJson.StringForKey("configuration.tracking", true) == "1")
			{
				this.Tracking = true;
			}
			else
			{
				this.Tracking = false;
			}
			this.CustomValues = this.RawJson.DictionaryForKey("configuration.customvalues");
			this.Endpoints = this.RawJson.DictionaryForKey("configuration.endpoints");
			this.Assets = this.RawJson.DictionaryForKey("configuration.assets");
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007FEC File Offset: 0x000061EC
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00007FF4 File Offset: 0x000061F4
		public string Name { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00008000 File Offset: 0x00006200
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00008008 File Offset: 0x00006208
		public bool KillSwitch { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00008014 File Offset: 0x00006214
		// (set) Token: 0x060001AF RID: 431 RVA: 0x0000801C File Offset: 0x0000621C
		public string ContentLanguage { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00008028 File Offset: 0x00006228
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00008030 File Offset: 0x00006230
		public Version Version { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000803C File Offset: 0x0000623C
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00008044 File Offset: 0x00006244
		public string Experience { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00008050 File Offset: 0x00006250
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00008058 File Offset: 0x00006258
		public Version MinimumVersion { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00008064 File Offset: 0x00006264
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x0000806C File Offset: 0x0000626C
		public bool Tracking { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00008078 File Offset: 0x00006278
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00008080 File Offset: 0x00006280
		public IDictionary<string, string> CustomValues { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000808C File Offset: 0x0000628C
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00008094 File Offset: 0x00006294
		public IDictionary<string, string> Endpoints { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000080A0 File Offset: 0x000062A0
		// (set) Token: 0x060001BD RID: 445 RVA: 0x000080A8 File Offset: 0x000062A8
		public IDictionary<string, string> Assets { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001BE RID: 446 RVA: 0x000080B4 File Offset: 0x000062B4
		// (set) Token: 0x060001BF RID: 447 RVA: 0x000080BC File Offset: 0x000062BC
		public JSONObject RawJson { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x000080C8 File Offset: 0x000062C8
		public string KillSwitchTitle
		{
			get
			{
				return this.RawJson.StringForKey("configuration.commonconfigurations.appdialogue.customvalues.warning", true);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x000080DC File Offset: 0x000062DC
		public string KillSwitchMessage
		{
			get
			{
				return this.RawJson.StringForKey("configuration.commonconfigurations.appdialogue.customvalues.killswitchtext", true);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x000080F0 File Offset: 0x000062F0
		public string MinimumVersionTitle
		{
			get
			{
				return this.RawJson.StringForKey("configuration.commonconfigurations.appdialogue.customvalues.warning", true);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00008104 File Offset: 0x00006304
		public string MinimumVersionMessage
		{
			get
			{
				return this.RawJson.StringForKey("configuration.commonconfigurations.appdialogue.customvalues.minimumversiontext", true);
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008118 File Offset: 0x00006318
		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			ApplicationConfiguration applicationConfiguration = (ApplicationConfiguration)obj;
			return this.Name == applicationConfiguration.Name && this.KillSwitch == applicationConfiguration.KillSwitch && this.ContentLanguage == applicationConfiguration.ContentLanguage && this.Version == applicationConfiguration.Version && this.Experience == applicationConfiguration.Experience && this.MinimumVersion == applicationConfiguration.MinimumVersion && this.Tracking == applicationConfiguration.Tracking && this.CustomValues.SequenceEqual(applicationConfiguration.CustomValues) && this.Endpoints.SequenceEqual(applicationConfiguration.Endpoints) && this.Assets.SequenceEqual(applicationConfiguration.Assets) && this.RawJson.ToString() == applicationConfiguration.RawJson.ToString();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000824C File Offset: 0x0000644C
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008260 File Offset: 0x00006460
		public override string ToString()
		{
			return this.RawJson.ToString();
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00008270 File Offset: 0x00006470
		public static bool operator ==(ApplicationConfiguration x, ApplicationConfiguration y)
		{
			return x.Equals(y);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00008280 File Offset: 0x00006480
		public static bool operator !=(ApplicationConfiguration x, ApplicationConfiguration y)
		{
			return !x.Equals(y);
		}
	}
}
