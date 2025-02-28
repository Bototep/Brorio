using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private GameManager gameManager;
    private int PowerUpScoreValue = 1000;

	public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
        FireFlower,
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
		Player playerComponent = player.GetComponent<Player>();

		switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoin();
                break;

            case Type.ExtraLife:
                GameManager.Instance.AddLife();
                break;

			case Type.MagicMushroom:
				player.GetComponent<Player>().Grow();
				break;

			case Type.Starpower:
				player.GetComponent<Player>().Starpower();
				break;

			case Type.FireFlower:
				if (playerComponent.big || playerComponent.white)
				{
					playerComponent.FireFlower();
				}
				break; 
		}

        Destroy(gameObject);
    }

}
