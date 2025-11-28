using UnityEngine;
using UnityEngine.UI;

namespace Core_Managers
{
    public class LanternController : MonoBehaviour
    {
        public enum LanternState { On, Flickering, Out }
        public LanternState state = LanternState.On;

        [Header("References")]
        public Image lanternGlow;            
        public SanityManager sanityManager;

        [Header("Sprites")]
        public Sprite lanternOnSprite;       
        public Sprite lanternOffSprite;      

        private Image lanternImage;          

        [Header("Flicker Settings")]
        public float flickerDuration = 2f;         
        public float strobeSpeed = 0.08f;          
        public float minFlickerInterval = 8f;      
        public float maxFlickerInterval = 15f;     

        private float flickerTimer = 0f;
        private float nextFlickerTime = 0f;
        private float strobeTimer = 0f;

        void Start()
        {
            lanternImage = GetComponent<Image>();
            lanternImage.sprite = lanternOnSprite;

            ScheduleNextFlicker();
        }

        void Update()
        {
            // random flicker trigger loop
            if (state == LanternState.On && Time.time >= nextFlickerTime)
            {
                StartFlicker();
            }

            switch (state)
            {
                case LanternState.Flickering:
                    RunFlicker();
                    break;

                case LanternState.Out:
                    lanternImage.sprite = lanternOffSprite;
                    lanternGlow.color = new Color(1f, 1f, 1f, 0.2f);
                    break;
            }
        }
        //relight/ stablize
        public void OnLanternClicked()
        {
            if (state == LanternState.Flickering)
                StabilizeLantern();
            else if (state == LanternState.Out)
                RelightLantern();
            else if (state == LanternState.On)
                FlashLantern();
        }
        // lanturn flickering
        public void StartFlicker()
        {
            if (state != LanternState.On) return;

            state = LanternState.Flickering;
            flickerTimer = flickerDuration;
            strobeTimer = 0f;
            
            lanternImage.sprite = lanternOnSprite;
            lanternGlow.color = Color.white;
        }
        
        private void RunFlicker()
        {
            strobeTimer += Time.deltaTime;

            if (strobeTimer >= strobeSpeed)
            {
                lanternImage.sprite = 
                    (lanternImage.sprite == lanternOnSprite) ? lanternOffSprite : lanternOnSprite;

                strobeTimer = 0f;
            }

            // light glow flickers
            float alpha = Mathf.PingPong(Time.time * 12f, 1f);
            lanternGlow.color = new Color(1f, 1f, 1f, alpha);

            // small sanity drain while flickering
            sanityManager.ReduceSanity(0.5f * Time.deltaTime);

            flickerTimer -= Time.deltaTime;

            if (flickerTimer <= 0f)
            {
                ExtinguishLantern();
            }
        }

        // succesfull stablize 
        private void StabilizeLantern()
        {
            state = LanternState.On;

            lanternImage.sprite = lanternOnSprite;
            lanternGlow.color = Color.white;

            // reward sanity
            sanityManager.AddSanity(2f);

            ScheduleNextFlicker();
        }
 
        //fail to stablize
        private void ExtinguishLantern()
        {
            state = LanternState.Out;

            lanternImage.sprite = lanternOffSprite;
            lanternGlow.color = new Color(1f, 1f, 1f, 0.2f);

            // start heavy sanity drain
            sanityManager.StartLanternOutDrain();
        }

      // relight
        private void RelightLantern()
        {
            state = LanternState.On;

            lanternImage.sprite = lanternOnSprite;
            lanternGlow.color = Color.white;

            sanityManager.StopLanternOutDrain();
            ScheduleNextFlicker();
        }
        
        // FLASH? not sure if will be sepearte light
        private void FlashLantern()
        {
            // reserved for monster system
        }
        
        // randomize (schedual next flicker)
        private void ScheduleNextFlicker()
        {
            nextFlickerTime = Time.time + Random.Range(minFlickerInterval, maxFlickerInterval);
        }
    }
}