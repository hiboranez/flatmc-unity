using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DifficultyButton : MonoBehaviour
    {
        public string difficulty;
        public WorldThread worldThread;
        public GameObject darkMaskPeaceful;
        public GameObject darkMaskEasy;
        public GameObject darkMaskNormal;
        public GameObject darkMaskHard;
        public AudioSource cameraAudioSource;
        public AudioClip clickAudioClip;

        public void OnClickCallBack()
        {
            if (difficulty.Equals("peaceful"))
            {
                darkMaskPeaceful.SetActive(true);
                darkMaskEasy.SetActive(false);
                darkMaskNormal.SetActive(false);
                darkMaskHard.SetActive(false);
            }else if (difficulty.Equals("easy"))
            {
                darkMaskPeaceful.SetActive(false);
                darkMaskEasy.SetActive(true);
                darkMaskNormal.SetActive(false);
                darkMaskHard.SetActive(false);
            }else if (difficulty.Equals("normal"))
            {
                darkMaskPeaceful.SetActive(false);
                darkMaskEasy.SetActive(false);
                darkMaskNormal.SetActive(true);
                darkMaskHard.SetActive(false);
            }else if (difficulty.Equals("hard"))
            {
                darkMaskPeaceful.SetActive(false);
                darkMaskEasy.SetActive(false);
                darkMaskNormal.SetActive(false);
                darkMaskHard.SetActive(true);
            }
            worldThread.difficulty = difficulty;
            cameraAudioSource.PlayOneShot(clickAudioClip);
        }
    }
}