using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject homingPrefab;
    [SerializeField] private GameObject straightPrefab;

    [SerializeField] private float fireRate = 0.15f;
    private float nextFireTime;
    bool isSlowMode;
    private GameObject currentLaser;
    public bool isLaserMode;
    int lastLaserPower;
    Player player;
    [SerializeField] private Transform[] homingPoints;
    [SerializeField] private Transform[] straightPoints;
    [SerializeField] private Transform[] laserPoints;
    List<GameObject> currentLasers = new List<GameObject>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip homingShotSE;
    [SerializeField] private AudioClip laserShotSE;
    void Start()
    {
        player = GetComponent<Player>();
        lastLaserPower = player.laserPower;
    }
    void Update()
    {
        isLaserMode = Input.GetKey(KeyCode.LeftShift);

        // Z状態管理
        if (Input.GetKey(KeyCode.Z) && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            DestroyLaserIfNeeded();
        }
        if (!isLaserMode && currentLasers.Count > 0)
        {
            DestroyLaserIfNeeded();
        }
    }
    void Shoot()
    {
        ShootStraight();
        if (isLaserMode)
        {
            audioSource.PlayOneShot(laserShotSE, 0.2f);
            ShootLaser();
        }
        else
        {
            audioSource.PlayOneShot(homingShotSE, 0.3f);
            ShootHoming();
        }
    }
    void DestroyLaserIfNeeded()
    {
        for (int i = 0; i < currentLasers.Count; i++)
        {
            if (currentLasers[i] != null)
            {
                Destroy(currentLasers[i]);
            }
        }

        currentLasers.Clear();
    }

    void ShootHoming()
    {
        if (player.homingPower == 1)
        {
            ShootHomingAt(0);
        }

        if (player.homingPower == 2)
        {
            ShootHomingAt(1);
            ShootHomingAt(2);
        }

        if (player.homingPower >= 3)
        {
            ShootHomingAt(1);
            ShootHomingAt(2);
            ShootHomingAt(3);
            ShootHomingAt(4);
        }
    }
    void ShootHomingAt(int index)
    {
        GameObject bullet = Instantiate(
            homingPrefab,
            homingPoints[index].position,
            Quaternion.identity
        );

        bullet.GetComponent<HomingBullet>().SetPower(player.homingPower);
    }
    void ShootStraight()
    {
        ShootStraightAt(1);
        ShootStraightAt(2);
    }
    void ShootStraightAt(int index)
    {
        GameObject bullet = Instantiate(
            straightPrefab,
            straightPoints[index].position,
            Quaternion.identity
        );
    }
    void ShootLaser()
    {
        if (player.laserPower != lastLaserPower)
        {
            DestroyLaserIfNeeded();
            lastLaserPower = player.laserPower;
        }


        if (currentLasers.Count > 0) return;

        if (player.laserPower < 4)
        {
            ShootLaserAt(0);
        }

        if (player.laserPower >= 4)
        {
            ShootLaserAt(1);
            ShootLaserAt(2);
        }
    }
    void ShootLaserAt(int index)
    {
        GameObject laser = Instantiate(
            laserPrefab,
            laserPoints[index].position,
            Quaternion.identity
        );

        LaserBullet lb = laser.GetComponent<LaserBullet>();

        lb.player = laserPoints[index];
        lb.SetPower(player.laserPower);

        currentLasers.Add(laser);
    }
}