using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueGameButton : MonoBehaviour {
    public GameObject pauseUI;
    public AudioSource cameraAudioSource;
    public AudioClip clickAudioClip;
    
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }

    private void OnClickCallBack() {
        pauseUI.SetActive(false);
        cameraAudioSource.PlayOneShot(clickAudioClip, 1f);
    }
}
