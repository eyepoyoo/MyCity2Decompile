using UnityEngine;

public class VertexColourChanger : MonoBehaviour, IFastPoolItem
{
	public Color32[] colArray;

	public Color32[] colors;

	public Vector3[] vertices;

	public bool[] colChangeVerts;

	private Mesh mesh;

	public bool DoRandomColourChange = true;

	public Color32 setAsSpecificColour;

	private void Start()
	{
		mesh = GetComponent<MeshFilter>().mesh;
		DoColourChange();
	}

	private void Awake()
	{
		mesh = GetComponent<MeshFilter>().mesh;
		DoColourChange();
	}

	public void SetNewColour(Color32 newColour)
	{
		for (int i = 0; i < vertices.Length; i++)
		{
			if (colChangeVerts[i])
			{
				colors[i] = newColour;
			}
		}
		mesh.colors32 = colors;
	}

	public void DoColourChange()
	{
		if (DoRandomColourChange)
		{
			int num = Random.Range(0, colArray.Length);
			SetNewColour(colArray[num]);
		}
		else
		{
			SetNewColour(setAsSpecificColour);
		}
	}

	public void OnFastInstantiate(FastPool pool)
	{
		DoColourChange();
	}

	public void OnCloned(FastPool pool)
	{
	}

	public void OnFastDestroy()
	{
	}
}
