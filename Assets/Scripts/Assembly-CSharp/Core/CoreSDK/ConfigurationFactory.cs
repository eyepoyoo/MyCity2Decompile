using System;
using System.Collections.Generic;
using System.Net;
using LEGO.CoreSDK.Extensions;
using LEGO.CoreSDK.Network;

namespace LEGO.CoreSDK
{
	// Token: 0x0200006D RID: 109
	public abstract class ConfigurationFactory<T> : IConfigurationFactory<T> where T : struct
	{
		// Token: 0x06000188 RID: 392 RVA: 0x000077C0 File Offset: 0x000059C0
		public ConfigurationFactory(IFilesystem fileSystem, IApplication application, ILogger logger, IKeyValueStore keyValueStore, IMarketService marketService, INetwork network, IConfigurationCreator<T> configurationCreator)
			: this(fileSystem, application, logger, (IPersistentStorage)keyValueStore, marketService, network, configurationCreator)
		{
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000077E4 File Offset: 0x000059E4
		public ConfigurationFactory(IFilesystem fileSystem, IApplication application, ILogger logger, IPersistentStorage persistentStorage, IMarketService marketService, INetwork network, IConfigurationCreator<T> configurationCreator)
		{
			this._fileSystem = fileSystem;
			this._application = application;
			this._logger = logger;
			this._persistentStorage = persistentStorage;
			this._marketService = marketService;
			this._network = network;
			this._configurationCreator = configurationCreator;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600018A RID: 394
		public abstract string BackendCacheIDKey { get; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600018B RID: 395
		public abstract string LocaleKey { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600018C RID: 396
		public abstract string CachePath { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600018D RID: 397
		public abstract string InjectionPath { get; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600018E RID: 398
		public abstract string DefaultConfigurationName { get; }

		// Token: 0x0600018F RID: 399 RVA: 0x00007824 File Offset: 0x00005A24
		public ConfigurationContainer<T> GetLocalModel()
		{
			string text = this._fileSystem.ReadAllTextOfBundledFile(this.DefaultConfigurationName);
			if (string.IsNullOrEmpty(text))
			{
				this._application.Quit();
				throw new ConfigurationNoBundledException("No bundled configuration was found. There needs to be an '" + this.DefaultConfigurationName + "' at the root level of a Resources folder.");
			}
			T t = this._configurationCreator.Create(text);
			ConfigurationSource configurationSource = ConfigurationSource.Bundled;
			if (this._fileSystem.FileExists(this.CachePath))
			{
				string text2 = this._fileSystem.FileReadAllText(this.CachePath);
				if (!string.IsNullOrEmpty(text2))
				{
					try
					{
						t = this._configurationCreator.Create(text2);
						configurationSource = ConfigurationSource.Cached;
					}
					catch (Exception ex)
					{
						this._logger.Error("Failed to parse cache configuration with error: '" + ex.Message + "'. Deleting it. ");
						this._fileSystem.FileDelete(this.CachePath);
					}
				}
			}
			if (this._fileSystem.FileExists(this.InjectionPath))
			{
				string text3 = this._fileSystem.FileReadAllText(this.InjectionPath);
				if (!string.IsNullOrEmpty(text3))
				{
					try
					{
						t = this._configurationCreator.Create(text3);
						configurationSource = ConfigurationSource.Injected;
					}
					catch (Exception ex2)
					{
						this._logger.Error("Failed to parse injected application configuration with error: '" + ex2.Message + "'. Ignoring it. ");
					}
				}
			}
			return new ConfigurationContainer<T>(t, configurationSource);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000079B0 File Offset: 0x00005BB0
		public INetworkRequest Download(string configurationURL, bool applyLocale, Action<ConfigurationContainer<T?>> completionHandler)
		{
			Asserts.ParameterNotNull(completionHandler, "completionHandler");
			string @string = this._persistentStorage.GetString(this.LocaleKey);
			if (this._marketService.ResolvedMarket.FullLocale != @string)
			{
				this._persistentStorage.SetString(this.BackendCacheIDKey, null);
			}
			Uri uri = ConfigurationFactory<T>.ValidateConfigurationURL(configurationURL, this._logger);
			if (applyLocale)
			{
				uri = ConfigurationFactory<T>.ApplyLocaleToConfigurationUri(uri, this._marketService.ResolvedMarket);
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string string2 = this._persistentStorage.GetString(this.BackendCacheIDKey);
			if (!string.IsNullOrEmpty(string2))
			{
				dictionary.Add(HTTPHeaderRequestField.IfModifiedSince.Identifier(), string2);
			}
			return this._network.GetJSONRequest(uri, null, dictionary, delegate(HttpStatusCode statusCode, string response, Dictionary<string, string> responseHeaders)
			{
				string text;
				if (responseHeaders != null && responseHeaders.TryGetValue(HTTPHeaderResponseField.LastModified.Identifier(), out text))
				{
					this._persistentStorage.SetString(this.BackendCacheIDKey, text);
				}
				if (statusCode == HttpStatusCode.NotModified)
				{
					this._logger.Debug("No new configuration was found on the remote. Using the existing configuration.");
					completionHandler(new ConfigurationContainer<T?>(null, ConfigurationSource.Downloaded));
					return;
				}
				if (string.IsNullOrEmpty(response))
				{
					this._logger.Warning("The configuration response did not contain a body and no error was reported from the backend. The HTTP status code was '" + statusCode + "'.");
					completionHandler(new ConfigurationContainer<T?>(null, ConfigurationSource.Downloaded));
					return;
				}
				T? t = null;
				try
				{
					t = new T?(this._configurationCreator.Create(response));
					this._fileSystem.FileWriteAllText(this.CachePath, response);
					this._persistentStorage.SetString(this.LocaleKey, this._marketService.ResolvedMarket.FullLocale);
				}
				catch (ConfigurationParsingException ex)
				{
					this._logger.Warning("Failed to parse downloaded configuration with error: " + ex.Message + ". Downloaded configuration: " + response);
				}
				completionHandler(new ConfigurationContainer<T?>(t, ConfigurationSource.Downloaded));
			}, delegate(HttpStatusCode statusCode, string errorMessage)
			{
				this._logger.Warning(string.Concat(new object[]
				{
					"Failed to download a configuration with status code: '",
					statusCode,
					"'",
					(!string.IsNullOrEmpty(errorMessage)) ? (". Error message: '" + errorMessage + "'.") : string.Empty
				}));
				completionHandler(new ConfigurationContainer<T?>(null, ConfigurationSource.Downloaded));
			}, null, true);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007AA8 File Offset: 0x00005CA8
		private static Uri ApplyLocaleToConfigurationUri(Uri configurationUri, Locale locale)
		{
			string text = configurationUri.AbsoluteUri.Replace("lego.com/", "lego.com/" + locale.SiteCoreLocale + "/");
			return new Uri(text);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00007AE4 File Offset: 0x00005CE4
		private static Uri ValidateConfigurationURL(string configurationURL, ILogger logger)
		{
			Uri uri;
			try
			{
				uri = new Uri(configurationURL);
			}
			catch (UriFormatException ex)
			{
				throw new UriFormatException(string.Concat(new string[] { "The configuration URL '", configurationURL, "' is invalid, please ensure that you have entered the correct URL. Reported error: '", ex.Message, "'." }));
			}
			catch (ArgumentNullException)
			{
				throw new ArgumentNullException("configurationURL");
			}
			if (!uri.IsValidNetworkURL())
			{
				throw new UriFormatException("The configuration URL '" + configurationURL + "' is invalid, please ensure that you have entered the correct URL.");
			}
			if (!uri.IsSecure())
			{
				throw new UriFormatException("The configuration URL can only be used with the HTTPS protocol.");
			}
			return uri;
		}

		// Token: 0x040000DB RID: 219
		private IFilesystem _fileSystem;

		// Token: 0x040000DC RID: 220
		private IApplication _application;

		// Token: 0x040000DD RID: 221
		private ILogger _logger;

		// Token: 0x040000DE RID: 222
		private IPersistentStorage _persistentStorage;

		// Token: 0x040000DF RID: 223
		private IMarketService _marketService;

		// Token: 0x040000E0 RID: 224
		private INetwork _network;

		// Token: 0x040000E1 RID: 225
		private IConfigurationCreator<T> _configurationCreator;
	}
}
