using DeepThought;
using UnityEngine;

public class UserAccountFacade : Neuron
{
	public UserAccountFacade(Neuron parent, CreationParameters parameters)
		: base(parent, parameters)
	{
		Facades<UserAccountFacade>.Register(this);
	}

	public void ShowUserNotification(string title, string message, string okButton, UserNotificationCallback okCallback)
	{
		ShowUserNotificationImpl("ShowSingleButtonDialog", title, message, okButton, okCallback);
	}

	public void ShowUserNotification(string title, string message, string okButton, UserNotificationCallback okCallback, string cancelButton, UserNotificationCallback cancelCallback)
	{
		ShowUserNotificationImpl("ShowDualButtonDialog", title, message, okButton, okCallback, cancelButton, cancelCallback);
	}

	private void ShowUserNotificationImpl(string jsScriptName, string title, string message, string acceptButton, UserNotificationCallback acceptCallback, string refuseButton = null, UserNotificationCallback refuseCallback = null)
	{
		ExternalMessageHandler.Instance.userNotificationCallback_accept.callback = acceptCallback;
		ExternalMessageHandler.Instance.userNotificationCallback_accept.name = Facades<LocalisationFacade>.Instance.GetString(acceptButton);
		if (refuseCallback == null || refuseButton == null)
		{
			ExternalMessageHandler.Instance.userNotificationCallback_refuse.callback = null;
			ExternalMessageHandler.Instance.userNotificationCallback_refuse.name = null;
			BeginExternalUIAction(string.Empty, Facades<LocalisationFacade>.Instance.GetString(title), Facades<LocalisationFacade>.Instance.GetString(message), Facades<LocalisationFacade>.Instance.GetString(acceptButton));
		}
		else
		{
			ExternalMessageHandler.Instance.userNotificationCallback_refuse.callback = refuseCallback;
			ExternalMessageHandler.Instance.userNotificationCallback_refuse.name = Facades<LocalisationFacade>.Instance.GetString(refuseButton);
			BeginExternalUIAction(string.Empty, Facades<LocalisationFacade>.Instance.GetString(title), Facades<LocalisationFacade>.Instance.GetString(message), Facades<LocalisationFacade>.Instance.GetString(acceptButton), Facades<LocalisationFacade>.Instance.GetString(refuseButton));
		}
	}

	public static void BeginExternalUIAction(string fn, params object[] args)
	{
		bool flag = false;
		if (true)
		{
			Debug.Log("Skipping dialog on unsupported platform");
		}
		else
		{
			ExternalMessageHandler.ExternalCall(fn, args);
		}
	}

	public static void EndExternalUIAction()
	{
		Debug.Log("Re-enabling UI interactivity");
	}
}
