using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class LeanAudioStream
{
	// Token: 0x0600008C RID: 140 RVA: 0x000036B8 File Offset: 0x000018B8
	public LeanAudioStream(float[] audioArr)
	{
		this.audioArr = audioArr;
	}

	// Token: 0x0600008D RID: 141 RVA: 0x000036C8 File Offset: 0x000018C8
	public void OnAudioRead(float[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = this.audioArr[this.position];
			this.position++;
		}
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00003708 File Offset: 0x00001908
	public void OnAudioSetPosition(int newPosition)
	{
		this.position = newPosition;
	}

	// Token: 0x04000027 RID: 39
	public int position;

	// Token: 0x04000028 RID: 40
	public AudioClip audioClip;

	// Token: 0x04000029 RID: 41
	public float[] audioArr;
}
