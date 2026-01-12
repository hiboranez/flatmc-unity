using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceExitButton : MonoBehaviour
{
    public GameObject furnaceUI;
    public AudioClip audioClip;
    public AudioSource audioSourcePlayer;
    void Start() {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }

    private void OnClickCallBack() {
        audioSourcePlayer.PlayOneShot(audioClip, 1f);
        furnaceUI.SetActive(false);
    }
}