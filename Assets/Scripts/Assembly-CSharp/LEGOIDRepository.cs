using System.Text;

public class LEGOIDRepository
{
	public static string testLxfml = "<?xml version='1.0' encoding='UTF-8' standalone='no' ?>\r\n\t\t\t<LXFML versionMajor='5' versionMinor='0' name='FusionExample'>\r\n\t\t\t  <Meta>\r\n\t\t\t    <Application name='LEGO Digital Designer' versionMajor='5' versionMinor='0'/>\r\n\t\t\t    <Brand name='Factory'/>\r\n\t\t\t    <Environment name='Standard'/>\r\n\t\t\t    <BrickSet version='1264'/>\r\n\t\t\t    <ModelRepository experience='Fusion_Townmaster' scanDate='2014-02-25T14:27:26Z'/>\r\n\t\t\t  </Meta>\r\n\t\t\t  <Cameras>\r\n\t\t\t    <Camera refID='0' fieldOfView='80' distance='89.0337905884' transformation='0.684099376202,0,-0.729388833046,-0.449146002531,0.787914991379,-0.42125749588,0.574696421623,0.615784049034,0.539012134075,54.3673973083,55.3945884705,53.4752922058'/>\r\n\t\t\t  </Cameras>\r\n\t\t\t  <Bricks cameraRef='0'>\r\n\t\t\t    <Brick refID='0' designID='2412' itemNos='241226'>\r\n\t\t\t      <Part refID='0' designID='2412' materials='26'>\r\n\t\t\t        <Bone refID='0' transformation='1,0,0,0,1,0,0,0,1,-9.20000076294,0,-2'>         </Bone>\r\n\t\t\t      </Part>\r\n\t\t\t    </Brick>\r\n\t\t\t  </Bricks>\r\n\t\t\t  <RigidSystems>\r\n\t\t\t    <RigidSystem>\r\n\t\t\t      <Rigid refID='0' transformation='1,0,0,0,1,0,0,0,1,-9.20000076294,0,-2' boneRefs='0'/>\r\n\t\t\t    </RigidSystem>\r\n\t\t\t  </RigidSystems>\r\n\t\t\t  <GroupSystems>\r\n\t\t\t    <GroupSystem>     </GroupSystem>\r\n\t\t\t  </GroupSystems>\r\n\t\t\t  <BuildingInstructions>\r\n\t\t\t    <BuildingInstruction name='BuildingGuide1'>\r\n\t\t\t      <Camera cameraRef='0'/>\r\n\t\t\t      <Step name='Step1'>\r\n\t\t\t        <PartRef partRef='0'/>\r\n\t\t\t      </Step>\r\n\t\t\t    </BuildingInstruction>\r\n\t\t\t  </BuildingInstructions>\r\n\t\t\t</LXFML>";

	private static string devRepositoryAPIRoot = "http://services.modelrepository.dev.corp.lego.com/api";

	private static string repositoryModelUploadExtension = "/model";

	private static string repositoryModelDownloadExtension = "/model/";

	private static string repositoryPublicModelDownloadExtension = "/model/";

	private static string repositoryShareModelExtension = "/share/";

	private static string repositorySharedModelsExtension = "/sharedmodels/";

	private static string repositoryUserModelsExtension = "/usermodels/";

	private static string repositoryMyModelsExtension = "/mymodels/";

	private static string repositoryModeratedExtension = "/moderatedmodels/";

	private static string standardContentType = "application/json";

	public static void TestMethod()
	{
		StoreModel(devRepositoryAPIRoot, testLxfml);
	}

	public static void StoreModel(string repositoryApiUrl, string model)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(model);
		LEGOSDKFacade.Instance.GenericPostRequest(StripSlash(repositoryApiUrl) + repositoryModelUploadExtension, bytes);
	}

	public static void GetPrivateModel(string repositoryApiUrl, string modelID)
	{
		LEGOSDKFacade.Instance.GetRequest(StripSlash(repositoryApiUrl) + repositoryModelDownloadExtension + modelID, standardContentType);
	}

	public static void GetPublicModel(string repositoryApiUrl, string modelID, string userID)
	{
		LEGOSDKFacade.Instance.GetRequest(StripSlash(repositoryApiUrl) + repositoryPublicModelDownloadExtension + modelID + "/" + userID, standardContentType);
	}

	public static void ShareModel(string repositoryApiUrl, string modelID, string experience)
	{
		LEGOSDKFacade.Instance.GenericPostRequest(StripSlash(repositoryApiUrl) + repositoryShareModelExtension + modelID + "/" + experience, new byte[0]);
	}

	public static void GetMyModels(string repositoryApiUrl, string experience, string orderBy = null)
	{
		string text = StripSlash(repositoryApiUrl) + repositoryMyModelsExtension + experience;
		if (orderBy != null)
		{
			text = text + "?orderby=" + orderBy;
		}
		LEGOSDKFacade.Instance.GetRequest(text, "application/json");
	}

	public static void GetMyModelsPaged(string repositoryApiUrl, string experience, string reference, string orderBy = null)
	{
		string text = StripSlash(repositoryApiUrl) + repositoryMyModelsExtension + experience + "/" + reference;
		if (orderBy != null)
		{
			text = text + "?orderby=" + orderBy;
		}
		LEGOSDKFacade.Instance.GetRequest(text, standardContentType);
	}

	public static void GetMyModeratedModels(string repositoryApiUrl, string experience)
	{
		LEGOSDKFacade.Instance.GetRequest(StripSlash(repositoryApiUrl) + repositoryModeratedExtension + experience, standardContentType);
	}

	public static void GetSharedModels(string repositoryApiUrl, string experience, string orderBy = null)
	{
		string text = StripSlash(repositoryApiUrl) + repositorySharedModelsExtension + experience;
		if (orderBy != null)
		{
			text = text + "?orderby=" + orderBy;
		}
		LEGOSDKFacade.Instance.GetRequest(text, standardContentType);
	}

	public static void GetSharedModelsPaged(string repositoryApiUrl, string experience, string reference, string orderBy = null)
	{
		string text = StripSlash(repositoryApiUrl) + repositorySharedModelsExtension + experience + "/" + reference;
		if (orderBy != null)
		{
			text = text + "?orderby=" + orderBy;
		}
		LEGOSDKFacade.Instance.GetRequest(text, standardContentType);
	}

	public static void GetUserPublicModels(string repositoryApiUrl, string userID, string experience, string orderBy = null)
	{
		string text = StripSlash(repositoryApiUrl) + repositoryUserModelsExtension + userID + "/" + experience;
		if (orderBy != null)
		{
			text = text + "?orderby=" + orderBy;
		}
		LEGOSDKFacade.Instance.GetRequest(text, standardContentType);
	}

	public static void GetUserPublicModelsPaged(string repositoryApiUrl, string userID, string experience, string reference, string orderBy = null)
	{
		string text = StripSlash(repositoryApiUrl) + repositoryUserModelsExtension + userID + "/" + experience + "/" + reference;
		if (orderBy != null)
		{
			text = text + "?orderby=" + orderBy;
		}
		LEGOSDKFacade.Instance.GetRequest(text, standardContentType);
	}

	private static string StripSlash(string url)
	{
		url = url.Trim();
		if (url[url.Length - 1] == '\\' || url[url.Length - 1] == '/')
		{
			url = url.Substring(0, url.Length - 1);
		}
		return url;
	}
}
