using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalModeButton : MonoBehaviour {
    public GameObject darkMask;
    public AudioClip clickAudioClip;
    public AudioSource audioSource;
        
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }
        
    private void OnClickCallBack() {
        audioSource.PlayOneShot(clickAudioClip, 1f);
        darkMask.SetActive(false);
    }
}
