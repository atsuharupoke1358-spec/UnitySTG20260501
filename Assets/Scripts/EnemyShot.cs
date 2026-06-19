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
    public bool canShoot = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<Player>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (canShoot && timer >= shotData.fireRate)
        {
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

            case ShotType.NWay:
                ShootNWay();
                break;
        }
    }

    void SpawnBullet(Vector2 dir, Vector2 offset = default)
    {
        Vector2 spawnPos = (Vector2)transform.position + offset;
        GameObject bullet = BulletPool.Instance.GetBullet(spawnPos, Quaternion.identity);

        EnemyBullet eb = bullet.GetComponent<EnemyBullet>();
        eb.Init(shotData.bulletData, dir);
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

    /*float baseAngle = -90f;
    for (int i = 0; i < shotData.shotCount; i++)
    {
        float angleOffset = shotData.spreadAngle * (i - (shotData.shotCount - 1) / 2f);

        float angle = baseAngle + angleOffset;
        float rad = angle * Mathf.Deg2Rad;

        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        SpawnBullet(dir);
    }*/
    // 💡 クラスの変数宣言エリア（Updateや関数の外側）にこれを1行足してください
    // 弾幕全体の傾きを記録しておくための変数です
    private float nwayRotationOffset = 0f;

    void ShootNWay()
    {
        float baseAngle = -90f + nwayRotationOffset;

        for (int i = 0; i < shotData.shotCount; i++)
        {
            float angleOffset = shotData.spreadAngle * (i - (shotData.shotCount - 1) / 2f);

            float angle = baseAngle + angleOffset;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
            SpawnBullet(dir);
        }

        if (nwayRotationOffset == 0f)
        {
            nwayRotationOffset = 5f;
        }
        else
        {
            nwayRotationOffset = 0f;
        }
    }

}
