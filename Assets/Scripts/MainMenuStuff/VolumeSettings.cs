using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    private static float MasterVolume = 0.5f;
    void Start()
    { 
     
    }

    void Update()
    {
        if(this.GetComponent<AudioSource>() == null)
        {MasterVolume = this.GetComponent<Slider>().value;}
        else
        {
            this.GetComponent<AudioSource>().volume = MasterVolume;
        }
        
        //Debug.unityLogger.Log("MasterVolume: " + MasterVolume);
    }
}
