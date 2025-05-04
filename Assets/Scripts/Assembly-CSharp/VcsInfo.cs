public class VcsInfo
{
	public bool _isOnVcs;

	public NodeLink _nodeLinkCurrent;

	public PathNode _pathNodeDummy;

	public PathNode _pathNodeCurrent;

	public PathNode _pathNodePrevious;

	public VcsInfo()
	{
		Reset();
	}

	public void Reset()
	{
		_isOnVcs = false;
		_nodeLinkCurrent = null;
		_pathNodeDummy = null;
		_pathNodeCurrent = null;
		_pathNodePrevious = null;
	}
}
