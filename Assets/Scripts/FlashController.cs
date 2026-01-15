using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using View_and_UI_Controllers; 


public class FlashController : MonoBehaviour
{
    [Header("Torch Settings")]
    public GameObject torchOverlay;
    public float flashTime = 0.15f;

    [Header("UI")]
    public Button torchButton;            
    public Color enabledColor = Color.white;
    public Color disabledColor = Color.gray;

    private Coroutine co;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateTorchButtonState(); 
    }

    void Update()
    {
        UpdateTorchButtonState(); // constantly update based on current view
    }

    // Press the torch button
    public void PressFlash()
    {
        if (torchButton == null || !torchButton.interactable) return;

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

    // Enable/disable torch button based on the player's current view
    private void UpdateTorchButtonState()
    {
        if (torchButton == null || ViewManager.Instance == null) return;

        var view = ViewManager.Instance.currentView;

        // Only enable torch if player is looking at a monster view
        bool canUseTorch = (view == ViewManager.PlayerView.Overboard ||
                            view == ViewManager.PlayerView.Window ||
                            view == ViewManager.PlayerView.Table);

        torchButton.interactable = canUseTorch;

        // Update visual color
        var colors = torchButton.colors;
        colors.normalColor = canUseTorch ? enabledColor : disabledColor;
        torchButton.colors = colors;
    }

    // Reset when loading a new scene
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (torchOverlay != null)
            torchOverlay.SetActive(false);

        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }

        UpdateTorchButtonState();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
