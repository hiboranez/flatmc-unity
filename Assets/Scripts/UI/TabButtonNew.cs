using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

public class TabButtonNew : MonoBehaviour {
    public WorldThread worldThread;
    public PlayerThread playerThread;
    public GameObject TabUI1;
    public GameObject TabUI2;
    public GameObject TabUI3;
    public GameObject DarkBack1;
    public GameObject DarkBack2;
    public GameObject DarkBack3;
    public Canvas canvas1;
    public Canvas canvas2;
    public Canvas canvas3;
    public AudioClip audioClip;
    public AudioSource audioSource;
    public int tabSort;
    public GameObject CraftingTargetContent;
    public GameObject TargetCraftGridPrefab;
    public InventoryContent inventoryContent;
    public GameObject InventoryUI;
    public CraftButton craftButton;
    public GameObject craftGrid1;
    public GameObject craftGrid2;
    public GameObject craftGrid3;
    public GameObject craftGrid4;
    public GameObject craftGrid5;
    public GameObject craftGrid6;
    public GameObject craftGrid7;
    public GameObject craftGrid8;
    public GameObject craftGrid9;
    public List<RectTransform> craftGridRectTransformList;
    public List<TargetCraftGrid> CurrentTargetCraftGridList;
    public RectTransform craftingContentRectTransform;
    public GameObject armorGridPrefab;
    public ArmorContent armorContent;
    public Image iconHelmet;
    public Image iconChest;
    public Image iconLeggings;
    public Image iconBoots;
    public Image iconArmorHelmet;
    public Image iconArmorChest;
    public Image iconArmorLeggings;
    public Image iconArmorBoots;
    public Image amountBarImageHelmet;
    public RectTransform amountBarRectTransformHelmet;
    public Image amountBarImageChest;
    public RectTransform amountBarRectTransformChest;
    public Image amountBarImageLeggings;
    public RectTransform amountBarRectTransformLeggings;
    public Image amountBarImageBoots;
    public RectTransform amountBarRectTransformBoots;
    void Start() {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
        CurrentTargetCraftGridList = new List<TargetCraftGrid>();
        if(tabSort == 2){
            craftGridRectTransformList.Add(craftGrid1.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid2.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid3.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid4.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid5.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid6.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid7.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid8.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid9.GetComponent<RectTransform>());
        }
    }

    public void UpdateCraftGrid() {
        if (craftGridRectTransformList.Count <= 0) {
            craftGridRectTransformList.Add(craftGrid1.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid2.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid3.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid4.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid5.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid6.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid7.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid8.GetComponent<RectTransform>());
            craftGridRectTransformList.Add(craftGrid9.GetComponent<RectTransform>());
        }
        if (!playerThread.onCraftingTable) {
            craftGridRectTransformList[0].anchoredPosition = new Vector2(471, 147);
            craftGridRectTransformList[1].anchoredPosition = new Vector2(601, 147);
            craftGrid3.SetActive(false);
            craftGridRectTransformList[3].anchoredPosition = new Vector2(471, 277);
            craftGridRectTransformList[4].anchoredPosition = new Vector2(601, 277);
            craftGrid6.SetActive(false);
            craftGrid7.SetActive(false);
            craftGrid8.SetActive(false);
            craftGrid9.SetActive(false);
        } else {
            craftGridRectTransformList[0].anchoredPosition = new Vector2(406, 82);
            craftGridRectTransformList[1].anchoredPosition = new Vector2(536, 82);
            craftGrid3.SetActive(true);
            craftGridRectTransformList[3].anchoredPosition = new Vector2(406, 212);
            craftGridRectTransformList[4].anchoredPosition = new Vector2(536, 212);
            craftGrid6.SetActive(true);
            craftGrid7.SetActive(true);
            craftGrid8.SetActive(true);
            craftGrid9.SetActive(true);
        }
    }

