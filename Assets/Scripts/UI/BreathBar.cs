using UnityEngine;

namespace UI
{
    public class BreathBar : MonoBehaviour
    {
        public PlayerThread playerThread;
        public GameObject halfBreathBar;
        public GameObject fullBreathBar;

        public void UpdateBreathBar()
        {
            if (playerThread.underWater)
            {
                halfBreathBar.SetActive(true);
                fullBreathBar.SetActive(true);
            }
            else if(playerThread.breathValue >= 20)
            {
                halfBreathBar.SetActive(false);
                fullBreathBar.SetActive(false);
            }
        }
    }
}