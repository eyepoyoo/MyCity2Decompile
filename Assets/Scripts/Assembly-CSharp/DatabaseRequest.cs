using System.Collections.Generic;

public class DatabaseRequest : DownloadRequest
{
	protected const string RAW_RESPONSE_KEY = "RawResponse";

	public List<FormDataEntry> responseData = new List<FormDataEntry>();

	public List<UserData> responseUserData = new List<UserData>();
}
