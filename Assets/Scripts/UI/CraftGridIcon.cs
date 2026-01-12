using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class CraftGridIcon : MonoBehaviour {
        private Image _image;
        private void Awake() {
            _image = GetComponent<Image>();
            _image.enabled = false;
        }
    }
}