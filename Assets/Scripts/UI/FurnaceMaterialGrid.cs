using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class FurnaceMaterialGrid : MonoBehaviour
    {
        public string materialName = "null";
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
            UpdateMaterialGrid();
            UpdateAmountBar();
        }
        
        private void OnEnable()
        {
            iconImage.enabled = false;
            amountText.enabled = false;
            UpdateMaterialGrid();
        }

        public void UpdateMaterialGrid()
        {
            if (!materialName.Equals("null"))
            {
                iconImage.sprite = Resources.Load<Sprite>("Icons/" + materialName);
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
            if (IndexAll.nameToIsDurable(materialName)) {
                float length = 96 * ((float)amount / IndexAll.nameToMaxAmount(materialName));
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
            if (!furnaceContent.selection.Equals("material"))
            {
                furnaceContent.SelectToType("material");
                audioSource.PlayOneShot(audioClipSelect,1f);
            }
            else if(!materialName.Equals("null"))
            {
                if(playerThread.IfGetItemLeft(materialName, amount, 36,false) <= amount){
                    int left = playerThread.getItem(materialName, amount, 36, true);
                    amount = left;
                    if(left <= 0) materialName = "null";
                    UpdateMaterialGrid();
                    furnaceContent.UpdateAllFurnaceGrid();
                    UpdateAmountBar();
                    furnaceContent.furnaceThread.material = materialName;
                    furnaceContent.furnaceThread.amountMaterial = amount;
                }else
                {
                    nameTextThread.nameText.text = "背包已满";
                    nameTextThread.timer = 1.5f;
                }
            }
        }
    }
}