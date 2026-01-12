using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

public class InventoryChestGrid : MonoBehaviour {
    public AudioClip audioClipClick;
    public AudioClip audioClipPop;
    public AudioSource audioSource;
    public PlayerThread playerThread;
    public int inventoryGridSort;
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
            string gridName = playerThread.InventoryName[inventoryGridSort];
            int amount = playerThread.InventoryAmount[inventoryGridSort];
            if(!gridName.Equals("Air")){
                int left = chestContent.IfStoreItem(gridName, amount, chestContent.chestThread.volume, false);
                if (left <= 0)
                {
                    chestContent.StoreItem(gridName, amount, chestContent.chestThread.volume, false);
                    playerThread.InventoryAmount[inventoryGridSort]-=amount;
                    if (playerThread.InventoryAmount[inventoryGridSort] <= 0)
                        playerThread.InventoryName[inventoryGridSort] = "Air";
                }
                else
                {
                    nameTextThread.nameText.text = "箱子已满";
                    nameTextThread.timer = 1.5f;
                }

                audioSource.PlayOneShot(audioClipClick, 1f);
                UpdateGrid();
                chestContent.UpdateItemBar();
                chestContent.UpdateAllChestGrid();
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
        Sprite sprite = Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[inventoryGridSort]);
        if (sprite == null) {
            _textMeshPro.text = "";
            _inventoryIconImage.enabled = false;
            amountBarBack.SetActive(false);
            amountBar.SetActive(false);
        }
        else {
            _inventoryIconImage.sprite = sprite;
            _inventoryIconImage.enabled = true;
            _textMeshPro.text = "";
            if (IndexAll.nameToIsDurable(playerThread.InventoryName[inventoryGridSort])) {
                float length = 90 * ((float)playerThread.InventoryAmount[inventoryGridSort] / IndexAll.nameToMaxAmount(playerThread.InventoryName[inventoryGridSort]));
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
                _textMeshPro.text = playerThread.InventoryAmount[inventoryGridSort].ToString();
                if (_textMeshPro.text == "1") _textMeshPro.text = "";
                amountBarBack.SetActive(false);
                amountBar.SetActive(false);
            }
        }
    }

    public void InitGrid(int sort)
    {
        inventoryGridSort = sort;
        UpdateGrid();
    }
    
    public void OnClickCallBack()
    {
        if(!functioned){
            chestContent.presssed = false;
            UpdatePressBar(0);
            if (chestContent.timerPressed < 0.75f)
            {
                string gridName = playerThread.InventoryName[inventoryGridSort];
                int amount = 1;
                if (IndexAll.nameToIsDurable(gridName)) amount = playerThread.InventoryAmount[inventoryGridSort];
                if(!gridName.Equals("Air")){
                    int left = chestContent.IfStoreItem(gridName, amount, chestContent.chestThread.volume, false);
                    if (left <= 0)
                    {
                        chestContent.StoreItem(gridName, amount, chestContent.chestThread.volume, false);
                        playerThread.InventoryAmount[inventoryGridSort]-=amount;
                        if (playerThread.InventoryAmount[inventoryGridSort] <= 0)
                            playerThread.InventoryName[inventoryGridSort] = "Air";
                    }
                    else
                    {
                        nameTextThread.nameText.text = "箱子已满";
                        nameTextThread.timer = 1.5f;
                    }

                    audioSource.PlayOneShot(audioClipClick, 1f);
                    UpdateGrid();
                    chestContent.UpdateItemBar();
                    chestContent.UpdateAllChestGrid();
                }
            }
        }
    }
}