    public void SwitchThisButton() {
        if (tabSort == 1) {
            TabUI1.SetActive(true);
            TabUI2.SetActive(false);
            TabUI3.SetActive(false);
            canvas1.sortingOrder = 4;
            canvas2.sortingOrder = 2;
            canvas3.sortingOrder = 2;
            if(DarkBack1.activeInHierarchy) DarkBack1.SetActive(false);
            DarkBack2.SetActive(true);
            DarkBack3.SetActive(true);
            inventoryContent.UpdateAll();
        }else if (tabSort == 2) {
            TabUI1.SetActive(false);
            TabUI2.SetActive(true);
            TabUI3.SetActive(false);
            canvas1.sortingOrder = 2;
            canvas2.sortingOrder = 4;
            canvas3.sortingOrder = 2;
            DarkBack1.SetActive(true);
            if(DarkBack2.activeInHierarchy) DarkBack2.SetActive(false);
            DarkBack3.SetActive(true);
            UpdateCraftGrid();
            UpdateAllCraftGrid(true);
            if(CurrentTargetCraftGridList.Count > 0) {
                CurrentTargetCraftGridList[0].SelectInit();
            }
        }else if (tabSort == 3) {
            TabUI1.SetActive(false);
            TabUI2.SetActive(false);
            TabUI3.SetActive(true);
            canvas1.sortingOrder = 2;
            canvas2.sortingOrder = 2;
            canvas3.sortingOrder = 4;
            DarkBack1.SetActive(true);
            DarkBack2.SetActive(true);
            if(DarkBack3.activeInHierarchy) DarkBack3.SetActive(false);
            UpdateArmorGrid();
            UpdateArmorSlot();
            UpdateArmorSlotAmount();
        }
    }

    public void UpdateArmorSlotAmount()
    {
        float length = 114 * ((float)playerThread.armorHelmetAmount / IndexAll.nameToMaxAmount(playerThread.armorHelmet));
        if(length > 76 && length <= 114) amountBarImageHelmet.color = Color.green;
        else if(length > 38 && length <= 76) amountBarImageHelmet.color = Color.yellow;
        else if(length >= 0 && length <= 38) amountBarImageHelmet.color = Color.red;
        amountBarRectTransformHelmet.sizeDelta = new Vector2(length, amountBarRectTransformHelmet.sizeDelta.y);
        amountBarRectTransformHelmet.anchoredPosition = new Vector2(-(114 - length) / 2, amountBarRectTransformHelmet.anchoredPosition.y);
        
        length = 114 * ((float)playerThread.armorChestAmount / IndexAll.nameToMaxAmount(playerThread.armorChest));
        if(length > 76 && length <= 114) amountBarImageChest.color = Color.green;
        else if(length > 38 && length <= 76) amountBarImageChest.color = Color.yellow;
        else if(length >= 0 && length <= 38) amountBarImageChest.color = Color.red;
        amountBarRectTransformChest.sizeDelta = new Vector2(length, amountBarRectTransformChest.sizeDelta.y);
        amountBarRectTransformChest.anchoredPosition = new Vector2(-(114 - length) / 2, amountBarRectTransformChest.anchoredPosition.y);

        length = 114 * ((float)playerThread.armorLeggingsAmount / IndexAll.nameToMaxAmount(playerThread.armorLeggings));
        if(length > 76 && length <= 114) amountBarImageLeggings.color = Color.green;
        else if(length > 38 && length <= 76) amountBarImageLeggings.color = Color.yellow;
        else if(length >= 0 && length <= 38) amountBarImageLeggings.color = Color.red;
        amountBarRectTransformLeggings.sizeDelta = new Vector2(length, amountBarRectTransformLeggings.sizeDelta.y);
        amountBarRectTransformLeggings.anchoredPosition = new Vector2(-(114 - length) / 2, amountBarRectTransformLeggings.anchoredPosition.y);

        length = 114 * ((float)playerThread.armorBootsAmount / IndexAll.nameToMaxAmount(playerThread.armorBoots));
        if(length > 76 && length <= 114) amountBarImageBoots.color = Color.green;
        else if(length > 38 && length <= 76) amountBarImageBoots.color = Color.yellow;
        else if(length >= 0 && length <= 38) amountBarImageBoots.color = Color.red;
        amountBarRectTransformBoots.sizeDelta = new Vector2(length, amountBarRectTransformBoots.sizeDelta.y);
        amountBarRectTransformBoots.anchoredPosition = new Vector2(-(114 - length) / 2, amountBarRectTransformBoots.anchoredPosition.y);

        playerThread.UpdateAmountBarAmount();
    }
    
