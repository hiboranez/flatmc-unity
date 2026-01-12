using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Util;

public class ChestThread : MonoBehaviour
{
    public Animator animator;
    public UseAudio useAudio;
    public WorldThread worldThread;
    public ChestContent chestContent;
    public GameObject items;
    public GameObject itemPrefab;
    public List<Vector2Int> blockPositionList;
    public GameObject largeChestPrefab;
    public GameObject chestPrefab;
    public Transform chestsTransform;
    public int volume;
    public string[] nameList;
    public int[] amountList;
    public bool connected;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void InitChest(int v, bool emptyChest)
    {
        volume = v;
        nameList = new string[volume];
        amountList = new int[volume];
        if (emptyChest)
        {
            for (int i = 0; i < volume; i++)
            {
                nameList[i] = "Air";
                amountList[i] = 0;
            }
        }
    }

    public bool AssembleNearbyChest()
    {
        bool assembled = false;
        bool stopSearching = false;
        List<ChestThread> chestListTmp = new List<ChestThread>(worldThread.chestList);
        foreach (var chest in chestListTmp)
        {
            foreach (var blockPosition in chest.blockPositionList)
            {
                if (blockPosition.y == blockPositionList[0].y)
                {
                    bool canAssemble = false;
                    int k = 0;
                    if (blockPosition.x - 1 == blockPositionList[0].x)
                    {
                        k = -1;
                        canAssemble = true;
                    }else if (blockPosition.x + 1 == blockPositionList[0].x)
                    {
                        k = 1;
                        canAssemble = true;
                    }
                    if (canAssemble)
                    {
                        if (chest.volume < 30)
                        {
                            Vector3 chestPosition = new Vector3(0.5f + blockPosition.x + 0.5f * k,
                                blockPosition.y, 3.5f);
                            largeChestPrefab.SetActive(true);
                            GameObject largeChest = Instantiate(largeChestPrefab, chestPosition,
                                Quaternion.identity, chestsTransform);
                            ChestThread chestThread = largeChest.GetComponent<ChestThread>();
                            chestThread.blockPositionList.Add(new Vector2Int(blockPosition.x,blockPosition.y));
                            chestThread.blockPositionList.Add(new Vector2Int(blockPosition.x+k,blockPosition.y));
                            chestThread.InitChest(54, true);
                            for (int i = 0; i < 27; i++)
                            {
                                chestThread.nameList[i] = chest.nameList[i];
                                chestThread.amountList[i] = chest.amountList[i];
                            }
                            worldThread.chestList.Add(chestThread);
                            worldThread.solidBlockList[blockPosition.y, blockPosition.x] = "Chest";
                            largeChestPrefab.SetActive(false);
                            worldThread.chestList.Remove(chest);
                            worldThread.chestList.Remove(this);
                            Destroy(chest.gameObject);
                            Destroy(gameObject);
                            stopSearching = true;
                            assembled = true;
                        }
                    }
                }
            }
            if (stopSearching) break;
        }

        return assembled;
    }
    
    public void OpenChest()
    {
        animator.SetTrigger("IdleToOpen");
        float xBlock = 0;
        float yBlock = 0;
        if (volume < 30)
        {
            xBlock = blockPositionList[0].x + 0.5f;
            yBlock = blockPositionList[0].y + 0.5f;
        }
        else
        {
            xBlock = blockPositionList[0].x;
            yBlock = blockPositionList[0].y + 0.5f;
        }
        useAudio.PlayUse(xBlock, yBlock, "ChestOpen");
    }
    
    public void CloseChest()
    {
        animator.SetTrigger("OpenToClose");
        float xBlock = 0;
        float yBlock = 0;
        if (volume < 30)
        {
            xBlock = blockPositionList[0].x + 0.5f;
            yBlock = blockPositionList[0].y + 0.5f;
        }
        else
        {
            xBlock = blockPositionList[0].x;
            yBlock = blockPositionList[0].y + 0.5f;
        }
        useAudio.PlayUse(xBlock, yBlock, "ChestClose");
    }

    public void DestroyChest(int x, int y)
    {
        if(volume > 30){
            worldThread.chestList.Remove(this);
            float xBlock = x + 0.5f;
            float yBlock = y + 0.5f;
            for (int i = 27; i < volume; i++)
            {
                if (!nameList[i].Equals("Air"))
                {
                    itemPrefab.SetActive(true);
                    GameObject item1 = Instantiate(itemPrefab,
                        new Vector3(xBlock, yBlock, 0),
                        Quaternion.identity, items.transform);
                    ItemThread itemThread1 = item1.gameObject.GetComponent<ItemThread>();
                    itemThread1.itemInit(nameList[i], amountList[i], 0);
                    if (IndexAll.BlockNameToIsLight(itemThread1.nameItem))
                    {
                        item1.GetComponentsInChildren<Transform>()[2].GetComponent<Light2D>().enabled = true;
                    }
                    itemPrefab.SetActive(false);
                }
            }
            int x1 = blockPositionList[0].x;
            int x2 = blockPositionList[1].x;
            int xRest = 0;
            if (x1 == x) xRest = x2;
            else xRest = x1;
            Vector3 chestPosition = new Vector3(xRest + 0.5f,
                blockPositionList[0].y, 3.5f);
            chestPrefab.SetActive(true);
            GameObject chest = Instantiate(chestPrefab, chestPosition,
                Quaternion.identity, chestsTransform);
            ChestThread chestThread = chest.GetComponent<ChestThread>();
            chestThread.blockPositionList.Add(new Vector2Int(xRest,blockPositionList[0].y));
            chestThread.InitChest(27, true);
            for (int i = 0; i < 27; i++)
            {
                chestThread.nameList[i] = nameList[i];
                chestThread.amountList[i] = amountList[i];
            }
            worldThread.chestList.Add(chestThread);
            worldThread.solidBlockList[y, x] = "Air";
            chestPrefab.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            worldThread.chestList.Remove(this);
            for (int i = 0; i < volume; i++)
            {
                if (!nameList[i].Equals("Air"))
                {
                    itemPrefab.SetActive(true);
                    float xBlock = blockPositionList[0].x + 0.5f;
                    float yBlock = blockPositionList[0].y + 0.5f;
                    GameObject item1 = Instantiate(itemPrefab,
                        new Vector3(xBlock, yBlock, 0),
                        Quaternion.identity, items.transform);
                    ItemThread itemThread1 = item1.gameObject.GetComponent<ItemThread>();
                    itemThread1.itemInit(nameList[i], amountList[i], 0);
                    if (IndexAll.BlockNameToIsLight(itemThread1.nameItem))
                    {
                        item1.GetComponentsInChildren<Transform>()[2].GetComponent<Light2D>().enabled = true;
                    }

                    itemPrefab.SetActive(false);
                }
            }

            Destroy(gameObject);
        }
    }
}
