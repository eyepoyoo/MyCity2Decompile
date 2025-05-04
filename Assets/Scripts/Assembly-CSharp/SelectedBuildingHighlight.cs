using UnityEngine;

public class SelectedBuildingHighlight : MonoBehaviour
{
	private const float SINK_OFFSET = 12.5f;

	private const float INTRO_DUR = 0.5f;

	private static SelectedBuildingHighlight _lastSelected;

	public MeshRenderer[] walls;

	private float _enableTime;

	private bool _tweenOut;

	private float _tweenOutTime;

	private Vector3 _rootPos;

	public static SelectedBuildingHighlight _pLastSelected
	{
		get
		{
			return _lastSelected;
		}
	}

	private void Awake()
	{
		_rootPos = base.transform.position;
	}

	private void OnEnable()
	{
		_tweenOut = false;
		_lastSelected = this;
		_enableTime = Time.time;
		base.transform.position = _rootPos + Vector3.down * 12.5f;
	}

	public void TweenOut()
	{
		_tweenOut = true;
		_tweenOutTime = Time.time;
	}

	private void Update()
	{
		float num = Time.time - _enableTime;
		_lastSelected = this;
		if (_tweenOut)
		{
			float num2 = Time.time - _tweenOutTime;
			if (num2 < 0.5f)
			{
				num2 = Easing.Ease(Easing.EaseType.EaseInCircle, num2, 0.5f, 0f, 1f);
				Vector3 position = Vector3.Lerp(_rootPos, _rootPos + Vector3.down * 12.5f, num2);
				base.transform.position = position;
			}
			else
			{
				base.transform.position = _rootPos + Vector3.down * 12.5f;
			}
			return;
		}
		if (num < 0.5f)
		{
			float t = Easing.Ease(Easing.EaseType.EaseOutCircle, num, 0.5f, 0f, 1f);
			Vector3 position2 = Vector3.Lerp(_rootPos + Vector3.down * 12.5f, _rootPos, t);
			base.transform.position = position2;
			return;
		}
		base.transform.position = _rootPos;
		if (walls != null && walls.Length > 0 && walls[0] != null)
		{
			Color color = walls[0].material.GetColor("_TintColor");
			float t2 = Mathf.Sin(Time.time * 5f) * 0.5f + 0.5f;
			color.a = Mathf.Lerp(0.45f, 0.65f, t2);
			walls[0].material.SetColor("_TintColor", color);
		}
	}
}