    public void UpdateArmorSlot()
    {
        iconArmorHelmet.enabled = true;
        iconHelmet.enabled = true;
        iconArmorChest.enabled = true;
        iconChest.enabled = true;
        iconArmorLeggings.enabled = true;
        iconLeggings.enabled = true;
        iconArmorBoots.enabled = true;
        iconBoots.enabled = true;
        if (playerThread.armorHelmet.Equals("null"))
        {
            iconArmorHelmet.sprite = null;
            iconHelmet.sprite = Resources.Load<Sprite>("Icons/EmptySlotHelmet");
            iconArmorHelmet.enabled = false;
            iconHelmet.enabled = true;
        }
        else
        {
            iconArmorHelmet.sprite = Resources.Load<Sprite>("Icons/" + playerThread.armorHelmet);
            iconHelmet.sprite = null;
            iconArmorHelmet.enabled = true;
            iconHelmet.enabled = false;
        }
        
        if (playerThread.armorChest.Equals("null"))
        {
            
            iconArmorChest.sprite = null;
            iconChest.sprite = Resources.Load<Sprite>("Icons/EmptySlotChestplate");
            iconArmorChest.enabled = false;
            iconChest.enabled = true;
        }
        else
        {
            iconArmorChest.sprite = Resources.Load<Sprite>("Icons/" + playerThread.armorChest);
            iconChest.sprite = null;
            iconArmorChest.enabled = true;
            iconChest.enabled = false;
        }
        
        if (playerThread.armorLeggings.Equals("null"))
        {
            
            iconArmorLeggings.sprite = null;
            iconLeggings.sprite = Resources.Load<Sprite>("Icons/EmptySlotLeggings");
            iconArmorLeggings.enabled = false;
            iconLeggings.enabled = true;
        }
        else
        {
            iconArmorLeggings.sprite = Resources.Load<Sprite>("Icons/" + playerThread.armorLeggings);
            iconLeggings.sprite = null;
            iconArmorLeggings.enabled = true;
            iconLeggings.enabled = false;
        }
        
        if (playerThread.armorBoots.Equals("null"))
        {
            
            iconArmorBoots.sprite = null;
            iconBoots.sprite = Resources.Load<Sprite>("Icons/EmptySlotBoots");
            iconArmorBoots.enabled = false;
            iconBoots.enabled = true;
        }
        else
        {
            iconArmorBoots.sprite = Resources.Load<Sprite>("Icons/" + playerThread.armorBoots);
            iconBoots.sprite = null;
            iconArmorBoots.enabled = true;
            iconBoots.enabled = false;
        }
    }
    
    private void OnClickCallBack() {
        audioSource.PlayOneShot(audioClip, 1f);
        SwitchThisButton();
    }

    public void UpdateArmorGrid()
    {
        foreach (var armorGrid in armorContent.armorGridList)
        {
            Destroy(armorGrid);
        }
        armorContent.armorGridList.Clear();
        Vector3 firstPosition = new Vector3(130, -102, 0);
        int count = 0;
        armorGridPrefab.SetActive(true);
        for (int i = 0; i < 36; i++)
        {
            if (IndexAll.nameToIsArmor(playerThread.InventoryName[i]))
            {
                GameObject armorGridTmp = Instantiate(armorGridPrefab, armorContent.transform);
                Vector3 tmpPosition = firstPosition + new Vector3(150 * (count % 4), -150 * (count / 4),0);
                RectTransform rectTransform = armorGridTmp.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = tmpPosition;
                armorGridTmp.GetComponent<ArmorGrid>().GridInit(i);
                armorContent.armorGridList.Add(armorGridTmp);
                count++;
            }
        }
        armorGridPrefab.SetActive(false);
    }

    public void OpenCraftingTable() {
        InventoryUI.SetActive(true);
        TabUI1.SetActive(false);
        TabUI2.SetActive(true);
        TabUI3.SetActive(false);
        canvas1.sortingOrder = 2;
        canvas2.sortingOrder = 4;
        canvas3.sortingOrder = 2;
        DarkBack1.SetActive(true);
        if(DarkBack2.activeInHierarchy) DarkBack2.SetActive(false);
        DarkBack3.SetActive(true);
        UpdateCraftGrid();
        UpdateAllCraftGrid(true);
    }

    public static T[] GetComponentsInRealChildren<T>(GameObject go, bool includeInactive = false) where T : Component
    {
        List<T> TList = go.GetComponentsInChildren<T>(includeInactive).ToList(); 
        List<T> TListReal = new List<T>();
        for (int i = 0; i < TList.Count; i++)
        {
            if (TList[i].transform.parent == go.transform)
            {
                TListReal.Add(TList[i]);
            }
        }
        return TListReal.ToArray();
    }
    
