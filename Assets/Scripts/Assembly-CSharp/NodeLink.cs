using System;

[Serializable]
public class NodeLink
{
	public int linkToNodeId;

	public int traverseLinkBehaviour;

	public NodeLink()
	{
		linkToNodeId = 0;
		traverseLinkBehaviour = 9;
	}
}
