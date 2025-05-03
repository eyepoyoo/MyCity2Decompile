using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class LeanAudio : MonoBehaviour
{
	// Token: 0x06000091 RID: 145 RVA: 0x00003734 File Offset: 0x00001934
	public static LeanAudioOptions options()
	{
		if (LeanAudio.generatedWaveDistances == null)
		{
			LeanAudio.generatedWaveDistances = new float[LeanAudio.PROCESSING_ITERATIONS_MAX];
			LeanAudio.longList = new float[LeanAudio.PROCESSING_ITERATIONS_MAX];
		}
		return new LeanAudioOptions();
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00003764 File Offset: 0x00001964
	public static LeanAudioStream createAudioStream(AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options = null)
	{
		if (options == null)
		{
			options = new LeanAudioOptions();
		}
		options.useSetData = false;
		int num = LeanAudio.createAudioWave(volume, frequency, options);
		LeanAudio.createAudioFromWave(num, options);
		return options.stream;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x0000379C File Offset: 0x0000199C
	public static AudioClip createAudio(AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options = null)
	{
		if (options == null)
		{
			options = new LeanAudioOptions();
		}
		int num = LeanAudio.createAudioWave(volume, frequency, options);
		return LeanAudio.createAudioFromWave(num, options);
	}

	// Token: 0x06000094 RID: 148 RVA: 0x000037C8 File Offset: 0x000019C8
	private static int createAudioWave(AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options)
	{
		float time = volume[volume.length - 1].time;
		int num = 0;
		float num2 = 0f;
		for (int i = 0; i < LeanAudio.PROCESSING_ITERATIONS_MAX; i++)
		{
			float num3 = frequency.Evaluate(num2);
			if (num3 < LeanAudio.MIN_FREQEUNCY_PERIOD)
			{
				num3 = LeanAudio.MIN_FREQEUNCY_PERIOD;
			}
			float num4 = volume.Evaluate(num2 + 0.5f * num3);
			if (options.vibrato != null)
			{
				for (int j = 0; j < options.vibrato.Length; j++)
				{
					float num5 = Mathf.Abs(Mathf.Sin(1.5708f + num2 * (1f / options.vibrato[j][0]) * 3.1415927f));
					float num6 = 1f - options.vibrato[j][1];
					num5 = options.vibrato[j][1] + num6 * num5;
					num4 *= num5;
				}
			}
			if (num2 + 0.5f * num3 >= time)
			{
				break;
			}
			if (num >= LeanAudio.PROCESSING_ITERATIONS_MAX - 1)
			{
				Debug.LogError("LeanAudio has reached it's processing cap. To avoid this error increase the number of iterations ex: LeanAudio.PROCESSING_ITERATIONS_MAX = " + LeanAudio.PROCESSING_ITERATIONS_MAX * 2);
				break;
			}
			LeanAudio.generatedWaveDistances[num / 2] = num3;
			num2 += num3;
			LeanAudio.longList[num] = num2;
			LeanAudio.longList[num + 1] = ((i % 2 != 0) ? num4 : (-num4));
			num += 2;
		}
		num += -2;
		LeanAudio.generatedWaveDistancesCount = num / 2;
		return num;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00003958 File Offset: 0x00001B58
	private static AudioClip createAudioFromWave(int waveLength, LeanAudioOptions options)
	{
		float num = LeanAudio.longList[waveLength - 2];
		float[] array = new float[(int)((float)options.frequencyRate * num)];
		int num2 = 0;
		float num3 = LeanAudio.longList[num2];
		float num4 = 0f;
		float num5 = LeanAudio.longList[num2];
		float num6 = LeanAudio.longList[num2 + 1];
		for (int i = 0; i < array.Length; i++)
		{
			float num7 = (float)i / (float)options.frequencyRate;
			if (num7 > LeanAudio.longList[num2])
			{
				num4 = LeanAudio.longList[num2];
				num2 += 2;
				num3 = LeanAudio.longList[num2] - LeanAudio.longList[num2 - 2];
				num6 = LeanAudio.longList[num2 + 1];
			}
			num5 = num7 - num4;
			float num8 = num5 / num3;
			float num9 = Mathf.Sin(num8 * 3.1415927f);
			num9 *= num6;
			array[i] = num9;
		}
		int num10 = array.Length;
		AudioClip audioClip;
		if (options.useSetData)
		{
			audioClip = AudioClip.Create("Generated Audio", num10, 1, options.frequencyRate, false, null, new AudioClip.PCMSetPositionCallback(LeanAudio.OnAudioSetPosition));
			audioClip.SetData(array, 0);
		}
		else
		{
			options.stream = new LeanAudioStream(array);
			Debug.Log(string.Concat(new object[] { "len:", array.Length, " lengthSamples:", num10, " freqRate:", options.frequencyRate }));
			audioClip = AudioClip.Create("Generated Audio", num10, 1, options.frequencyRate, false, new AudioClip.PCMReaderCallback(options.stream.OnAudioRead), new AudioClip.PCMSetPositionCallback(options.stream.OnAudioSetPosition));
			options.stream.audioClip = audioClip;
		}
		return audioClip;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00003B10 File Offset: 0x00001D10
	private static void OnAudioSetPosition(int newPosition)
	{
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00003B14 File Offset: 0x00001D14
	public static AudioClip generateAudioFromCurve(AnimationCurve curve, int frequencyRate = 44100)
	{
		float time = curve[curve.length - 1].time;
		float num = time;
		float[] array = new float[(int)((float)frequencyRate * num)];
		for (int i = 0; i < array.Length; i++)
		{
			float num2 = (float)i / (float)frequencyRate;
			array[i] = curve.Evaluate(num2);
		}
		int num3 = array.Length;
		AudioClip audioClip = AudioClip.Create("Generated Audio", num3, 1, frequencyRate, false);
		audioClip.SetData(array, 0);
		return audioClip;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00003B90 File Offset: 0x00001D90
	public static AudioSource play(AudioClip audio, [Optional] Vector3 pos, float volume = 1f, float pitch = 1f)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.position = pos;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.volume = volume;
		audioSource.pitch = pitch;
		audioSource.clip = audio;
		audioSource.Play();
		global::UnityEngine.Object.Destroy(gameObject, audio.length);
		return audioSource;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00003BE0 File Offset: 0x00001DE0
	public static void printOutAudioClip(AudioClip audioClip, ref AnimationCurve curve, float scaleX = 1f)
	{
		float[] array = new float[audioClip.samples * audioClip.channels];
		audioClip.GetData(array, 0);
		int i = 0;
		Keyframe[] array2 = new Keyframe[array.Length];
		while (i < array.Length)
		{
			array2[i] = new Keyframe((float)i * scaleX, array[i]);
			i++;
		}
		curve = new AnimationCurve(array2);
	}

	// Token: 0x0400002A RID: 42
	public static float MIN_FREQEUNCY_PERIOD = 0.000115f;

	// Token: 0x0400002B RID: 43
	public static int PROCESSING_ITERATIONS_MAX = 50000;

	// Token: 0x0400002C RID: 44
	public static float[] generatedWaveDistances;

	// Token: 0x0400002D RID: 45
	public static int generatedWaveDistancesCount;

	// Token: 0x0400002E RID: 46
	private static float[] longList;
}
