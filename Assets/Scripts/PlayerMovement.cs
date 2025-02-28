using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

	private Player player;
	private WaterTrigger water;

	private Vector2 velocity;
    private float inputAxis;

    public float moveSpeed = 6f;
    public float maxJumpHeight = 4.3f;
    public float maxJumpTime = 1.2f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);
    public bool falling => velocity.y < 0f && !grounded;
	public bool sitting;
	public bool canmove = true;

	public CapsuleCollider2D capsuleCollider { get; private set; }

	public GameObject projectile;
	public Vector2 Shootvelocity;
	public bool canShoot = false;
	public Vector2 offset = new Vector2(0.4f, 0.1f);
	public float cooldown = 1f;
    public Transform shootPoint;
	private GameObject currentProjectile;
	public int activeProjectiles = 0;

	public bool drown = false;
	public bool lifted = false;

	private FloatingPlatform currentPlatform;

	public bool spring = false;

	public ParticleSystem dust;
	public ParticleSystem stomp;

	private SFXPlaying sFXPlaying;

	private void Awake()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
		canmove= true;

		capsuleCollider = GetComponent<CapsuleCollider2D>();
		player = FindObjectOfType<Player>();

		sFXPlaying = GetComponent<SFXPlaying>();
		sFXPlaying = FindObjectOfType<SFXPlaying>();
	}

    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        collider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
		grounded= true;
    }

    private void Update()
    {
        HorizontalMovement();

		grounded = rigidbody.Raycast(Vector2.down);

        if (grounded) 
		{
            GroundedMovement();
		}
		if (drown)
		{
			DrownedMovement();
		}
		if (lifted)
		{
			LiftMovement();
		}
		if (canmove)
		{
			if (player != null && player.big || player != null && player.white)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					sitting = !sitting;

					if (sitting == true)
					{
						capsuleCollider.size = new Vector2(1f, 1f);
						capsuleCollider.offset = new Vector2(0f, -0.1f);
					}
				}
				if (Input.GetKeyUp(KeyCode.S))
				{
					sitting = false;
					capsuleCollider.size = new Vector2(1f, 2f);
					capsuleCollider.offset = new Vector2(0f, 0.4f);
				}
			}
		}

        ApplyGravity();

		if (Input.anyKeyDown && transform.parent != null && transform.parent.CompareTag("FloatingPlatform"))
		{
			// Move the parent setting code here or in FixedUpdate
			currentPlatform = null;
			grounded = true;
			// Unparent the player from the platform
			transform.parent = null;
		}

		if (grounded)
		{
			if (Mathf.Abs(velocity.x) > 0.1f)
			{
				CreateDust();
			}
		}
	}

    private void FixedUpdate()
    {
        // move mario based on his velocity
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        // clamp within the screen bounds
        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(position);
		if (canmove)
		{
			if (player != null && player.white && canShoot)
			{
				if (Input.GetKeyDown(KeyCode.LeftControl) && canShoot && activeProjectiles < 2)
				{
					Vector2 shootDirection = transform.rotation.eulerAngles.y == 0f ? Vector2.right : Vector2.left;

					// Use the shoot point's position instead of the character's position
					GameObject go = Instantiate(projectile, transform.position + transform.right * offset.x * Mathf.Sign(transform.localScale.x) + transform.up * offset.y, Quaternion.identity);
					activeProjectiles++; // Increment the active projectiles count
					go.GetComponent<Rigidbody2D>().velocity = new Vector2(Shootvelocity.x * shootDirection.x, Shootvelocity.y);

					StartCoroutine(CanShoot());
				}
			}
		}
	}

    private void HorizontalMovement()
    {
		if(canmove)
		{
			if (!sitting)
			{
				inputAxis = Input.GetAxis("Horizontal");

				// Define the maximum speed
				float maxSpeed = 6f;

				// Define acceleration values based on grounded state
				float groundedAcceleration = 15f;
				float airAcceleration = 8f;

				// Determine which acceleration to use based on grounded state
				float acceleration = grounded ? groundedAcceleration : airAcceleration;

				// Calculate the target velocity based on input
				float targetVelocity = inputAxis * maxSpeed;

				// Accelerate towards the target velocity
				velocity.x = Mathf.MoveTowards(velocity.x, targetVelocity, acceleration * Time.deltaTime);

				// flip sprite to face direction
				if (velocity.x > 0f)
				{
					transform.eulerAngles = Vector3.zero;
				}
				else if (velocity.x < 0f)
				{
					transform.eulerAngles = new Vector3(0f, 180f, 0f);
				}
			}
			else
			{
				// If sitting, set velocity.x to 0 to prevent movement
				velocity.x = Mathf.MoveTowards(velocity.x, 0f, moveSpeed * Time.deltaTime);
			}
		}
	}

    private void GroundedMovement()
    {
        // prevent gravity from infinitly building up
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;
		if (canmove)
		{
			if (Input.GetButtonDown("Jump"))
			{
				sFXPlaying.JumpSFX();

				velocity.y = jumpForce;
				jumping = true;

				CreateDust();
			}
		}
	}

	private void DrownedMovement()
	{
		if (Input.GetButtonDown("Jump"))
		{
			velocity.y = jumpForce;
			jumping = true;
		}
	}

	private void LiftMovement()
	{
		if (Input.GetButtonDown("Jump"))
		{
			velocity.y = jumpForce;
			jumping = true;

			CreateDust();
		}
	}

	public void ApplyGravity()
    {
        // check if falling
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");

		if (!drown)
		{
			float multiplier = falling ? 2f : 1f;
			velocity.y += gravity * multiplier * Time.deltaTime;
			velocity.y = Mathf.Max(velocity.y, gravity / 2f);
		}
		else if (drown)
		{
			float multiplier = falling ? 1f : 1f;
			velocity.y += gravity * multiplier * Time.deltaTime;
			velocity.y = Mathf.Max(velocity.y, gravity / 2f);
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
			sFXPlaying.StompSFX();

			if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }

			currentProjectile = null;
			activeProjectiles = Mathf.Max(0, activeProjectiles - 1);

			stomp.Play();
		}
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            // stop vertical movement if mario bonks his head
            if (transform.DotTest(collision.transform, Vector2.up)) {
                velocity.y = 0f;
            }
        }

		if (collision.gameObject.CompareTag("FloatingPlatform"))
		{
			currentPlatform = collision.gameObject.GetComponent<FloatingPlatform>();
			grounded = true;
			transform.parent = collision.transform;
		}

		if (collision.gameObject.CompareTag("LiftDownFix"))
		{
			lifted = true;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Spring"))
		{
			if (Input.GetButtonDown("Jump"))
			{
				maxJumpHeight = 6.5f;
				spring = true;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("FloatingPlatform"))
		{
			currentPlatform = null;
			grounded = false;
			transform.parent = null;
		}

		if (collision.gameObject.CompareTag("Spring"))
		{
			jumping = true;
			maxJumpHeight = 4.3f;
			spring = false;
		}

		if (collision.gameObject.CompareTag("LiftDownFix"))
		{
			lifted = false;
		}
	}

	IEnumerator CanShoot()
	{
		yield return null;
		canShoot = true;
	}

	public bool IsDrown
	{
		get { return drown; }
	}

	void CreateDust()
	{
		dust.Play();
	}
}
