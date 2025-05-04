using UnityEngine;

public class MinigamesController : MonoBehaviour
{
	public MinigameController[] _minigameControllers;

	public int _defaultMinigameIndex;

	public float _gameAreaMinX;

	public float _gameAreaMaxX;

	public float _gameAreaMinZ;

	public float _gameAreaMaxZ;

	public SmoothFollow _cameraPrefab;

	public DirectorArrow _directorArrowPrefab;

	public static MinigamesController _pInstance { get; private set; }

	private void Awake()
	{
		_pInstance = this;
		for (int i = 0; i < _minigameControllers.Length; i++)
		{
			_minigameControllers[i].gameObject.SetActive(false);
		}
		if ((bool)MinigameManager._pInstance)
		{
			for (int j = 0; j < _minigameControllers.Length; j++)
			{
				if (_minigameControllers[j]._type == MinigameManager._pInstance._pCurrentMinigameType)
				{
					_minigameControllers[j].Init(false);
				}
			}
		}
		else
		{
			_minigameControllers[_defaultMinigameIndex].Init(true);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Vector3 vector = new Vector3(_gameAreaMinX, 0f, _gameAreaMinZ);
		Vector3 vector2 = new Vector3(_gameAreaMinX, 0f, _gameAreaMaxZ);
		Vector3 vector3 = new Vector3(_gameAreaMaxX, 0f, _gameAreaMaxZ);
		Vector3 vector4 = new Vector3(_gameAreaMaxX, 0f, _gameAreaMinZ);
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector3, vector4);
		Gizmos.DrawLine(vector4, vector);
	}
}
