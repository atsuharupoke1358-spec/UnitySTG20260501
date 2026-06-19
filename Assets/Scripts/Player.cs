using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed = 5;
    public int slowSpeed = 2;
    public int life = 3;
    public Vector3 respawnPosition = new Vector3(-3, 0, 0);
    bool isInvincible = false;
    public bool isDead;
    public int laserPower = 1;
    public int homingPower = 1;
    PlayerShot playerShot;
    public AudioSource audioSource;
    public AudioClip hitSE;
    void Start()
    {
        playerShot = GetComponent<PlayerShot>();
    }
    void Update()
    {
        //移動
        float x = 0;
        float y = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            x += 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            y += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            y -= 1;
        }
        float length = Mathf.Sqrt(x * x + y * y);
        if (length != 0)
        {
            x /= length;
            y /= length;
        }
        Vector3 dir = new Vector3(x, y, 0);
        int currentSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = slowSpeed;
        }

        transform.position += dir * currentSpeed * Time.deltaTime;

        // 範囲制限
        float px =
            Mathf.Clamp(
                transform.position.x,
                GameConfig.Left,
                GameConfig.Right
            );

        float py =
            Mathf.Clamp(
                transform.position.y,
                GameConfig.Bottom,
                GameConfig.Top
            );

        transform.position =
            new Vector3(px, py, 0);
    }
    //時期破壊
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Debug.Log("被弾判定 isInvincible=" + isInvincible);
        }

        if (other.CompareTag("EnemyBullet") && !isInvincible)
        {
            audioSource.PlayOneShot(hitSE);
            life--;

            StartCoroutine(DamageFlash());

            if (life <= 0)
            {
                gameObject.SetActive(false);
                SceneManager.LoadScene("GameOverScene");
            }
            else
            {
                StartCoroutine(Respawn());
            }
        }

        if (other.CompareTag("ScoreItem"))
        {
            Destroy(other.gameObject);
        }
    }
    IEnumerator Respawn()
    {
        isInvincible = true;

        transform.position = respawnPosition;

        Debug.Log("Respawn:" + transform.position);

        StartCoroutine(InvincibleTime());

        isDead = false;

        yield return null;
    }
    IEnumerator InvincibleTime()
    {
        SpriteRenderer sr =
            GetComponent<SpriteRenderer>();

        float timer = 0f;

        while (timer < 2f)
        {
            sr.enabled = !sr.enabled;

            yield return new WaitForSeconds(0.1f);

            timer += 0.1f;
        }

        sr.enabled = true;

        isInvincible = false;
    }
    public void GetPowerUp()
    {
        if (playerShot.isLaserMode)
        {
            laserPower++;
            Debug.Log(laserPower);
        }
        else
        {
            homingPower++;
        }
    }
    IEnumerator DamageFlash()
    {
        SpriteRenderer sr =
            GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        for (int i = 0; i < 3; i++)
        {
            sr.color = Color.white;

            yield return new WaitForSeconds(0.05f);

            sr.color = Color.red;

            yield return new WaitForSeconds(0.05f);
        }

        sr.color = originalColor;
    }
}
