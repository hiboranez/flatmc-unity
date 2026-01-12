using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerNewButton : MonoBehaviour
{
    public GameObject createWorldUI;
    public AudioClip clickAudioClip;
    public AudioSource audioSource;
        
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }
        
    private void OnClickCallBack() {
        audioSource.PlayOneShot(clickAudioClip, 1f);
        createWorldUI.SetActive(true);
    }
}
