using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class SinglePlayerEditButton : MonoBehaviour {
    public GameObject worldContent;
    public AudioClip clickAudioClip;
    public AudioSource audioSource;
    public bool onEditing;
        
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }

    private void OnDisable() {
        Transform[] worldTransformList = IndexAll.GetComponentsInRealChildren<Transform>(worldContent, true);
        int length = worldTransformList.Length;
        SinglePlayerWorldThread[] worldList = new SinglePlayerWorldThread[length];
        for (int i = 0; i < length; i++) {
            worldList[i] = worldTransformList[i].GetComponent<SinglePlayerWorldThread>();
        }
        for (int i = 0; i < length; i++) {
            worldList[i].deleteButton.SetActive(false);
        }
        onEditing = false;
    }

    private void OnClickCallBack() {
        audioSource.PlayOneShot(clickAudioClip, 1f);
        Transform[] worldTransformList = IndexAll.GetComponentsInRealChildren<Transform>(worldContent, true);
        int length = worldTransformList.Length;
        SinglePlayerWorldThread[] worldList = new SinglePlayerWorldThread[length];
        for (int i = 0; i < length; i++) {
            worldList[i] = worldTransformList[i].GetComponent<SinglePlayerWorldThread>();
        }
        for (int i = 0; i < length; i++) {
            worldList[i].deleteButton.SetActive(!onEditing);
        }
        onEditing = !onEditing;
    }
}