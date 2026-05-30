using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float speed = 6f;
    Transform target;
    int power;

    public void SetPower(int p)
    {
        power = p;
    }

    void Start()
    {
        target = GameObject.FindWithTag("Enemy")?.transform;
    }

    void Update()
    {
        if (target == null)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(power);
            }

            Destroy(gameObject);
        }
    }
}