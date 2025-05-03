using System;
using LEGO.CoreSDK.IO;
using LEGO.CoreSDK.IO.Unity;
using LEGO.CoreSDK.Legal;
using LEGO.CoreSDK.Legal.Unity;
using LEGO.CoreSDK.Native;
using LEGO.CoreSDK.Native.Unity;
using LEGO.CoreSDK.Network;
using LEGO.CoreSDK.Network.Unity;
using LEGO.CoreSDK.Unity;
using LEGO.CoreSDK.VPC;
using LEGO.CoreSDK.VPC.Unity;
using UnityEngine;

namespace LEGO.CoreSDK.DependencyInjection.Unity
{
	// Token: 0x02000060 RID: 96
	internal static class UnityInstaller
	{
		// Token: 0x0600016A RID: 362 RVA: 0x000075A0 File Offset: 0x000057A0
		public static void RegisterDependencies(IDependencyResolver dependencyResolver)
		{
			dependencyResolver.Register<ILogger, Log>(Lifecycle.Singleton);
			dependencyResolver.Register<INetwork, WWWNetwork>(Lifecycle.Transient);
			dependencyResolver.Register<IPaths, Paths>(Lifecycle.Transient);
			dependencyResolver.Register<IKeyValueStore, PersistentStorage>(Lifecycle.Transient);
			dependencyResolver.Register<IPersistentStorage, PersistentStorage>(Lifecycle.Transient);
			dependencyResolver.Register<IApplication, LEGO.CoreSDK.Unity.Application>(Lifecycle.Transient);
			dependencyResolver.Register<IAlert, Alert>(Lifecycle.Transient);
			dependencyResolver.Register<IPolicyView, PolicyView>(Lifecycle.Transient);
			dependencyResolver.Register<IParentalGateView, ParentalGateView>(Lifecycle.Transient);
			dependencyResolver.Register<IFilesystem, Filesystem>(Lifecycle.Transient);
			RuntimePlatform platform = global::UnityEngine.Application.platform;
			switch (platform)
			{
			case RuntimePlatform.WindowsEditor:
				dependencyResolver.Register<INativeAPI, NativeWindowsAPI>(Lifecycle.Transient);
				break;
			case RuntimePlatform.IPhonePlayer:
				dependencyResolver.Register<INativeAPI, NativeIOSAPI>(Lifecycle.Transient);
				break;
			default:
				if (platform != RuntimePlatform.OSXEditor)
				{
					throw new NotImplementedException();
				}
				dependencyResolver.Register<INativeAPI, NativeMacAPI>(Lifecycle.Transient);
				break;
			case RuntimePlatform.Android:
			{
				INativeAPI nativeAPI = (INativeAPI)NativeUtils.CreateType("LEGO.CoreSDK.Native.Unity.NativeAndroidAPI", "LEGOCore_Android");
				dependencyResolver.RegisterInstance<INativeAPI>(nativeAPI);
				break;
			}
			}
		}
	}
}
