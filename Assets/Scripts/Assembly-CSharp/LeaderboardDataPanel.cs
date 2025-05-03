using UnityEngine;

public class LeaderboardDataPanel : MonoBehaviour
{
	public UILabel _playernameLabel;

	public UILabel _positionLabel;

	public UILabel _totalLabel;

	public UILabel _playernameShadow;

	public UILabel _positionShadow;

	public UILabel _totalShadow;

	public void SetPositionData(LeaderboardEntry entry)
	{
		base.gameObject.SetActive(true);
		setText(_positionLabel, entry._position.ToString(), _positionShadow);
		if ((entry._name == LocalisationFacade.Instance.GetString("General.DefaultName") || entry._name == "No Default Name") && LEGOID._pInstance != null && !string.IsNullOrEmpty(LEGOID._pInstance._pLogInName))
		{
			setText(_playernameLabel, LEGOID._pInstance._pLogInName, _playernameShadow);
		}
		else
		{
			setText(_playernameLabel, entry._name, _playernameShadow);
		}
		setText(_totalLabel, entry.getData("score"), _totalShadow);
	}

	public void hidePanel()
	{
		base.gameObject.SetActive(false);
	}

	private void setText(UILabel label, string text, UILabel shadow)
	{
		if (!(label == null))
		{
			label.text = ((text != null) ? text : string.Empty);
			if (!(shadow == null))
			{
				shadow.text = ((text != null) ? text : string.Empty);
			}
		}
	}
}
