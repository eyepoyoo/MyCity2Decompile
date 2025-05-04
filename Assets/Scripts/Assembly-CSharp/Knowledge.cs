using System;
using KnowledgeSystem;
using UnityEngine;

public class Knowledge : IComparable, IComparable<Knowledge>, IEquatable<Knowledge>
{
	public int _idKnowledge;

	public int _idActorOwner;

	public int _idActorTarget;

	public int _projectData;

	public bool _hasUpdated;

	public bool _isDummyTarget;

	public bool _isPlayer;

	public float _age;

	public float _threatValue;

	public float _distanceSqrd;

	public float _lifeTime;

	public SENSE _senseType;

	public Vector3 _position;

	public Vector3 _direction;

	public Knowledge(int idKnowledge, int idActorOwner)
	{
		_idKnowledge = idKnowledge;
		_idActorOwner = idActorOwner;
		Reset();
	}

	public void Reset()
	{
		_idActorTarget = -1;
		_projectData = 0;
		_hasUpdated = false;
		_isDummyTarget = false;
		_isPlayer = false;
		_age = 0f;
		_threatValue = 0f;
		_distanceSqrd = float.PositiveInfinity;
		_lifeTime = -1f;
		_senseType = SENSE.INVALID;
		_position = Vector3.zero;
		_direction = Vector3.zero;
	}

	public override bool Equals(object other)
	{
		if (other == null)
		{
			return false;
		}
		if (!(other is Knowledge))
		{
			return false;
		}
		return Equals((Knowledge)other);
	}

	public bool Equals(Knowledge other)
	{
		if (other == null)
		{
			return false;
		}
		return _idKnowledge == other._idKnowledge;
	}

	public override int GetHashCode()
	{
		return _idKnowledge.GetHashCode();
	}

	public int CompareTo(Knowledge other)
	{
		if (Equals(other))
		{
			return 0;
		}
		if (_threatValue != other._threatValue)
		{
			return (!(_threatValue > other._threatValue)) ? 1 : (-1);
		}
		if (_senseType != other._senseType)
		{
			return _senseType - other._senseType;
		}
		if (_distanceSqrd != other._distanceSqrd)
		{
			return (!(_distanceSqrd < other._distanceSqrd)) ? 1 : (-1);
		}
		return (GetHashCode() > other.GetHashCode()) ? 1 : (-1);
	}

	public int CompareTo(object other)
	{
		if (!(other is Knowledge))
		{
			Debug.LogError("Knowledge::CompareTo - other is not knowledge, result will be inaccurate. other type:" + other.GetType().ToString());
			return -1;
		}
		return CompareTo((Knowledge)other);
	}

	public static bool operator <(Knowledge k1, Knowledge k2)
	{
		return k1.CompareTo(k2) < 0;
	}

	public static bool operator >(Knowledge k1, Knowledge k2)
	{
		return k1.CompareTo(k2) > 0;
	}

	public static bool operator <=(Knowledge k1, Knowledge k2)
	{
		return k1.CompareTo(k2) <= 0;
	}

	public static bool operator >=(Knowledge k1, Knowledge k2)
	{
		return k1.CompareTo(k2) >= 0;
	}

	public static bool operator ==(Knowledge k1, Knowledge k2)
	{
		if (object.ReferenceEquals(k1, k2))
		{
			return true;
		}
		if ((object)k1 == null || (object)k2 == null)
		{
			return false;
		}
		return k1.Equals(k2);
	}

	public static bool operator !=(Knowledge k1, Knowledge k2)
	{
		return !(k1 == k2);
	}
}
