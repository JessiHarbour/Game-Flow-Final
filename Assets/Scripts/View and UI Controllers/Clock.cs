using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    private static int Hours = 12;
    private static int Minutes;
    private TextMeshProUGUI ass;
    [SerializeField] private float delay = 1f;
    void Start()
    {
      
       
       ass = this.GetComponent<TextMeshProUGUI>();
            ass.text = Hours.ToString() + ":" + Minutes.ToString("00") + " AM";
       Invoke("clock",delay);
            Debug.Log("fuck u");
    }

    public void Cut()
    {
        CancelInvoke();
    }

    private void clock()
    {
        Debug.Log("im tesing it im testing it");
        Minutes += 1;
        if (Minutes == 60)
        {
            Minutes = 0;
            if (Hours == 12)
            {
                Hours = 1;
            }
            else
            {
                Hours += 1;
            }
        }

        if (Hours == 12)
        {
            ass.text = Hours.ToString() + ":" + Minutes.ToString("00") + " AM"; 
        }
        else
        {
            ass.text = Hours.ToString("00") + ":" + Minutes.ToString("00") + " AM";
        }
        
        if (Hours != 3)
        {
            Invoke("clock",delay);
        }
        else
        {
            SceneManager.LoadScene("Ending");
        }
        
    }
}
