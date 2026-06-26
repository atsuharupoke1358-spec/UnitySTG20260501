using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static int score;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text lifeText;
    [SerializeField] private TMP_Text bombText;
    private Player player;

    void Start()
    {
        score = 0;
        player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score\n" + score.ToString("D6");
        }

        if (player == null) return;

        if (lifeText != null)
        {
            lifeText.text = player.life.ToString("D3");
        }

        if (bombText != null)
        {
            bombText.text = player.bombStock.ToString("D3");
        }
    }

    public static void AddScore(int amount)
    {
        score += amount;
    }
}