using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpButton : MonoBehaviour {
    public PlayerThread playerThread;
    public bool startPressed;
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }

    private void Update()
    {
        if (startPressed)
        {
            playerThread.joyStick.yJoy = 1f;
        }
    }

    private void OnClickCallBack() {
        startPressed = false;
        playerThread.joyStick.yJoy = 0f;
    }
}
