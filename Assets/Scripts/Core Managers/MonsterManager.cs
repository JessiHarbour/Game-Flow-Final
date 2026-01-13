using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core_Managers;

public enum ThreatType { Overboard, Window, Table }
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

    public ThreatType currentView = ThreatType.Overboard;

    private readonly Dictionary<ThreatType, ThreatState> states = new();
    private readonly Dictionary<ThreatType, Coroutine> running = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        foreach (var t in threats)
        {
            states[t.type] = ThreatState.Idle;
            StartCoroutine(AmbientThreatLoop(t));
        }
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

            // Increase spawn rate when sanity < 50%
            if (SanityManager.Instance != null &&
                SanityManager.Instance.currentSanity < 50f)
            {
                sanityMultiplier = 0.65f; // faster spawns
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

        // Kill timer
        yield return new WaitForSeconds(2f);

        if (states[type] == ThreatState.Primed)
        {
            TriggerGameOver(type);
        }
    }

   
    // FLASH 
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
            Debug.Log($"Flash SUCCESS at {type}");
        }
    }
    
    // GAME OVER
    void TriggerGameOver(ThreatType type)
    {
        Debug.Log("GAME OVER — Monster killed player at " + type);
        Time.timeScale = 0f;

        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    }
}