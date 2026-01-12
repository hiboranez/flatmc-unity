using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkDelay : MonoBehaviour {

    public float showTimer;
    void OnEnable() {
        showTimer = 1f;
    }
    
    void Update() {
        if (showTimer > 0) showTimer -= Time.deltaTime;
        else {
            gameObject.SetActive(false);
        }
    }
}
