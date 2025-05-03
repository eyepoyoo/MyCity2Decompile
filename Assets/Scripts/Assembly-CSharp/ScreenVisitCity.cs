using UnityEngine;

public class ScreenVisitCity : ScreenBase
{
	private const float SPACEX = 720f;

	private const float SPACEY = 151f;

	public UIPanel backingPanel;

	public GameObject templateObject;

	private int _numUsers;

	private string[] _userNameList;

	private GameObject[] _objectList;

	public void OnCancel()
	{
		TryChangeWidgetSets(base.gameObject, "NoFade");
		Navigate("OnCancel");
	}

	public void GetUserList()
	{
		_userNameList = new string[10] { "Sir Cuthbert", "Eades9000", "Hope42", "Earl Earlington", "Boylaaaaan", "McFahy83", "Lewis1", "Miss Peers", "Sibbick552", "Iglelelele" };
		_numUsers = _userNameList.Length;
	}

	protected override void Update()
	{
		base.Update();
		if (Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			backingPanel.SetDirty();
		}
	}

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		CameraHUB._pInstance._pCameraControllable = false;
		GenerateScreen();
	}

	protected override void OnScreenExitComplete()
	{
		base.OnScreenExitComplete();
		if (_objectList == null)
		{
			return;
		}
		int num = _objectList.Length;
		for (int i = 0; i < num; i++)
		{
			if (!(_objectList[i] == null))
			{
				Object.Destroy(_objectList[i]);
			}
		}
	}

	private void GenerateScreen()
	{
		GetUserList();
		_objectList = new GameObject[_numUsers];
		templateObject.SetActive(true);
		Vector3 localPosition = templateObject.transform.localPosition;
		for (int i = 0; i < _numUsers; i++)
		{
			GameObject gameObject = Object.Instantiate(templateObject);
			gameObject.transform.parent = templateObject.transform.parent;
			gameObject.transform.localPosition = localPosition;
			gameObject.transform.localScale = templateObject.transform.localScale;
			if (i % 2 == 0)
			{
				localPosition.x += 720f;
			}
			else
			{
				localPosition.x = templateObject.transform.localPosition.x;
				localPosition.y -= 151f;
			}
			gameObject.name = _userNameList[i];
			_objectList[i] = gameObject;
			UILabel componentInChildren = gameObject.GetComponentInChildren<UILabel>();
			componentInChildren.text = _userNameList[i];
		}
		templateObject.SetActive(false);
	}

	public void OnChosenUser(GameObject source)
	{
		TryChangeWidgetSets(base.gameObject, "Default");
		GlobalInGameData._pCurrentSocialCity = source.name;
		Navigate("OnVisitCity");
	}
}
