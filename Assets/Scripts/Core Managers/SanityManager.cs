using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core_Managers
{
    public class SanityManager : MonoBehaviour
    {
        public static SanityManager Instance;

        [Header("Sanity Settings")]
        [Range(0, 100)]
        public float currentSanity = 100f;

        [Header("Lantern Drain")]
        public bool lanternOut = false;
        public float lanternOutDrainRate = 5f;

        [Header("Game Over")]
        public string gameOverSceneName = "GameOver";
        private bool gameOverTriggered = false;

        private void Awake()
        {
            gameOverTriggered = false;
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            currentSanity = 100f;
        }

        private void Update()
        {
            if (gameOverTriggered)
                return;

            // Drain sanity when lantern is out
            if (lanternOut)
                ReduceSanity(lanternOutDrainRate * Time.deltaTime);

            
            currentSanity = Mathf.Clamp(currentSanity, 0f, 100f);

           
            if (currentSanity <= 0f)
            {
                TriggerGameOver();
            }
        }

        // Reduce sanity 
        public void ReduceSanity(float amount)
        {
            currentSanity -= amount;
            currentSanity = Mathf.Clamp(currentSanity, 0f, 100f);
            Debug.Log("Sanity reduced: " + currentSanity);
        }

        // Increase sanity 
        public void AddSanity(float amount)
        {
            currentSanity += amount;
            currentSanity = Mathf.Clamp(currentSanity, 0f, 100f);
            Debug.Log("Sanity increased: " + currentSanity);
        }

        // Check if player has zero sanity
        public bool IsInsane()
        {
            return currentSanity <= 0f;
        }

        
        public void StartLanternOutDrain()
        {
            lanternOut = true;
        }

        public void StopLanternOutDrain()
        {
            lanternOut = false;
        }

        // Trigger game over
        private void TriggerGameOver()
        {
            if (gameOverTriggered)
                return;

            gameOverTriggered = true;
            SceneManager.LoadScene(gameOverSceneName);
        }
    }
}
