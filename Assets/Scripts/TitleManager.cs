using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OpenHowToPlay()
    {
        SceneManager.LoadScene("HowToPlayScene");
    }
}