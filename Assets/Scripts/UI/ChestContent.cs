using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace UI
{
    public class ChestContent : MonoBehaviour
    {
        public PlayerThread playerThread;
        public ChestThread chestThread;
        public int selectSort;
        public ItemBar itemBar;
        public WorldThread worldThread;
        public List<InventoryChestGrid> inventoryChestGridList;
        public List<ChestGrid> chestGridList;
        public GameObject inventoryChestGridPrefab;
        public GameObject chestGridPrefab;
        public GameObject inventoryChestGridScroll;
        public GameObject chestGridScroll;
        public RectTransform inventoryContentRectTransform;
        public RectTransform chestContentRectTransform;
        public string chestPressType;
        public float timerPressed;
        public bool presssed;
        public int pressSort;
        
        private void Awake()
        {
            inventoryChestGridList = new List<InventoryChestGrid>();
            chestGridList = new List<ChestGrid>();
        }
        
        private void OnEnable()
        {
            UpdateAllChestGrid();
            UpdateAllInventoryChestGrid();
        }
        
        private void OnDisable()
        {
            chestThread.CloseChest();
            chestThread.connected = false;
            chestThread.chestContent = null;
            chestThread = null;
        }
        
        private void Update()
        {
            if (presssed)
            {
                timerPressed += Time.deltaTime;
                if (timerPressed > 0.75f)
                {
                    if(chestPressType.Equals("inventory") && !playerThread.InventoryName[pressSort].Equals("Air"))
                        inventoryChestGridList[pressSort].UpdatePressBar(timerPressed);
                    else if(chestPressType.Equals("chest") && !chestThread.nameList[pressSort].Equals("Air"))
                        chestGridList[pressSort].UpdatePressBar(timerPressed);
                }
            }
            else
            {
                timerPressed = 0;
            }
        }
        
        private void Start()
        {
            foreach (var inventoryChestGrid in inventoryChestGridList)
            {
                inventoryChestGrid.UpdatePressBar(0);
            }
        }
        
        public void UpdateItemBar()
        {
            itemBar.UpdateAll();
        }
        
        public void UpdateAllChestGrid()
        {
            foreach (var chestGrid in chestGridList)
            {
                Destroy(chestGrid.gameObject);
            }
            chestGridList.Clear();
            Vector3 firstPosition = new Vector3(137.5f, -102, 0);
            int count = 0;
            chestGridPrefab.SetActive(true);
            for (int i = 0; i < chestThread.volume; i++)
            {
                GameObject chestGridTmp = Instantiate(chestGridPrefab, chestGridScroll.transform);
                Vector3 tmpPosition = firstPosition + new Vector3(150 * (count % 4), -150 * (count / 4),0);
                RectTransform rectTransform = chestGridTmp.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = tmpPosition;
                ChestGrid chestGrid = chestGridTmp.GetComponent<ChestGrid>();
                chestGrid.InitGrid(i);
                chestGridList.Add(chestGrid);
                count++;
            }
            chestGridPrefab.SetActive(false);
            float tmpY = chestContentRectTransform.anchoredPosition3D.y;
            chestContentRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,-tmpY , 150 * ((chestGridList.Count-1) / 4 + 1) + 60);
        }
        
        public void UpdateAllInventoryChestGrid()
        {
            foreach (var inventoryChestGrid in inventoryChestGridList)
            {
                Destroy(inventoryChestGrid.gameObject);
            }
            inventoryChestGridList.Clear();
            Vector3 firstPosition = new Vector3(137.5f, -102, 0);
            int count = 0;
            inventoryChestGridPrefab.SetActive(true);
            for (int i = 0; i < 36; i++)
            {
                GameObject inventoryChestGridTmp = Instantiate(inventoryChestGridPrefab, inventoryChestGridScroll.transform);
                Vector3 tmpPosition = firstPosition + new Vector3(150 * (count % 4), -150 * (count / 4),0);
                RectTransform rectTransform = inventoryChestGridTmp.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = tmpPosition;
                InventoryChestGrid inventoryChestGrid = inventoryChestGridTmp.GetComponent<InventoryChestGrid>();
                inventoryChestGrid.InitGrid(i);
                inventoryChestGridList.Add(inventoryChestGrid);
                count++;
            }
            inventoryChestGridPrefab.SetActive(false);
            float tmpY = inventoryContentRectTransform.anchoredPosition3D.y;
            inventoryContentRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,-tmpY , 150 * ((inventoryChestGridList.Count-1) / 4 + 1) + 60);
        }
        
        public int StoreItem(String name, int amount, int searchSize, bool soundOn) {
        // 定义还未被捡完的掉落物数量
        int amountLeft = amount;
        // 如果不是工具
        if (!IndexAll.nameToIsDurable(name)) {
            // 搜索背包内是否已经存在该物品
            for (int i = 0; i < searchSize; i++)
                // 如果存在
                if (chestThread.nameList[i] == name) {
                    // 如果物品数量小于最大堆叠数
                    if (chestThread.amountList[i] < IndexAll.nameToMaxAmount(name)) {
                        // 如果物品数量加上全部物品多于最大堆叠数
                        if (chestThread.amountList[i] + amountLeft > IndexAll.nameToMaxAmount(name)) {
                            // 掉落物剩余数量扣除已经捡走的数量
                            amountLeft -= (IndexAll.nameToMaxAmount(name) - chestThread.amountList[i]);
                            // 该物品堆叠达到上限，设为最大堆叠数
                            chestThread.amountList[i] = IndexAll.nameToMaxAmount(name);
                        } else {
                            // 否则该物品直接堆叠全部掉落物数量
                            chestThread.amountList[i] += amountLeft;
                            // 掉落物剩余数量扣除已经捡走的数量
                            amountLeft = 0;
                            // 退出循环
                            break;
                        }
                    }
                }
        }
        // 如果掉落物数量还有剩余
        if (amountLeft > 0)
            // 搜寻背包内第一个空位
            for (int i = 0; i < searchSize; i++)
                // 如果搜索到了
                if (chestThread.nameList[i] == "Air") {
                    // 如果物品剩余数量小于等于最大堆叠数
                    if (amountLeft <= IndexAll.nameToMaxAmount(name)) {
                        // 该物品栏直接堆叠剩余数量
                        chestThread.amountList[i] += amountLeft;
                        // 设置此物品栏存在该物品
                        chestThread.nameList[i] = name;
                        // 掉落物剩余数量扣除已经捡走的数量
                        amountLeft = 0;
                        // 退出循环
                        break;
                    } else {
                        // 否则堆叠达到上限，设为最大堆叠数
                        chestThread.amountList[i] = IndexAll.nameToMaxAmount(name);
                        // 设置此物品栏存在该物品
                        chestThread.nameList[i] = name;
                        // 掉落物剩余数量扣除最大堆叠数
                        amountLeft -= IndexAll.nameToMaxAmount(name);
                    }
                }
        // 如果得到了东西，播放pop音效
        if (amountLeft < amount && soundOn) {
           UpdateAllChestGrid();
        }
        // 返回剩余数量
        return amountLeft;
    }
    
    // 玩家拾取掉落物，返回未被拾取的数量
    public int IfStoreItem(String name, int amount, int searchSize, bool soundOn) {
        // 定义还未被捡完的掉落物数量
        int amountLeft = amount;
        // 如果不是工具
        if (!IndexAll.nameToIsDurable(name)) {
            // 搜索背包内是否已经存在该物品
            for (int i = 0; i < searchSize; i++)
                // 如果存在
                if (chestThread.nameList[i] == name) {
                    // 如果物品数量小于最大堆叠数
                    if (chestThread.amountList[i] < IndexAll.nameToMaxAmount(name)) {
                        // 如果物品数量加上全部物品多于最大堆叠数
                        if (chestThread.amountList[i] + amountLeft > IndexAll.nameToMaxAmount(name)) {
                            // 掉落物剩余数量扣除已经捡走的数量
                            amountLeft -= (IndexAll.nameToMaxAmount(name) - chestThread.amountList[i]);
                            // 该物品堆叠达到上限，设为最大堆叠数
                        } else {
                            // 否则该物品直接堆叠全部掉落物数量
                            // 掉落物剩余数量扣除已经捡走的数量
                            amountLeft = 0;
                            // 退出循环
                            break;
                        }
                    }
                }
        }
        // 如果掉落物数量还有剩余
        if (amountLeft > 0)
            // 搜寻背包内第一个空位
            for (int i = 0; i < searchSize; i++)
                // 如果搜索到了
                if (chestThread.nameList[i] == "Air") {
                    // 如果物品剩余数量小于等于最大堆叠数
                    if (amountLeft <= IndexAll.nameToMaxAmount(name)) {
                        // 该物品栏直接堆叠剩余数量
                        // 设置此物品栏存在该物品
                        // 掉落物剩余数量扣除已经捡走的数量
                        amountLeft = 0;
                        // 退出循环
                        break;
                    } else {
                        // 否则堆叠达到上限，设为最大堆叠数
                        // 设置此物品栏存在该物品
                        // 掉落物剩余数量扣除最大堆叠数
                        amountLeft -= IndexAll.nameToMaxAmount(name);
                    }
                }
        // 返回剩余数量
        return amountLeft;
    }
    }
}