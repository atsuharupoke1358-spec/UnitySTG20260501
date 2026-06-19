using System.Collections.Generic;
using UnityEngine;
public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int defaultPoolSize = 100;
    private List<GameObject> pooledBullets = new List<GameObject>();
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        for (int i = 0; i < defaultPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            pooledBullets.Add(bullet);
        }
    }
    public GameObject GetBullet(Vector2 position, Quaternion rotation)
    {
        for (int i = 0; i < pooledBullets.Count; i++)
        {
            if (pooledBullets[i] == null)
            {
                GameObject replacedBullet = Instantiate(bulletPrefab, transform);
                replacedBullet.SetActive(false);
                pooledBullets[i] = replacedBullet;
            }
            if (!pooledBullets[i].activeInHierarchy)
            {
                GameObject bullet = pooledBullets[i];
                bullet.transform.position = position;
                bullet.transform.rotation = rotation;
                bullet.SetActive(true);
                return bullet;
            }
        }
        GameObject newBullet = Instantiate(bulletPrefab, position, rotation, transform);
        pooledBullets.Add(newBullet);
        return newBullet;
    }
}