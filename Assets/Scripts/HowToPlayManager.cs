using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayManager : MonoBehaviour
{
    public void BackTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}