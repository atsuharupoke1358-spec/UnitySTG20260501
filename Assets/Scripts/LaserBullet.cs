
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LaserBullet : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 1f, 0);

    int power;

    public void SetPower(int p)
    {
        power = p;
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>()?.TakeDamage(power);
        }
    }
}