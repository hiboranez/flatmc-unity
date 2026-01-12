using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu {
    public class SettingButton : MonoBehaviour {
        public GameObject settingUI;
        public AudioClip clickAudioClip;
        public AudioSource audioSource;
        
        private void Awake() {
            GetComponent<Button>().onClick.AddListener(OnClickCallBack);
        }
        
        private void OnClickCallBack() {
            settingUI.SetActive(true);
            audioSource.PlayOneShot(clickAudioClip, 1f);
        }
    }
}