using System;
using LitJson;
using UnityEngine;

public class ObjectViewer : MonoBehaviour
{
	private const float SPEED_MOD = 1.5f;

	private const float BUTTON_HEIGHT_DECIMAL = 0.2f;

	private const float BUTTON_WIDTH_DECIMAL = 0.2f;

	private GUISkin viewerSkin;

	public Transform pivot;

	public PreviewObject[] objectArray;

	public Color topColour;

	public Color bottomColour;

	public Shader skyboxShader;

	private int currentIndex;

	private GameObject currentGameObjectInstance;

	private Texture2D currentTextureInstance;

	private AudioClip currentAudioInstance;

	private MovieTextureInfo currentMovieInfo;

	private string downloadedText = string.Empty;

	private string errorMessage = string.Empty;

	private string objectName = string.Empty;

	private bool hasInited;

	private bool hasActiveInstance;

	private bool wasMouseDown;

	private Vector2 scrollPosition = Vector2.zero;

	private Vector2 previousMousePos = new Vector2(0f, 0f);

	private Bounds currentBounds;

	private Vector3 lastForward = -Vector3.forward;

	public float imageScale = 1f;

	private Vector3 initialLocalPos;

	private GUIStyle displayStyle;

	private Rect scrollAreaRect = new Rect(1f, 1f, 1f, 1f);

	private float screenWidth;

	private float screenHeight;

	private int _pCurrentIndex
	{
		get
		{
			if (objectArray == null)
			{
				return 0;
			}
			if (objectArray.Length == 0)
			{
				return 0;
			}
			if (objectArray.Length == 1)
			{
				return 0;
			}
			if (currentIndex >= objectArray.Length)
			{
				currentIndex = 0;
			}
			if (currentIndex < 0)
			{
				currentIndex = objectArray.Length - 1;
			}
			return currentIndex;
		}
		set
		{
			currentIndex = value;
			if (currentIndex >= objectArray.Length)
			{
				currentIndex = 0;
			}
			if (currentIndex < 0)
			{
				currentIndex = objectArray.Length - 1;
			}
		}
	}

	public void clear()
	{
		imageScale = 1f;
		if (currentMovieInfo != null)
		{
			currentMovieInfo.stopMovie();
			currentMovieInfo = null;
		}
		objectName = string.Empty;
		errorMessage = string.Empty;
		downloadedText = string.Empty;
		Utils.DestroyGameObject(currentGameObjectInstance);
		currentGameObjectInstance = null;
		currentTextureInstance = null;
		currentAudioInstance = null;
		hasActiveInstance = false;
	}

	public void init()
	{
		if (objectArray != null && objectArray.Length != 0 && !(pivot == null))
		{
			hasInited = true;
			currentIndex = 0;
			refreshInstance();
		}
	}

	private void Start()
	{
		initialLocalPos = base.transform.localPosition;
		createSkybox();
		viewerSkin = (GUISkin)ScriptableObject.CreateInstance(typeof(GUISkin));
		viewerSkin.label.alignment = TextAnchor.MiddleCenter;
		if (!hasInited)
		{
			init();
		}
	}

	private void refreshInstance()
	{
		lastForward = -Vector3.forward;
		if (hasActiveInstance)
		{
			if (currentGameObjectInstance != null)
			{
				lastForward = currentGameObjectInstance.transform.forward;
			}
			clear();
		}
		if (objectArray == null || objectArray.Length == 0)
		{
			return;
		}
		if (objectArray[_pCurrentIndex] == null || !objectArray[_pCurrentIndex].isPopulated())
		{
			_pCurrentIndex++;
		}
		if (objectArray[_pCurrentIndex] == null || !objectArray[_pCurrentIndex].isPopulated())
		{
			Debug.Log("objectArray has two consecutive null entries");
			return;
		}
		try
		{
			switch (objectArray[_pCurrentIndex].type)
			{
			case PreviewType.TEXTURE:
				currentTextureInstance = (Texture2D)objectArray[_pCurrentIndex].previewObject;
				hasActiveInstance = currentTextureInstance != null;
				break;
			case PreviewType.VIDEO:
				hasActiveInstance = false;
				break;
			case PreviewType.GAME_OBJECT:
				currentGameObjectInstance = (GameObject)UnityEngine.Object.Instantiate(objectArray[_pCurrentIndex].previewObject, Vector3.zero, Quaternion.identity);
				hasActiveInstance = currentGameObjectInstance != null;
				break;
			case PreviewType.AUDIO:
				currentAudioInstance = (AudioClip)objectArray[_pCurrentIndex].previewObject;
				hasActiveInstance = currentAudioInstance != null;
				break;
			case PreviewType.TEXT:
				downloadedText = objectArray[_pCurrentIndex].downloadedText;
				hasActiveInstance = !string.IsNullOrEmpty(downloadedText);
				break;
			}
		}
		catch (Exception ex)
		{
			if (ex.Message == null)
			{
				Debug.Log("How odd.");
			}
		}
		if (!hasActiveInstance)
		{
			errorMessage = "Failed to preview object [" + objectArray[_pCurrentIndex].name + "]";
			hasActiveInstance = true;
			return;
		}
		objectName = objectArray[_pCurrentIndex].name;
		if (currentGameObjectInstance != null)
		{
			setUpGameObject();
		}
		if (!string.IsNullOrEmpty(downloadedText))
		{
			setUpText();
		}
	}

