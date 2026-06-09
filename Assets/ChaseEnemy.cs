using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : MonoBehaviour
{
    public GameObject player;
    public float searchSpeed = 2f;
    public float chaseSpeed = 4f;
    public float chaseRange = 3f;
    public float attackRange = 1f;
    public Vector3 moveDirection { get; set; }
    public EnemyShot enemyShot { get; private set; }
    public Enemy Enemy { get; private set; }
    private IChaseEnemyState _currentState;
    public IChaseEnemyState SearchState { get; private set; }
    public IChaseEnemyState ChaseState { get; private set; }
    public IChaseEnemyState AttackState { get; private set; }
    public interface IChaseEnemyState
    {
        void OnEnter(ChaseEnemy enemy);
        void OnUpdate(ChaseEnemy enemy);
        void OnExit(ChaseEnemy enemy);
    }
    public class EnemySearchState : IChaseEnemyState
    {
        private Vector3[] patrolPoints = new Vector3[]
        {
            new Vector3(4, -4, 0),  // 最初の目標
            new Vector3(-4, -4, 0), // 2番目の目標
            new Vector3(-4, 4, 0),  // 3番目の目標
            new Vector3(4, 4, 0)    // 4番目の目標
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
                enemy.ChangeState(enemy.ChaseState);
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
                enemy.ChangeState(enemy.SearchState);
                return;
            }
            Vector3 dir = (enemy.player.transform.position - enemy.transform.position).normalized;
            enemy.transform.position += dir * enemy.chaseSpeed * Time.deltaTime;
            float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
            if (distance <= enemy.attackRange)
            {
                Debug.Log("go attackestate");
                enemy.ChangeState(enemy.AttackState);
            }
            else if (distance >= enemy.chaseRange)
            {
                enemy.ChangeState(enemy.SearchState);
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
                enemy.ChangeState(enemy.SearchState);
                return;
            }
            if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) >= enemy.attackRange)
            {
                Debug.Log("go changestate");
                enemy.ChangeState(enemy.ChaseState);
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
        Enemy = GetComponent<Enemy>();
        enemyShot = GetComponent<EnemyShot>();

        enemyShot.canShoot = false;
        Player realPlayer = FindFirstObjectByType<Player>();
        if (realPlayer != null)
        {
            player = realPlayer.gameObject;
        }

        SearchState = new EnemySearchState();
        ChaseState = new EnemyChaseState();
        AttackState = new EnemyAttackState();
        ChangeState(SearchState);
    }
    void Update()
    {
        if (this.Enemy.hp <= 0)
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
