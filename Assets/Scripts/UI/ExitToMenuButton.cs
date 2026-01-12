using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitToMenuButton : MonoBehaviour {
    public AudioClip clickAudioClip;
    public AudioSource audioSource;
    public WorldThread worldThread;
    public PlayerThread playerThread;
    public GameObject deathUI;
        
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }
        
    private void OnClickCallBack() {
        String solidBlockListPath = Application.persistentDataPath + "/Worlds/" + worldThread.worldName + "/solidBlockList.csv";
        String backBlockListPath = Application.persistentDataPath + "/Worlds/" + worldThread.worldName + "/backBlockList.csv";
        String liquidBlockListPath = Application.persistentDataPath + "/Worlds/" + worldThread.worldName + "/liquidBlockList.csv";
        String noReachBlockListPath = Application.persistentDataPath + "/Worlds/" + worldThread.worldName + "/noReachBlockList.csv";
        String playerListPath = Application.persistentDataPath + "/Worlds/" + worldThread.worldName + "/playerList.csv"; 
        String attributesPath = Application.persistentDataPath + "/Worlds/" + worldThread.worldName + "/attributes.csv";
        String containersPath = Application.persistentDataPath + "/Worlds/" + worldThread.worldName + "/containers.csv";
        String specialBlocksPath = Application.persistentDataPath + "/Worlds/" + worldThread.worldName + "/specialBlocks.csv";
        String entityListPath = Application.persistentDataPath + "/Worlds/" + worldThread.worldName + "/entityList.csv";

        if (playerThread.dead) {
            playerThread.transform.position =
                new Vector3(worldThread.xWorldSpawn, worldThread.yWorldSpawn, playerThread.transform.position.z);
            playerThread.health = 20;
            playerThread.hunger = 20;
            playerThread.playerRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            playerThread.skinnedMeshRenderer.enabled = true;
            playerThread.headSpriteRenderer.enabled = true;
            playerThread.playerNameText.enabled = true;
            playerThread.dead = false;
            deathUI.SetActive(false);
        }
        
        audioSource.PlayOneShot(clickAudioClip, 1f);
        string[] lines = File.ReadAllLines(attributesPath);
        for (int i = 0; i < lines.Length; i++) {
            // 更新行的内容
            if (i == 1) {
                lines[i] = "time," + worldThread.timeThread.timeNumber;
            }
        }
        File.WriteAllLines(attributesPath, lines);
        using (StreamWriter sw = new StreamWriter(solidBlockListPath)) {
            // 遍历二维数组，写入每个单元格的数据
            for (int i = 0; i < worldThread.solidBlockList.GetLength(0); i++) {
                for (int j = 0; j < worldThread.solidBlockList.GetLength(1); j++) {
                    // 在每个单元格数据之间添加逗号
                    sw.Write(worldThread.solidBlockList[i, j]);
                    // 在最后一个单元格后面不添加逗号，换行
                    if (j < worldThread.solidBlockList.GetLength(1) - 1) {
                        sw.Write(",");
                    }
                    else {
                        sw.Write("\n");
                    }
                }
            }
            // 关闭 StreamWriter
            sw.Close();
        }
        using (StreamWriter sw = new StreamWriter(backBlockListPath)) {
            // 遍历二维数组，写入每个单元格的数据
            for (int i = 0; i < worldThread.backBlockList.GetLength(0); i++) {
                for (int j = 0; j < worldThread.backBlockList.GetLength(1); j++) {
                    // 在每个单元格数据之间添加逗号
                    sw.Write(worldThread.backBlockList[i, j]);
                    // 在最后一个单元格后面不添加逗号，换行
                    if (j < worldThread.backBlockList.GetLength(1) - 1) {
                        sw.Write(",");
                    }
                    else {
                        sw.Write("\n");
                    }
                }
            }
            // 关闭 StreamWriter
            sw.Close();
        }
        using (StreamWriter sw = new StreamWriter(liquidBlockListPath)) {
            // 遍历二维数组，写入每个单元格的数据
            for (int i = 0; i < worldThread.liquidBlockList.GetLength(0); i++) {
                for (int j = 0; j < worldThread.liquidBlockList.GetLength(1); j++) {
                    // 在每个单元格数据之间添加逗号
                    sw.Write(worldThread.liquidBlockList[i, j]);
                    // 在最后一个单元格后面不添加逗号，换行
                    if (j < worldThread.liquidBlockList.GetLength(1) - 1) {
                        sw.Write(",");
                    }
                    else {
                        sw.Write("\n");
                    }
                }
            }
            // 关闭 StreamWriter
            sw.Close();
        }
        using (StreamWriter sw = new StreamWriter(noReachBlockListPath)) {
            // 遍历二维数组，写入每个单元格的数据
            for (int i = 0; i < worldThread.noReachBlockList.GetLength(0); i++) {
                for (int j = 0; j < worldThread.noReachBlockList.GetLength(1); j++) {
                    // 在每个单元格数据之间添加逗号
                    sw.Write(worldThread.noReachBlockList[i, j]);
                    // 在最后一个单元格后面不添加逗号，换行
                    if (j < worldThread.noReachBlockList.GetLength(1) - 1) {
                        sw.Write(",");
                    }
                    else {
                        sw.Write("\n");
                    }
                }
            }
            // 关闭 StreamWriter
            sw.Close();
        }
        using (StreamWriter sw = new StreamWriter(playerListPath)) {
            String inventoryName = "";
            String inventoryAmount = "";
            // 写入 CSV 文件的列名
            for (int i = 0; i < 36; i++) {
                inventoryName += playerThread.InventoryName[i];
                if (i != 35) inventoryName += ",";
            }
            for (int i = 0; i < 36; i++) {
                inventoryAmount += playerThread.InventoryAmount[i];
                if (i != 35) inventoryAmount += ",";
            }

            Vector3 playerPosition = playerThread.transform.position;
            sw.WriteLine("Player," + playerThread.playerName + "," + playerPosition.x + "," + playerPosition.y + "," + playerThread.health + "," + playerThread.hunger + "," + playerThread.armorHelmet + "," + playerThread.armorHelmetAmount + "," + playerThread.armorChest + "," + playerThread.armorChestAmount + "," + playerThread.armorLeggings + "," + playerThread.armorLeggingsAmount + "," + playerThread.armorBoots + "," + playerThread.armorBootsAmount);
            sw.WriteLine(inventoryName);
            sw.WriteLine(inventoryAmount);
            // 关闭 StreamWriter
            sw.Close();
        }
        
        using (StreamWriter sw = new StreamWriter(containersPath)) {
            foreach (var furnace in worldThread.furnaceList)
            {
                sw.WriteLine("Furnace," + furnace.onBurning + "," + furnace.xBlock + "," + furnace.yBlock + "," + furnace.material + "," + furnace.amountMaterial + "," + furnace.fuel + "," + furnace.amountFuel + "," + furnace.product + "," + furnace.amountProduct + "," + furnace.timeTotal + "," + furnace.timeLeft + "," + furnace.progressTimer);
            }
            foreach (var chest in worldThread.chestList)
            {
                if (chest.volume < 30) {
                    sw.Write("Chest," + chest.volume + "," + chest.blockPositionList[0].x + "," + chest.blockPositionList[0].y + ",null,null,");
                    for (int i = 0; i < chest.volume; i++)
                    {
                        sw.Write(chest.nameList[i] + ",");
                        sw.Write(chest.amountList[i] + ",");
                    }
                    sw.Write("\n");
                }else {
                    sw.Write("Chest," + chest.volume + "," + chest.blockPositionList[0].x + "," + chest.blockPositionList[0].y + "," + chest.blockPositionList[1].x + "," + chest.blockPositionList[1].y + ",");
                    for (int i = 0; i < chest.volume; i++)
                    {
                        sw.Write(chest.nameList[i] + ",");
                        sw.Write(chest.amountList[i] + ",");
                    }
                    sw.Write("\n");
                }
            }
            // 关闭 StreamWriter
            sw.Close();
        }
        
        using (StreamWriter sw = new StreamWriter(specialBlocksPath)) {
            sw.WriteLine("GrassBlock");
            for (int x = 0; x < worldThread.width; x++)
            {
                for (int y = 0; y < worldThread.height; y++)
                {
                    if (worldThread.solidBlockList[y, x].Equals("GrassBlock"))
                    {
                        sw.WriteLine(x + "," + y);
                    }
                }
            }
            sw.WriteLine("Sapling");
            for (int x = 0; x < worldThread.width; x++)
            {
                for (int y = 0; y < worldThread.height; y++)
                {
                    if (worldThread.solidBlockList[y, x].Contains("Sapling"))
                    {
                        sw.WriteLine(x + "," + y);
                    }
                }
            }
            sw.Close();
        }
        
        using (StreamWriter sw = new StreamWriter(entityListPath)) {
            foreach (var item in worldThread.itemList)
            {
                Vector3 itemPosition = item.gameObject.transform.position;
                sw.WriteLine("Item," + item.nameItem + "," + item.amount + "," + itemPosition.x + "," + itemPosition.y);
            }
            // 关闭 StreamWriter
            sw.Close();
        }
        
        SceneManager.LoadScene("MenuScene");
    }
}