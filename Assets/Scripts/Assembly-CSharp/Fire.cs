using UnityEngine;

public class Fire : MonoBehaviour
{
	public Transform _centre;

	private ParticleSystem[] _particleSystems;

	private float[] _sizes;

	public float _pNormIntensity
	{
		set
		{
			for (int num = _sizes.Length - 1; num >= 0; num--)
			{
				bool flag = value > 0f;
				if (flag != _particleSystems[num].emission.enabled)
				{
					ParticleSystem.EmissionModule emission = _particleSystems[num].emission;
					emission.enabled = flag;
				}
				_particleSystems[num].startSize = _sizes[num] * value;
			}
		}
	}

	private void Awake()
	{
		_particleSystems = GetComponentsInChildren<ParticleSystem>();
		_sizes = new float[_particleSystems.Length];
		for (int num = _particleSystems.Length - 1; num >= 0; num--)
		{
			_sizes[num] = _particleSystems[num].startSize;
		}
	}
}
