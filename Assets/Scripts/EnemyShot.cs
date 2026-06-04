using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public ShotData shotData;
    Player playerScript;
    Transform player;
    float angle1 = 0f;
    float angle2 = 180f;

    float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<Player>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shotData.fireRate)
        {
            Debug.Log(shotData.fireRate);
            Shoot();
            timer = 0;
        }
    }

    public void Shoot()
    {
        switch (shotData.shotType)
        {
            case ShotType.Aim:
                ShootAim();
                break;

            case ShotType.Spiral:
                ShootSpiral();
                break;

            case ShotType.ClusterAim:
                ShootClusterAim();
                break;
        }
    }

    void SpawnBullet(Vector2 dir, Vector2 offset = default)
    {
        GameObject bullet =
            Instantiate(
                bulletPrefab,
                (Vector2)transform.position + offset,
                Quaternion.identity
            );

        EnemyBullet eb =
            bullet.GetComponent<EnemyBullet>();
        eb.data = shotData.bulletData;
        eb.direction = dir;
    }

    void ShootAim()
    {
        Vector2 dir;

        if (playerScript.isDead)
        {
            dir = Vector2.down;
        }
        else
        {
            dir = (player.position - transform.position).normalized;
        }

        SpawnBullet(dir);
    }
    void ShootSpiral()
    {
        ShootSpiralPattern(angle1);
        ShootSpiralPattern(angle2);

        angle1 -= shotData.spiralSpeed;
        angle2 -= shotData.spiralSpeed;
    }
    void ShootSpiralPattern(float baseAngle)
    {
        for (int i = 0; i < shotData.shotCount; i++)
        {
            float angleOffset =
                shotData.spreadAngle *
                (i - (shotData.shotCount - 1) / 2f);

            float angle =
                baseAngle + angleOffset;

            float rad =
                angle * Mathf.Deg2Rad;

            Vector2 dir =
                new Vector2(
                    Mathf.Cos(rad),
                    Mathf.Sin(rad)
                ).normalized;

            SpawnBullet(dir);
        }
    }
    void ShootClusterAim()
    {
        Vector2 baseDir;

        if (player == null ||
            playerScript == null ||
            playerScript.isDead)
        {
            baseDir = Vector2.down;
        }
        else
        {
            baseDir =
                (player.position - transform.position)
                .normalized;
        }

        for (int i = 0; i < shotData.shotCount; i++)
        {
            Vector2 randomOffset =
                Random.insideUnitCircle
                * shotData.clusterRadius;

            SpawnBullet(baseDir, randomOffset);
        }

    }
}
