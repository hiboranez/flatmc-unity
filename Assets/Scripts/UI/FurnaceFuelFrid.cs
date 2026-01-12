using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class FurnaceFuelGrid : MonoBehaviour
    {
        public string fuelName = "null";
        public int amount = 0;
        public Image iconImage;
        public TMP_Text amountText;
        public FurnaceContent furnaceContent;
        public PlayerThread playerThread;
        public NameTextThread nameTextThread;
        public AudioClip audioClipSelect;
        public AudioSource audioSource;
        public GameObject amountBarBack;
        public GameObject amountBar;

        private void Awake()
        {
            iconImage.enabled = false;
            amountText.enabled = false;
            UpdateFuelGrid();
            UpdateAmountBar();
        }
        
        private void OnEnable()
        {
            iconImage.enabled = false;
            amountText.enabled = false;
            UpdateFuelGrid();
        }

        public void UpdateFuelGrid()
        {
            if (!fuelName.Equals("null"))
            {
                iconImage.sprite = Resources.Load<Sprite>("Icons/" + fuelName);
                amountText.text = amount.ToString();
                iconImage.enabled = true;
                amountText.enabled = true;
            }
            else
            {
                iconImage.sprite = null;
                amountText.text = "0";
                amountText.enabled = false;
                iconImage.enabled = false;
            }
            if (amountText.text == "1") amountText.text = "";
        }

        public void UpdateAmountBar()
        {
            if (IndexAll.nameToIsDurable(fuelName)) {
                float length = 96 * ((float)amount / IndexAll.nameToMaxAmount(fuelName));
                RectTransform amountBarRectTransform = amountBar.GetComponent<RectTransform>();
                Image amountBarImage = amountBar.GetComponent<Image>();
                if(length > 64 && length <= 96) amountBarImage.color = Color.green;
                else if(length > 32 && length <= 64) amountBarImage.color = Color.yellow;
                else if(length >= 0 && length <= 32) amountBarImage.color = Color.red;
                amountBarRectTransform.sizeDelta = new Vector2(length, amountBarRectTransform.sizeDelta.y);
                amountBarRectTransform.anchoredPosition = new Vector2(-(96 - length) / 2, amountBarRectTransform.anchoredPosition.y);
                amountBarBack.SetActive(true);
                amountBar.SetActive(true);
                amountText.enabled = false;
            }
            else {
                amountText.text = amount.ToString();
                if (amountText.text == "1") amountText.text = "";
                amountBarBack.SetActive(false);
                amountBar.SetActive(false);
                if(amount > 0) amountText.enabled = true;
            }
        }
        
        public void ClickOnCallBack()
        {
            if (!furnaceContent.selection.Equals("fuel"))
            {
                furnaceContent.SelectToType("fuel");
                audioSource.PlayOneShot(audioClipSelect,1f);
            }
            else if(!fuelName.Equals("null"))
            {
                if(playerThread.IfGetItemLeft(fuelName, amount, 36,false) <= amount){
                    int left = playerThread.getItem(fuelName, amount, 36, true);
                    amount = left;
                    if(left <= 0) fuelName = "null";
                    UpdateFuelGrid();
                    furnaceContent.UpdateAllFurnaceGrid();
                    UpdateAmountBar();
                    furnaceContent.furnaceThread.fuel = fuelName;
                    furnaceContent.furnaceThread.amountFuel = amount;
                }else
                {
                    nameTextThread.nameText.text = "背包已满";
                    nameTextThread.timer = 1.5f;
                }
            }
        }
    }
}