using System.Collections.Generic;

public class DLCAsset : DownloadRequest
{
	private const char ASSET_TAG_DELIMITER = ',';

	public List<string> assetTags;

	public AssetType assetType;

	public DLCAsset(string assetKey, string downloadURL, string tagList, string assetTypeString, string downloadSizeString)
	{
		requestName = assetKey;
		base.requestUrl = downloadURL;
		if (tagList != null && tagList.Length > 0)
		{
			string[] array = tagList.Split(',');
			assetTags = new List<string>();
			for (int i = 0; i < array.Length; i++)
			{
				assetTags.Add(array[i]);
			}
		}
		else
		{
			assetTags = new List<string>(0);
		}
		int.TryParse(downloadSizeString, out downloadSize);
		switch (assetTypeString)
		{
		case "IMAGE":
			assetType = AssetType.IMAGE;
			break;
		case "VIDEO":
			assetType = AssetType.VIDEO;
			break;
		case "ITEM_BUNDLE":
			assetType = AssetType.ITEM_BUNDLE;
			break;
		case "LEVEL_BUNDLE":
			assetType = AssetType.LEVEL_BUNDLE;
			break;
		case "IMAGE_ATLAS":
			assetType = AssetType.TEXTURE_ATLAS;
			break;
		case "IMAGE_TEXTURE":
			assetType = AssetType.TEXTURE_ATLAS_TEXTURE;
			break;
		default:
			assetType = AssetType.TEXT;
			break;
		}
	}
}
