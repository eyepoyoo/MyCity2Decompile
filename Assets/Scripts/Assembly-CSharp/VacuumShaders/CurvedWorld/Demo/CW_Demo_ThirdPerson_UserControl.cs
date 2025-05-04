using UnityEngine;

namespace VacuumShaders.CurvedWorld.Demo
{
	[AddComponentMenu("VacuumShaders/Curved World/Demo/Third Person/UserControl")]
	[RequireComponent(typeof(CW_Demo_ThirdPerson_Character))]
	public class CW_Demo_ThirdPerson_UserControl : MonoBehaviour
	{
		private CW_Demo_ThirdPerson_Character m_Character;

		private Vector3 m_CamForward;

		private Vector3 m_Move;

		private bool m_Jump;

		public bool runner;

		private bool uiButtonJump;

		private Vector2 touchPivot;

		private void Start()
		{
			m_Character = GetComponent<CW_Demo_ThirdPerson_Character>();
		}

		private void Update()
		{
			if (!m_Jump)
			{
				m_Jump = Input.GetButtonDown("Jump");
			}
			if (uiButtonJump)
			{
				uiButtonJump = false;
				m_Jump = true;
			}
		}

		private void FixedUpdate()
		{
			float num = 0f;
			float num2 = 0f;
			if (Input.touchSupported && Input.touchCount > 0)
			{
				Touch touch = Input.touches[0];
				if (touch.phase == TouchPhase.Began)
				{
					touchPivot = touch.position;
				}
				if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary)
				{
					Vector2 normalized = (touch.position - touchPivot).normalized;
					num = normalized.x;
					num2 = normalized.y;
				}
			}
			else
			{
				num = Input.GetAxis("Horizontal");
				num2 = Input.GetAxis("Vertical");
			}
			if (runner)
			{
				num2 = 1f;
			}
			m_Move = num2 * Vector3.forward + num * Vector3.right;
			m_Character.Move(m_Move, false, m_Jump);
			m_Jump = false;
			if (runner)
			{
				Vector3 position = base.transform.position;
				position.z = -4f;
				base.transform.position = position;
			}
		}

		public void UIJumpButtonOn()
		{
			uiButtonJump = true;
		}
	}
}
