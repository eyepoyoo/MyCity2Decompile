using UnityEngine;

namespace UnitySampleAssets._2D
{
	public class PlatformerCharacter2D : MonoBehaviour
	{
		private bool facingRight = true;

		[SerializeField]
		private float maxSpeed = 10f;

		[SerializeField]
		private float jumpForce = 400f;

		[SerializeField]
		[Range(0f, 1f)]
		private float crouchSpeed = 0.36f;

		[SerializeField]
		private bool airControl;

		[SerializeField]
		private LayerMask whatIsGround;

		private Transform groundCheck;

		private float groundedRadius = 0.2f;

		private bool grounded;

		private Transform ceilingCheck;

		private float ceilingRadius = 0.01f;

		private Animator anim;

		private void Awake()
		{
			groundCheck = base.transform.Find("GroundCheck");
			ceilingCheck = base.transform.Find("CeilingCheck");
			anim = GetComponent<Animator>();
		}

		private void FixedUpdate()
		{
			grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
			anim.SetBool("Ground", grounded);
			anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);
		}

		public void Move(float move, bool crouch, bool jump)
		{
			if (!crouch && anim.GetBool("Crouch") && (bool)Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
			{
				crouch = true;
			}
			anim.SetBool("Crouch", crouch);
			if (grounded || airControl)
			{
				move = ((!crouch) ? move : (move * crouchSpeed));
				anim.SetFloat("Speed", Mathf.Abs(move));
				GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
				if (move > 0f && !facingRight)
				{
					Flip();
				}
				else if (move < 0f && facingRight)
				{
					Flip();
				}
			}
			if (grounded && jump && anim.GetBool("Ground"))
			{
				grounded = false;
				anim.SetBool("Ground", false);
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
			}
		}

		private void Flip()
		{
			facingRight = !facingRight;
			Vector3 localScale = base.transform.localScale;
			localScale.x *= -1f;
			base.transform.localScale = localScale;
		}
	}
}