	private void setUpText()
	{
		if (!downloadedText.StartsWith("<") && (downloadedText.StartsWith("{") || downloadedText.StartsWith("[")))
		{
			JsonData jsonData = null;
			try
			{
				JsonReader reader = new JsonReader(downloadedText);
				jsonData = JsonMapper.ToObject(reader);
			}
			catch (Exception ex)
			{
				Debug.Log("Failed to parse suspected JSON file. Error [" + ex.Message + "]");
			}
			if (jsonData != null)
			{
				downloadedText = jsonData.ToJson(false, true);
			}
		}
	}

	private void setUpMovie()
	{
		currentMovieInfo = null;
	}

	private void setUpGameObject()
	{
		if (currentGameObjectInstance == null)
		{
			return;
		}
		MonoBehaviour[] componentsInChildren = currentGameObjectInstance.GetComponentsInChildren<MonoBehaviour>();
		MonoBehaviour[] array = componentsInChildren;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (!(monoBehaviour == null))
			{
				monoBehaviour.enabled = false;
			}
		}
		currentBounds = new Bounds(Vector3.zero, new Vector3(0.01f, 0.01f, 0.01f));
		Renderer[] componentsInChildren2 = currentGameObjectInstance.GetComponentsInChildren<Renderer>();
		Renderer[] array2 = componentsInChildren2;
		foreach (Renderer renderer in array2)
		{
			currentBounds.Encapsulate(renderer.bounds.center);
		}
		pivot.transform.position = currentBounds.center;
		currentGameObjectInstance.transform.forward = lastForward;
		base.transform.localPosition = initialLocalPos;
	}

	private void Update()
	{
		if (currentMovieInfo != null)
		{
			currentMovieInfo.update();
		}
		if (!hasActiveInstance)
		{
			return;
		}
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
		float axis = Input.GetAxis("Mouse ScrollWheel");
		imageScale += Time.deltaTime * axis * 25f;
		if (!(currentGameObjectInstance == null))
		{
			if ((double)Input.GetAxis("Horizontal") < -0.1)
			{
				currentGameObjectInstance.transform.Rotate(new Vector3(0f, 1.5f, 0f), Space.World);
			}
			if ((double)Input.GetAxis("Horizontal") > 0.1)
			{
				currentGameObjectInstance.transform.Rotate(new Vector3(0f, -1.5f, 0f), Space.World);
			}
			base.transform.position += base.transform.forward * axis;
			if (wasMouseDown)
			{
				Vector2 vector = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				vector -= previousMousePos;
				pivot.Rotate(new Vector3(vector.y, 0f, 0f), Space.World);
				currentGameObjectInstance.transform.Rotate(new Vector3(0f, 0f - vector.x, 0f), Space.World);
			}
			wasMouseDown = Input.GetKey(KeyCode.Mouse0);
			previousMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		}
	}

	private void OnGUI()
	{
		GUISkin skin = GUI.skin;
		GUI.skin = viewerSkin;
		GUI.contentColor = Color.yellow;
		if (hasActiveInstance)
		{
			GUI.Label(new Rect(0f, 10f, Screen.width, 30f), objectName, "Label");
		}
		GUI.skin = skin;
		doErrorGUI();
		doAudioGUI();
		doImageGUI();
		doMovieGUI();
		doTextGUI();
		doLeftRightButtonsGUI();
	}

	private void doTextGUI()
	{
		if (!string.IsNullOrEmpty(downloadedText))
		{
			if (screenWidth != (float)Screen.width || screenHeight != (float)Screen.height)
			{
				setRects();
			}
			GUILayout.BeginArea(scrollAreaRect);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, displayStyle);
			GUILayout.Box(downloadedText, displayStyle);
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
	}

	private void setRects()
	{
		float num = 0.1f;
		displayStyle = new GUIStyle();
		scrollAreaRect.x = (float)Screen.width * num;
		scrollAreaRect.width = (float)Screen.width - (float)Screen.width * num * 2f;
		scrollAreaRect.y = (float)Screen.height * num;
		scrollAreaRect.height = (float)Screen.height - (float)Screen.height * num * 2f;
		GUI.skin.verticalScrollbar.fixedWidth = (float)Screen.width * 0.05f;
		GUI.skin.verticalScrollbarThumb.fixedWidth = (float)Screen.width * 0.05f;
		GUI.skin.verticalScrollbarDownButton.fixedWidth = (float)Screen.width * 0.05f;
		GUI.skin.verticalScrollbarUpButton.fixedWidth = (float)Screen.width * 0.05f;
		displayStyle.fontSize = Mathf.RoundToInt(0.017578125f * (float)Screen.width);
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		displayStyle.wordWrap = true;
	}

	private void doMovieGUI()
	{
		if (currentMovieInfo == null)
		{
			return;
		}
		if (currentMovieInfo.IsPlaying)
		{
			Rect position = new Rect((float)Screen.width * 0.1f, (float)Screen.height * 0.1f, (float)Screen.width * 0.8f, (float)Screen.height * 0.8f);
			GUI.BeginGroup(position);
			currentMovieInfo.onGUI();
			GUI.EndGroup();
		}
		else
		{
			Rect position2 = new Rect((float)Screen.width / 2f - (float)Screen.width * 0.2f / 2f, (float)Screen.height / 2f - (float)Screen.height * 0.2f / 2f, (float)Screen.width * 0.2f, (float)Screen.height * 0.2f);
			if (GUI.Button(position2, "|>"))
			{
				currentMovieInfo.playMovie();
			}
		}
	}

	private void doLeftRightButtonsGUI()
	{
		if (objectArray == null || objectArray.Length <= 1)
		{
			return;
		}
		GUISkin skin = GUI.skin;
		GUI.skin = viewerSkin;
		Rect position = new Rect(0f, (float)Screen.height * 0.8f, (float)Screen.width * 0.2f, (float)Screen.height * 0.2f);
		GUI.skin = skin;
		GUI.Box(position, string.Empty);
		GUI.skin = viewerSkin;
		if (GUI.Button(position, "<", "Label"))
		{
			int pCurrentIndex = _pCurrentIndex;
			_pCurrentIndex--;
			if (_pCurrentIndex != pCurrentIndex)
			{
				refreshInstance();
			}
		}
		position.x = (float)Screen.width * 0.8f;
		GUI.skin = skin;
		GUI.Box(position, string.Empty);
		GUI.skin = viewerSkin;
		if (GUI.Button(position, ">", "Label"))
		{
			int pCurrentIndex2 = _pCurrentIndex;
			_pCurrentIndex++;
			if (_pCurrentIndex != pCurrentIndex2)
			{
				refreshInstance();
			}
		}
		GUI.skin = skin;
	}

	private void doErrorGUI()
	{
		if (!string.IsNullOrEmpty(errorMessage))
		{
			GUISkin skin = GUI.skin;
			GUI.skin = viewerSkin;
			Rect position = new Rect(0f, (float)Screen.height * 0.45f, Screen.width, (float)Screen.height * 0.1f);
			GUI.Label(position, "ERROR: " + errorMessage, "Label");
			GUI.skin = skin;
		}
	}

	private void doImageGUI()
	{
		if (!(currentTextureInstance == null))
		{
			float num = currentTextureInstance.width / currentTextureInstance.height;
			float num2 = (float)Screen.height * 0.6f * imageScale;
			float num3 = num2 * num;
			Rect position = new Rect((float)Screen.width * 0.5f - num3 * 0.5f, (float)Screen.height * 0.1f, num3, num2);
			Color color = GUI.color;
			GUI.color = Color.white;
			GUI.DrawTexture(position, currentTextureInstance);
			GUI.color = color;
		}
	}

	private void doAudioGUI()
	{
		if (!(currentAudioInstance == null))
		{
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
