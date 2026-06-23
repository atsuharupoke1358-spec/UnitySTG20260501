using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int score;

    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        score = 0;
    }

    void Update()
    {
        scoreText.text = "SCORE : " + score;
    }

    public static void AddScore(int amount)
    {
        score += amount;
    }
}