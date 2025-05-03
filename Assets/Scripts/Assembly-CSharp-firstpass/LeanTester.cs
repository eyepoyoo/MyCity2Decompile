using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class LeanTester : MonoBehaviour
{
	// Token: 0x0600009E RID: 158 RVA: 0x00003C90 File Offset: 0x00001E90
	public void Start()
	{
		base.StartCoroutine(this.timeoutCheck());
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00003CA0 File Offset: 0x00001EA0
	private IEnumerator timeoutCheck()
	{
		float pauseEndTime = Time.realtimeSinceStartup + this.timeout;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}
		if (!LeanTest.testsFinished)
		{
			Debug.Log(LeanTest.formatB("Tests timed out!"));
			LeanTest.overview();
		}
		yield break;
	}

	// Token: 0x04000033 RID: 51
	public float timeout = 15f;
}
