using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void retry()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
