
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    private int Hours = 12;
    private int Minutes = 0;

    private TextMeshProUGUI clockText;

    [SerializeField] private float delay = 1f;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        clockText = GetComponent<TextMeshProUGUI>();
        ResetClock();
    }

    private void Tick()
    {
        Minutes += 1;

        if (Minutes == 60)
        {
            Minutes = 0;

            if (Hours == 12)
                Hours = 1;
            else
                Hours += 1;
        }

        // display
        if (Hours == 12)
            clockText.text = Hours + ":" + Minutes.ToString("00") + " AM";
        else
            clockText.text = Hours.ToString("00") + ":" + Minutes.ToString("00") + " AM";

        // end condition
        if (Hours != 3)
        {
            Invoke(nameof(Tick), delay);
        }
        else
        {
            SceneManager.LoadScene("Ending");
        }
    }

    private void ResetClock()
    {
        CancelInvoke();

        Hours = 12;
        Minutes = 0;

        clockText.text = "12:00 AM";
        Invoke(nameof(Tick), delay);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "deck")
        {
            ResetClock();
        }
    }

    private void OnDestroy()
    {
        CancelInvoke();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}