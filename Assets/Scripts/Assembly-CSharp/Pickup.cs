using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
	private const float GRID_CELL_SIZE = 10f;

	public const float STUD_MAGNETIZE_SQR_DIST = 10f;

	[NonSerialized]
	public FastPool _pool;

	private static List<Pickup>[,] _grid;

	private int _gridX;

	private int _gridZ;

	private Transform _gravitateTarget;

	private static Pickup[] _dummyReturn;

	private static int _dummyReturnSize;

	private bool _collected;

	private MeshRenderer _meshRenderer;

	private Stud_FloatToHud _gravitateToPlayerStud;

	public virtual float _pRadius
	{
		get
		{
			return 0f;
		}
	}

	private MeshRenderer _pMeshRenderer
	{
		get
		{
			if (_meshRenderer == null)
			{
				_meshRenderer = GetComponentInChildren<MeshRenderer>();
			}
			return _meshRenderer;
		}
	}

	public bool _pIsGravitating
	{
		get
		{
			return _gravitateTarget != null;
		}
	}

	public bool _pCollected
	{
		get
		{
			return _collected;
		}
	}

	public static event Action<Pickup> _onPickupCollected;

	protected virtual void Start()
	{
		_collected = false;
		AddToGrid(this);
	}

	public void Gravitate(Transform target)
	{
		if (!(_gravitateTarget != null))
		{
			_gravitateTarget = target;
			_pMeshRenderer.enabled = false;
			_gravitateToPlayerStud = MinigameController._pInstance._pPool_FloatingStuds.FastInstantiate<Stud_FloatToHud>();
			Action<Stud_FloatToHud> onComplete = delegate
			{
				MinigameController._pInstance._pPool_FloatingStuds.FastDestroy(_gravitateToPlayerStud);
				Collect(_gravitateTarget.position);
			};
			_gravitateToPlayerStud.FloatToTransform(target, base.transform.position, base.transform.rotation, onComplete);
		}
	}

	public void Collect(Vector3 pos)
	{
		if (Pickup._onPickupCollected != null)
		{
			Pickup._onPickupCollected(this);
		}
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.PlayOneShotSFX("CollectStud", 0f, null, null, "SoundSpawnBehaviourRetrigger");
		}
		RemoveFromGrid(this);
		DestroyOrReturnToPool();
		_collected = true;
		OnCollected(pos);
	}

	protected virtual void OnCollected(Vector3 pos)
	{
	}

	public void DestroyOrReturnToPool()
	{
		if (_pool != null)
		{
			_pool.FastDestroy(base.gameObject);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	public static void SetUpGrid()
	{
		_grid = new List<Pickup>[Mathf.CeilToInt((MinigamesController._pInstance._gameAreaMaxX - MinigamesController._pInstance._gameAreaMinX) / 10f), Mathf.CeilToInt((MinigamesController._pInstance._gameAreaMaxZ - MinigamesController._pInstance._gameAreaMinZ) / 10f)];
	}

	public static void ClearGrid()
	{
		_grid = null;
	}

	public static void GetGridCoords(Vector3 pos, out int x, out int z)
	{
		x = (int)((pos.x - MinigamesController._pInstance._gameAreaMinX) / 10f);
		z = (int)((pos.z - MinigamesController._pInstance._gameAreaMinZ) / 10f);
	}

	private static void AddToGrid(Pickup pickup)
	{
		GetGridCoords(pickup.transform.position, out pickup._gridX, out pickup._gridZ);
		List<Pickup> list = _grid[pickup._gridX, pickup._gridZ] ?? (_grid[pickup._gridX, pickup._gridZ] = new List<Pickup>());
		list.Add(pickup);
	}

	private static void RemoveFromGrid(Pickup pickup)
	{
		if (_grid == null)
		{
			return;
		}
		List<Pickup> list = _grid[pickup._gridX, pickup._gridZ];
		if (list != null)
		{
			list.Remove(pickup);
			if (list.Count == 0)
			{
				_grid[pickup._gridX, pickup._gridZ] = null;
			}
		}
	}

	public static void RefreshInGrid(Pickup pickup)
	{
		RemoveFromGrid(pickup);
		AddToGrid(pickup);
	}

	public static Pickup[] GetPickupsAtPos(Vector3 pos, out int numItemsReturned)
	{
		int x;
		int z;
		GetGridCoords(pos, out x, out z);
		if (_dummyReturn == null)
		{
			_dummyReturnSize = 16;
			_dummyReturn = new Pickup[_dummyReturnSize];
		}
		int num = 0;
		for (int i = Mathf.Max(x - 1, 0); i <= Mathf.Min(x + 1, _grid.GetLength(0) - 1); i++)
		{
			for (int j = Mathf.Max(z - 1, 0); j <= Mathf.Min(z + 1, _grid.GetLength(1) - 1); j++)
			{
				if (_grid[i, j] == null)
				{
					continue;
				}
				int count = _grid[i, j].Count;
				for (int k = 0; k < count; k++)
				{
					_dummyReturn[num] = _grid[i, j][k];
					num++;
					if (num == _dummyReturnSize - 1)
					{
						ExtendDummyList();
					}
				}
			}
		}
		numItemsReturned = num;
		return _dummyReturn;
	}

	private static void ExtendDummyList()
	{
		Pickup[] dummyReturn = _dummyReturn;
		int dummyReturnSize = _dummyReturnSize;
		_dummyReturnSize += 16;
		_dummyReturn = new Pickup[_dummyReturnSize];
		for (int i = 0; i < dummyReturnSize; i++)
		{
			_dummyReturn[i] = dummyReturn[i];
		}
	}
}
