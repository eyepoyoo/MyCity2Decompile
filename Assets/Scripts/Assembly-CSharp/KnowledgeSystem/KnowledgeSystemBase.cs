using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace KnowledgeSystem
{
	public class KnowledgeSystemBase
	{
		public const float INFINITE = -1f;

		private const int MEM_POOL_SIZE_INITIAL = 20;

		private const float ITERATION_TIME_FIND_KNOWLEDGE = 1.5f;

		private const float ITERATION_TIME_UPDATE_KNOWLEDGE = 0.65f;

		private const float RANGE_SIGHT_DEFAULT = -1f;

		private const float TIME_MEMORY_DEFAULT = -1f;

		private static int _nextKnowledgeId;

		protected float _sightRangeSqrd = -1f;

		protected float _memoryTime = -1f;

		protected float _timeUpdateKnowledgePeriod = 0.65f;

		protected float _timerUpdateKnowledge;

		protected float _timeFindKnowledgePeriod = 1.5f;

		protected float _timerFindKnowledge;

		protected Vector3 _dummyVector;

		protected JsonData _dummyData;

		protected ActorBase _dummyActor;

		protected Knowledge _dummyKnowledge;

		protected ActorBase _actorOwner;

		protected BrainActorBase _brainOwner;

		protected List<Knowledge> _knowledge;

		protected ActorDummyTarget _dummyTarget;

		private List<Knowledge> _knowledgeUnused;

		protected static int GetNewKnowledgeId()
		{
			return _nextKnowledgeId++;
		}

		public virtual void Initialise(ActorBase actorOwner, BrainActorBase brainOwner, JsonData data)
		{
			_actorOwner = actorOwner;
			if (_actorOwner == null)
			{
				return;
			}
			_brainOwner = brainOwner;
			if (_brainOwner != null)
			{
				_timerFindKnowledge = Random.Range(0f, _timeFindKnowledgePeriod);
				_timerUpdateKnowledge = Random.Range(0f, _timeUpdateKnowledgePeriod);
				_knowledge = new List<Knowledge>(20);
				_knowledgeUnused = new List<Knowledge>(20);
				for (int i = 0; i < 20; i++)
				{
					_dummyKnowledge = new Knowledge(GetNewKnowledgeId(), _actorOwner._pIdActor);
					_knowledgeUnused.Add(_dummyKnowledge);
				}
			}
		}

		public virtual void Update()
		{
			if (!(_actorOwner == null) && _actorOwner._pIsAlive && _brainOwner != null)
			{
				_timerFindKnowledge -= Time.deltaTime;
				_timerUpdateKnowledge -= Time.deltaTime;
				if (_timerFindKnowledge <= 0f)
				{
					_timerFindKnowledge = _timeFindKnowledgePeriod;
					FindNewKnowledge();
				}
				UpdateExistingKnowledge();
				IssueBestThreat();
			}
		}

		protected virtual void IssueBestThreat()
		{
			if (_brainOwner != null)
			{
				if (_actorOwner != null && _actorOwner._pSquad != null)
				{
					_brainOwner._pIdActorBestTarget = _brainOwner._pOwnerActor._pSquad.IssueBestTargetFromCollection(ref _knowledge, _actorOwner._pIdActor);
				}
				else if (_knowledge == null || _knowledge.Count == 0)
				{
					_brainOwner._pIdActorBestTarget = -1;
				}
				else
				{
					_brainOwner._pIdActorBestTarget = _knowledge[0]._idActorTarget;
				}
			}
		}

		protected virtual void FindNewKnowledge()
		{
			if (_actorOwner == null || _brainOwner == null)
			{
				return;
			}
			for (int i = 0; i < ActorBase._pActorIds.Length; i++)
			{
				_dummyActor = ActorBase._pDictActors[ActorBase._pActorIds[i]];
				if (_dummyActor == null || !_dummyActor._pIsAlive || !_dummyActor._pIsTargetable || _dummyActor._pIdActor == _actorOwner._pIdActor || _dummyActor._pBrain == null || _dummyActor._pBrain._pAllegiance == EAllegiance.NEUTRAL || _dummyActor._pBrain._pAllegiance == _brainOwner._pAllegiance || _dummyActor._pIsChild)
				{
					continue;
				}
				bool flag = _dummyActor.GetType() == typeof(ActorDummyTarget);
				if (_dummyActor._pIsPlayer && (!_dummyActor._pIsInCameraViewPart || !_actorOwner._pIsInCameraViewPart))
				{
					continue;
				}
				_dummyVector = _dummyActor._pTargetPos - _actorOwner._pTargetPos;
				float sqrMagnitude = _dummyVector.sqrMagnitude;
				if ((flag && sqrMagnitude > ((ActorDummyTarget)_dummyActor)._pEffectiveRange * ((ActorDummyTarget)_dummyActor)._pEffectiveRange) || (_sightRangeSqrd != -1f && sqrMagnitude > _sightRangeSqrd))
				{
					continue;
				}
				if (GetExistingKnowledge(_dummyActor._pIdActor, ref _dummyKnowledge))
				{
					_dummyKnowledge._age = 0f;
					continue;
				}
				GetOrCreateNewKnowledge(ref _dummyKnowledge);
				if (!(_dummyKnowledge == null))
				{
					_dummyKnowledge._senseType = SENSE.VISION;
					_dummyKnowledge._idActorTarget = _dummyActor._pIdActor;
					_dummyKnowledge._lifeTime = _memoryTime;
					_dummyKnowledge._isDummyTarget = flag;
					_dummyKnowledge._distanceSqrd = sqrMagnitude;
					_dummyKnowledge._isPlayer = _dummyActor._pIsPlayer;
					ScoreKnowledge(ref _dummyKnowledge);
					AddKnowledge(ref _dummyKnowledge);
				}
			}
		}

		protected virtual void UpdateExistingKnowledge()
		{
			int count = _knowledge.Count;
			if (count == 0)
			{
				return;
			}
			bool flag = _timerUpdateKnowledge <= 0f;
			if (flag)
			{
				_timerUpdateKnowledge = _timeUpdateKnowledgePeriod;
			}
			for (int num = count - 1; num >= 0; num--)
			{
				_dummyKnowledge = _knowledge[num];
				_dummyKnowledge._hasUpdated = true;
				_dummyKnowledge._age += Time.deltaTime;
				if (!ActorBase.FindActor(_dummyKnowledge._idActorTarget, ref _dummyActor))
				{
					RemoveKnowledge(num);
				}
				else if (_dummyActor == null || !_dummyActor._pIsAlive || !_dummyActor._pIsTargetable)
				{
					RemoveKnowledge(num);
				}
				else if (_dummyKnowledge._lifeTime != -1f && _dummyKnowledge._age > _dummyKnowledge._lifeTime)
				{
					RemoveKnowledge(num);
				}
				else if (flag)
				{
					_dummyKnowledge._position = _dummyActor._pTargetPos;
					_dummyKnowledge._direction = _dummyActor._pTargetPos - _actorOwner._pTargetPos;
					_dummyKnowledge._distanceSqrd = _knowledge[num]._direction.sqrMagnitude;
					_dummyKnowledge._direction.Normalize();
					_dummyKnowledge._isPlayer = _dummyActor._pIsPlayer;
					if (_dummyKnowledge._isDummyTarget && _dummyKnowledge._distanceSqrd > ((ActorDummyTarget)_dummyActor)._pEffectiveRange * ((ActorDummyTarget)_dummyActor)._pEffectiveRange)
					{
						RemoveKnowledge(num);
					}
					else
					{
						ScoreKnowledge(ref _dummyKnowledge);
					}
				}
			}
			_knowledge.Sort();
		}

		protected virtual void ScoreKnowledge(ref Knowledge knowledge)
		{
			float num = 300f;
			float num2 = 15f;
			float num3 = 1.2f;
			knowledge._threatValue = num - Mathf.Sqrt(knowledge._distanceSqrd);
			if (knowledge._senseType == SENSE.TOUCH)
			{
				knowledge._threatValue += num2;
			}
			knowledge._threatValue -= Mathf.Abs(knowledge._direction.y) * num3;
			ActorDummyTarget.FindActorDummyTarget(knowledge._idActorTarget, ref _dummyTarget);
			if (_dummyTarget != null)
			{
				knowledge._threatValue += _dummyTarget._pPriorityScore;
				_dummyTarget = null;
			}
			knowledge._threatValue += _brainOwner.ScoreKnowledgeBrainSpecific(ref knowledge);
		}

		public virtual void Shutdown()
		{
			int count = _knowledge.Count;
			for (int i = 0; i < count; i++)
			{
				if (!(_knowledge[i] == null))
				{
					_knowledge[i].Reset();
				}
			}
			_knowledge.Clear();
			_knowledge = null;
			count = _knowledgeUnused.Count;
			for (int j = 0; j < count; j++)
			{
				if (!(_knowledgeUnused[j] == null))
				{
					_knowledgeUnused[j].Reset();
				}
			}
			_knowledgeUnused.Clear();
			_knowledgeUnused = null;
		}

		public virtual void ClearAllKnowledge()
		{
			while (_knowledge.Count > 0)
			{
				_dummyKnowledge = _knowledge[0];
				RemoveKnowledge(ref _dummyKnowledge);
			}
		}

		protected bool GetExistingKnowledge(int idActorTarget, ref Knowledge knowledge)
		{
			foreach (Knowledge item in _knowledge)
			{
				if (item == null || item._idActorTarget != idActorTarget)
				{
					continue;
				}
				knowledge = item;
				return true;
			}
			return false;
		}

		protected void GetOrCreateNewKnowledge(ref Knowledge newKnowledge)
		{
			if (!(_actorOwner == null))
			{
				if (_knowledgeUnused.Count > 0)
				{
					newKnowledge = _knowledgeUnused[0];
					return;
				}
				newKnowledge = new Knowledge(GetNewKnowledgeId(), _actorOwner._pIdActor);
				_knowledgeUnused.Add(newKnowledge);
			}
		}

		protected void AddKnowledge(ref Knowledge knowledge)
		{
			_knowledgeUnused.Remove(knowledge);
			_knowledge.Add(knowledge);
		}

		protected void RemoveKnowledge(ref Knowledge knowledge)
		{
			knowledge.Reset();
			_knowledgeUnused.Add(knowledge);
			_knowledge.Remove(knowledge);
		}

		protected void RemoveKnowledge(int listIndex)
		{
			if (listIndex < _knowledge.Count)
			{
				_knowledge[listIndex].Reset();
				_knowledgeUnused.Add(_knowledge[listIndex]);
				_knowledge.RemoveAt(listIndex);
			}
		}
	}
}
