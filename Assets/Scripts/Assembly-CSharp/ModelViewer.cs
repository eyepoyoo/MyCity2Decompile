using System;
using UnityEngine;

public class ModelViewer : MonoBehaviour
{
	private const float SPEED_MOD = 1.5f;

	private GUISkin viewerSkin;

	public Transform pivot;

	public GameObject[] modelArray;

	public eCenteringMethod centeringMethod;

	public float scrollWheelSpeed = 500f;

	public Color topColour;

	public Color bottomColour;

	public Shader skyboxShader;

	public GUIStyle titleTextStyle;

	private int currentIndex;

	private GameObject currentModelInstance;

	private bool wasMouseDown;

	private Vector2 previousMousePos = new Vector2(0f, 0f);

	private Bounds currentBounds;

	private Vector3 lastForward = Vector3.forward;

	private Vector3 initialLocalPos;

	private int _pCurrentIndex
	{
		get
		{
			if (modelArray == null)
			{
				return 0;
			}
			if (modelArray.Length == 0)
			{
				return 0;
			}
			if (modelArray.Length == 1)
			{
				return 0;
			}
			if (currentIndex >= modelArray.Length)
			{
				currentIndex = 0;
			}
			if (currentIndex < 0)
			{
				currentIndex = modelArray.Length - 1;
			}
			return currentIndex;
		}
		set
		{
			currentIndex = value;
			if (currentIndex >= modelArray.Length)
			{
				currentIndex = 0;
			}
			if (currentIndex < 0)
			{
				currentIndex = modelArray.Length - 1;
			}
		}
	}

	private void Start()
	{
		initialLocalPos = base.transform.localPosition;
		createSkybox();
		if (modelArray != null && modelArray.Length != 0 && !(pivot == null))
		{
			viewerSkin = (GUISkin)ScriptableObject.CreateInstance(typeof(GUISkin));
			viewerSkin.label.alignment = TextAnchor.MiddleCenter;
			refreshInstance();
		}
	}

	private void refreshInstance()
	{
		lastForward = Vector3.forward;
		if (currentModelInstance != null)
		{
			lastForward = currentModelInstance.transform.forward;
			UnityEngine.Object.Destroy(currentModelInstance);
		}
		if (modelArray == null || modelArray.Length == 0)
		{
			return;
		}
		if (modelArray[_pCurrentIndex] == null)
		{
			_pCurrentIndex++;
		}
		if (modelArray[_pCurrentIndex] == null)
		{
			return;
		}
		try
		{
			currentModelInstance = (GameObject)UnityEngine.Object.Instantiate(modelArray[_pCurrentIndex], Vector3.zero, Quaternion.identity);
		}
		catch (Exception ex)
		{
			if (ex.Message == null)
			{
			}
		}
		if (currentModelInstance == null)
		{
			Debug.Log("Failed to create prefab.");
			return;
		}
		MonoBehaviour[] componentsInChildren = currentModelInstance.GetComponentsInChildren<MonoBehaviour>();
		MonoBehaviour[] array = componentsInChildren;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (!(monoBehaviour == null))
			{
				monoBehaviour.enabled = false;
			}
		}
		Animator component = currentModelInstance.GetComponent<Animator>();
		if (component != null)
		{
			component.speed = 1f;
		}
		switch (centeringMethod)
		{
		case eCenteringMethod.BOUNDING_BOX:
		{
			currentBounds = new Bounds(Vector3.zero, new Vector3(0.01f, 0.01f, 0.01f));
			Renderer[] componentsInChildren2 = currentModelInstance.GetComponentsInChildren<Renderer>();
			Renderer[] array2 = componentsInChildren2;
			foreach (Renderer renderer in array2)
			{
				currentBounds.Encapsulate(renderer.bounds.center);
			}
			pivot.transform.position = currentBounds.center;
			currentModelInstance.transform.forward = lastForward;
			break;
		}
		case eCenteringMethod.MODEL_PIVOT:
			pivot.transform.position = Vector3.zero;
			break;
		}
		base.transform.localPosition = initialLocalPos;
	}

	private void Update()
	{
		if (!(currentModelInstance == null))
		{
			UpdateControls();
		}
	}

	private void UpdateControls()
	{
		float num = pivot.eulerAngles.x % 360f;
		if (num > 180f)
		{
			num -= 360f;
		}
		if (Input.GetKeyUp("."))
		{
			_pCurrentIndex++;
			refreshInstance();
		}
		if (Input.GetKeyUp(","))
		{
			_pCurrentIndex--;
			refreshInstance();
		}
		if (num < 90f && (double)Input.GetAxisRaw("Vertical") < -0.1)
		{
			pivot.Rotate(new Vector3(1.5f, 0f, 0f), Space.World);
		}
		if (num > -60f && (double)Input.GetAxisRaw("Vertical") > 0.1)
		{
			pivot.Rotate(new Vector3(-1.5f, 0f, 0f), Space.World);
		}
		if ((double)Input.GetAxis("Horizontal") < -0.1)
		{
			currentModelInstance.transform.Rotate(new Vector3(0f, 1.5f, 0f), Space.World);
		}
		if ((double)Input.GetAxis("Horizontal") > 0.1)
		{
			currentModelInstance.transform.Rotate(new Vector3(0f, -1.5f, 0f), Space.World);
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		base.transform.position += base.transform.forward * (axis * Time.deltaTime * scrollWheelSpeed);
		if (wasMouseDown)
		{
			Vector2 vector = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			vector -= previousMousePos;
			pivot.Rotate(new Vector3(vector.y, 0f, 0f), Space.World);
			currentModelInstance.transform.Rotate(new Vector3(0f, 0f - vector.x, 0f), Space.World);
		}
		wasMouseDown = Input.GetKey(KeyCode.Mouse0);
		previousMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	}

	private void OnGUI()
	{
		if (!(currentModelInstance == null))
		{
			GUI.skin = viewerSkin;
			string text = currentModelInstance.name.Substring(0, currentModelInstance.name.IndexOf('('));
			GUI.Label(new Rect(0f, 10f, Screen.width, 30f), text, titleTextStyle);
		}
	}

	private void OnDrawGizmos()
	{
	}

	private void createSkybox()
	{
		Material material = new Material(skyboxShader);
		material.name = "CustomSkybox";
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(0, 0, topColour);
		texture2D.Apply();
		texture2D.wrapMode = TextureWrapMode.Clamp;
		material.SetTexture("_UpTex", texture2D);
		Texture2D texture2D2 = new Texture2D(1, 1);
		texture2D2.SetPixel(0, 0, bottomColour);
		texture2D2.Apply();
		texture2D2.wrapMode = TextureWrapMode.Clamp;
		material.SetTexture("_DownTex", texture2D2);
		int num = 512;
		Texture2D texture2D3 = new Texture2D(1, num);
		texture2D3.wrapMode = TextureWrapMode.Clamp;
		for (int i = 0; i < num; i++)
		{
			texture2D3.SetPixel(0, i, Color.Lerp(bottomColour, topColour, (float)i / (float)num));
		}
		texture2D3.Apply();
		material.SetTexture("_FrontTex", texture2D3);
		material.SetTexture("_BackTex", texture2D3);
		material.SetTexture("_LeftTex", texture2D3);
		material.SetTexture("_RightTex", texture2D3);
		RenderSettings.skybox = material;
	}
}
