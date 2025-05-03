using UnityEngine;

public class LogoCinematic : MonoBehaviour
{
	public GameObject lean;

	public GameObject tween;

	private void Awake()
	{
	}

	private void Start()
	{
		tween.transform.localPosition += -Vector3.right * 15f;
		LeanTween.moveLocalX(tween, tween.transform.localPosition.x + 15f, 0.4f).setEase(LeanTweenType.linear).setDelay(0f)
			.setOnComplete(playBoom);
		tween.transform.RotateAround(tween.transform.position, Vector3.forward, -30f);
		LeanTween.rotateAround(tween, Vector3.forward, 30f, 0.4f).setEase(LeanTweenType.easeInQuad).setDelay(0.4f)
			.setOnComplete(playBoom);
		lean.transform.position += Vector3.up * 5.1f;
		LeanTween.moveY(lean, lean.transform.position.y - 5.1f, 0.6f).setEase(LeanTweenType.easeInQuad).setDelay(0.6f)
			.setOnComplete(playBoom);
	}

	private void playBoom()
	{
		AnimationCurve volume = new AnimationCurve(new Keyframe(-0.001454365f, 0.006141067f, -3.698472f, -3.698472f), new Keyframe(0.007561419f, 1.006896f, -3.613532f, -3.613532f), new Keyframe(0.9999977f, 0.00601998f, -0.1788428f, -0.1788428f));
		AnimationCurve frequency = new AnimationCurve(new Keyframe(0f, 0.001724138f, 0.01912267f, 0.01912267f), new Keyframe(0.9981073f, 0.007586207f, 0f, 0f));
		AudioClip audio = LeanAudio.createAudio(volume, frequency, LeanAudio.options().setVibrato(new Vector3[1]
		{
			new Vector3(0.1f, 0f, 0f)
		}).setFrequency(11025));
		LeanAudio.play(audio, Vector3.zero);
	}
}
