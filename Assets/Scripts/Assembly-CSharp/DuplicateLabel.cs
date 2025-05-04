using UnityEngine;

[ExecuteInEditMode]
public class DuplicateLabel : MonoBehaviour
{
	public UILabel sourceLabel;

	private UILabel _label;

	private UILabel _pLabel
	{
		get
		{
			if (_label == null)
			{
				_label = GetComponent<UILabel>();
			}
			return _label;
		}
	}

	private void Update()
	{
		if (sourceLabel != null && _pLabel != null)
		{
			_pLabel.text = sourceLabel.text;
			_pLabel.fontSize = sourceLabel.fontSize;
		}
	}
}
