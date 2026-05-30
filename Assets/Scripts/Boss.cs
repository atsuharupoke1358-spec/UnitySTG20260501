using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
public class Boss : MonoBehaviour
{
    public EnemyShot shot;
    public ShotData phase1;
    public ShotData phase2;
    public ShotData phase3;
    public enum BossState
    {
        Phase1,
        Phase2,
        Phase3
    }
    Enemy enemy;
    BossState state;
    bool isChanging = false;
    public GameObject hpBar;
    public Image hpBarFill;
    public GameObject explosionPrefab;
    public AudioSource audioSource;
    public AudioClip BossDeathSE;

    void Start()
    {
        hpBar.SetActive(false);
        enemy = GetComponent<Enemy>();
        state = BossState.Phase1;
        shot.shotData = phase1;
        StartCoroutine(ShowHpBar());
    }

    void Update()
    {
        Debug.Log(enemy.hp);
        hpBarFill.fillAmount = (float)enemy.hp / enemy.maxHp;

        if (state == BossState.Phase1 && enemy.hp <= 1000 && !isChanging)
        {
            isChanging = true;
            StartCoroutine(ChangeStateWithDelay(BossState.Phase2, phase2));
        }

        if (state == BossState.Phase2 && enemy.hp <= 500 && !isChanging)
        {
            isChanging = true;
            StartCoroutine(ChangeStateWithDelay(BossState.Phase3, phase3));
        }
        if (enemy.hp <= 0)
        {
            Debug.Log("BossDeath開始");

            enabled = false;

            ClearBullets();

            StartCoroutine(BossDeath());

            return;
        }
    }
    IEnumerator ChangeStateWithDelay(BossState newState, ShotData data)
    {
        shot.enabled = false;
        yield return new WaitForSeconds(0.5f);
        transform.localScale =
            Vector3.one * 1.3f;

        yield return new WaitForSeconds(0.1f);

        transform.localScale =
            Vector3.one;
        ClearBullets();

        yield return new WaitForSeconds(0.5f);

        state = newState;
        shot.shotData = data;

        shot.enabled = true;

        isChanging = false;
    }

    void ClearBullets()
    {
        EnemyBullet[] bullets = FindObjectsOfType<EnemyBullet>();

        foreach (var b in bullets)
        {
            Destroy(b.gameObject);
        }
    }
    IEnumerator ShowHpBar()
    {
        yield return new WaitForSeconds(1f);

        hpBar.SetActive(true);
    }
    IEnumerator BossDeath()
    {
        shot.enabled = false;

        ClearBullets();

        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos =
                transform.position +
                (Vector3)Random.insideUnitCircle * 2f;

            Instantiate(
                explosionPrefab,
                randomPos,
                Quaternion.identity
            );
            audioSource.PlayOneShot(BossDeathSE, 0.3f);

            float strength = 0.1f + i * 0.02f;

            StartCoroutine(CameraShake(0.15f, strength));

            yield return new WaitForSeconds(0.15f);
        }
        ScoreManager.AddScore(5000);

        // ボス画像消す
        GetComponent<SpriteRenderer>().enabled = false;

        // 最後の大爆発
        Instantiate(
            explosionPrefab,
            transform.position,
            Quaternion.identity
        );

        StartCoroutine(CameraShake(0.5f, 0.4f));

        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("ClearScene");
    }
    IEnumerator CameraShake(float duration, float strength)
    {
        Vector3 startPos = Camera.main.transform.position;

        float timer = 0f;

        while (timer < duration)
        {
            Camera.main.transform.position =
                startPos +
                Random.insideUnitSphere * strength;

            timer += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.position = startPos;
    }
}
