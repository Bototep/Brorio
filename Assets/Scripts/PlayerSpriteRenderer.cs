using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteRenderer : MonoBehaviour
{
    private PlayerMovement movement;
	private Player player;
    private FlagPole flagPole;
	private SkyEnter skyEnter;

	public SpriteRenderer spriteRenderer { get; private set; }
    public AnimatedSprite idle;
    public Sprite jump;
    public Sprite slide;
	public AnimatedSprite fall;
	public AnimatedSprite run;
	public AnimatedSprite swim;
	//public AnimatedSprite grow;
	//public AnimatedSprite shrink;
	public Sprite sit;
    public Sprite slideDown;
	public AnimatedSprite climb;

	public bool slidedownSprite = false;
	public bool climbAnim = false;

	private void Awake()
    {
        movement = GetComponentInParent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
		player = FindObjectOfType<Player>();
		flagPole = FindObjectOfType<FlagPole>();
		skyEnter = FindObjectOfType<SkyEnter>();

		run.enabled= false;
	}

	private void FixedUpdate()
	{
		if (flagPole != null)
		{
			if (flagPole.slideDown)
			{
				slidedownSprite = true;
			}
			else
			{
				slidedownSprite = false;
			}
		}

		if (skyEnter != null)
		{

			if (skyEnter.climb)
			{
				climbAnim = true;
			}
			else
			{
				climbAnim = false;
			}
		}
	}

	private void LateUpdate()
    {
		if (!movement.drown)
		{
			OnFoot();
		}
		else
		{
			OnWater();
		}
	}

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        run.enabled = false;
	}

    public void OnFoot()
    {
		if(!player.shrinkAnim || !player.growAnim || !player.growAnim && !player.shrinkAnim)
		{
			climb.enabled = climbAnim;
			fall.enabled = movement.falling;
			run.enabled = movement.running && movement.grounded && !movement.falling && !movement.drown && !climbAnim;
			idle.enabled = !movement.running && !movement.falling && !movement.sitting;

			if (slidedownSprite)
			{
				spriteRenderer.sprite = slideDown;
			}
			else if (climbAnim)
			{
				climb.enabled = climbAnim;
				run.enabled = false;
			}
			else if (movement.jumping && !movement.falling)
			{
				spriteRenderer.sprite = jump;
			}
			else if (movement.sliding && movement.grounded)
			{
				spriteRenderer.sprite = slide;
			}
			else if (movement.sitting && movement.grounded)
			{
				spriteRenderer.sprite = sit;
			}
		}

		//grow.enabled = player.growAnim;
		//shrink.enabled = player.shrinkAnim;

		/*fall.enabled = movement.falling;
		run.enabled = movement.running && movement.grounded && !movement.falling && !movement.drown;
		idle.enabled = !movement.running && !movement.falling && !movement.sitting;

		if (movement.jumping && !movement.falling)
		{
			spriteRenderer.sprite = jump;
		}
		else if (movement.sliding && movement.grounded)
		{
			spriteRenderer.sprite = slide;
		}
		else if (movement.sitting && movement.grounded)
		{
			spriteRenderer.sprite = sit;
		}
		else if (slideDown)
		{
			spriteRenderer.sprite = ladslide;
		}*/
	}

	public void OnWater()
	{
		swim.enabled = movement.falling || movement.jumping;
		run.enabled = movement.running && movement.grounded;
		idle.enabled = !movement.running && !movement.falling && !movement.sitting;

		if (movement.sliding && movement.grounded)
		{
			spriteRenderer.sprite = slide;
		}
	}
}
