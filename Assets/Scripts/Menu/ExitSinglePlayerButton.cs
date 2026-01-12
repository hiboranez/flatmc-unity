using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu {
    public class ExitSinglePlayerButton : MonoBehaviour {
        public GameObject singlePlayerUI;
        public AudioClip clickAudioClip;
        public AudioSource audioSource;
        
        private void Awake() {
            GetComponent<Button>().onClick.AddListener(OnClickCallBack);
        }
        
        private void OnClickCallBack() {
            singlePlayerUI.SetActive(false);
            audioSource.PlayOneShot(clickAudioClip, 1f);
        }
    }
}