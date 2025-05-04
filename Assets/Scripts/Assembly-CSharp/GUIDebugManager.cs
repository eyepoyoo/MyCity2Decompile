using System.Collections.Generic;
using UnityEngine;

public class GUIDebugManager : MonoBehaviour
{
	private class GUIDebugElement
	{
		public int m_iID = -1;

		public string m_sContent;

		public float m_fTime = 1f;

		public GUIDebugElement(int iID, string sContent, float fTime = 1f)
		{
			m_iID = iID;
			m_sContent = sContent;
			m_fTime = 1f;
		}
	}

	private static GUIDebugManager m_Instance;

	private List<GUIDebugElement> m_ContentList = new List<GUIDebugElement>();

	public static GUIDebugManager Instance
	{
		get
		{
			return m_Instance;
		}
	}

	private void Start()
	{
		m_Instance = this;
	}

	private void Update()
	{
		int num = 0;
		while (num < m_ContentList.Count)
		{
			m_ContentList[num].m_fTime -= Time.deltaTime;
			if (m_ContentList[num].m_fTime <= 0f)
			{
				m_ContentList.Remove(m_ContentList[num]);
			}
			else
			{
				num++;
			}
		}
	}

	public void AddElement(int iID, string sContent, float fTime = 1f)
	{
		GUIDebugElement gUIDebugElement = m_ContentList.Find((GUIDebugElement oGUIDebugElement) => oGUIDebugElement.m_iID == iID);
		if (gUIDebugElement == null)
		{
			GUIDebugElement item = new GUIDebugElement(iID, sContent, fTime);
			m_ContentList.Add(item);
		}
		else
		{
			gUIDebugElement.m_sContent = sContent;
			gUIDebugElement.m_fTime = fTime;
		}
	}

	public void RemoveElement(int iID)
	{
		GUIDebugElement gUIDebugElement = m_ContentList.Find((GUIDebugElement oGUIDebugElement) => oGUIDebugElement.m_iID == iID);
		if (gUIDebugElement != null)
		{
			m_ContentList.Remove(gUIDebugElement);
		}
		else
		{
			Debug.Log("Trying to remove a debug element that doesn't exist");
		}
	}
}
