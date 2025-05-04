public abstract class VCSBehaveBase : BehaveBase
{
	protected BehaveBase _dummyBehaviour;

	private VcsInfo _vcsInfo;

	public override bool _pIsVcsBehaviour
	{
		get
		{
			return true;
		}
	}

	public VcsInfo _pVcsInfo
	{
		protected get
		{
			return _vcsInfo;
		}
		set
		{
			_vcsInfo = value;
		}
	}

	protected PathNode _pPathNode
	{
		get
		{
			if (_pVcsInfo == null)
			{
				return null;
			}
			return _pVcsInfo._pathNodeCurrent;
		}
	}

	protected NodeLink _pPathNodeLink
	{
		get
		{
			if (_pVcsInfo == null)
			{
				return null;
			}
			return _pVcsInfo._nodeLinkCurrent;
		}
	}

	protected override bool _pShouldUpdate
	{
		get
		{
			return base._pShouldUpdate && _pPathNode != null;
		}
	}

	public VCSBehaveBase()
	{
	}

	protected override void OnShutdown()
	{
		_pVcsInfo = null;
	}

	protected override void Finish()
	{
		if (base._pOwnerBrain != null)
		{
			base._pOwnerBrain.VogonConstructorScriptExit();
		}
		base.Finish();
	}
}
