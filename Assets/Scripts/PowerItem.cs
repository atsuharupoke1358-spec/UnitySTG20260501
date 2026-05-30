using UnityEngine;

public class PowerItem : MonoBehaviour
{
    bool collected;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            Player player =
                other.GetComponent<Player>();

            if (player != null)
            {
                player.GetPowerUp();
            }

            Destroy(gameObject);
        }
    }
}