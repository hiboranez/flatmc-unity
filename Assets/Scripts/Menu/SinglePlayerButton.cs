using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu {
    public class SinglePlayerButton : MonoBehaviour {
        public GameObject singlePlayerUI;
        public AudioClip clickAudioClip;
        public AudioSource audioSource;
        
        private void Awake() {
            GetComponent<Button>().onClick.AddListener(OnClickCallBack);
        }
        
        private void OnClickCallBack() {
            singlePlayerUI.SetActive(true);
            audioSource.PlayOneShot(clickAudioClip, 1f);
        }
    }
}