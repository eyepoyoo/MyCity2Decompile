using System;
using System.IO;
using LEGO.CoreSDK.IO;
using LEGO.CoreSDK.Network;

namespace LEGO.CoreSDK
{
	// Token: 0x02000071 RID: 113
	internal class ApplicationConfigurationFactory : ConfigurationFactory<ApplicationConfiguration>
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x00007D6C File Offset: 0x00005F6C
		internal ApplicationConfigurationFactory(IFilesystem fileSystem, IApplication application, ILogger logger, IPersistentStorage persistentStorage, IMarketService marketService, INetwork network, IConfigurationCreator<ApplicationConfiguration> configurationCreator, IPaths paths)
			: base(fileSystem, application, logger, persistentStorage, marketService, network, configurationCreator)
		{
			this._paths = paths;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00007D94 File Offset: 0x00005F94
		public override string BackendCacheIDKey
		{
			get
			{
				return "ApplicationConfigurationBackendCacheIdKey";
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00007D9C File Offset: 0x00005F9C
		public override string LocaleKey
		{
			get
			{
				return "ApplicationConfigurationLocaleKey";
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00007DA4 File Offset: 0x00005FA4
		public override string CachePath
		{
			get
			{
				return Path.Combine(this._paths.CacheDirectory(), this.DefaultConfigurationName);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00007DBC File Offset: 0x00005FBC
		public override string InjectionPath
		{
			get
			{
				return Path.Combine(this._paths.InjectionDirectory(), this.DefaultConfigurationName);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00007DD4 File Offset: 0x00005FD4
		public override string DefaultConfigurationName
		{
			get
			{
				return "ApplicationConfigurationDefault.json";
			}
		}

		// Token: 0x040000EA RID: 234
		private IPaths _paths;
	}
}
