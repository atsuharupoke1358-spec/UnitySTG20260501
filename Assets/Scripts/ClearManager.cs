using TMPro;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    public TMP_Text scoreText;

    void Start()
    {
        scoreText.text =
            "SCORE\n" + ScoreManager.score;
    }
}