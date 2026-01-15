using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlashController : MonoBehaviour
{
    public GameObject torchOverlay;
    public float flashTime = 0.15f;

    private Coroutine co;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PressFlash()
    {
        if (co != null)
            StopCoroutine(co);

        co = StartCoroutine(FlashRoutine());

        if (MonsterManager.Instance != null)
            MonsterManager.Instance.Flash();
    }

    IEnumerator FlashRoutine()
    {
        if (torchOverlay != null)
            torchOverlay.SetActive(true);

        yield return new WaitForSeconds(flashTime);

        if (torchOverlay != null)
            torchOverlay.SetActive(false);

        co = null;
    }

    // RESET when starting a new game
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (torchOverlay != null)
            torchOverlay.SetActive(false);

        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}