namespace Core_Managers
{
    using UnityEngine;

    public class SanityManager : MonoBehaviour
    {
        // Singleton
        public static SanityManager Instance;

        [Header("Sanity Settings")]
        [Range(0, 100)]
        public float currentSanity = 100f;

        public float lowSanityThreshold1 = 75f;
        public float lowSanityThreshold2 = 50f;
        public float lowSanityThreshold3 = 25f;

        [Header("Lantern Drain")]
        public bool lanternOut = false;               // sanity drains when true
        public float lanternOutDrainRate = 5f;        // sanity per second

        [Header("Effects")]
        public bool whispersActive = false;
        public bool visualDistortionsActive = false;

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
            // Drain sanity ONLY when lantern is out
            if (lanternOut)
                ReduceSanity(lanternOutDrainRate * Time.deltaTime);

            // Trigger effects
            if (currentSanity < lowSanityThreshold1 && !whispersActive)
                whispersActive = true;

            if (currentSanity < lowSanityThreshold2 && !visualDistortionsActive)
                visualDistortionsActive = true;

            currentSanity = Mathf.Clamp(currentSanity, 0f, 100f);
        }

        //--------------------------------------------
        // FLOAT-BASED SANITY FUNCTIONS
        //--------------------------------------------

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

        //--------------------------------------------
        // LANTERN CONTROL
        //--------------------------------------------

        public void StartLanternOutDrain()
        {
            lanternOut = true;
        }

        public void StopLanternOutDrain()
        {
            lanternOut = false;
        }
    }
}