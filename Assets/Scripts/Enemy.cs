using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp = 100;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool stopAtPosition;
    [SerializeField] private float stopY = 2f;
    [SerializeField] private GameObject powerItemPrefab;
    [SerializeField] private GameObject scoreItemPrefab;
    public int hp { get; private set; }
    public Vector2 moveDirection = Vector2.down;
    private bool isDead = false;
    void Start()
    {
        hp = maxHp;
    }
    void Update()
    {
        if (stopAtPosition && transform.position.y <= stopY)
        {
            moveDirection = Vector2.zero;
        }
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
        if (
            transform.position.x < GameConfig.Left - GameConfig.DestroyMargin ||
            transform.position.x > GameConfig.Right + GameConfig.DestroyMargin ||
            transform.position.y < GameConfig.Bottom - GameConfig.DestroyMargin ||
            transform.position.y > GameConfig.Top + GameConfig.DestroyMargin
        )
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        hp -= damage;
        if (hp <= 0)
        {
            if (GetComponent<Boss>() == null)
            {
                isDead = true;
                ItemDrop();
                ScoreManager.AddScore(100);
                //if (ScoreManager.Instance != null)
                //{
                //  ScoreManager.Instance.AddScore(100);
                //}
                Destroy(gameObject);
            }
        }
    }
    public void ItemDrop()
    {
        Instantiate(powerItemPrefab, transform.position, Quaternion.identity);
        Instantiate(scoreItemPrefab, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
    }
}
