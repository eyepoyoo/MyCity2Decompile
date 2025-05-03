using System;

namespace LEGO.CoreSDK
{
	// Token: 0x02000079 RID: 121
	internal class ApplicationConfigurationGuard
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x000082CC File Offset: 0x000064CC
		internal ApplicationConfigurationGuard(IApplicationConfigurationService applicationConfigurationService, IApplication application, ILogger logger, IAlert alert)
		{
			this._application = application;
			this._logger = logger;
			this._alert = alert;
			this.GuardApplicationConfiguration(applicationConfigurationService.ApplicationConfiguration);
			applicationConfigurationService.ApplicationConfigurationChanged += this.GuardApplicationConfiguration;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00008314 File Offset: 0x00006514
		private void GuardApplicationConfiguration(IApplicationConfiguration configuration)
		{
			ApplicationConfigurationGuard.State state = ApplicationConfigurationGuard.StateFromConfiguration(configuration, this._application.Version());
			if (state == ApplicationConfigurationGuard.State.Ok)
			{
				return;
			}
			ApplicationConfigurationGuard.Dialog dialog = ApplicationConfigurationGuard.CreateDialogFromState(state, configuration);
			this._logger.Debug("Showing guard for state: " + state);
			this._alert.Show(dialog.Title, dialog.Message);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008378 File Offset: 0x00006578
		private static ApplicationConfigurationGuard.State StateFromConfiguration(IApplicationConfiguration applicationConfiguration, Version appVersion)
		{
			if (applicationConfiguration.KillSwitch)
			{
				return ApplicationConfigurationGuard.State.ShowKillSwitch;
			}
			if (applicationConfiguration.MinimumVersion != null && appVersion.CompareTo(applicationConfiguration.MinimumVersion) < 0)
			{
				return ApplicationConfigurationGuard.State.ShowTooOldVersion;
			}
			return ApplicationConfigurationGuard.State.Ok;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000083B8 File Offset: 0x000065B8
		private static ApplicationConfigurationGuard.Dialog CreateDialogFromState(ApplicationConfigurationGuard.State state, IApplicationConfiguration applicationConfiguration)
		{
			switch (state)
			{
			case ApplicationConfigurationGuard.State.ShowKillSwitch:
				return new ApplicationConfigurationGuard.Dialog(applicationConfiguration.KillSwitchTitle, applicationConfiguration.KillSwitchMessage);
			case ApplicationConfigurationGuard.State.ShowTooOldVersion:
				return new ApplicationConfigurationGuard.Dialog(applicationConfiguration.MinimumVersionTitle, applicationConfiguration.MinimumVersionMessage);
			case ApplicationConfigurationGuard.State.Ok:
				throw new Exception("Cannot create a dialog from the ok state.");
			default:
				throw new Exception("Unknown state received: '" + state + "'");
			}
		}

		// Token: 0x040000F7 RID: 247
		private IApplication _application;

		// Token: 0x040000F8 RID: 248
		private ILogger _logger;

		// Token: 0x040000F9 RID: 249
		private IAlert _alert;

		// Token: 0x0200007A RID: 122
		internal enum State
		{
			// Token: 0x040000FB RID: 251
			ShowKillSwitch,
			// Token: 0x040000FC RID: 252
			ShowTooOldVersion,
			// Token: 0x040000FD RID: 253
			Ok
		}

		// Token: 0x0200007B RID: 123
		public struct Dialog
		{
			// Token: 0x060001EC RID: 492 RVA: 0x00008428 File Offset: 0x00006628
			public Dialog(string title, string message)
			{
				this.Title = title;
				this.Message = message;
			}

			// Token: 0x040000FE RID: 254
			public readonly string Title;

			// Token: 0x040000FF RID: 255
			public readonly string Message;
		}
	}
}
