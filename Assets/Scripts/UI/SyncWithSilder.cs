using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyncWithSilder : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider slider;
    
    public void UpdateAudioSourceVolume() {
        audioSource.volume = slider.value;
    }
}
