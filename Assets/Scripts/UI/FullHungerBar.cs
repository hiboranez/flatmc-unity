using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHungerBar : MonoBehaviour {
    public PlayerThread playerThread;
    public Transform[] childTransforms;
    void Awake() {
        childTransforms = GetComponentsInChildren<Transform>();
    }
    
    void Update() {
        int amount = playerThread.hunger / 2;
        if (amount < 0) amount = 0;
        if (amount > 10) amount = 10;
        for (int i = amount; i < 10; i++) {
            childTransforms[i+1].gameObject.SetActive(false);
        }
        for (int i = 0; i < amount; i++) {
            childTransforms[i+1].gameObject.SetActive(true);
        }
    }
}