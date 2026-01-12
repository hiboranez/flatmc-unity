using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

public class CraftButton : MonoBehaviour {
    public WorldThread worldThread;
    public PlayerThread playerThread;
    public String targetName;
    public TargetCraftGrid targetCraftGrid;
    public AudioSource audioSourcePlayer;
    public AudioClip audioClipClick;
    public Image imageCraftButton;
    public float pressedTimer;
    public TabButtonNew tabButton2;
    public ItemBar itemBar;
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
    public GameObject imageTargetCraftDark;
    
    public IEnumerator PressAnimation() {
        imageCraftButton.sprite = Resources.Load<Sprite>("Textures/GUI/CraftButtonPressed");
        pressedTimer = 0.02f;
        while (pressedTimer > 0)
        {
            // 等待一段时间
            yield return new WaitForSeconds(Time.deltaTime);
            // 逐步减小flashTimer的值
            pressedTimer -= Time.deltaTime;
        }
        imageCraftButton.sprite = Resources.Load<Sprite>("Textures/GUI/CraftButton");
    }
    
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
    }

    public void OnClickCallBack() {
        audioSourcePlayer.PlayOneShot(audioClipClick, 1f);
        StartCoroutine(PressAnimation());
        if (playerThread.IfGetItemLeft(targetName, worldThread.craftTargetAmount[targetName],36,false) > 0) {
            playerThread.nameTextThread.nameText.text = "背包已满";
            playerThread.nameTextThread.timer = 1.5f;
        }else {
            if (IndexAll.nameToIsDurable(targetName)) {
                playerThread.getItem(targetName, IndexAll.nameToMaxAmount(targetName), 36, false);
            } else {
                playerThread.getItem(targetName, worldThread.craftTargetAmount[targetName], 36, false);
            }
            String[] recipe = worldThread.craftRecipeDictionary[targetName];
            foreach (var ingredient in recipe) {
                playerThread.ClearItem(ingredient,1);
            }
            tabButton2.UpdateAllCraftGrid(false);
            itemBar.UpdateAll();
            foreach (var targetCraftGridNew in tabButton2.CurrentTargetCraftGridList) {
                if (targetCraftGridNew.targetName.Equals(targetName)) {
                    targetCraftGridNew.UpdateCanCraft();
                    targetCraftGridNew.UpdateCraftGrid();
                    targetCraftGridNew.imageTargetCraftGrid.color = Color.gray;
                }
            }
            if (!UpdateCanCraft()) {
                for (int i = 0; i < 9; i++) {
                    if (ImageGraftGridList[i].enabled)
                        ImageGraftGridList[i].color = Color.gray;
                }
                imageTargetCraftDark.SetActive(true);
            }
         }
    }
    
    public bool UpdateCanCraft() {
        int craftMaxAmount = 99;
        Dictionary<String, int> ingredientRequiredAmountList = new Dictionary<string, int>();
        foreach (var ingredient in worldThread.craftRecipeDictionary[targetName]) {
            if(ingredient.Equals("Air")) continue;
            if (ingredientRequiredAmountList.ContainsKey(ingredient)) {
                ingredientRequiredAmountList[ingredient]++;
            } else {
                ingredientRequiredAmountList.Add(ingredient, 1);
            }
        }

        bool canCraft = true;
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

        return canCraft;
    }
}
