using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThreatType { Overboard, Window, Table }
public enum ThreatState { Idle, Warning1, Warning2, Primed }

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    [System.Serializable]
    public class Threat
    {
        public ThreatType type;

        [Header("Chance (when entering view)")]
        [Range(0f, 1f)] public float warningChanceOnEnter = 0.35f;

        [Header("Time")]
        //Time before Stage1 = Stage2
        public float timeToStage2 = 1.5f;

        //Time player has to Flash during Stage2 before it Primed
        public float timeToPrime = 2.0f;

        [Header("STAGES")]
        public GameObject stage1Object; // Window Close 1 / Overboard Bubbles / Table Thump 1
        public GameObject stage2Object; // Window Close 2 / Overboard MonsterOut / Table Thump 2
        public GameObject attackObject; // Window Attack / Overboard Attack / Table Attack 
    }

    [Header("Threat configs")]
    public List<Threat> threats = new();

    [Header("Game Over UI ")] //need to work on this
    public GameObject gameOverView;

    // View the player is currently in // so flash knows what monster to kill
    public ThreatType currentView = ThreatType.Overboard;

    private readonly Dictionary<ThreatType, ThreatState> states = new();
    private readonly Dictionary<ThreatType, Coroutine> running = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        foreach (var t in threats)
            states[t.type] = ThreatState.Idle;
    }

    Threat Get(ThreatType type) => threats.Find(t => t.type == type);

    void SetActive(GameObject go, bool on)
    {
        if (go != null) go.SetActive(on);
    }

    void ClearAll(Threat t)
    {
        if (t == null) return;
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

    // Call this when view is opened
    public void EnterView(ThreatType type)
    {
        currentView = type;

        var t = Get(type);
        if (t == null)
        {
            Debug.LogWarning($"MonsterManager: Missing config for {type}.");
            return;
        }

      // makes ure no monsters double spwan
        ClearAll(t);
        StopRoutine(type);

        // If monster is Primed, entering again = attack/game over
        if (states[type] == ThreatState.Primed)
        {
            SetActive(t.attackObject, true);
            GameOver($"Monster attacked at {type}");
            return;
        }

        // Otherwise, start warnings
        if (states[type] == ThreatState.Idle && Random.value < t.warningChanceOnEnter)
        {
            states[type] = ThreatState.Warning1;
            SetActive(t.stage1Object, true);
            running[type] = StartCoroutine(WarningFlow(type, t));
        }
        else
        {
            // nothing happens
            states[type] = ThreatState.Idle;
        }
    }

    IEnumerator WarningFlow(ThreatType type, Threat t)
    {
        // Stage1 -> wait -> Stage2
        yield return new WaitForSeconds(t.timeToStage2);

        if (states[type] != ThreatState.Warning1)
        {
            running[type] = null;
            yield break;
        }

        states[type] = ThreatState.Warning2;
        SetActive(t.stage1Object, false);
        SetActive(t.stage2Object, true);

        // Stage2 -> wait -> Primed (if player didn't Flash)
        yield return new WaitForSeconds(t.timeToPrime);

        if (states[type] == ThreatState.Warning2)
        {
            states[type] = ThreatState.Primed;
            SetActive(t.stage2Object, false);
        }

        running[type] = null;
    }

    // Always-on Flash button calls this (no parameters)
    public void Flash()
    {
        var type = currentView;

        if (!states.ContainsKey(type))
            return;

        var t = Get(type);
        if (t == null)
            return;

        // Flash only matters if something is happening / primed
        var s = states[type];
        if (s == ThreatState.Warning1 || s == ThreatState.Warning2 || s == ThreatState.Primed)
        {
            StopRoutine(type);
            states[type] = ThreatState.Idle;
            ClearAll(t);
            Debug.Log($"Flash SUCCESS at {type}");
        }
        else
        {
            Debug.Log("Flash did nothing (no threat right now).");
        }
    }

    public void GameOver(string reason)
    {
        Debug.Log("GAME OVER: " + reason);

        if (gameOverView != null)
            gameOverView.SetActive(true);

        Time.timeScale = 0f;
    }
}