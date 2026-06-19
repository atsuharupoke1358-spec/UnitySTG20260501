using System.Collections;
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
        transform.localScale = Vector3.one * data.scale;
    }

    void Update()
    {
        if (data == null) return;

        transform.position +=
            (Vector3)direction *
            data.speed *
            Time.deltaTime;
    }
    void OnEnable()
    {
        StartCoroutine(DisableAfterSeconds(5f));
    }
    public void Init(BulletData newData, Vector2 newDirection)
    {
        data = newData;
        direction = newDirection;

        if (sr == null) sr = GetComponent<SpriteRenderer>();

        if (data != null)
        {
            sr.sprite = data.sprite;
            sr.color = data.color;
            transform.localScale = Vector3.one * data.scale;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
    private IEnumerator DisableAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}