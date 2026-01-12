using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

public class InventoryGrid : MonoBehaviour {
    public AudioClip audioClip;
    public AudioSource audioSource;
    public PlayerThread playerThread;
    public int InventoryGridSort;
    public ItemBarButton itemBarButton1;
    public ItemBarButton itemBarButton2;
    public ItemBarButton itemBarButton3;
    public ItemBarButton itemBarButton4;
    public ItemBarButton itemBarButton5;
    public ItemBarButton itemBarButton6;
    public ItemBarButton itemBarButton7;
    public ItemBarButton itemBarButton8;
    public ItemBarButton itemBarButton9;
    public GameObject amountBarBack;
    public GameObject amountBar;
    public NameTextThread nameTextThread;
    private Image _inventoryIconImage;
    private TMP_Text _textMeshPro;
    
    void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        _inventoryIconImage = childTransforms[1].gameObject.GetComponent<Image>();
        _textMeshPro = childTransforms[2].gameObject.GetComponent<TMP_Text>();
        amountBarBack = childTransforms[3].gameObject;
        amountBar = childTransforms[4].gameObject;
        Sprite sprite = Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[InventoryGridSort]);
        if (sprite == null) {
            _inventoryIconImage.enabled = false;
            amountBarBack.SetActive(false);
            amountBar.SetActive(false);
        }
        else {
            _inventoryIconImage.sprite = sprite;
            _inventoryIconImage.enabled = true;
            if (IndexAll.nameToIsDurable(playerThread.InventoryName[InventoryGridSort])) {
                float length = 90 * ((float)playerThread.InventoryAmount[InventoryGridSort] / IndexAll.nameToMaxAmount(playerThread.InventoryName[InventoryGridSort]));
                RectTransform amountBarRectTransform = amountBar.GetComponent<RectTransform>();
                Image amountBarImage = amountBar.GetComponent<Image>();
                if(length > 60 && length <= 90) amountBarImage.color = Color.green;
                else if(length > 30 && length <= 60) amountBarImage.color = Color.yellow;
                else if(length >= 0 && length <= 30) amountBarImage.color = Color.red;
                amountBarRectTransform.sizeDelta = new Vector2(length, amountBarRectTransform.sizeDelta.y);
                amountBarRectTransform.anchoredPosition = new Vector2(-(90 - length) / 2, amountBarRectTransform.anchoredPosition.y);
                amountBarBack.SetActive(true);
                amountBar.SetActive(true);
            }
            else {
                _textMeshPro.text = playerThread.InventoryAmount[InventoryGridSort].ToString();
                if (_textMeshPro.text == "1") _textMeshPro.text = "";
                amountBarBack.SetActive(false);
                amountBar.SetActive(false);
            }
        }
    }

    private void UpdateTextAmountBar(ItemBarButton itemBarButton) {
        if (IndexAll.nameToIsDurable(playerThread.InventoryName[InventoryGridSort])) {
            float length = 90 * ((float)playerThread.InventoryAmount[InventoryGridSort] / IndexAll.nameToMaxAmount(playerThread.InventoryName[InventoryGridSort]));
            RectTransform amountBarRectTransform = itemBarButton.amountBar.GetComponent<RectTransform>();
            Image amountBarImage = itemBarButton.amountBar.GetComponent<Image>();
            if(length > 60 && length <= 90) amountBarImage.color = Color.green;
            else if(length > 30 && length <= 60) amountBarImage.color = Color.yellow;
            else if(length >= 0 && length <= 30) amountBarImage.color = Color.red;
            amountBarRectTransform.sizeDelta = new Vector2(length, amountBarRectTransform.sizeDelta.y);
            amountBarRectTransform.anchoredPosition = new Vector2(-(90 - length) / 2, amountBarRectTransform.anchoredPosition.y);
            itemBarButton.textMeshPro.text = "";
            itemBarButton.amountBarBack.SetActive(true);
            itemBarButton.amountBar.SetActive(true);
        }
        else {
            itemBarButton.textMeshPro.text = playerThread.InventoryAmount[InventoryGridSort].ToString();
            if (itemBarButton.textMeshPro.text == "1") itemBarButton.textMeshPro.text = "";
            itemBarButton.amountBarBack.SetActive(false);
            itemBarButton.amountBar.SetActive(false);
        }
    }
    
    private void OnClickCallBack() {
        if(playerThread.InventoryName[InventoryGridSort] != "Air"){
            nameTextThread.nameText.text = IndexAll.nameToNameShow(playerThread.InventoryName[InventoryGridSort]);
            nameTextThread.timer = 1.5f;
            if (playerThread.ItemBarChosen == 0) {
                UpdateTextAmountBar(itemBarButton1);
                UpdateOtherItemBarButton(playerThread.ItemBarChosen, itemBarButton1);
                itemBarButton1.iconImage.sprite =
                    Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[InventoryGridSort]);
                itemBarButton1.iconImage.enabled = true;
                itemBarButton1.InventorySort = InventoryGridSort;
                audioSource.PlayOneShot(audioClip, 1f);
                itemBarButton1.StartCoroutine(itemBarButton1.Flash());
            } else if (playerThread.ItemBarChosen == 1) {
                UpdateTextAmountBar(itemBarButton2);
                UpdateOtherItemBarButton(playerThread.ItemBarChosen, itemBarButton2);
                itemBarButton2.iconImage.sprite =
                    Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[InventoryGridSort]);
                itemBarButton2.iconImage.enabled = true;
                itemBarButton2.InventorySort = InventoryGridSort;
                audioSource.PlayOneShot(audioClip, 1f);
                itemBarButton2.StartCoroutine(itemBarButton2.Flash());
            } else if (playerThread.ItemBarChosen == 2) {
                UpdateTextAmountBar(itemBarButton3);
                UpdateOtherItemBarButton(playerThread.ItemBarChosen, itemBarButton3);
                itemBarButton3.iconImage.sprite =
                    Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[InventoryGridSort]);
                itemBarButton3.iconImage.enabled = true;
                itemBarButton3.InventorySort = InventoryGridSort;
                audioSource.PlayOneShot(audioClip, 1f);
                itemBarButton3.StartCoroutine(itemBarButton3.Flash());
            } else if (playerThread.ItemBarChosen == 3) {
                UpdateTextAmountBar(itemBarButton4);
                UpdateOtherItemBarButton(playerThread.ItemBarChosen, itemBarButton4);
                itemBarButton4.iconImage.sprite =
                    Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[InventoryGridSort]);
                itemBarButton4.iconImage.enabled = true;
                itemBarButton4.InventorySort = InventoryGridSort;
                audioSource.PlayOneShot(audioClip, 1f);
                itemBarButton4.StartCoroutine(itemBarButton4.Flash());
            } else if (playerThread.ItemBarChosen == 4) {
                UpdateTextAmountBar(itemBarButton5);
                UpdateOtherItemBarButton(playerThread.ItemBarChosen, itemBarButton5);
                itemBarButton5.iconImage.sprite =
                    Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[InventoryGridSort]);
                itemBarButton5.iconImage.enabled = true;
                itemBarButton5.InventorySort = InventoryGridSort;
                audioSource.PlayOneShot(audioClip, 1f);
                itemBarButton5.StartCoroutine(itemBarButton5.Flash());
            } else if (playerThread.ItemBarChosen == 5) {
                UpdateTextAmountBar(itemBarButton6);
                UpdateOtherItemBarButton(playerThread.ItemBarChosen, itemBarButton6);
                itemBarButton6.iconImage.sprite =
                    Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[InventoryGridSort]);
                itemBarButton6.iconImage.enabled = true;
                itemBarButton6.InventorySort = InventoryGridSort;
                audioSource.PlayOneShot(audioClip, 1f);
                itemBarButton6.StartCoroutine(itemBarButton6.Flash());
            } else if (playerThread.ItemBarChosen == 6) {
                UpdateTextAmountBar(itemBarButton7);
                UpdateOtherItemBarButton(playerThread.ItemBarChosen, itemBarButton7);
                itemBarButton7.iconImage.sprite =
                    Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[InventoryGridSort]);
                itemBarButton7.iconImage.enabled = true;
                itemBarButton7.InventorySort = InventoryGridSort;
                audioSource.PlayOneShot(audioClip, 1f);
                itemBarButton7.StartCoroutine(itemBarButton7.Flash());
            } else if (playerThread.ItemBarChosen == 7) {
                UpdateTextAmountBar(itemBarButton8);
                UpdateOtherItemBarButton(playerThread.ItemBarChosen, itemBarButton8);
                itemBarButton8.iconImage.sprite =
                    Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[InventoryGridSort]);
                itemBarButton8.iconImage.enabled = true;
                itemBarButton8.InventorySort = InventoryGridSort;
                audioSource.PlayOneShot(audioClip, 1f);
                itemBarButton8.StartCoroutine(itemBarButton8.Flash());
            } else if (playerThread.ItemBarChosen == 8) {
                UpdateTextAmountBar(itemBarButton9);
                UpdateOtherItemBarButton(playerThread.ItemBarChosen, itemBarButton9);
                itemBarButton9.iconImage.sprite =
                    Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[InventoryGridSort]);
                itemBarButton9.iconImage.enabled = true;
                itemBarButton9.InventorySort = InventoryGridSort;
                audioSource.PlayOneShot(audioClip, 1f);
                itemBarButton9.StartCoroutine(itemBarButton9.Flash());
            }
        }
    }

    private void UpdateTextAmountBarOther(ItemBarButton itemBarButton) {
        if (IndexAll.nameToIsDurable(playerThread.InventoryName[itemBarButton.InventorySort])) {
            float length = 90 * ((float)playerThread.InventoryAmount[itemBarButton.InventorySort] / IndexAll.nameToMaxAmount(playerThread.InventoryName[itemBarButton.InventorySort]));
            RectTransform amountBarRectTransform = itemBarButton.amountBar.GetComponent<RectTransform>();
            Image amountBarImage = itemBarButton.amountBar.GetComponent<Image>();
            if(length > 60 && length <= 90) amountBarImage.color = Color.green;
            else if(length > 30 && length <= 60) amountBarImage.color = Color.yellow;
            else if(length >= 0 && length <= 30) amountBarImage.color = Color.red;
            amountBarRectTransform.sizeDelta = new Vector2(length, amountBarRectTransform.sizeDelta.y);
            amountBarRectTransform.anchoredPosition = new Vector2(-(90 - length) / 2, amountBarRectTransform.anchoredPosition.y);
            itemBarButton.textMeshPro.text = "";
            itemBarButton.amountBarBack.SetActive(true);
            itemBarButton.amountBar.SetActive(true);
        } else {
            itemBarButton.textMeshPro.text = playerThread.InventoryAmount[itemBarButton.InventorySort].ToString();
            if (itemBarButton.textMeshPro.text == "1") itemBarButton.textMeshPro.text = "";
            itemBarButton.amountBarBack.SetActive(false);
            itemBarButton.amountBar.SetActive(false);
        }
    }
    
    private void UpdateOtherItemBarButton(int ItemBarChosen, ItemBarButton itemBarButton) {
        if (itemBarButton1.ItemBarSort != ItemBarChosen && itemBarButton1.InventorySort == InventoryGridSort) {
            itemBarButton1.InventorySort = itemBarButton.InventorySort;
            if(playerThread.InventoryName[itemBarButton.InventorySort] != "Air") {
                itemBarButton1.iconImage.sprite = itemBarButton.iconImage.sprite;
                UpdateTextAmountBarOther(itemBarButton1);
            } else {
                itemBarButton1.iconImage.enabled = false;
                itemBarButton1.iconImage.sprite = null;
                itemBarButton1.textMeshPro.text = "";
                itemBarButton1.amountBarBack.SetActive(false);
                itemBarButton1.amountBar.SetActive(false);
            }
        } else if (itemBarButton2.ItemBarSort != ItemBarChosen && itemBarButton2.InventorySort == InventoryGridSort) {
            itemBarButton2.InventorySort = itemBarButton.InventorySort;
            if(playerThread.InventoryName[itemBarButton.InventorySort] != "Air") {
                itemBarButton2.iconImage.sprite = itemBarButton.iconImage.sprite;
                UpdateTextAmountBarOther(itemBarButton2);
            } else {
                itemBarButton2.iconImage.enabled = false;
                itemBarButton2.iconImage.sprite = null;
                itemBarButton2.textMeshPro.text = "";
                itemBarButton2.amountBarBack.SetActive(false);
                itemBarButton2.amountBar.SetActive(false);
            }
        }else if (itemBarButton3.ItemBarSort != ItemBarChosen && itemBarButton3.InventorySort == InventoryGridSort) {
            itemBarButton3.InventorySort = itemBarButton.InventorySort;
            if(playerThread.InventoryName[itemBarButton.InventorySort] != "Air") {
                itemBarButton3.iconImage.sprite = itemBarButton.iconImage.sprite;
                UpdateTextAmountBarOther(itemBarButton3);
            } else {
                itemBarButton3.iconImage.enabled = false;
                itemBarButton3.iconImage.sprite = null;
                itemBarButton3.textMeshPro.text = "";
                itemBarButton3.amountBarBack.SetActive(false);
                itemBarButton3.amountBar.SetActive(false);
            }
        }else if (itemBarButton4.ItemBarSort != ItemBarChosen && itemBarButton4.InventorySort == InventoryGridSort) {
            itemBarButton4.InventorySort = itemBarButton.InventorySort;
            if(playerThread.InventoryName[itemBarButton.InventorySort] != "Air") {
                itemBarButton4.iconImage.sprite = itemBarButton.iconImage.sprite;
                UpdateTextAmountBarOther(itemBarButton4);
            } else {
                itemBarButton4.iconImage.enabled = false;
                itemBarButton4.iconImage.sprite = null;
                itemBarButton4.textMeshPro.text = "";
                itemBarButton4.amountBarBack.SetActive(false);
                itemBarButton4.amountBar.SetActive(false);
            }
        }else if (itemBarButton5.ItemBarSort != ItemBarChosen && itemBarButton5.InventorySort == InventoryGridSort) {
            itemBarButton5.InventorySort = itemBarButton.InventorySort;
            if(playerThread.InventoryName[itemBarButton.InventorySort] != "Air") {
                itemBarButton5.iconImage.sprite = itemBarButton.iconImage.sprite;
                UpdateTextAmountBarOther(itemBarButton5);
            } else {
                itemBarButton5.iconImage.enabled = false;
                itemBarButton5.iconImage.sprite = null;
                itemBarButton5.textMeshPro.text = "";
                itemBarButton5.amountBarBack.SetActive(false);
                itemBarButton5.amountBar.SetActive(false);
            }
        }else if (itemBarButton6.ItemBarSort != ItemBarChosen && itemBarButton6.InventorySort == InventoryGridSort) {
            itemBarButton6.InventorySort = itemBarButton.InventorySort;
            if(playerThread.InventoryName[itemBarButton.InventorySort] != "Air") {
                itemBarButton6.iconImage.sprite = itemBarButton.iconImage.sprite;
                UpdateTextAmountBarOther(itemBarButton6);
            } else {
                itemBarButton6.iconImage.enabled = false;
                itemBarButton6.iconImage.sprite = null;
                itemBarButton6.textMeshPro.text = "";
                itemBarButton6.amountBarBack.SetActive(false);
                itemBarButton6.amountBar.SetActive(false);
            }
        }else if (itemBarButton7.ItemBarSort != ItemBarChosen && itemBarButton7.InventorySort == InventoryGridSort) {
            itemBarButton7.InventorySort = itemBarButton.InventorySort;
            if(playerThread.InventoryName[itemBarButton.InventorySort] != "Air") {
                itemBarButton7.iconImage.sprite = itemBarButton.iconImage.sprite;
                UpdateTextAmountBarOther(itemBarButton7);
            } else {
                itemBarButton7.iconImage.enabled = false;
                itemBarButton7.iconImage.sprite = null;
                itemBarButton7.textMeshPro.text = "";
                itemBarButton7.amountBarBack.SetActive(false);
                itemBarButton7.amountBar.SetActive(false);
            }
        }else if (itemBarButton8.ItemBarSort != ItemBarChosen && itemBarButton8.InventorySort == InventoryGridSort) {
            itemBarButton8.InventorySort = itemBarButton.InventorySort;
            if(playerThread.InventoryName[itemBarButton.InventorySort] != "Air") {
                itemBarButton8.iconImage.sprite = itemBarButton.iconImage.sprite;
                UpdateTextAmountBarOther(itemBarButton8);
            } else {
                itemBarButton8.iconImage.enabled = false;
                itemBarButton8.iconImage.sprite = null;
                itemBarButton8.textMeshPro.text = "";
                itemBarButton8.amountBarBack.SetActive(false);
                itemBarButton8.amountBar.SetActive(false);
            }
        }else if (itemBarButton9.ItemBarSort != ItemBarChosen && itemBarButton9.InventorySort == InventoryGridSort) {
            itemBarButton9.InventorySort = itemBarButton.InventorySort;
            if(playerThread.InventoryName[itemBarButton.InventorySort] != "Air") {
                itemBarButton9.iconImage.sprite = itemBarButton.iconImage.sprite;
                UpdateTextAmountBarOther(itemBarButton9);
            } else {
                itemBarButton9.iconImage.enabled = false;
                itemBarButton9.iconImage.sprite = null;
                itemBarButton9.textMeshPro.text = "";
                itemBarButton9.amountBarBack.SetActive(false);
                itemBarButton9.amountBar.SetActive(false);
            }
        }
    }
}
