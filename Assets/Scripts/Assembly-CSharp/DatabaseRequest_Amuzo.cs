using System.Collections.Generic;

public class DatabaseRequest_Amuzo : DatabaseRequest
{
	private const char ENTRY_DELIMITER = '|';

	private const char VARIABLE_DELIMITER = '#';

	private const char KEY_VALUE_DELIMITER = '=';

	public bool doDecryptResponse = true;

	public List<FormDataEntry[]> decryptedResponseVariables;

	private string commandString;

	private int _encryptionIndex;

	private string[] encryptedResponseEntries;

	protected override void preProcessRequest()
	{
		int i = 0;
		for (int count = formData.Count; i < count; i++)
		{
			if (!(formData[i].key != "command"))
			{
				commandString = formData[i].value;
				break;
			}
		}
		new FormDataEncryption(this, onFormDataEncrypted);
	}

	private void onFormDataEncrypted(DownloadRequest authRequest)
	{
		onPreProcessComplete();
	}

	protected override void postProcessResponse()
	{
		if (!base._pHasResponse)
		{
			onPostProcessComplete();
		}
		else if (!doDecryptResponse)
		{
			onDecryptionComplete(base._pResponseString);
		}
		else
		{
			BasicEncryption.Instance.decrypt(base._pResponseString, onDecryptionComplete);
		}
	}

	private void onDecryptionComplete(string decrypted)
	{
		Log("Download final response [" + decrypted + "]");
		_encryptionIndex = 0;
		responseData.Add(new FormDataEntry("RawResponse", decrypted));
		encryptedResponseEntries = decrypted.Split('|');
		decryptedResponseVariables = new List<FormDataEntry[]>();
		parseNextEntry();
	}

	private void parseNextEntry()
	{
		if (!string.IsNullOrEmpty(encryptedResponseEntries[_encryptionIndex]))
		{
			string[] array = encryptedResponseEntries[_encryptionIndex].Split('#');
			FormDataEntry[] array2 = new FormDataEntry[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				string[] array3 = array[i].Split('=');
				if (array3.Length == 2)
				{
					array2[i] = new FormDataEntry(array3[0], array3[1]);
				}
			}
			decryptedResponseVariables.Add(array2);
		}
		_encryptionIndex++;
		if (_encryptionIndex < encryptedResponseEntries.Length)
		{
			parseNextEntry();
			return;
		}
		switch (commandString)
		{
		case "init":
			parseNewToken();
			break;
		case "saveUser":
			parseSaveUser();
			break;
		case "getLeaderboard":
			parseHighScores();
			break;
		default:
			Log("AMUZO: Command not found. Unable to parse.");
			break;
		}
		onPostProcessComplete();
	}

	private void parseNewToken()
	{
	}

	private void parseSaveUser()
	{
		DatabaseFacade.Instance._databases[DatabaseType.AMUZO]._userUniqueId = Database_Amuzo.findIdIn(decryptedResponseVariables, "deviceId");
	}

	private void parseHighScores()
	{
		int i = 0;
		for (int count = decryptedResponseVariables.Count; i < count; i++)
		{
			UserData userData = new UserData();
			userData._timestamp = TimeManager.GetCurrentTime().Ticks;
			FormDataEntry[] array = decryptedResponseVariables[i];
			if (array == null || array.Length == 0)
			{
				continue;
			}
			int j = 0;
			for (int num = array.Length; j < num; j++)
			{
				if (array[j] != null)
				{
					switch (array[j].key)
					{
					case "deviceId":
						userData._uniqueId = array[j].value;
						break;
					case "name":
						userData._name = array[j].value;
						break;
					case "position":
						userData.addData("position", array[j].value);
						break;
					case "score":
						userData.addData("score", array[j].value);
						break;
					case "challenges":
						userData.addData("challenges", array[j].value);
						break;
					default:
						userData.addData(array[j].key, array[j].value);
						break;
					}
				}
			}
			responseUserData.Add(userData);
		}
	}
}
