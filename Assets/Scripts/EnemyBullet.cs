using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public BulletData data;
    public Vector2 direction;
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = data.sprite;
        sr.color = data.color;
        transform.localScale =
    Vector3.one * data.scale;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position +=
            (Vector3)direction *
            data.speed *
            Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}