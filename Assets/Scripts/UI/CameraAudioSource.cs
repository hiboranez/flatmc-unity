using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAudioSource : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip clickAudioClip;

    public void PlayClickAudioClip() {
        audioSource.PlayOneShot(clickAudioClip, 1f);
    }
}
