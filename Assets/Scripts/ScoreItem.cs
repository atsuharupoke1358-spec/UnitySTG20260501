using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.AddScore(1000);

            Destroy(gameObject);
        }
    }
}