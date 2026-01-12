using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

public class ChestGrid : MonoBehaviour {
    public AudioClip audioClipClick;
    public AudioClip audioClipPop;
    public AudioSource audioSource;
    public PlayerThread playerThread;
    public int gridSort;
    public GameObject amountBarBack;
    public GameObject amountBar;
    public ChestContent chestContent;
    public NameTextThread nameTextThread;
    private RectTransform _pressBarImageRectTransform;
    public Image pressBarImage;
    public Image pressBarBackImage;
    public bool functioned;
    private Image _inventoryIconImage;
    private TMP_Text _textMeshPro;
    
    void Awake()
    {
        functioned = false;
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        _inventoryIconImage = childTransforms[1].gameObject.GetComponent<Image>();
        _textMeshPro = childTransforms[2].gameObject.GetComponent<TMP_Text>();
        amountBarBack = childTransforms[3].gameObject;
        amountBar = childTransforms[4].gameObject;
        pressBarBackImage = childTransforms[5].gameObject.GetComponent<Image>();
        pressBarImage = childTransforms[6].gameObject.GetComponent<Image>();
        _pressBarImageRectTransform = pressBarImage.GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateGrid();
    }
    
    public void UpdatePressBar(float timer)
    {
        if (timer < 0.75f)
        {
            pressBarBackImage.gameObject.SetActive(false);
            pressBarImage.gameObject.SetActive(false);
        }
        
        if (timer >= 0.75f && timer < 1.75f)
        {
            float length = 90 * ((timer - 0.75f) / 1f);
            pressBarImage.color = Color.green;
            _pressBarImageRectTransform.sizeDelta =
                new Vector2(length, _pressBarImageRectTransform.sizeDelta.y);
            _pressBarImageRectTransform.anchoredPosition =
                new Vector2(-(90 - length) / 2, _pressBarImageRectTransform.anchoredPosition.y);
            pressBarBackImage.gameObject.SetActive(true);
            pressBarImage.gameObject.SetActive(true);
        }
        else if (timer >= 1.75f)
        {
            string gridName = chestContent.chestThread.nameList[gridSort];
            int amount = chestContent.chestThread.amountList[gridSort];
            if(!gridName.Equals("Air")){
                int left = playerThread.IfGetItemLeft(gridName, amount, 36, false);
                if (left <= 0)
                {
                    playerThread.getItem(gridName, amount, 36, false);
                    chestContent.chestThread.amountList[gridSort]-=amount;
                    if (chestContent.chestThread.amountList[gridSort] <= 0)
                        chestContent.chestThread.nameList[gridSort] = "Air";
                }
                else
                {
                    nameTextThread.nameText.text = "背包已满";
                    nameTextThread.timer = 1.5f;
                }

                audioSource.PlayOneShot(audioClipClick, 1f);
                UpdateGrid();
                chestContent.UpdateItemBar();
                chestContent.UpdateAllInventoryChestGrid();
            }
            chestContent.presssed = false;
            pressBarBackImage.gameObject.SetActive(false);
            pressBarImage.gameObject.SetActive(false);
        }
    }
    
    public void UpdateGrid()
    {
        pressBarImage.gameObject.SetActive(false);
        pressBarBackImage.gameObject.SetActive(false);
        Sprite sprite = Resources.Load<Sprite>("Icons/" + chestContent.chestThread.nameList[gridSort]);
        if (sprite == null) {
            _textMeshPro.text = "";
            _inventoryIconImage.enabled = false;
            amountBarBack.SetActive(false);
            amountBar.SetActive(false);
        }
        else {
            _inventoryIconImage.sprite = sprite;
            _inventoryIconImage.enabled = true;
            if (IndexAll.nameToIsDurable(chestContent.chestThread.nameList[gridSort])) {
                float length = 90 * ((float)chestContent.chestThread.amountList[gridSort] / IndexAll.nameToMaxAmount(chestContent.chestThread.nameList[gridSort]));
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
                _textMeshPro.text = chestContent.chestThread.amountList[gridSort].ToString();
                if (_textMeshPro.text == "1") _textMeshPro.text = "";
                amountBarBack.SetActive(false);
                amountBar.SetActive(false);
            }
        }
    }
    
    public void InitGrid(int sort)
    {
        gridSort = sort;
        UpdateGrid();
    }
    
    public void OnClickCallBack()
    {
        if(!functioned){
            chestContent.presssed = false;
            UpdatePressBar(0);
            if (chestContent.timerPressed < 0.75f)
            {
                string gridName = chestContent.chestThread.nameList[gridSort];
                int amount = 1;
                if (IndexAll.nameToIsDurable(gridName)) amount = chestContent.chestThread.amountList[gridSort];
                if(!gridName.Equals("Air")){
                    int left = playerThread.IfGetItemLeft(gridName, amount, 36, false);
                    if (left <= 0)
                    {
                        playerThread.getItem(gridName, amount, 36, false);
                        chestContent.chestThread.amountList[gridSort]-=amount;
                        if (chestContent.chestThread.amountList[gridSort] <= 0)
                            chestContent.chestThread.nameList[gridSort] = "Air";
                    }
                    else
                    {
                        nameTextThread.nameText.text = "背包已满";
                        nameTextThread.timer = 1.5f;
                    }

                    audioSource.PlayOneShot(audioClipClick, 1f);
                    UpdateGrid();
                    chestContent.UpdateItemBar();
                    chestContent.UpdateAllInventoryChestGrid();
                }
            }
        }
    }
}
