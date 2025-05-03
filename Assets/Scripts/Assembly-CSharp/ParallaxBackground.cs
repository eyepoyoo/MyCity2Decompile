using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
	public Vector3 _deviceOrientation = new Vector3(0f, 0f, 0f);

	public Vector3 _maxTilt = new Vector3(0.5f, 0.5f, 0f);

	public Vector3 _midgroundMaxMovement = new Vector3(25f, 25f, 0f);

	public Vector3 _midBackMaxMovement = new Vector3(50f, 50f, 0f);

	public Vector3 _foregroundMaxMovement = new Vector3(150f, 150f, 0f);

	public float _speed = 3f;

	[SerializeField]
	private GameObject _midGround;

	[SerializeField]
	private GameObject _midBackGround;

	[SerializeField]
	private GameObject _backGround;

	private Vector3 _midgroundStart;

	private Vector3 _backgroundStart;

	private Vector3 _midBackStart;

	private Vector3 _orientationDiff = new Vector3(0f, 0f, 0f);

	private void Start()
	{
		_midgroundStart = _midGround.transform.localPosition;
		_midBackStart = _midBackGround.transform.localPosition;
		_backgroundStart = _backGround.transform.localPosition;
	}

	private void Update()
	{
		_deviceOrientation = Input.acceleration;
		_deviceOrientation += _orientationDiff;
		float num = Mathf.Clamp(_deviceOrientation.x, 0f - _maxTilt.x, _maxTilt.x);
		float num2 = Mathf.Clamp(_deviceOrientation.y, 0f - _maxTilt.y, _maxTilt.y);
		float b = ((!(num > 0f)) ? Mathf.Lerp(_midgroundStart.x, _midgroundMaxMovement.x, Mathf.Abs(num)) : Mathf.Lerp(_midgroundStart.x, 0f - _midgroundMaxMovement.x, num));
		float b2 = ((!(num2 > 0f)) ? Mathf.Lerp(_midgroundStart.y, _midgroundMaxMovement.y, Mathf.Abs(num2)) : Mathf.Lerp(_midgroundStart.y, 0f - _midgroundMaxMovement.y, num2));
		float x = Mathf.Lerp(_midGround.transform.localPosition.x, b, Time.deltaTime * _speed);
		float y = Mathf.Lerp(_midGround.transform.localPosition.y, b2, Time.deltaTime * _speed);
		_midGround.transform.localPosition = new Vector3(x, y, 0f);
		float b3 = ((!(num > 0f)) ? Mathf.Lerp(_backgroundStart.x, _foregroundMaxMovement.x, Mathf.Abs(num)) : Mathf.Lerp(_backgroundStart.x, 0f - _foregroundMaxMovement.x, num));
		float b4 = ((!(num2 > 0f)) ? Mathf.Lerp(_backgroundStart.y, _foregroundMaxMovement.y, Mathf.Abs(num2)) : Mathf.Lerp(_backgroundStart.y, 0f - _foregroundMaxMovement.y, num2));
		float x2 = Mathf.Lerp(_backGround.transform.localPosition.x, b3, Time.deltaTime * _speed);
		float y2 = Mathf.Lerp(_backGround.transform.localPosition.y, b4, Time.deltaTime * _speed);
		_backGround.transform.localPosition = new Vector3(x2, y2, 0f);
		float b5 = ((!(num > 0f)) ? Mathf.Lerp(_midBackStart.x, _midBackMaxMovement.x, Mathf.Abs(num)) : Mathf.Lerp(_midBackStart.x, 0f - _midBackMaxMovement.x, num));
		float b6 = ((!(num2 > 0f)) ? Mathf.Lerp(_midBackStart.y, _midBackMaxMovement.y, Mathf.Abs(num2)) : Mathf.Lerp(_midBackStart.y, 0f - _midBackMaxMovement.y, num2));
		float x3 = Mathf.Lerp(_midBackGround.transform.localPosition.x, b5, Time.deltaTime * _speed);
		float y3 = Mathf.Lerp(_midBackGround.transform.localPosition.y, b6, Time.deltaTime * _speed);
		_midBackGround.transform.localPosition = new Vector3(x3, y3, 0f);
	}

	public void SetDefaultOrientation()
	{
		_orientationDiff = -Input.acceleration;
	}
}
