using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class LeanAudioOptions
{
	// Token: 0x0600009B RID: 155 RVA: 0x00003C64 File Offset: 0x00001E64
	public LeanAudioOptions setFrequency(int frequencyRate)
	{
		this.frequencyRate = frequencyRate;
		return this;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00003C70 File Offset: 0x00001E70
	public LeanAudioOptions setVibrato(Vector3[] vibrato)
	{
		this.vibrato = vibrato;
		return this;
	}

	// Token: 0x0400002F RID: 47
	public Vector3[] vibrato;

	// Token: 0x04000030 RID: 48
	public int frequencyRate = 44100;

	// Token: 0x04000031 RID: 49
	public bool useSetData = true;

	// Token: 0x04000032 RID: 50
	public LeanAudioStream stream;
}