    public void UpdateAllCraftGrid(bool updateTarget) {
        String craftButtonTargetName = "null";
        if (!updateTarget) {
            craftButtonTargetName = craftButton.targetCraftGrid.targetName;
        }
        CurrentTargetCraftGridList.Clear();
        Transform[] currentTargetCraftGridList = GetComponentsInRealChildren<Transform>(CraftingTargetContent, true);
        for (int i = 1; i < currentTargetCraftGridList.Length; i++) {
            Destroy(currentTargetCraftGridList[i].gameObject);
        }
        List<String> targetList = new List<string>();
            for (int i = 0; i < 36; i++) {
                if(playerThread.InventoryName[i].Equals("Air")) continue;
                if(!worldThread.craftInvolvedDictionary.ContainsKey(playerThread.InventoryName[i])) continue;
                List<String> targetListTmp = worldThread.craftInvolvedDictionary[playerThread.InventoryName[i]];
                foreach (var target in targetListTmp) {
                    if (worldThread.craftRecipeNeedCraftingTableDictionary[target]) {
                        if (!targetList.Contains(target) && playerThread.onCraftingTable) {
                            targetList.Add(target);
                        }
                    } else if (!targetList.Contains(target)) {
                        targetList.Add(target);
                    }
                }
            }

            List<String> firstTargetList = new List<string>();
            List<String> lastTargetList = new List<string>();
            foreach (var target in targetList) {
                if (IsCanCraft(target)) {
                    firstTargetList.Add(target);
                } else {
                    lastTargetList.Add(target);
                }
            }

            Vector3 FirstPosition = new Vector3(130, -102, 0);
            int targetCount = 0;
            TargetCraftGridPrefab.SetActive(true);
            foreach (var target in firstTargetList) {
                Vector3 tmpPosition = FirstPosition + new Vector3(150 * (targetCount % 6), -150 * (targetCount / 6),0);
                GameObject TargetCraftGridTmp = Instantiate(TargetCraftGridPrefab, CraftingTargetContent.transform);
                RectTransform rectTransform = TargetCraftGridTmp.GetComponent<RectTransform>();
                TargetCraftGrid targetCraftGrid = TargetCraftGridTmp.GetComponent<TargetCraftGrid>();
                rectTransform.anchoredPosition = tmpPosition;
                targetCraftGrid.targetName = target;
                targetCraftGrid.GridInit();
                CurrentTargetCraftGridList.Add(targetCraftGrid);
                if (targetCount == 0 && updateTarget) {
                    targetCraftGrid.craftButtonDetect.targetCraftGrid = targetCraftGrid;
                    targetCraftGrid.craftButtonDetect.targetName = target;
                    // targetCraftGrid.UpdateCanCraft();
                    // targetCraftGrid.UpdateCraftGrid();
                    targetCraftGrid.SelectInit();
                }
                targetCount++;
            }
            foreach (var target in lastTargetList) {
                Vector3 tmpPosition = FirstPosition + new Vector3(150 * (targetCount % 6), -150 * (targetCount / 6),0);
                GameObject TargetCraftGridTmp = Instantiate(TargetCraftGridPrefab, tmpPosition, Quaternion.identity,CraftingTargetContent.transform);
                RectTransform rectTransform = TargetCraftGridTmp.GetComponent<RectTransform>();
                TargetCraftGrid targetCraftGrid = TargetCraftGridTmp.GetComponent<TargetCraftGrid>();
                rectTransform.anchoredPosition = tmpPosition;targetCraftGrid.targetName = target;
                targetCraftGrid.GridInit();
                CurrentTargetCraftGridList.Add(targetCraftGrid);
                targetCount++;
            }
            if(CurrentTargetCraftGridList.Count <= 0) {
                TargetCraftGrid targetCraftGridOrigin = TargetCraftGridPrefab.GetComponent<TargetCraftGrid>();
                craftButton.imageCraftButton.sprite = Resources.Load<Sprite>("Textures/GUI/CraftButton");
                targetCraftGridOrigin.craftButton.SetActive(false);
                for (int i = 0; i < 9; i++) {
                    targetCraftGridOrigin.ImageGraftGridList[i].enabled = false;
                }
                targetCraftGridOrigin.targetText.text = "";
            } else {
                float tmpY = craftingContentRectTransform.anchoredPosition3D.y;
                craftingContentRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,-tmpY , 150 * (CurrentTargetCraftGridList.Count / 6 + 1) + 60);
            }
            TargetCraftGridPrefab.SetActive(false);
            // Transform[] newTargetCraftGridList = GetComponentsInRealChildren<Transform>(CraftingTargetContent, true);
            // foreach (var newTargetCraftGridTransform in newTargetCraftGridList) {
            //     TargetCraftGrid newTargetCraftGrid = newTargetCraftGridTransform.gameObject.GetComponent<TargetCraftGrid>();
            //     if (craftButtonTargetName.Equals(newTargetCraftGrid.targetName)) {
            //         newTargetCraftGrid.UpdateCanCraft();
            //         newTargetCraftGrid.UpdateCraftGrid();
            //     }
            // }
    }
    
    public bool IsCanCraft(String targetName) {
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
            if (count < ingredientRequiredAmountList[ingredient]) {
                canCraft = false;
            }
        }
        return canCraft;
    }
}