using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfBreathBar : MonoBehaviour {
    public PlayerThread playerThread;
    public Transform[] childTransforms;
    void Awake() {
        childTransforms = GetComponentsInChildren<Transform>();
    }
    
    void Update() {
        int amount = playerThread.breathValue / 2;
        int odd = playerThread.breathValue % 2;
        if (amount < 0) amount = 0;
        if (amount > 10) amount = 10;
        for (int i = 0; i < 10; i++) {
            childTransforms[i + 1].gameObject.SetActive(false);
        }
        if(odd == 1 && amount < 10) {
            int sort = amount + 1;
            childTransforms[sort].gameObject.SetActive(true);
        }
    }
}