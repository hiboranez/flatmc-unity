using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Util;

public class ItemThread : MonoBehaviour {
    public static float timerItemAssemble;
    public static bool hasNewItemAssemble;
    public static List<ItemThread> itemThreadList = new List<ItemThread>();
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRendererDouble;
    public Rigidbody2D rigidbody2DItem;
    public GameObject itemPrefab;
    public WorldThread worldThread;
    // **定义掉落物名称
    public String nameItem;
    // 定义掉落物物品堆叠数量
    public int amount = 1;
    // 定义掉落物不可被拾取计时器
    public float timerNoCollect = 1f;

    private void Start() {
        if (amount > 1) {
            spriteRendererDouble.enabled = true;
        } else {
            spriteRendererDouble.enabled = false;
        }
        worldThread = GameObject.FindWithTag("World").GetComponent<WorldThread>();
    }

    private void Update() {
        updateItemData();
        UpdateInBlock();
    }

    private void FixedUpdate()
    {
        UpdateInWater();
    }

    void UpdateInWater() {
        Vector3Int blockPosition =
            worldThread.liquidBlockTileMap.WorldToCell(transform.position + new Vector3(0, 0.25f, 0));
        if (blockPosition.x >= 0 && blockPosition.x < worldThread.width && blockPosition.y >= 0 &&
            blockPosition.y < worldThread.height) {
            if(!worldThread.liquidBlockList[blockPosition.y, blockPosition.x].Equals("Air"))
            {
                rigidbody2DItem.gravityScale = 1f;
                Vector3 velocity = rigidbody2DItem.velocity;
                velocity.x *= 0.9f;
                if (velocity.y > 2f) velocity.y = 2;
                rigidbody2DItem.velocity = velocity;
            }
            else
            {
                rigidbody2DItem.gravityScale = 5;
            }
        }
    }
    
    void UpdateInBlock() {
        Vector3Int blockPosition =
            worldThread.solidBlockTileMap.WorldToCell(transform.position + new Vector3(0, 0.25f, 0));
        if (blockPosition.x >= 0 && blockPosition.x < worldThread.width && blockPosition.y >= 0 &&
            blockPosition.y < worldThread.height) {
            if(!worldThread.noReachBlockList[blockPosition.y, blockPosition.x]){
                if (!worldThread.solidBlockList[blockPosition.y, blockPosition.x].Equals("Air")) {
                    rigidbody2DItem.constraints = RigidbodyConstraints2D.FreezeAll;
                } else {
                    rigidbody2DItem.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }
    }
    
    // 更新掉落物数据
    public void updateItemData() {
        // 更新不可被拾取时间
        if (timerNoCollect > 0) {
            timerNoCollect-= Time.deltaTime;
        }else if (timerNoCollect < 0) {
            timerNoCollect = 0;
        }
        if (timerItemAssemble > 0) {
            timerItemAssemble -= Time.deltaTime;
        }else if (timerItemAssemble < 0) {
            timerItemAssemble = 0;
        } else if (hasNewItemAssemble && timerItemAssemble == 0) {
            NewAssembledItem();
            hasNewItemAssemble = false;
            itemThreadList.Clear();
        }
    }

    public void NewAssembledItem() {
        ItemThread itemThread1 = itemThreadList[0];
        ItemThread itemThread2 = itemThreadList[1];
        if(itemThread1 != null && itemThread2 != null)
        {
            GameObject itemNew = Instantiate(itemPrefab,
                (itemThread1.gameObject.transform.position + itemThread2.gameObject.transform.position) / 2,
                Quaternion.identity);
            ItemThread itemNewThread = itemNew.gameObject.GetComponent<ItemThread>();
            itemNewThread.itemInit(itemThread1.nameItem, itemThread1.amount + itemThread2.amount, 0);
            itemNewThread.rigidbody2DItem.velocity =
                (itemThread1.rigidbody2DItem.velocity + itemThread2.rigidbody2DItem.velocity) / 2;
            if (IndexAll.BlockNameToIsLight(itemNewThread.nameItem)) {
                itemNew.GetComponentsInChildren<Transform>()[2].GetComponent<Light2D>().enabled = true;
            }
            worldThread.itemList.Remove(itemThreadList[1]);
            worldThread.itemList.Remove(itemThreadList[0]);
            Destroy(itemThreadList[1].gameObject);
            Destroy(itemThreadList[0].gameObject);
        }
    }
    
    public void OnCollisionEnter2D(Collision2D other) {
        if (timerNoCollect <= 0) {
            if(other.gameObject.tag.Equals("Player")) {
                PlayerThread playerThread = other.gameObject.GetComponent<PlayerThread>();
                int amountLeft = playerThread.getItem(nameItem, amount, 36, true);
                if(amountLeft == 0)
                {
                    if(worldThread.itemList.Contains(this)){
                        worldThread.itemList.Remove(this);
                    }
                    Destroy(gameObject);
                }
                else if(amountLeft < amount){
                    GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
                    ItemThread itemThread = item.gameObject.GetComponent<ItemThread>();
                    itemThread.itemInit(nameItem,amountLeft,0);
                    worldThread.itemList.Remove(this);
                    Destroy(gameObject);
                }
            }else if(other.gameObject.tag.Equals("Item") && !IndexAll.nameToIsDurable(nameItem)) {
                ItemThread itemThread = other.gameObject.GetComponent<ItemThread>();
                if (nameItem.Equals(itemThread.nameItem) && amount + itemThread.amount <= IndexAll.nameToMaxAmount(itemThread.nameItem)) {
                    itemThreadList.Add(this);
                    timerItemAssemble = 0.02f;
                    hasNewItemAssemble = true;
                }
            }
        }
    }

    public void itemInit(String nameItem1, int amount1, float timerNoCollect1) {
        nameItem = nameItem1;
        amount = amount1;
        timerNoCollect = timerNoCollect1;
        spriteRenderer.sprite = Resources.Load<Sprite>("Icons/" + nameItem);
        if (amount > 1 && !IndexAll.nameToIsDurable(nameItem)) {
            spriteRendererDouble.enabled = true;
            spriteRendererDouble.sprite = spriteRenderer.sprite;
        } else {
            spriteRendererDouble.enabled = true;
            spriteRendererDouble.sprite = null;
        }
        worldThread.itemList.Add(this);
    }
}
