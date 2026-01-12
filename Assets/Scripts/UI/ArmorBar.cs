using UnityEngine;

namespace UI
{
    public class ArmorBar : MonoBehaviour
    {
        public PlayerThread playerThread;
        public GameObject emptyArmorBar;
        public GameObject halfArmorBar;
        public GameObject fullArmorBar;

        public void UpdateArmorBar()
        {
            if (playerThread.armorValue <= 0)
            {
                emptyArmorBar.SetActive(false);
                halfArmorBar.SetActive(false);
                fullArmorBar.SetActive(false);
            }
            else
            {
                emptyArmorBar.SetActive(true);
                halfArmorBar.SetActive(true);
                fullArmorBar.SetActive(true);
            }
        }
    }
}