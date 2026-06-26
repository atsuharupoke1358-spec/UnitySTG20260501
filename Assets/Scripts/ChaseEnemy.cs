using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float searchSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float chaseRange = 3f;
    [SerializeField] private float attackRange = 1f;
    public Vector3 moveDirection { get; set; }
    public EnemyShot enemyShot { get; private set; }
    public Enemy enemy { get; private set; }
    private IChaseEnemyState _currentState;
    public IChaseEnemyState searchState { get; private set; }
    public IChaseEnemyState chaseState { get; private set; }
    public IChaseEnemyState attackState { get; private set; }
    public interface IChaseEnemyState
    {
        void OnEnter(ChaseEnemy enemy);
        void OnUpdate(ChaseEnemy enemy);
        void OnExit(ChaseEnemy enemy);
    }
    private class EnemySearchState : IChaseEnemyState
    {
        private Vector3[] patrolPoints = new Vector3[]
        {
            new Vector3(2, -2, 0),  // 最初の目標
            new Vector3(-2, -2, 0), // 2番目の目標
            new Vector3(-2, 2, 0),  // 3番目の目標
            new Vector3(2, 2, 0)    // 4番目の目標
        };
        private int currentPointIndex = 0;
        public void OnEnter(ChaseEnemy enemy)
        {

        }
        public void OnUpdate(ChaseEnemy enemy)
        {
            bool hasPlayer = enemy.player != null;
            Vector3 targetPosition = patrolPoints[currentPointIndex];
            if (Vector3.Distance(enemy.transform.position, targetPosition) < 0.1f)
            {
                currentPointIndex++;
                if (currentPointIndex >= patrolPoints.Length)
                {
                    currentPointIndex = 0;
                }
                targetPosition = patrolPoints[currentPointIndex];
            }
            enemy.moveDirection = (targetPosition - enemy.transform.position).normalized;
            enemy.transform.position += enemy.moveDirection * enemy.searchSpeed * Time.deltaTime;
            if (hasPlayer && Vector3.Distance(enemy.transform.position, enemy.player.transform.position) <= enemy.chaseRange)
            {
                enemy.ChangeState(enemy.chaseState);
            }
        }
        public void OnExit(ChaseEnemy enemy)
        {

        }
    }
    public class EnemyChaseState : IChaseEnemyState
    {
        public void OnEnter(ChaseEnemy enemy)
        {

        }
        public void OnUpdate(ChaseEnemy enemy)
        {
            if (enemy.player == null)
            {
                enemy.ChangeState(enemy.searchState);
                return;
            }
            Vector3 dir = (enemy.player.transform.position - enemy.transform.position).normalized;
            enemy.transform.position += dir * enemy.chaseSpeed * Time.deltaTime;
            float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
            if (distance <= enemy.attackRange)
            {
                Debug.Log("go attackestate");
                enemy.ChangeState(enemy.attackState);
            }
            else if (distance >= enemy.chaseRange)
            {
                enemy.ChangeState(enemy.searchState);
            }
        }
        public void OnExit(ChaseEnemy enemy)
        {

        }
    }
    public class EnemyAttackState : IChaseEnemyState
    {
        public void OnEnter(ChaseEnemy enemy)
        {
            enemy.enemyShot.canShoot = true;
        }
        public void OnUpdate(ChaseEnemy enemy)
        {
            if (enemy.player == null)
            {
                enemy.ChangeState(enemy.searchState);
                return;
            }
            if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) >= enemy.attackRange)
            {
                Debug.Log("go changestate");
                enemy.ChangeState(enemy.chaseState);
            }
            //Vector3 dir = (enemy.player.position - enemy.transform.position).normalized;
            //enemy.transform.position += dir * (enemy.chaseSpeed * 0.3f) * Time.deltaTime;
        }
        public void OnExit(ChaseEnemy enemy)
        {
            enemy.enemyShot.canShoot = false;
        }
    }
    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyShot = GetComponent<EnemyShot>();

        enemyShot.canShoot = false;
        Player realPlayer = FindFirstObjectByType<Player>();
        if (realPlayer != null)
        {
            player = realPlayer.gameObject;
        }

        searchState = new EnemySearchState();
        chaseState = new EnemyChaseState();
        attackState = new EnemyAttackState();
        ChangeState(searchState);
    }
    void Update()
    {
        if (enemy != null && enemy.hp <= 0)
        {
            Destroy(gameObject);
            return;
        }
        _currentState?.OnUpdate(this);
    }
    public void ChangeState(IChaseEnemyState newState)
    {
        _currentState?.OnExit(this);
        _currentState = newState;
        _currentState?.OnEnter(this);
    }
}
