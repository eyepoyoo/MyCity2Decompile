using UnityEngine;

namespace VacuumShaders.CurvedWorld.Demo
{
	[RequireComponent(typeof(CW_Demo_2D_PlatformerCharacter))]
	[AddComponentMenu("VacuumShaders/Curved World/Demo/2D/Platformer User Control")]
	public class CW_Demo_2D_PlatformerUserControl : MonoBehaviour
	{
		private CW_Demo_2D_PlatformerCharacter m_Character;

		private bool m_Jump;

		private bool uiButtonJump;

		private Vector2 touchPivot;

		private void Awake()
		{
			m_Character = GetComponent<CW_Demo_2D_PlatformerCharacter>();
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
			float move = 0f;
			if (Input.touchSupported && Input.touchCount > 0)
			{
				Touch touch = Input.touches[0];
				if (touch.phase == TouchPhase.Began)
				{
					touchPivot = touch.position;
				}
				if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary)
				{
					move = (touch.position - touchPivot).normalized.x;
				}
			}
			else
			{
				move = Input.GetAxis("Horizontal");
			}
			m_Character.Move(move, false, m_Jump);
			m_Jump = false;
		}

		public void UIJumpButtonOn()
		{
			uiButtonJump = true;
		}
	}
}
