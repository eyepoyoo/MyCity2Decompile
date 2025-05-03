using System;
using System.Collections;
using UnityEngine;

public class TestingEverything : MonoBehaviour
{
	public GameObject cube1;

	public GameObject cube2;

	public GameObject cube3;

	public GameObject cube4;

	private bool eventGameObjectWasCalled;

	private bool eventGeneralWasCalled;

	private int lt1Id;

	private LTDescr lt2;

	private LTDescr lt3;

	private LTDescr lt4;

	private LTDescr[] groupTweens;

	private GameObject[] groupGOs;

	private int groupTweensCnt;

	private int rotateRepeat;

	private int rotateRepeatAngle;

	private GameObject boxNoCollider;

	private float timeElapsedNormalTimeScale;

	private float timeElapsedIgnoreTimeScale;

	private void Awake()
	{
		boxNoCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
		UnityEngine.Object.Destroy(boxNoCollider.GetComponent(typeof(BoxCollider)));
	}

	private void Start()
	{
		LeanTest.timeout = 45f;
		LeanTest.expected = 31;
		LeanTween.init(1210);
		LeanTween.addListener(cube1, 0, eventGameObjectCalled);
		LeanTest.expect(!LeanTween.isTweening(), "NOTHING TWEEENING AT BEGINNING");
		LeanTest.expect(!LeanTween.isTweening(cube1), "OBJECT NOT TWEEENING AT BEGINNING");
		LeanTween.scaleX(cube4, 2f, 0f).setOnComplete((Action)delegate
		{
			LeanTest.expect(cube4.transform.localScale.x == 2f, "TWEENED WITH ZERO TIME");
		});
		LeanTween.dispatchEvent(0);
		LeanTest.expect(eventGameObjectWasCalled, "EVENT GAMEOBJECT RECEIVED");
		LeanTest.expect(!LeanTween.removeListener(cube2, 0, eventGameObjectCalled), "EVENT GAMEOBJECT NOT REMOVED");
		LeanTest.expect(LeanTween.removeListener(cube1, 0, eventGameObjectCalled), "EVENT GAMEOBJECT REMOVED");
		LeanTween.addListener(1, eventGeneralCalled);
		LeanTween.dispatchEvent(1);
		LeanTest.expect(eventGeneralWasCalled, "EVENT ALL RECEIVED");
		LeanTest.expect(LeanTween.removeListener(1, eventGeneralCalled), "EVENT ALL REMOVED");
		lt1Id = LeanTween.move(cube1, new Vector3(3f, 2f, 0.5f), 1.1f).id;
		LeanTween.move(cube2, new Vector3(-3f, -2f, -0.5f), 1.1f);
		LeanTween.reset();
		LTSpline lTSpline = new LTSpline(new Vector3(-1f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(4f, 0f, 0f), new Vector3(20f, 0f, 0f), new Vector3(30f, 0f, 0f));
		lTSpline.place(cube4.transform, 0.5f);
		LeanTest.expect(Vector3.Distance(cube4.transform.position, new Vector3(10f, 0f, 0f)) <= 0.7f, "SPLINE POSITIONING AT HALFWAY", string.Concat("position is:", cube4.transform.position, " but should be:(10f,0f,0f)"));
		LeanTween.color(cube4, Color.green, 0.01f);
		GameObject gameObject = UnityEngine.Object.Instantiate(boxNoCollider);
		gameObject.name = "normalTimeScale";
		LeanTween.moveX(gameObject, 12f, 1f).setIgnoreTimeScale(false).setOnComplete((Action)delegate
		{
			timeElapsedNormalTimeScale = Time.time;
		});
		LTDescr[] array = LeanTween.descriptions(gameObject);
		LeanTest.expect(array.Length >= 0 && array[0].to.x == 12f, "WE CAN RETRIEVE A DESCRIPTION");
		gameObject = UnityEngine.Object.Instantiate(boxNoCollider);
		gameObject.name = "ignoreTimeScale";
		LeanTween.moveX(gameObject, 5f, 1f).setIgnoreTimeScale(true).setOnComplete((Action)delegate
		{
			timeElapsedIgnoreTimeScale = Time.time;
		});
		StartCoroutine(timeBasedTesting());
	}

	private IEnumerator timeBasedTesting()
	{
		yield return new WaitForSeconds(1f);
		yield return new WaitForEndOfFrame();
		LeanTest.expect(Mathf.Abs(timeElapsedNormalTimeScale - timeElapsedIgnoreTimeScale) < 0.15f, "START IGNORE TIMING", "timeElapsedIgnoreTimeScale:" + timeElapsedIgnoreTimeScale + " timeElapsedNormalTimeScale:" + timeElapsedNormalTimeScale);
		Time.timeScale = 4f;
		int pauseCount = 0;
		LeanTween.value(base.gameObject, 0f, 1f, 1f).setOnUpdate((Action<float>)delegate
		{
			pauseCount++;
		}).pause();
		Vector3[] roundCirc = new Vector3[16]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(-9.1f, 25.1f, 0f),
			new Vector3(-1.2f, 15.9f, 0f),
			new Vector3(-25f, 25f, 0f),
			new Vector3(-25f, 25f, 0f),
			new Vector3(-50.1f, 15.9f, 0f),
			new Vector3(-40.9f, 25.1f, 0f),
			new Vector3(-50f, 0f, 0f),
			new Vector3(-50f, 0f, 0f),
			new Vector3(-40.9f, -25.1f, 0f),
			new Vector3(-50.1f, -15.9f, 0f),
			new Vector3(-25f, -25f, 0f),
			new Vector3(-25f, -25f, 0f),
			new Vector3(0f, -15.9f, 0f),
			new Vector3(-9.1f, -25.1f, 0f),
			new Vector3(0f, 0f, 0f)
		};
		GameObject cubeRound = UnityEngine.Object.Instantiate(boxNoCollider);
		cubeRound.name = "bRound";
		Vector3 onStartPos = cubeRound.transform.position;
		LeanTween.moveLocal(cubeRound, roundCirc, 0.5f).setOnComplete((Action)delegate
		{
			LeanTest.expect(cubeRound.transform.position == onStartPos, "BEZIER CLOSED LOOP SHOULD END AT START", string.Concat("onStartPos:", onStartPos, " onEnd:", cubeRound.transform.position));
		});
		Vector3[] roundSpline = new Vector3[6]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(0.9f, 2f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f)
		};
		GameObject cubeSpline = UnityEngine.Object.Instantiate(boxNoCollider);
		cubeSpline.name = "bSpline";
		Vector3 onStartPosSpline = cubeSpline.transform.position;
		LeanTween.moveSplineLocal(cubeSpline, roundSpline, 0.5f).setOnComplete((Action)delegate
		{
			LeanTest.expect(Vector3.Distance(onStartPosSpline, cubeSpline.transform.position) <= 0.01f, "BEZIER CLOSED LOOP SHOULD END AT START", string.Concat("onStartPos:", onStartPosSpline, " onEnd:", cubeSpline.transform.position, " dist:", Vector3.Distance(onStartPosSpline, cubeSpline.transform.position)));
		});
		groupTweens = new LTDescr[1200];
		groupGOs = new GameObject[groupTweens.Length];
		groupTweensCnt = 0;
		int descriptionMatchCount = 0;
		for (int i = 0; i < groupTweens.Length; i++)
		{
			GameObject cube = UnityEngine.Object.Instantiate(boxNoCollider);
			cube.name = "c" + i;
			cube.transform.position = new Vector3(0f, 0f, i * 3);
			groupGOs[i] = cube;
		}
		yield return new WaitForEndOfFrame();
		bool hasGroupTweensCheckStarted = false;
		int setOnStartNum = 0;
		for (int i2 = 0; i2 < groupTweens.Length; i2++)
		{
			groupTweens[i2] = LeanTween.move(groupGOs[i2], base.transform.position + Vector3.one * 3f, 3f).setOnStart(delegate
			{
				setOnStartNum++;
			}).setOnComplete((Action)delegate
			{
				if (!hasGroupTweensCheckStarted)
				{
					hasGroupTweensCheckStarted = true;
					LeanTween.delayedCall(base.gameObject, 0.1f, (Action)delegate
					{
						LeanTest.expect(setOnStartNum == groupTweens.Length, "SETONSTART CALLS", "expected:" + groupTweens.Length + " was:" + setOnStartNum);
						LeanTest.expect(groupTweensCnt == groupTweens.Length, "GROUP FINISH", "expected " + groupTweens.Length + " tweens but got " + groupTweensCnt);
					});
				}
				groupTweensCnt++;
			});
			if (LeanTween.description(groupTweens[i2].id).trans == groupTweens[i2].trans)
			{
				descriptionMatchCount++;
			}
		}
		while (LeanTween.tweensRunning < groupTweens.Length)
		{
			yield return null;
		}
		LeanTest.expect(descriptionMatchCount == groupTweens.Length, "GROUP IDS MATCH");
		LeanTest.expect(LeanTween.maxSearch <= groupTweens.Length + 5, "MAX SEARCH OPTIMIZED", "maxSearch:" + LeanTween.maxSearch);
		LeanTest.expect(LeanTween.isTweening(), "SOMETHING IS TWEENING");
		float previousXlt4 = cube4.transform.position.x;
		lt4 = LeanTween.moveX(cube4, 5f, 1.1f).setOnComplete((Action)delegate
		{
			LeanTest.expect(cube4 != null && previousXlt4 != cube4.transform.position.x, "RESUME OUT OF ORDER", string.Concat("cube4:", cube4, " previousXlt4:", previousXlt4, " cube4.transform.position.x:", (!(cube4 != null)) ? 0f : cube4.transform.position.x));
		});
		lt4.resume();
		TestingEverything testingEverything = this;
		TestingEverything testingEverything2 = this;
		int num = 0;
		testingEverything2.rotateRepeatAngle = 0;
		testingEverything.rotateRepeat = num;
		LeanTween.rotateAround(cube3, Vector3.forward, 360f, 0.1f).setRepeat(3).setOnComplete(rotateRepeatFinished)
			.setOnCompleteOnRepeat(true)
			.setDestroyOnComplete(true);
		yield return new WaitForEndOfFrame();
		LeanTween.delayedCall(1.8f, rotateRepeatAllFinished);
		int countBeforeCancel = LeanTween.tweensRunning;
		LeanTween.cancel(lt1Id);
		LeanTest.expect(countBeforeCancel == LeanTween.tweensRunning, "CANCEL AFTER RESET SHOULD FAIL", "expected " + countBeforeCancel + " but got " + LeanTween.tweensRunning);
		LeanTween.cancel(cube2);
		int tweenCount = 0;
		for (int i3 = 0; i3 < groupTweens.Length; i3++)
		{
			if (LeanTween.isTweening(groupGOs[i3]))
			{
				tweenCount++;
			}
			if (i3 % 3 == 0)
			{
				LeanTween.pause(groupGOs[i3]);
			}
			else if (i3 % 3 == 1)
			{
				groupTweens[i3].pause();
			}
			else
			{
				LeanTween.pause(groupTweens[i3].id);
			}
		}
		LeanTest.expect(tweenCount == groupTweens.Length, "GROUP ISTWEENING", "expected " + groupTweens.Length + " tweens but got " + tweenCount);
		yield return new WaitForEndOfFrame();
		tweenCount = 0;
		for (int i4 = 0; i4 < groupTweens.Length; i4++)
		{
			if (i4 % 3 == 0)
			{
				LeanTween.resume(groupGOs[i4]);
			}
			else if (i4 % 3 == 1)
			{
				groupTweens[i4].resume();
			}
			else
			{
				LeanTween.resume(groupTweens[i4].id);
			}
			if ((i4 % 2 != 0) ? LeanTween.isTweening(groupGOs[i4]) : LeanTween.isTweening(groupTweens[i4].id))
			{
				tweenCount++;
			}
		}
		LeanTest.expect(tweenCount == groupTweens.Length, "GROUP RESUME");
		LeanTest.expect(!LeanTween.isTweening(cube1), "CANCEL TWEEN LTDESCR");
		LeanTest.expect(!LeanTween.isTweening(cube2), "CANCEL TWEEN LEANTWEEN");
		LeanTest.expect(pauseCount == 0, "ON UPDATE NOT CALLED DURING PAUSE", "expect pause count of 0, but got " + pauseCount);
		yield return new WaitForEndOfFrame();
		Time.timeScale = 0.25f;
		float tweenTime = 0.2f;
		float expectedTime = tweenTime * (1f / Time.timeScale);
		float start = Time.realtimeSinceStartup;
		bool onUpdateWasCalled = false;
		LeanTween.moveX(cube1, -5f, tweenTime).setOnUpdate((Action<float>)delegate
		{
			onUpdateWasCalled = true;
		}).setOnComplete((Action)delegate
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float num3 = realtimeSinceStartup - start;
			LeanTest.expect(Mathf.Abs(expectedTime - num3) < 0.05f, "SCALED TIMING DIFFERENCE", "expected to complete in roughly " + expectedTime + " but completed in " + num3);
			LeanTest.expect(Mathf.Approximately(cube1.transform.position.x, -5f), "SCALED ENDING POSITION", "expected to end at -5f, but it ended at " + cube1.transform.position.x);
			LeanTest.expect(onUpdateWasCalled, "ON UPDATE FIRED");
		});
		yield return new WaitForSeconds(expectedTime);
		Time.timeScale = 1f;
		int ltCount = 0;
		GameObject[] allGos = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		GameObject[] array = allGos;
		foreach (GameObject go in array)
		{
			if (go.name == "~LeanTween")
			{
				ltCount++;
			}
		}
		LeanTest.expect(ltCount == 1, "RESET CORRECTLY CLEANS UP");
		lotsOfCancels();
	}

	private IEnumerator lotsOfCancels()
	{
		yield return new WaitForEndOfFrame();
		Time.timeScale = 4f;
		int cubeCount = 10;
		int[] tweensA = new int[cubeCount];
		GameObject[] aGOs = new GameObject[cubeCount];
		for (int i = 0; i < aGOs.Length; i++)
		{
			GameObject cube = UnityEngine.Object.Instantiate(boxNoCollider);
			cube.transform.position = new Vector3(0f, 0f, (float)i * 2f);
			cube.name = "a" + i;
			aGOs[i] = cube;
			tweensA[i] = LeanTween.move(cube, cube.transform.position + new Vector3(10f, 0f, 0f), 0.5f + 1f * (1f / (float)aGOs.Length)).id;
			LeanTween.color(cube, Color.red, 0.01f);
		}
		yield return new WaitForSeconds(1f);
		int[] tweensB = new int[cubeCount];
		GameObject[] bGOs = new GameObject[cubeCount];
		for (int j = 0; j < bGOs.Length; j++)
		{
			GameObject cube2 = UnityEngine.Object.Instantiate(boxNoCollider);
			cube2.transform.position = new Vector3(0f, 0f, (float)j * 2f);
			cube2.name = "b" + j;
			bGOs[j] = cube2;
			tweensB[j] = LeanTween.move(cube2, cube2.transform.position + new Vector3(10f, 0f, 0f), 2f).id;
		}
		for (int k = 0; k < aGOs.Length; k++)
		{
			LeanTween.cancel(aGOs[k]);
			GameObject cube3 = aGOs[k];
			tweensA[k] = LeanTween.move(cube3, new Vector3(0f, 0f, (float)k * 2f), 2f).id;
		}
		yield return new WaitForSeconds(0.5f);
		for (int l = 0; l < aGOs.Length; l++)
		{
			LeanTween.cancel(aGOs[l]);
			GameObject cube4 = aGOs[l];
			tweensA[l] = LeanTween.move(cube4, new Vector3(0f, 0f, (float)l * 2f) + new Vector3(10f, 0f, 0f), 2f).id;
		}
		for (int m = 0; m < bGOs.Length; m++)
		{
			LeanTween.cancel(bGOs[m]);
			GameObject cube5 = bGOs[m];
			tweensB[m] = LeanTween.move(cube5, new Vector3(0f, 0f, (float)m * 2f), 2f).id;
		}
		yield return new WaitForSeconds(2.1f);
		bool inFinalPlace = true;
		for (int n = 0; n < aGOs.Length; n++)
		{
			if (Vector3.Distance(aGOs[n].transform.position, new Vector3(0f, 0f, (float)n * 2f) + new Vector3(10f, 0f, 0f)) > 0.1f)
			{
				inFinalPlace = false;
			}
		}
		for (int num = 0; num < bGOs.Length; num++)
		{
			if (Vector3.Distance(bGOs[num].transform.position, new Vector3(0f, 0f, (float)num * 2f)) > 0.1f)
			{
				inFinalPlace = false;
			}
		}
		LeanTest.expect(inFinalPlace, "AFTER LOTS OF CANCELS");
	}

	private void rotateRepeatFinished()
	{
		if (Mathf.Abs(cube3.transform.eulerAngles.z) < 0.0001f)
		{
			rotateRepeatAngle++;
		}
		rotateRepeat++;
	}

	private void rotateRepeatAllFinished()
	{
		LeanTest.expect(rotateRepeatAngle == 3, "ROTATE AROUND MULTIPLE", "expected 3 times received " + rotateRepeatAngle + " times");
		LeanTest.expect(rotateRepeat == 3, "ROTATE REPEAT");
		LeanTest.expect(cube3 == null, "DESTROY ON COMPLETE", "cube3:" + cube3);
	}

	private void eventGameObjectCalled(LTEvent e)
	{
		eventGameObjectWasCalled = true;
	}

	private void eventGeneralCalled(LTEvent e)
	{
		eventGeneralWasCalled = true;
	}
}
