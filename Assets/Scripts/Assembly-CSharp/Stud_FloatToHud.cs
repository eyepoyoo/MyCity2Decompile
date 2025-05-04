using System;
using UnityEngine;

public class Stud_FloatToHud : MonoBehaviour
{
	public static readonly Vector3 _floatToPos = new Vector3(-3.4f, 2.4f, 4.9f);

	private Tween _tweenRef;

	private Transform _target;

	private void Awake()
	{
		MeshRenderer componentInChildren = GetComponentInChildren<MeshRenderer>();
		if (componentInChildren != null && ScenarioManager._pInstance != null && ScenarioManager._pInstance._pCurrentScenario != null)
		{
			componentInChildren.sharedMaterial = ScenarioManager._pInstance._pCurrentScenario.inGameStudMaterial;
		}
	}

	private void Update()
	{
		if (_target != null && _tweenRef != null)
		{
			_tweenRef._pPosTo = _target.transform.position;
		}
	}

	public void FloatToTransform(Transform target, Vector3 fromPos, Quaternion fromRot, Action<Stud_FloatToHud> onComplete = null)
	{
		base.transform.position = fromPos;
		base.transform.rotation = fromRot;
		base.transform.parent = null;
		_target = target;
		_tweenRef = base.transform.TweenToPos(target.transform.position, 0.4f, delegate
		{
			if (onComplete != null)
			{
				onComplete(this);
			}
		}, Easing.EaseType.Linear, false, true, 0f);
	}

	public void FloatToHud(Vector3 fromPos, Quaternion fromRot, Action<Stud_FloatToHud> onComplete = null)
	{
		_target = null;
		_tweenRef = null;
		base.transform.position = fromPos;
		base.transform.rotation = fromRot;
		base.transform.parent = Camera.main.transform;
		base.transform.TweenToPos(_floatToPos, 0.4f, delegate
		{
			if (onComplete != null)
			{
				onComplete(this);
			}
		}, Easing.EaseType.Linear, false, true, 0f);
	}
}
