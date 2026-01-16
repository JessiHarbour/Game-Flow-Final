using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core_Managers;
using UnityEngine.SceneManagement;

public enum ThreatType { Overboard, Window, Table, DeckView, BelowDeckView }
public enum ThreatState { Idle, Warning1, Warning2, Primed }

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    [System.Serializable]
    public class Threat
    {
        public ThreatType type;

        [Header("Time")]
        public float timeToStage2 = 1.5f;
        public float timeToPrime = 2.0f;

        [Header("Ambient Spawning")]
        public float minSpawnDelay = 6f;
        public float maxSpawnDelay = 12f;

        [Header("STAGES")]
        public GameObject stage1Object;
        public GameObject stage2Object;
        public GameObject attackObject;
    }

    [Header("Threat configs")]
    public List<Threat> threats = new();

    [Header("Audio")]
    public AudioClip monsterDeathSound;   // flash kills monster
    public AudioClip playerDeathSound;    // monster kills player
    private AudioSource audioSource;

    public ThreatType currentView = ThreatType.Overboard;

    private readonly Dictionary<ThreatType, ThreatState> states = new();
    private readonly Dictionary<ThreatType, Coroutine> running = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        ResetAllThreats();
    }

    Threat Get(ThreatType type) => threats.Find(t => t.type == type);

    void SetActive(GameObject go, bool on)
    {
        if (go != null) go.SetActive(on);
    }

    void ClearAll(Threat t)
    {
        SetActive(t.stage1Object, false);
        SetActive(t.stage2Object, false);
        SetActive(t.attackObject, false);
    }

    void StopRoutine(ThreatType type)
    {
        if (running.TryGetValue(type, out var co) && co != null)
            StopCoroutine(co);

        running[type] = null;
    }

    // PLAYER VIEW
    public void EnterView(ThreatType type)
    {
        currentView = type;

        var t = Get(type);
        if (t == null) return;

        if (states[type] == ThreatState.Primed)
        {
            SetActive(t.attackObject, true);
        }
    }

    // AMBIENT SPAWNING
    IEnumerator AmbientThreatLoop(Threat t)
    {
        while (true)
        {
            float sanityMultiplier = 1f;

            if (SanityManager.Instance != null &&
                SanityManager.Instance.currentSanity < 50f)
            {
                sanityMultiplier = 0.65f;
            }

            float wait = Random.Range(t.minSpawnDelay, t.maxSpawnDelay) * sanityMultiplier;
            yield return new WaitForSeconds(wait);

            if (states[t.type] != ThreatState.Idle)
                continue;

            states[t.type] = ThreatState.Warning1;
            SetActive(t.stage1Object, true);
            running[t.type] = StartCoroutine(WarningFlow(t.type, t));
        }
    }

    IEnumerator WarningFlow(ThreatType type, Threat t)
    {
        yield return new WaitForSeconds(t.timeToStage2);

        if (states[type] != ThreatState.Warning1) yield break;

        states[type] = ThreatState.Warning2;
        SetActive(t.stage1Object, false);
        SetActive(t.stage2Object, true);

        yield return new WaitForSeconds(t.timeToPrime);

        if (states[type] != ThreatState.Warning2) yield break;

        states[type] = ThreatState.Primed;
        SetActive(t.stage2Object, false);
        SetActive(t.attackObject, true);

        // 1 second to flash
        yield return new WaitForSeconds(1f);

        if (states[type] == ThreatState.Primed)
        {
            TriggerGameOver(type);
        }
    }

    // FLASH (KILLS MONSTER)
    public void Flash()
    {
        var type = currentView;

        if (!states.ContainsKey(type)) return;

        var t = Get(type);
        if (t == null) return;

        if (states[type] != ThreatState.Idle)
        {
            StopRoutine(type);
            states[type] = ThreatState.Idle;
            ClearAll(t);

            if (audioSource != null && monsterDeathSound != null)
            {
                audioSource.ignoreListenerPause = true;
                audioSource.PlayOneShot(monsterDeathSound);
            }
        }
    }

    // GAME OVER
    void TriggerGameOver(ThreatType type)
    {
        if (audioSource != null && playerDeathSound != null)
        {
            audioSource.ignoreListenerPause = true;
            audioSource.PlayOneShot(playerDeathSound);
        }

        Time.timeScale = 0f;
        StartCoroutine(LoadGameOverDelayed());
    }

    IEnumerator LoadGameOverDelayed()
    {
        yield return new WaitForSecondsRealtime(1.2f);
        Triggerwin();
        SceneManager.LoadScene("GameOver");
    }

    // RESET ON NEW GAME
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "deck")
        {
            ResetAllThreats();
        }
    }

    private void ResetAllThreats()
    {
        StopAllCoroutines();
        states.Clear();
        running.Clear();

        foreach (var t in threats)
        {
            states[t.type] = ThreatState.Idle;
            ClearAll(t);
            StartCoroutine(AmbientThreatLoop(t));
        }

        currentView = ThreatType.Overboard;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public void Triggerwin()
    {
        Time.timeScale = 0f;
    }
}