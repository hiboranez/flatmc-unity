using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

public class TargetCraftGrid : MonoBehaviour {
    public String targetName;
    public AudioSource audioSourcePlayer;
    public AudioClip audioClipSelect;
    public Image imageTargetCraftGrid;
    public Image ImageCraftGrid1;
    public Image ImageCraftGrid2;
    public Image ImageCraftGrid3;
    public Image ImageCraftGrid4;
    public Image ImageCraftGrid5;
    public Image ImageCraftGrid6;
    public Image ImageCraftGrid7;
    public Image ImageCraftGrid8;
    public Image ImageCraftGrid9;
    public Image[] ImageGraftGridList;
    public Image imageCraftButtonTarget;
    public TMP_Text targetText;
    public TMP_Text targetAmountText;
    public GameObject craftButton;
    public WorldThread worldThread;
    public PlayerThread playerThread;
    public bool canCraft;
    public int craftMaxAmount;
    public TMP_Text textTargetGridAmount;
    public GameObject imageTargetCraftDark;
    public CraftButton craftButtonDetect;
    public TabButtonNew tabButtonNew2;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
        ImageGraftGridList = new Image[9];
        ImageGraftGridList[0] = ImageCraftGrid1;
        ImageGraftGridList[1] = ImageCraftGrid2;
        ImageGraftGridList[2] = ImageCraftGrid3;
        ImageGraftGridList[3] = ImageCraftGrid4;
        ImageGraftGridList[4] = ImageCraftGrid5;
        ImageGraftGridList[5] = ImageCraftGrid6;
        ImageGraftGridList[6] = ImageCraftGrid7;
        ImageGraftGridList[7] = ImageCraftGrid8;
        ImageGraftGridList[8] = ImageCraftGrid9;
        imageTargetCraftGrid.enabled = true;
        imageTargetCraftGrid.sprite = Resources.Load<Sprite>("Icons/" + targetName);
    }

    public void GridInit() {
        // GetComponent<Button>().onClick.AddListener(OnClickCallBack);
        ImageGraftGridList = new Image[9];
        ImageGraftGridList[0] = ImageCraftGrid1;
        ImageGraftGridList[1] = ImageCraftGrid2;
        ImageGraftGridList[2] = ImageCraftGrid3;
        ImageGraftGridList[3] = ImageCraftGrid4;
        ImageGraftGridList[4] = ImageCraftGrid5;
        ImageGraftGridList[5] = ImageCraftGrid6;
        ImageGraftGridList[6] = ImageCraftGrid7;
        ImageGraftGridList[7] = ImageCraftGrid8;
        ImageGraftGridList[8] = ImageCraftGrid9;
        imageTargetCraftGrid.enabled = true;
        imageTargetCraftGrid.sprite = Resources.Load<Sprite>("Icons/" + targetName);
        UpdateCanCraft();
        if (!canCraft) {
            for (int i = 0; i < 9; i++) {
                if (ImageGraftGridList[i].enabled)
                    ImageGraftGridList[i].color = Color.gray;
            }
            textTargetGridAmount.color = Color.gray;
            imageTargetCraftGrid.color = Color.gray;
            imageTargetCraftDark.SetActive(true);
        } else {
            for (int i = 0; i < 9; i++) {
                if (ImageGraftGridList[i].enabled)
                    ImageGraftGridList[i].color = Color.white;
            }
            textTargetGridAmount.color = Color.white;
            imageTargetCraftGrid.color = Color.white;
            imageTargetCraftDark.SetActive(false);
        }
    }
    
    private void OnClickCallBack() {
        audioSourcePlayer.PlayOneShot(audioClipSelect, 1f);
        tabButtonNew2.UpdateAllCraftGrid(false);
        foreach (var targetCraftGrid in tabButtonNew2.CurrentTargetCraftGridList) {
            if (targetCraftGrid.targetName.Equals(targetName)) {
                craftButtonDetect.targetCraftGrid = targetCraftGrid;
                craftButtonDetect.targetName = targetName;
                targetCraftGrid.UpdateCanCraft();
                targetCraftGrid.UpdateCraftGrid();
                targetCraftGrid.imageTargetCraftGrid.color = Color.gray;
            }
        }
    }

    public void SelectInit() {
        foreach (var targetCraftGrid in tabButtonNew2.CurrentTargetCraftGridList) {
            if (targetCraftGrid.targetName.Equals(targetName)) {
                craftButtonDetect.targetCraftGrid = targetCraftGrid;
                craftButtonDetect.targetName = targetName;
                targetCraftGrid.UpdateCanCraft();
                targetCraftGrid.UpdateCraftGrid();
                targetCraftGrid.imageTargetCraftGrid.color = Color.gray;
            }
        }
    }
    
    public void UpdateCraftGrid() {
        if (!targetText.enabled) targetText.enabled = true;
        if(!craftButton.activeSelf) craftButton.SetActive(true);
        targetAmountText.text = worldThread.craftTargetAmount[targetName].ToString();
        targetText.text = IndexAll.nameToNameShow(targetName);
        imageCraftButtonTarget.sprite = Resources.Load<Sprite>("Icons/" + targetName);
        for (int i = 0; i < 9; i++) {
            String[] recipe = worldThread.craftRecipeDictionary[targetName];
            if (recipe[i].Equals("Air")) {
                ImageGraftGridList[i].enabled = false;
            } else {
                ImageGraftGridList[i].enabled = true;
                ImageGraftGridList[i].sprite = Resources.Load<Sprite>("Icons/" + recipe[i]);
            }
        }
        if (!canCraft) {
            for (int i = 0; i < 9; i++) {
                if (ImageGraftGridList[i].enabled)
                    ImageGraftGridList[i].color = Color.gray;
            }
            textTargetGridAmount.color = Color.gray;
            imageTargetCraftGrid.color = Color.gray;
            imageTargetCraftDark.SetActive(true);
        } else {
            for (int i = 0; i < 9; i++) {
                if (ImageGraftGridList[i].enabled)
                    ImageGraftGridList[i].color = Color.white;
            }
            textTargetGridAmount.color = Color.white;
            imageTargetCraftGrid.color = Color.white;
            imageTargetCraftDark.SetActive(false);
        }
    }

    public void UpdateCanCraft() {
        craftMaxAmount = 99;
        Dictionary<String, int> ingredientRequiredAmountList = new Dictionary<string, int>();
        foreach (var ingredient in worldThread.craftRecipeDictionary[targetName]) {
            if(ingredient.Equals("Air")) continue;
            if (ingredientRequiredAmountList.ContainsKey(ingredient)) {
                ingredientRequiredAmountList[ingredient]++;
            } else {
                ingredientRequiredAmountList.Add(ingredient, 1);
            }
        }

        canCraft = true;
        foreach (var ingredient in ingredientRequiredAmountList.Keys) {
            int count = 0;
            for (int i = 0; i < 36; i++) {
                if (playerThread.InventoryName[i].Equals(ingredient)) {
                    count += playerThread.InventoryAmount[i];
                }
            }
            int craftAmount = count / ingredientRequiredAmountList[ingredient];
            if (craftAmount < craftMaxAmount) {
                craftMaxAmount = craftAmount;
            }
            if (count < ingredientRequiredAmountList[ingredient]) {
                canCraft = false;
            }
        }
        textTargetGridAmount.text = craftMaxAmount.ToString();
    }
}
