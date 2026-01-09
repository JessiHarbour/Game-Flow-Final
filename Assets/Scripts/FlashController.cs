using System.Collections;
using UnityEngine;

public class FlashController : MonoBehaviour
{
    public GameObject torchOverlay;   
    public float flashTime = 0.15f;

    Coroutine co;

    public void PressFlash()
    {
      
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(FlashRoutine());

        if (MonsterManager.Instance != null)
            MonsterManager.Instance.Flash();
    }

    IEnumerator FlashRoutine()
    {
        if (torchOverlay != null) torchOverlay.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        if (torchOverlay != null) torchOverlay.SetActive(false);
    }
}