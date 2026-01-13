using UnityEngine;
using UnityEngine.UI;
using Core_Managers;

public class InsanityBarUI : MonoBehaviour
{
    public Image insanityFillImage;

    void Update()
    {
        if (SanityManager.Instance == null) return;

        // Convert sanity (0–100) to fill (0–1)
        float fill = 1f - (SanityManager.Instance.currentSanity / 100f);

        insanityFillImage.fillAmount = fill;
    }
}