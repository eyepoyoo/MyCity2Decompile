using UnityEngine;

public class BlinkDeathEffect : MonoBehaviour
{
	public float duration = 2f;

	public float blinkPeriod = 0.5f;

	public float blinkSplit = 0.5f;

	public Renderer _targetRenderer;

	private float _timer;

	private void Awake()
	{
		if (_targetRenderer == null)
		{
			_targetRenderer = GetComponent<Renderer>();
		}
	}

	private void OnEnable()
	{
		_timer = 0f;
	}

	private void OnDisable()
	{
		if (_targetRenderer != null)
		{
			_targetRenderer.enabled = true;
		}
	}

	private void Update()
	{
		_timer += Time.deltaTime;
		if (_timer >= duration)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			if (!(_targetRenderer != null))
			{
				return;
			}
			float num = _timer % blinkPeriod / blinkPeriod;
			if (num < 1f - blinkSplit)
			{
				if (_targetRenderer.enabled)
				{
					_targetRenderer.enabled = false;
				}
			}
			else if (!_targetRenderer.enabled)
			{
				_targetRenderer.enabled = true;
			}
		}
	}
}
