using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrolling : MonoBehaviour
{
    private new Camera camera;
    private Transform player;

    public float superskyHeight = 22f;
    public float height = 6.5f;
    public float undergroundHeight = -15.5f;
    public float superUndergroundHeight = -36.5f;
    public float undergroundThreshold = 0f;
    public float superUndergroundThreshold = -15f;


	private void Awake()
    {
        camera = GetComponent<Camera>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        // track the player moving to the right
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);
        transform.position = cameraPosition;
    }

	public void SetSuperskyHeight(bool supersky)
	{
		// set underground height offset
		Vector3 cameraPosition = transform.position;
		cameraPosition.y = supersky ? superskyHeight : height;
		transform.position = cameraPosition;
	}

	public void SetUnderground(bool underground)
    {
        // set underground height offset
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : height;
        transform.position = cameraPosition;
    }

	public void SetSuperUnderground(bool superunderground)
	{
		// set underground height offset
		Vector3 cameraPosition = transform.position;
		cameraPosition.y = superunderground ? superUndergroundHeight : undergroundHeight;
		transform.position = cameraPosition;
	}
}
