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

        public float lowSanityThreshold1 = 75f;
        public float lowSanityThreshold2 = 50f;
        public float lowSanityThreshold3 = 25f;

        [Header("Lantern Drain")]
        public bool lanternOut = false;
        public float lanternOutDrainRate = 5f;

        [Header("Effects")]
        public bool whispersActive = false;
        public bool visualDistortionsActive = false;

        [Header("Game Over")]
        public string gameOverSceneName = "GameOver";
        private bool gameOverTriggered = false;

        private void Awake()
        {
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

           
            if (currentSanity < lowSanityThreshold1 && !whispersActive)
                whispersActive = true;

            if (currentSanity < lowSanityThreshold2 && !visualDistortionsActive)
                visualDistortionsActive = true;

            currentSanity = Mathf.Clamp(currentSanity, 0f, 100f);

            // Game over 
            if (currentSanity <= 0f)
            {
                TriggerGameOver();
            }
        }

        public void ReduceSanity(float amount)
        {
            currentSanity -= amount;
            currentSanity = Mathf.Clamp(currentSanity, 0f, 100f);
            Debug.Log("Sanity reduced: " + currentSanity);
        }

        public void AddSanity(float amount)
        {
            currentSanity += amount;
            currentSanity = Mathf.Clamp(currentSanity, 0f, 100f);
            Debug.Log("Sanity increased: " + currentSanity);
        }

        public bool IsInsane()
        {
            return currentSanity <= 0f;
        }

        // LANTERN CONTROL
        public void StartLanternOutDrain()
        {
            lanternOut = true;
        }

        public void StopLanternOutDrain()
        {
            lanternOut = false;
        }

        // GAME OVER
        private void TriggerGameOver()
        {
            if (gameOverTriggered)
                return;

            gameOverTriggered = true;
            SceneManager.LoadScene(gameOverSceneName);
        }
    }
}