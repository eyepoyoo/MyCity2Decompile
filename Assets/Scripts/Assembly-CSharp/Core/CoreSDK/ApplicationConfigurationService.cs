using System;
using System.Runtime.InteropServices;

namespace LEGO.CoreSDK
{
	// Token: 0x0200006F RID: 111
	public class ApplicationConfigurationService : IApplicationConfigurationService
	{
		// Token: 0x06000194 RID: 404 RVA: 0x00007BC8 File Offset: 0x00005DC8
		internal ApplicationConfigurationService(IConfigurationFactory<ApplicationConfiguration> factory, ILogger logger)
		{
			this._factory = factory;
			this._logger = logger;
			ConfigurationContainer<ApplicationConfiguration> localModel = factory.GetLocalModel();
			this.Source = localModel.Source;
			this.ApplicationConfiguration = localModel.Model;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000195 RID: 405 RVA: 0x00007C10 File Offset: 0x00005E10
		// (remove) Token: 0x06000196 RID: 406 RVA: 0x00007C2C File Offset: 0x00005E2C
		public event ApplicationConfigurationChangedEventHandler ApplicationConfigurationChanged;

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00007C48 File Offset: 0x00005E48
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00007C50 File Offset: 0x00005E50
		public ConfigurationSource Source { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00007C5C File Offset: 0x00005E5C
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00007C64 File Offset: 0x00005E64
		public IApplicationConfiguration ApplicationConfiguration { get; private set; }

		// Token: 0x0600019B RID: 411 RVA: 0x00007C70 File Offset: 0x00005E70
		internal void DownloadApplicationConfiguration(string applicationConfigurationURL)
		{
			this._factory.Download(applicationConfigurationURL, true, delegate(ConfigurationContainer<ApplicationConfiguration?> configuration)
			{
				if (configuration.Model != null)
				{
					this.SetApplicationConfiguration(new ConfigurationContainer<ApplicationConfiguration>(configuration.Model.Value, configuration.Source));
				}
			});
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007C8C File Offset: 0x00005E8C
		private void SetApplicationConfiguration(ConfigurationContainer<ApplicationConfiguration> configuration)
		{
			if (this.Source == ConfigurationSource.Injected)
			{
				this._logger.Info("Ignoring application configuration as an injected one is already being used.");
				return;
			}
			this.ApplicationConfiguration = configuration.Model;
			this.Source = configuration.Source;
			this._logger.Debug("Application Configuration set to '" + this.Source + "'");
			if (this.ApplicationConfigurationChanged != null)
			{
				this.ApplicationConfigurationChanged(this.ApplicationConfiguration);
			}
		}

		// Token: 0x040000E4 RID: 228
		private IConfigurationFactory<ApplicationConfiguration> _factory;

		// Token: 0x040000E5 RID: 229
		private ILogger _logger;

		// Token: 0x02000070 RID: 112
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		internal struct Parameters
		{
			// Token: 0x0600019E RID: 414 RVA: 0x00007D4C File Offset: 0x00005F4C
			public Parameters(string applicationConfigurationURL)
			{
				this.ApplicationConfigurationURL = applicationConfigurationURL;
			}

			// Token: 0x17000035 RID: 53
			// (get) Token: 0x0600019F RID: 415 RVA: 0x00007D58 File Offset: 0x00005F58
			// (set) Token: 0x060001A0 RID: 416 RVA: 0x00007D60 File Offset: 0x00005F60
			public string ApplicationConfigurationURL { get; private set; }
		}
	}
}
