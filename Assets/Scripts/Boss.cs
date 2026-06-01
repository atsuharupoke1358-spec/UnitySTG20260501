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
    public Enemy Enemy { get; private set; }
    BossState state;
    public GameObject hpBar;
    public Image hpBarFill;
    public GameObject explosionPrefab;
    public AudioSource audioSource;
    public AudioClip BossDeathSE;
    private IBossState _currentState;
    public IBossState Phase1State { get; private set; }
    public IBossState Phase2State { get; private set; }
    public IBossState Phase3State { get; private set; }
    public IBossState DeathState { get; private set; }
    public interface IBossState
    {
        void OnEnter(Boss boss);  // この状態に入った瞬間
        void OnUpdate(Boss boss); // 毎フレームの処理
        void OnExit(Boss boss);   // この状態から出る瞬間
    }
    public class BossPhase1State : IBossState
    {
        public void OnEnter(Boss boss)
        {
            boss.shot.shotData = boss.phase1;
            boss.shot.enabled = true;
        }

        public void OnUpdate(Boss boss)
        {
            if (boss.Enemy.hp <= 1000)
            {
                boss.ChangeState(boss.Phase2State);
            }
        }

        public void OnExit(Boss boss) { }
    }

    public class BossPhase2State : IBossState
    {
        public void OnEnter(Boss boss)
        {
            boss.StartCoroutine(boss.ChangeStateWithDelay(boss.phase2));
        }

        public void OnUpdate(Boss boss)
        {
            if (boss.Enemy.hp <= 500)
            {
                boss.ChangeState(boss.Phase3State);
            }
        }

        public void OnExit(Boss boss) { }
    }

    public class BossPhase3State : IBossState
    {
        public void OnEnter(Boss boss)
        {
            boss.StartCoroutine(boss.ChangeStateWithDelay(boss.phase3));
        }

        public void OnUpdate(Boss boss) { }
        public void OnExit(Boss boss) { }
    }

    public class BossDeathState : IBossState
    {
        public void OnEnter(Boss boss)
        {
            Debug.Log("BossDeath開始");
            boss.shot.enabled = false;
            boss.ClearBullets();
            boss.StartCoroutine(boss.BossDeath());
        }

        public void OnUpdate(Boss boss) { }
        public void OnExit(Boss boss) { }
    }
    void Start()
    {
        hpBar.SetActive(false);
        Enemy = GetComponent<Enemy>();

        Phase1State = new BossPhase1State();
        Phase2State = new BossPhase2State();
        Phase3State = new BossPhase3State();
        DeathState = new BossDeathState();

        shot.shotData = phase1;
        StartCoroutine(ShowHpBar());

        ChangeState(Phase1State);
    }

    void Update()
    {
        hpBarFill.fillAmount = (float)Enemy.hp / Enemy.maxHp;
        if (Enemy.hp <= 0 && _currentState is not BossDeathState)
        {
            ChangeState(DeathState);
            return;
        }
        _currentState?.OnUpdate(this);

        /*if (state == BossState.Phase1 && enemy.hp <= 1000 && !isChanging)
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
        }*/
    }
    public void ChangeState(IBossState nextState)
    {
        _currentState?.OnExit(this);
        _currentState = nextState;
        _currentState?.OnEnter(this);
    }
    IEnumerator ChangeStateWithDelay(ShotData data)
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

        shot.shotData = data;

        shot.enabled = true;
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

        GetComponent<SpriteRenderer>().enabled = false;

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
