using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialSwapper : MonoBehaviour
{
	public Material _matOn;

	public Material _matOff;

	public float _timeOn;

	public float _timeOff;

	public bool _randomOffset;

	public ParticleSystem[] _particles;

	private MeshRenderer _mr;

	private float _timer;

	private bool _isMat1 = true;

	private float _duration;

	private bool _pIsMat1
	{
		get
		{
			return _isMat1;
		}
		set
		{
			if (value != _isMat1)
			{
				_isMat1 = value;
				_mr.sharedMaterial = ((!value) ? _matOff : _matOn);
			}
		}
	}

	private void Awake()
	{
		_duration = _timeOn + _timeOff;
		if (_randomOffset)
		{
			_timer = Random.value * _duration;
			for (int i = 0; i < _particles.Length; i++)
			{
				_particles[i].time = _timer;
			}
		}
		_mr = GetComponent<MeshRenderer>();
	}

	private void Update()
	{
		_timer += Time.deltaTime;
		_pIsMat1 = _timer % _duration < _timeOn;
	}
}
