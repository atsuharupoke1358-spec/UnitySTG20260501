using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void BackTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}