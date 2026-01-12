using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunButton : MonoBehaviour {
    public PlayerThread playerThread;
    public AudioSource audioSource;
    public AudioClip audioClipClick;
    private Image _image;
    private bool _pressed;

    private void Awake() {
        _image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }

    private void OnClickCallBack() {
        if (!_pressed) {
            _image.sprite = Resources.Load<Sprite>("Textures/GUI/RunButtonPressed");
            _pressed = true;
            playerThread.canRun3 = true;
        } else {
            _image.sprite = Resources.Load<Sprite>("Textures/GUI/RunButton");
            _pressed = false;
            playerThread.canRun3 = false;
        }
        audioSource.PlayOneShot(audioClipClick, 1f);
    }
}
