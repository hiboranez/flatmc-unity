using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PulseSize : MonoBehaviour {
    private TMP_Text _text;
    private float _timer;
    void Start() {
        _text = GetComponent<TMP_Text>();
    }
    
    void Update() {
        _timer += Time.deltaTime;
        if (_timer > 0 && _timer <= 0.5f) {
            _text.fontSize = 50 + _timer * 20;
        } else if (_timer > 0.5f && _timer <= 1f) {
            _text.fontSize = 60 - (_timer - 0.5f) * 20;
        } else {
            _timer = 0;
        }
        
    }
}
