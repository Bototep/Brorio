using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Water"))
		{
			Debug.Log("wadwad");
		}
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		
	}

}
