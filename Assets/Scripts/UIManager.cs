using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static int score;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text lifeText;
    [SerializeField] private TMP_Text bombText;
    [SerializeField] private TMP_Text laserPowerText;
    [SerializeField] private TMP_Text homingPowerText;
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
            lifeText.text = "Life\n" + player.life.ToString("D3");
        }

        if (bombText != null)
        {
            bombText.text = "Bomb\n" + player.bombStock.ToString("D3");
        }
        if (laserPowerText != null)
        {
            laserPowerText.text = "Laser Pw\n" + player.laserPower.ToString("D2");
        }
        if (homingPowerText != null)
        {
            homingPowerText.text = "Homing Pw\n" + player.homingPower.ToString("D2");
        }
    }

    public static void AddScore(int amount)
    {
        score += amount;
    }
}