using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeathBarrier : MonoBehaviour
{
    private Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
		//Debug.Log("Trigger entered");
		if (other.CompareTag("Player"))
        {
            //other.gameObject.SetActive(false);
            GameManager.Instance.ResetLevel(3f);
            
            //Debug.Log("Bob");
        }
        else
        {
            Destroy(other.gameObject);
			//Debug.Log("Obo");
		}
    }

}
