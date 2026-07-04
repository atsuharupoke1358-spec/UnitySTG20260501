using TMPro;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    void Start()
    {
        scoreText.text = "SCORE\n" + UIManager.score;

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "HI-SCORE\n" + highScore.ToString("D6");
    }
}