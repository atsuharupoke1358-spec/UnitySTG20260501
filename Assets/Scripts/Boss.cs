using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
public class Boss : MonoBehaviour
{
    [SerializeField] private EnemyShot shot;
    [SerializeField] private ShotData phase1;
    [SerializeField] private ShotData phase2;
    [SerializeField] private ShotData phase3;
    [SerializeField] private ShotData phase4;
    public enum BossState
    {
        Phase1,
        Phase2,
        Phase3,
        Phase4
    }
    public Enemy enemy { get; private set; }
    private BossState state;
    [SerializeField] private GameObject hpBar;
    [SerializeField] private Image hpBarFill;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bossDeathSE;
    private IBossState _currentState;
    public IBossState phase1State { get; private set; }
    public IBossState phase2State { get; private set; }
    public IBossState phase3State { get; private set; }
    public IBossState phase4State { get; private set; }
    public IBossState deathState { get; private set; }
    public interface IBossState
    {
        void OnEnter(Boss boss);  // この状態に入った瞬間
        void OnUpdate(Boss boss); // 毎フレームの処理
        void OnExit(Boss boss);   // この状態から出る瞬間
    }
    private class BossPhase1State : IBossState
    {
        public void OnEnter(Boss boss)
        {
            boss.shot.shotData = boss.phase1;
            boss.shot.enabled = true;
        }

        public void OnUpdate(Boss boss)
        {
            if (boss.enemy.hp <= 2000)
            {
                boss.ChangeState(boss.phase2State);
            }
        }

        public void OnExit(Boss boss) { }
    }

    private class BossPhase2State : IBossState
    {
        public void OnEnter(Boss boss)
        {
            boss.StartCoroutine(boss.ChangeStateWithDelay(boss.phase2));
        }

        public void OnUpdate(Boss boss)
        {
            if (boss.enemy.hp <= 1800)
            {
                boss.ChangeState(boss.phase3State);
            }
        }

        public void OnExit(Boss boss) { }
    }

    private class BossPhase3State : IBossState
    {
        public void OnEnter(Boss boss)
        {
            boss.StartCoroutine(boss.ChangeStateWithDelay(boss.phase3));
        }

        public void OnUpdate(Boss boss)
        {
            if (boss.enemy.hp <= 1500)
            {
                boss.ChangeState(boss.phase4State);
            }
        }

        public void OnExit(Boss boss) { }
    }
    public class BossPhase4State : IBossState
    {
        public void OnEnter(Boss boss)
        {
            boss.StartCoroutine(boss.ChangeStateWithDelay(boss.phase4));
        }

        public void OnUpdate(Boss boss) { }
        public void OnExit(Boss boss) { }
    }

    private class BossDeathState : IBossState
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
    public void SetupHpBar(GameObject bar, Image fill)
    {
        hpBar = bar;
        hpBarFill = fill;
    }
    void Start()
    {
        hpBar.SetActive(false);
        enemy = GetComponent<Enemy>();

        phase1State = new BossPhase1State();
        phase2State = new BossPhase2State();
        phase3State = new BossPhase3State();
        phase4State = new BossPhase4State();
        deathState = new BossDeathState();

        shot.shotData = phase1;
        StartCoroutine(ShowHpBar());

        ChangeState(phase1State);
    }

    void Update()
    {
        hpBarFill.fillAmount = (float)enemy.hp / enemy.maxHp;
        if (enemy.hp <= 0 && _currentState is not BossDeathState)
        {
            ChangeState(deathState);
            return;
        }
        _currentState?.OnUpdate(this);
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
            b.gameObject.SetActive(false);
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
            audioSource.PlayOneShot(bossDeathSE, 0.3f);

            float strength = 0.1f + i * 0.02f;

            StartCoroutine(CameraShake(0.15f, strength));

            yield return new WaitForSeconds(0.15f);
        }
        UIManager.AddScore(5000);
        //ScoreManager.Instance.AddScore(5000);

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
