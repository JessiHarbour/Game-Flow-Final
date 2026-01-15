using UnityEngine;
using UnityEngine.SceneManagement;
using Core_Managers;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Reset time in case GameOver paused it
        Time.timeScale = 1f;

        // Destroy old singleton 
        if (MonsterManager.Instance != null)
        {
            Destroy(MonsterManager.Instance.gameObject);
        }

        if (SanityManager.Instance != null)
        {
            Destroy(SanityManager.Instance.gameObject);
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}