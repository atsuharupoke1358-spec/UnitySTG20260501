using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyAimPrefab;
    public GameObject enemySpiralPrefab;
    public GameObject enemyClusterAimPrefab;
    public GameObject chaseEnemyPrefab;
    public GameObject bossPrefab;
    public GameObject hpBar;
    public Image hpBarFill;
    IEnumerator Start()
    {
        // 右から3体
        for (int i = 0; i < 3; i++)
        {
            SpawnEnemy(enemyAimPrefab, new Vector3(GameConfig.Right + GameConfig.SpawnMargin, 4 - i, 0), Vector2.left);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2f);
        SpawnChaseEnemy(Vector3.zero);
        yield return new WaitForSeconds(2f);
        // 左から3体
        for (int i = 0; i < 3; i++)
        {
            SpawnEnemy(enemyAimPrefab, new Vector3(GameConfig.Left - GameConfig.SpawnMargin, 4 - i, 0), Vector2.right);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2f);
        //真ん中上から1体
        SpawnEnemy(enemySpiralPrefab, new Vector3(0, GameConfig.Top + GameConfig.SpawnMargin, 0), Vector2.down);
        yield return new WaitForSeconds(2f);
        //左右自機狙い交互
        for (int i = 0; i < 10; i++)
        {
            SpawnEnemy(enemyAimPrefab, new Vector3(GameConfig.Right + GameConfig.SpawnMargin, 4, 0), Vector2.left);
            yield return new WaitForSeconds(0.5f);
            SpawnEnemy(enemyAimPrefab, new Vector3(GameConfig.Left - GameConfig.SpawnMargin, 4, 0), Vector2.right);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2f);
        //上から2体
        SpawnEnemy(enemyClusterAimPrefab, new Vector3(3, GameConfig.Top + GameConfig.SpawnMargin, 0), Vector2.down);
        SpawnEnemy(enemyClusterAimPrefab, new Vector3(-3, GameConfig.Top + GameConfig.SpawnMargin, 0), Vector2.down);
        yield return new WaitForSeconds(5f);

        //ボス戦
        SpawnBoss();
    }
    void SpawnEnemy(GameObject prefab, Vector3 pos, Vector2 dir)
    {
        GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);
        enemy.GetComponent<Enemy>().moveDirection = dir;
    }
    void SpawnChaseEnemy(Vector3 pos)
    {
        if (chaseEnemyPrefab != null)
        {
            Instantiate(chaseEnemyPrefab, pos, Quaternion.identity);
        }
    }

    void SpawnBoss()
    {
        GameObject boss = Instantiate(
            bossPrefab,
            new Vector3(
                0,
                GameConfig.Top + GameConfig.SpawnMargin,
                0
            ),
            Quaternion.identity
        );

        Boss bossScript = boss.GetComponent<Boss>();

        if (bossScript != null)
        {
            bossScript.SetupHpBar(hpBar, hpBarFill);
        }
    }
}
