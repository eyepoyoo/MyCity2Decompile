using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitySampleAssets.Effects;

namespace UnitySampleAssets.SceneUtils
{
	public class ParticleSceneControls : MonoBehaviour
	{
		public enum Mode
		{
			Activate = 0,
			Instantiate = 1,
			Trail = 2
		}

		public enum AlignMode
		{
			Normal = 0,
			Up = 1
		}

		[Serializable]
		public class DemoParticleSystem
		{
			public Transform transform;

			public Mode mode;

			public AlignMode align;

			public int maxCount;

			public float minDist;

			public int camOffset = 15;

			public string instructionText;
		}

		[Serializable]
		public class DemoParticleSystemList
		{
			public DemoParticleSystem[] items;
		}

		public DemoParticleSystemList demoParticles;

		public float distFromSurface = 0.5f;

		public float multiply = 1f;

		public bool clearOnChange;

		public Text titleGuiText;

		public Transform demoCam;

		public Text interactionGuiText;

		public Button previousButton;

		public Button nextButton;

		private ParticleSystemMultiplier particleMultiplier;

		private List<Transform> currentParticleList = new List<Transform>();

		private Transform instance;

		private static int selectedIndex;

		private Vector3 camOffsetVelocity = Vector3.zero;

		private Vector3 lastPos;

		private static DemoParticleSystem selected;

		private void Awake()
		{
			Select(selectedIndex);
			previousButton.onClick.AddListener(Previous);
			nextButton.onClick.AddListener(Next);
		}

		private void OnDisable()
		{
			previousButton.onClick.RemoveAllListeners();
			previousButton.onClick.RemoveAllListeners();
		}

		private void Previous()
		{
			selectedIndex--;
			if (selectedIndex == -1)
			{
				selectedIndex = demoParticles.items.Length - 1;
			}
			Select(selectedIndex);
		}

		public void Next()
		{
			selectedIndex++;
			if (selectedIndex == demoParticles.items.Length)
			{
				selectedIndex = 0;
			}
			Select(selectedIndex);
		}

		private void Update()
		{
			demoCam.localPosition = Vector3.SmoothDamp(demoCam.localPosition, Vector3.forward * -selected.camOffset, ref camOffsetVelocity, 1f);
			if (selected.mode == Mode.Activate)
			{
				return;
			}
			bool flag = Input.GetMouseButtonDown(0) && selected.mode == Mode.Instantiate;
			bool flag2 = Input.GetMouseButton(0) && selected.mode == Mode.Trail;
			if (!flag && !flag2)
			{
				return;
			}
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo))
			{
				return;
			}
			Quaternion rotation = Quaternion.LookRotation(hitInfo.normal);
			if (selected.align == AlignMode.Up)
			{
				rotation = Quaternion.identity;
			}
			Vector3 vector = hitInfo.point + hitInfo.normal * distFromSurface;
			if (!((vector - lastPos).magnitude > selected.minDist))
			{
				return;
			}
			if (selected.mode != Mode.Trail || instance == null)
			{
				instance = (Transform)UnityEngine.Object.Instantiate(selected.transform, vector, rotation);
				if (particleMultiplier != null)
				{
					instance.GetComponent<ParticleSystemMultiplier>().multiplier = multiply;
				}
				currentParticleList.Add(instance);
				if (selected.maxCount > 0 && currentParticleList.Count > selected.maxCount)
				{
					if (currentParticleList[0] != null)
					{
						UnityEngine.Object.Destroy(currentParticleList[0].gameObject);
					}
					currentParticleList.RemoveAt(0);
				}
			}
			else
			{
				instance.position = vector;
				instance.rotation = rotation;
			}
			if (selected.mode == Mode.Trail)
			{
				instance.transform.GetComponent<ParticleSystem>().enableEmission = false;
				instance.transform.GetComponent<ParticleSystem>().Emit(1);
			}
			instance.parent = hitInfo.transform;
			lastPos = vector;
		}

		private void Select(int i)
		{
			selected = demoParticles.items[i];
			instance = null;
			DemoParticleSystem[] items = demoParticles.items;
			foreach (DemoParticleSystem demoParticleSystem in items)
			{
				if (demoParticleSystem != selected && demoParticleSystem.mode == Mode.Activate)
				{
					demoParticleSystem.transform.gameObject.SetActive(false);
				}
			}
			if (selected.mode == Mode.Activate)
			{
				selected.transform.gameObject.SetActive(true);
			}
			particleMultiplier = selected.transform.GetComponent<ParticleSystemMultiplier>();
			multiply = 1f;
			if (clearOnChange)
			{
				while (currentParticleList.Count > 0)
				{
					UnityEngine.Object.Destroy(currentParticleList[0].gameObject);
					currentParticleList.RemoveAt(0);
				}
			}
			interactionGuiText.text = selected.instructionText;
			titleGuiText.text = selected.transform.name;
		}
	}
}
