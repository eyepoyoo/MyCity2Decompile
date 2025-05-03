using UnityEngine;

public class HoseStream
{
	private const float TEX_STRETCH = 10f;

	private const float LERP_END_TIME = 0.5f;

	private static readonly Color _colourStart = new Color(13f / 15f, 83f / 85f, 1f, 1f);

	private static readonly Color _colourEnd = new Color(0f, 71f / 85f, 1f, 0f);

	public Vector3[] _trajectoryPoints = new Vector3[0];

	public Vector3[] _trajectoryVelocities = new Vector3[0];

	private float _timeStart;

	private float _timeEnd = float.PositiveInfinity;

	private SpecialAbility_Hose _hose;

	private bool _destroyed;

	private LineRenderer _lineRenderer;

	private bool _hasEnded;

	private int _rayCastLayerMask;

	public HoseStream(SpecialAbility_Hose hose)
	{
		_timeStart = Time.time;
		_hose = hose;
		_lineRenderer = _hose.GetLineRenderer();
		_rayCastLayerMask = LayerMask.GetMask("Geometry", "Debris", "Vehicle", "InvincibleCube", "CollateralStatic", "CollateralDynamic1", "CollateralDynamic2");
	}

	public void UpdateTrajectory()
	{
		float num = Mathf.Min(Time.time - Mathf.Min(_timeEnd, Time.time), _hose._trajectoryTime);
		float num2 = Mathf.Min(Time.time - _timeStart, _hose._trajectoryTime);
		if (num == _hose._trajectoryTime)
		{
			_hose.DestroyStream(this);
		}
		else if (num != num2)
		{
			_hose.GetTrajectory(num, num2, out _trajectoryPoints, out _trajectoryVelocities);
			_lineRenderer.SetVertexCount(_trajectoryPoints.Length);
			for (int i = 0; i < _trajectoryPoints.Length; i++)
			{
				_lineRenderer.SetPosition(i, _trajectoryPoints[i]);
			}
			_lineRenderer.SetWidth(Mathf.LerpUnclamped(_hose._widthStart, _hose._widthEnd, num / 0.5f), Mathf.LerpUnclamped(_hose._widthStart, _hose._widthEnd, num2 / 0.5f));
			_lineRenderer.SetColors(Color.Lerp(_colourStart, _colourEnd, num / 0.5f), Color.Lerp(_colourStart, _colourEnd, num2 / 0.5f));
			if (!_hasEnded)
			{
				_lineRenderer.material.mainTextureOffset += Vector2.left * Time.deltaTime * _trajectoryVelocities[0].magnitude / 10f;
			}
			_lineRenderer.material.mainTextureScale = new Vector2((float)_trajectoryPoints.Length * _hose._widthEnd * _hose._waterSpeed / 1000f, 1f);
		}
	}

	public void DoRaycast(SpecialAbility_Hose.DOnRaycastHit onHitCallback)
	{
		float num = Mathf.Min(5, _trajectoryPoints.Length);
		if (num < 2f)
		{
			return;
		}
		for (int i = 0; (float)i < num - 1f; i++)
		{
			int num2 = Mathf.RoundToInt((float)((_trajectoryPoints.Length - 1) * i) / (num - 1f));
			int num3 = Mathf.RoundToInt((float)((_trajectoryPoints.Length - 1) * (i + 1)) / (num - 1f));
			Vector3 vector = _trajectoryPoints[num2];
			Vector3 vector2 = _trajectoryPoints[num3];
			RaycastHit hitInfo;
			if (Physics.Raycast(vector, vector2 - vector, out hitInfo, Vector3.Distance(vector, vector2), _rayCastLayerMask) && !VehicleController_Player.IsPlayer(hitInfo.collider))
			{
				onHitCallback(hitInfo, _trajectoryVelocities[i]);
				break;
			}
		}
	}

	public void End()
	{
		if (!_hasEnded)
		{
			_hasEnded = true;
			_timeEnd = Time.time;
		}
	}

	public void Destroy()
	{
		if (!_destroyed)
		{
			_destroyed = true;
			_hose.ReturnLineRenderer(_lineRenderer);
		}
	}
}
