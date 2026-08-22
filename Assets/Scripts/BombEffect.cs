using UnityEngine;

public class BombEffect : MonoBehaviour
{
    [SerializeField] private float expandSpeed = 15f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private int bombDamage = 50;

    private float timer;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;

        ClearAllEnemyBullets();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
            return;
        }

        transform.localScale += Vector3.one * expandSpeed * Time.deltaTime;
    }

    private void ClearAllEnemyBullets()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject bullet in bullets)
        {
            if (bullet != null)
            {
                Destroy(bullet);
            }
        }
        Debug.Log($"{bullets.Length} 個の敵の弾を消去しました！");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(bombDamage);
            }
        }
        if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }
}