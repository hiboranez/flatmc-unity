using System;
using TMPro;
using UnityEngine;

namespace UI {
    public class NameTextThread : MonoBehaviour{
        public float timer ;
        public WorldThread worldThread;
        public String nameID;
        public TMP_Text nameText;

        private void Start() {
            nameText.text = "世界名：" + worldThread.worldName;
            timer = 6;
        }

        private void Update() {
            if(timer > 0){
                timer -= Time.deltaTime;
            }
            if (timer > 0.25f) {
                nameText.color = new Color(1, 1, 1, 1);
            }else if (timer >= 0 && timer <= 0.25f) {
                nameText.color = new Color(1, 1, 1, timer * 4);
            }else if (timer < 0) {
                timer = 0;
            }
        }
    }
}