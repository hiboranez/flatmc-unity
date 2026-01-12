using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

public class FurnaceGrid : MonoBehaviour {
    public AudioClip audioClipClick;
    public AudioClip audioClipPop;
    public AudioSource audioSource;
    public PlayerThread playerThread;
    public int inventoryGridSort;
    public GameObject amountBarBack;
    public GameObject amountBar;
    public FurnaceContent furnaceContent;
    public FurnaceMaterialGrid furnaceMaterialGrid;
    public FurnaceFuelGrid furnaceFuelGrid;
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
        UpdateGrid();
    }

    private void OnEnable()
    {
        pressBarImage.gameObject.SetActive(false);
        pressBarBackImage.gameObject.SetActive(false);
    }

    public void UpdatePressBar(float timer)
    {
        if (timer < 0.75f)
        {
            pressBarBackImage.gameObject.SetActive(false);
            pressBarImage.gameObject.SetActive(false);
        }
        
        if(furnaceContent.selection.Equals("material") && furnaceMaterialGrid.amount < IndexAll.nameToMaxAmount(furnaceMaterialGrid.materialName)){
            if (!IndexAll.nameToBurnProduct(playerThread.InventoryName[inventoryGridSort]).Equals("null"))
            {
                if(playerThread.InventoryName[inventoryGridSort].Equals(furnaceMaterialGrid.materialName) || furnaceMaterialGrid.materialName.Equals("null")){
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
                        if (playerThread.InventoryName[inventoryGridSort].Equals(furnaceMaterialGrid.materialName))
                        {
                            int maxAmount = IndexAll.nameToMaxAmount(furnaceMaterialGrid.materialName);
                            if (playerThread.InventoryAmount[inventoryGridSort] + furnaceMaterialGrid.amount <=
                                maxAmount)
                            {
                                furnaceMaterialGrid.amount += playerThread.InventoryAmount[inventoryGridSort];
                                furnaceContent.furnaceThread.amountMaterial = furnaceMaterialGrid.amount;
                                playerThread.InventoryAmount[inventoryGridSort] = 0;
                                playerThread.InventoryName[inventoryGridSort] = "Air";
                                UpdateGrid();
                                furnaceContent.UpdateItemBar();
                                furnaceMaterialGrid.UpdateMaterialGrid();
                                audioSource.PlayOneShot(audioClipPop, 1f);
                                functioned = true;
                            }
                            else
                            {
                                playerThread.InventoryAmount[inventoryGridSort] -=
                                    (maxAmount - furnaceMaterialGrid.amount);
                                furnaceMaterialGrid.amount = maxAmount;
                                furnaceContent.furnaceThread.amountMaterial = maxAmount;
                                UpdateGrid();
                                furnaceContent.UpdateItemBar();
                                furnaceMaterialGrid.UpdateMaterialGrid();
                                audioSource.PlayOneShot(audioClipPop, 1f);
                                functioned = true;
                            }
                        }
                        else
                        {
                            furnaceMaterialGrid.materialName = playerThread.InventoryName[inventoryGridSort];
                            furnaceMaterialGrid.amount = playerThread.InventoryAmount[inventoryGridSort];
                            furnaceContent.furnaceThread.material = furnaceMaterialGrid.materialName;
                            furnaceContent.furnaceThread.amountMaterial = furnaceMaterialGrid.amount;
                            playerThread.InventoryAmount[inventoryGridSort] = 0;
                            playerThread.InventoryName[inventoryGridSort] = "Air";
                            UpdateGrid();
                            furnaceContent.UpdateItemBar();
                            furnaceMaterialGrid.UpdateMaterialGrid();
                            audioSource.PlayOneShot(audioClipPop, 1f);
                            functioned = true;
                        }

                        furnaceContent.presssed = false;
                        pressBarBackImage.gameObject.SetActive(false);
                        pressBarImage.gameObject.SetActive(false);
                    }
                }
            }
        }else if(furnaceContent.selection.Equals("fuel") && furnaceFuelGrid.amount < IndexAll.nameToMaxAmount(furnaceFuelGrid.fuelName)){
            if (IndexAll.nameToBurnTime(playerThread.InventoryName[inventoryGridSort]) > 0)
            {
                if(playerThread.InventoryName[inventoryGridSort].Equals(furnaceFuelGrid.fuelName) || furnaceFuelGrid.fuelName.Equals("null")){
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
                        if (playerThread.InventoryName[inventoryGridSort].Equals(furnaceFuelGrid.fuelName))
                        {
                            int maxAmount = IndexAll.nameToMaxAmount(furnaceFuelGrid.fuelName);
                            if (playerThread.InventoryAmount[inventoryGridSort] + furnaceFuelGrid.amount <=
                                maxAmount)
                            {
                                furnaceFuelGrid.amount += playerThread.InventoryAmount[inventoryGridSort];
                                furnaceContent.furnaceThread.amountFuel = furnaceFuelGrid.amount;
                                playerThread.InventoryAmount[inventoryGridSort] = 0;
                                playerThread.InventoryName[inventoryGridSort] = "Air";
                                UpdateGrid();
                                furnaceContent.UpdateItemBar();
                                furnaceFuelGrid.UpdateFuelGrid();
                                audioSource.PlayOneShot(audioClipPop, 1f);
                                functioned = true;
                            }
                            else
                            {
                                playerThread.InventoryAmount[inventoryGridSort] -=
                                    (maxAmount - furnaceFuelGrid.amount);
                                furnaceFuelGrid.amount = maxAmount;
                                furnaceContent.furnaceThread.amountFuel = maxAmount;
                                UpdateGrid();
                                furnaceContent.UpdateItemBar();
                                furnaceFuelGrid.UpdateFuelGrid();
                                audioSource.PlayOneShot(audioClipPop, 1f);
                                functioned = true;
                            }
                        }
                        else
                        {
                            furnaceFuelGrid.fuelName = playerThread.InventoryName[inventoryGridSort];
                            furnaceFuelGrid.amount = playerThread.InventoryAmount[inventoryGridSort];
                            furnaceContent.furnaceThread.fuel = furnaceFuelGrid.fuelName;
                            furnaceContent.furnaceThread.amountFuel = furnaceFuelGrid.amount;
                            playerThread.InventoryAmount[inventoryGridSort] = 0;
                            playerThread.InventoryName[inventoryGridSort] = "Air";
                            UpdateGrid();
                            furnaceContent.UpdateItemBar();
                            furnaceFuelGrid.UpdateFuelGrid();
                            audioSource.PlayOneShot(audioClipPop, 1f);
                            functioned = true;
                        }
                        furnaceContent.presssed = false;
                        pressBarBackImage.gameObject.SetActive(false);
                        pressBarImage.gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            furnaceContent.timerPressed = 0;
        }
    }
    
    public void UpdateGrid()
    {
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
    
    public void OnClickCallBack()
    {
        if(!functioned){
            furnaceContent.presssed = false;
            UpdatePressBar(0);
            if (furnaceContent.selection.Equals("material"))
            {
                string product = IndexAll.nameToBurnProduct(playerThread.InventoryName[inventoryGridSort]);
                string gridName = playerThread.InventoryName[inventoryGridSort];
                if (!product.Equals("null"))
                {
                    if (furnaceContent.timerPressed < 0.75f)
                    {
                        if (furnaceMaterialGrid.materialName.Equals(gridName) && !IndexAll.nameToIsDurable(gridName))
                        {
                            if (furnaceMaterialGrid.amount < IndexAll.nameToMaxAmount(gridName))
                            {
                                playerThread.InventoryAmount[inventoryGridSort]--;
                                if (playerThread.InventoryAmount[inventoryGridSort] <= 0)
                                {
                                    playerThread.InventoryAmount[inventoryGridSort] = 0;
                                    playerThread.InventoryName[inventoryGridSort] = "Air";
                                }

                                furnaceMaterialGrid.amount++;
                            }
                        }
                        else if (!furnaceMaterialGrid.materialName.Equals("null"))
                        {
                            if (playerThread.IfGetItemLeft(furnaceMaterialGrid.materialName, furnaceMaterialGrid.amount,
                                    36, false) <= 0)
                            {
                                playerThread.getItem(furnaceMaterialGrid.materialName, furnaceMaterialGrid.amount, 36,
                                    false);
                                if (IndexAll.nameToIsDurable(gridName))
                                {
                                    furnaceMaterialGrid.materialName = gridName;
                                    furnaceMaterialGrid.amount += playerThread.InventoryAmount[inventoryGridSort];
                                    playerThread.InventoryName[inventoryGridSort] = "Air";
                                    playerThread.InventoryAmount[inventoryGridSort] = 0;
                                }
                                else
                                {
                                    furnaceMaterialGrid.materialName = gridName;
                                    furnaceMaterialGrid.amount = 1;
                                    playerThread.InventoryAmount[inventoryGridSort]--;
                                    if (playerThread.InventoryAmount[inventoryGridSort] <= 0)
                                        playerThread.InventoryName[inventoryGridSort] = "Air";
                                }

                                furnaceMaterialGrid.UpdateMaterialGrid();
                                furnaceContent.UpdateAllFurnaceGrid();
                            }
                            else
                            {
                                nameTextThread.nameText.text = "背包已满";
                                nameTextThread.timer = 1.5f;
                            }
                        }
                        else
                        {
                            if (IndexAll.nameToIsDurable(gridName))
                            {
                                furnaceMaterialGrid.materialName = gridName;
                                furnaceMaterialGrid.amount += playerThread.InventoryAmount[inventoryGridSort];
                                playerThread.InventoryName[inventoryGridSort] = "Air";
                                playerThread.InventoryAmount[inventoryGridSort] = 0;
                            }
                            else
                            {
                                furnaceMaterialGrid.materialName = gridName;
                                furnaceMaterialGrid.amount = 1;
                                playerThread.InventoryAmount[inventoryGridSort]--;
                                if (playerThread.InventoryAmount[inventoryGridSort] <= 0)
                                {
                                    playerThread.InventoryAmount[inventoryGridSort] = 0;
                                    playerThread.InventoryName[inventoryGridSort] = "Air";
                                }
                            }
                        }

                        audioSource.PlayOneShot(audioClipClick, 1f);
                        furnaceMaterialGrid.UpdateMaterialGrid();
                        UpdateGrid();
                        furnaceMaterialGrid.UpdateAmountBar();
                    }
                }
                else if (!gridName.Equals("Air"))
                {
                    nameTextThread.nameText.text = "该物品不可作为熔炼材料";
                    nameTextThread.timer = 1.5f;
                }

                furnaceContent.furnaceThread.material = furnaceMaterialGrid.materialName;
                furnaceContent.furnaceThread.amountMaterial = furnaceMaterialGrid.amount;
            }
            else if (furnaceContent.selection.Equals("fuel"))
            {
                float burnTime = IndexAll.nameToBurnTime(playerThread.InventoryName[inventoryGridSort]);
                string gridName = playerThread.InventoryName[inventoryGridSort];
                if (burnTime > 0)
                {
                    if (furnaceContent.timerPressed < 0.75f)
                    {
                        if (furnaceFuelGrid.fuelName.Equals(gridName) && !IndexAll.nameToIsDurable(gridName))
                        {
                            if (furnaceFuelGrid.amount < IndexAll.nameToMaxAmount(gridName))
                            {
                                playerThread.InventoryAmount[inventoryGridSort]--;
                                if (playerThread.InventoryAmount[inventoryGridSort] <= 0)
                                {
                                    playerThread.InventoryAmount[inventoryGridSort] = 0;
                                    playerThread.InventoryName[inventoryGridSort] = "Air";
                                }

                                furnaceFuelGrid.amount++;
                            }
                        }
                        else if (!furnaceFuelGrid.fuelName.Equals("null"))
                        {
                            if (playerThread.IfGetItemLeft(furnaceFuelGrid.fuelName, furnaceFuelGrid.amount, 36,
                                    false) <= 0)
                            {
                                playerThread.getItem(furnaceFuelGrid.fuelName, furnaceFuelGrid.amount, 36, false);
                                if (IndexAll.nameToIsDurable(gridName))
                                {
                                    furnaceFuelGrid.fuelName = gridName;
                                    furnaceFuelGrid.amount += playerThread.InventoryAmount[inventoryGridSort];
                                    playerThread.InventoryName[inventoryGridSort] = "Air";
                                    playerThread.InventoryAmount[inventoryGridSort] = 0;
                                }
                                else
                                {
                                    furnaceFuelGrid.fuelName = gridName;
                                    furnaceFuelGrid.amount = 1;
                                    playerThread.InventoryAmount[inventoryGridSort]--;
                                    if (playerThread.InventoryAmount[inventoryGridSort] <= 0)
                                        playerThread.InventoryName[inventoryGridSort] = "Air";
                                }

                                furnaceFuelGrid.UpdateFuelGrid();
                                furnaceContent.UpdateAllFurnaceGrid();
                            }
                            else
                            {
                                nameTextThread.nameText.text = "背包已满";
                                nameTextThread.timer = 1.5f;
                            }
                        }
                        else
                        {
                            if (IndexAll.nameToIsDurable(gridName))
                            {
                                furnaceFuelGrid.fuelName = gridName;
                                furnaceFuelGrid.amount += playerThread.InventoryAmount[inventoryGridSort];
                                playerThread.InventoryName[inventoryGridSort] = "Air";
                                playerThread.InventoryAmount[inventoryGridSort] = 0;
                            }
                            else
                            {
                                furnaceFuelGrid.fuelName = gridName;
                                furnaceFuelGrid.amount = 1;
                                playerThread.InventoryAmount[inventoryGridSort]--;
                                if (playerThread.InventoryAmount[inventoryGridSort] <= 0)
                                {
                                    playerThread.InventoryAmount[inventoryGridSort] = 0;
                                    playerThread.InventoryName[inventoryGridSort] = "Air";
                                }
                            }
                        }

                        audioSource.PlayOneShot(audioClipClick, 1f);
                        furnaceFuelGrid.UpdateFuelGrid();
                        UpdateGrid();
                        furnaceFuelGrid.UpdateAmountBar();
                    }
                }
                else if (!gridName.Equals("Air"))
                {
                    nameTextThread.nameText.text = "该物品不可作为熔炼燃料";
                    nameTextThread.timer = 1.5f;
                }

                furnaceContent.furnaceThread.fuel = furnaceFuelGrid.fuelName;
                furnaceContent.furnaceThread.amountFuel = furnaceFuelGrid.amount;
            }

            furnaceContent.UpdateItemBar();
        }
    }
}
