using UnityEngine;

namespace AmuzoEngine
{
	public class ObjectSpawnPoints : MonoBehaviour, ObjectSpawner.ISpawnPoints
	{
		public enum EType
		{
			SELF = 0,
			POINTS = 1,
			AREAS = 2
		}

		private const string LOG_TAG = "[ObjectSpawnPoints] ";

		public EType _type;

		public bool _isSequential;

		public PointList _spawnPoints;

		public PolyArea[] _spawnAreas;

		private int _lastSpawnPoint = -1;

		private float _totalSpawnArea = -1f;

		bool ObjectSpawner.ISpawnPoints.ChooseSpawnPoint(out Vector3 position)
		{
			position = ChooseSpawnPoint();
			return true;
		}

		private Vector3 ChooseSpawnPoint()
		{
			Vector3 result = base.transform.position;
			switch (_type)
			{
			case EType.POINTS:
				if (!(_spawnPoints != null))
				{
					break;
				}
				if (_isSequential)
				{
					int num2 = 0;
					if (_lastSpawnPoint >= 0)
					{
						num2 = (_lastSpawnPoint + 1) % _spawnPoints._pVertexCount;
					}
					if (0 <= num2 && num2 < _spawnPoints._pVertexCount)
					{
						result = _spawnPoints._pVertices[num2]._pScenePos;
						_lastSpawnPoint = num2;
					}
				}
				else
				{
					result = _spawnPoints.ChooseRandomPoint();
				}
				break;
			case EType.AREAS:
				if (_spawnAreas != null)
				{
					int num = ChooseSpawnArea();
					if (num >= 0)
					{
						result = _spawnAreas[num].ChooseRandomPoint();
					}
				}
				break;
			}
			return result;
		}

		private int ChooseSpawnArea()
		{
			if (_totalSpawnArea < 0f)
			{
				CalculateTotalSpawnArea();
			}
			if (_totalSpawnArea <= 0f)
			{
				return -1;
			}
			float num = Random.Range(0f, _totalSpawnArea);
			float num2 = 0f;
			for (int i = 0; i < _spawnAreas.Length; i++)
			{
				if (!(_spawnAreas[i] == null))
				{
					num2 += _spawnAreas[i]._pTotalArea;
					if (num <= num2)
					{
						return i;
					}
				}
			}
			return -1;
		}

		private void CalculateTotalSpawnArea()
		{
			_totalSpawnArea = 0f;
			if (_spawnAreas == null)
			{
				return;
			}
			for (int i = 0; i < _spawnAreas.Length; i++)
			{
				if (!(_spawnAreas[i] == null))
				{
					_totalSpawnArea += _spawnAreas[i]._pTotalArea;
				}
			}
		}
	}
}
