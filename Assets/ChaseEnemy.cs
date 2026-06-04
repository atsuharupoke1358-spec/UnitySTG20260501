using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : MonoBehaviour
{
    public Transform player;
    public float searchSpeed = 2f;
    public float chaseSpeed = 4f;
    public float chaseRange = 5f;
    public float attackRange = 3f;
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
    public class SearchState : IChaseEnemyState
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
            if (Vector3.Distance(enemy.transform.position, enemy.player.position) <= enemy.chaseRange)
            {
                enemy.ChangeState(enemy.ChaseState);
            }
        }
        public void OnExit(ChaseEnemy enemy)
        {

        }
    }
    public class ChaseState : IChaseEnemyState
    {
        public void OnEnter(ChaseEnemy enemy)
        {

        }
        public void OnUpdate(ChaseEnemy enemy)
        {
            Vector3 dir = (enemy.player.position - enemy.transform.position).normalized;
            enemy.transform.positon += dir * enemy.chaseSpeed * Time.deltaTime;
            float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);
            if (distance <= enemy.chaseRange)
            {
                enemy.ChangeState(enemy.AttackState);
            }
            if (enemy.chaseRange <= distance)
            {
                enemy.ChangeState(enemy.SearchState);
            }
        }
        public void OnExit(ChaseEnemy enemy)
        {

        }
    }
    public class AttackState : IChaseEnemyState
    {
        public void OnEnter(ChaseEnemy enemy)
        {

        }
        public void OnUpdate(ChaseEnemy enemy)
        {

        }
        public void OnExit(ChaseEnemy enemy)
        {

        }
    }
    void Start()
    {
        Enemy = GetComponent<Enemy>();
        SearchState = new SearchState();
        ChaseState = new ChaseState();
        AttackState = new AttackState();
        ChangeState(SearchState);
    }
    void Update()
    {
        _currentState?.OnUpdate(this);
    }
    public void ChangeState(IChaseEnemyState newState)
    {
        _currentState?.OnExit(this);
        _currentState = newState;
        _currentState?.OnEnter(this);
    }
}
