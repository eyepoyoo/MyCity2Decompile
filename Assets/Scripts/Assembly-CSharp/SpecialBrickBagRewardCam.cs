using UnityEngine;

[ExecuteInEditMode]
public class SpecialBrickBagRewardCam : MonoBehaviour
{
	public bool useRootUICam;

	public Camera camera3D;

	public Camera uiCamera;

	public Transform referenceTL;

	public Transform referenceBR;

	public Transform referenceTL2;

	public Transform referenceBR2;

	public Transform referenceTL3;

	public Transform referenceBR3;

	public Vector3 globalOffset;

	public Vector3 camPos1;

	public Vector3 camPos2;

	public Vector3 camPos3;

	[Range(0f, 2f)]
	public float referenceInterploate;

	public void Update()
	{
		if (!(referenceBR == null) && !(referenceTL == null) && !(referenceBR2 == null) && !(referenceTL2 == null) && !(referenceBR3 == null) && !(referenceTL3 == null) && !(camera3D == null) && (useRootUICam || !(uiCamera == null)))
		{
			if (useRootUICam && ScreenRoot._pInstance != null)
			{
				uiCamera = ScreenRoot._pInstance._pUiCam;
			}
			if (!(uiCamera == null))
			{
				Vector3 position = ((!(referenceInterploate < 1f)) ? Vector3.Lerp(referenceTL2.position, referenceTL3.position, referenceInterploate - 1f) : Vector3.Lerp(referenceTL.position, referenceTL2.position, referenceInterploate));
				Vector3 position2 = ((!(referenceInterploate < 1f)) ? Vector3.Lerp(referenceBR2.position, referenceBR3.position, referenceInterploate - 1f) : Vector3.Lerp(referenceBR.position, referenceBR2.position, referenceInterploate));
				Vector3 vector = uiCamera.WorldToViewportPoint(position);
				Vector3 vector2 = uiCamera.WorldToViewportPoint(position2);
				base.transform.localPosition = ((!(referenceInterploate < 1f)) ? Vector3.Lerp(camPos2, camPos3, referenceInterploate - 1f) : Vector3.Lerp(camPos1, camPos2, referenceInterploate));
				base.transform.localPosition += globalOffset;
				camera3D.rect = new Rect(vector.x, vector2.y, vector2.x - vector.x, vector.y - vector2.y);
			}
		}
	}
}
