using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
	private float Lenght, StartPos;

	public GameObject cam;
	public float parallaxEffect;

	void Start()
	{
		StartPos = transform.position.x;
		Lenght = GetComponent<SpriteRenderer>().bounds.size.x;
	}

	void Update()
	{
		float temp = (cam.transform.position.x * (1 - parallaxEffect));
		float dist = (cam.transform.position.x * parallaxEffect);

		transform.position = new Vector3(StartPos + dist, transform.position.y, transform.position.z);

		if (temp > StartPos + Lenght) 
		{
			StartPos += Lenght;
		}
		else if (temp < StartPos - Lenght)
		{
			StartPos -= Lenght;
		}
	}
}
