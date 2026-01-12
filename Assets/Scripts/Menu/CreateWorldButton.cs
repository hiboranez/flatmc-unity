using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Random = UnityEngine.Random;

public class CreateWorldButton : MonoBehaviour
{
    public AudioClip clickAudioClip;
    public AudioSource audioSource;
    public GameObject createWorldUI;
    public SinglePlayerWorldContent singlePlayerWorldContent;
    public TMP_InputField worldNameTMPText;
    public TMP_InputField seedTMPText;
    public String[,] blockIdList;
    public int[] heightDeviationList;
    public double[] dirtDepthList;
    public String[,] solidBlockList;
    public String[,] backBlockList;
    public String[,] liquidBlockList;
    public bool[,] noReachBlockList;
    public int width;
    public int height;
    public String currentSettingsPath;
    public String playerName;
    public TMP_Text worldNameWarningText;
    public float WorldNameNullWarningShowTimer;
    public List<Vector2Int> grassBlockList;
        
    private void Awake()
    {
        grassBlockList = new List<Vector2Int>();
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
        width = 1001;
        height = 301;
        currentSettingsPath = Application.persistentDataPath + "/currentSettings.csv";
    }

    private void OnEnable() {
        worldNameWarningText.enabled = false;
    }

    public IEnumerator WorldNameNullWarning() {
        WorldNameNullWarningShowTimer = 1.5f;
        worldNameWarningText.enabled = true;
        while (WorldNameNullWarningShowTimer > 0)
        {
            // 等待一段时间，例如0.1秒
            yield return new WaitForSeconds(Time.deltaTime);
            // 逐步减小flashTimer的值
            WorldNameNullWarningShowTimer -= Time.deltaTime;
        }
        worldNameWarningText.enabled = false;
    }
        
    private void OnClickCallBack() {
        audioSource.PlayOneShot(clickAudioClip, 1f);
        if (worldNameTMPText.text == "") {
            StartCoroutine(WorldNameNullWarning());
        }
        else{
            if (File.Exists(currentSettingsPath)) {
                // 使用 StreamReader 逐行读取文件内容
                using (StreamReader reader = new StreamReader(currentSettingsPath)) {
                    // 逐行读取，直到文件末尾
                    while (!reader.EndOfStream) {
                        // 读取一行数据
                        string line = reader.ReadLine();
                        // 分割行数据成单元格
                        string[] cells = line.Split(',');
                        // 遍历单元格
                        if (cells[0].Equals("PlayerName")) {
                            playerName = cells[1];
                        }
                    }
                }
            }
            int seed = Random.Range(0, 100000000);
            if (int.TryParse(seedTMPText.text, out int intValue)) {
                seed = int.Parse(seedTMPText.text);
            }

            solidBlockList = GenerateWorld(width, height, seed);
            backBlockList = new string[height, width];
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    backBlockList[y, x] = "Air";
                    if (solidBlockList[y, x].Contains("Water")) {
                        backBlockList[y, x] = "SandWall";
                    }
                    if (y >= heightDeviationList[x]) continue;
                    if (y >= dirtDepthList[x]) {
                        if (solidBlockList[y, x].Equals("GrassBlock")) {
                            backBlockList[y, x] = "DirtWall";
                        } else if (solidBlockList[y, x].Equals("Sand")) {
                            backBlockList[y, x] = "SandWall";
                        } else if (solidBlockList[y, x].Equals("Dirt")) {
                            backBlockList[y, x] = "DirtWall";
                        } else if (solidBlockList[y, x].Equals("Air")) {
                            backBlockList[y, x] = "DirtWall";
                        }
                    } else if (solidBlockList[y, x].Equals("Bedrock")) {
                        backBlockList[y, x] = "BedrockWall";
                    } else {
                        backBlockList[y, x] = "StoneWall";
                    }
                }
            }

            liquidBlockList = new string[height, width];
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (solidBlockList[y, x].Contains("Water")) {
                        liquidBlockList[y, x] = solidBlockList[y, x];
                    } else {
                        liquidBlockList[y, x] = "Air";
                    }
                }
            }

            noReachBlockList = new bool[height, width];
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (solidBlockList[y, x].Equals("LogOak") || solidBlockList[y, x].Equals("Leaves")) {
                        noReachBlockList[y, x] = true;
                    }
                }
            }

            String solidBlockListPath = Application.persistentDataPath + "/Worlds/" + worldNameTMPText.text +
                                        "/solidBlockList.csv";
            String backBlockListPath = Application.persistentDataPath + "/Worlds/" + worldNameTMPText.text +
                                       "/backBlockList.csv";
            String liquidBlockListPath = Application.persistentDataPath + "/Worlds/" + worldNameTMPText.text +
                                         "/liquidBlockList.csv";
            String noReachBlockListPath = Application.persistentDataPath + "/Worlds/" + worldNameTMPText.text +
                                          "/noReachBlockList.csv";

            using (StreamWriter sw = new StreamWriter(solidBlockListPath)) {
                // 遍历二维数组，写入每个单元格的数据
                for (int i = 0; i < solidBlockList.GetLength(0); i++) {
                    for (int j = 0; j < solidBlockList.GetLength(1); j++) {
                        // 在每个单元格数据之间添加逗号
                        if (solidBlockList[i, j].Contains("Water")) {
                            sw.Write("Air");
                        } else {
                            sw.Write(solidBlockList[i, j]);
                        }
                        // 在最后一个单元格后面不添加逗号，换行
                        if (j < solidBlockList.GetLength(1) - 1) {
                            sw.Write(",");
                        } else {
                            sw.Write("\n");
                        }
                    }
                }

                // 关闭 StreamWriter
                sw.Close();
            }

            using (StreamWriter sw = new StreamWriter(backBlockListPath)) {
                // 遍历二维数组，写入每个单元格的数据
                for (int i = 0; i < backBlockList.GetLength(0); i++) {
                    for (int j = 0; j < backBlockList.GetLength(1); j++) {
                        // 在每个单元格数据之间添加逗号
                        sw.Write(backBlockList[i, j]);
                        // 在最后一个单元格后面不添加逗号，换行
                        if (j < backBlockList.GetLength(1) - 1) {
                            sw.Write(",");
                        } else {
                            sw.Write("\n");
                        }
                    }
                }

                // 关闭 StreamWriter
                sw.Close();
            }

            using (StreamWriter sw = new StreamWriter(liquidBlockListPath)) {
                // 遍历二维数组，写入每个单元格的数据
                for (int i = 0; i < liquidBlockList.GetLength(0); i++) {
                    for (int j = 0; j < liquidBlockList.GetLength(1); j++) {
                        // 在每个单元格数据之间添加逗号
                        sw.Write(liquidBlockList[i, j]);
                        // 在最后一个单元格后面不添加逗号，换行
                        if (j < liquidBlockList.GetLength(1) - 1) {
                            sw.Write(",");
                        } else {
                            sw.Write("\n");
                        }
                    }
                }

                // 关闭 StreamWriter
                sw.Close();
            }

            using (StreamWriter sw = new StreamWriter(noReachBlockListPath)) {
                // 遍历二维数组，写入每个单元格的数据
                for (int i = 0; i < noReachBlockList.GetLength(0); i++) {
                    for (int j = 0; j < noReachBlockList.GetLength(1); j++) {
                        // 在每个单元格数据之间添加逗号
                        sw.Write(noReachBlockList[i, j]);
                        // 在最后一个单元格后面不添加逗号，换行
                        if (j < noReachBlockList.GetLength(1) - 1) {
                            sw.Write(",");
                        } else {
                            sw.Write("\n");
                        }
                    }
                }

                // 关闭 StreamWriter
                sw.Close();
            }
            
            String specialBlocksPath = Application.persistentDataPath + "/Worlds/" + worldNameTMPText.text +
                                          "/specialBlocks.csv";

            using (StreamWriter sw = new StreamWriter(specialBlocksPath)) {
                sw.WriteLine("GrassBlock");
                // 遍历二维数组，写入每个单元格的数据
                foreach (var grassBlock in grassBlockList)
                {
                    if (solidBlockList[grassBlock.y, grassBlock.x].Equals("GrassBlock")) {
                        sw.WriteLine(grassBlock.x + "," + grassBlock.y);
                }
            }
                // 关闭 StreamWriter
                sw.Close();
            }

            singlePlayerWorldContent.UpdateWorldContent();
            createWorldUI.SetActive(false);
        }
    }
    
    public String[,] GenerateWorld(int width, int height, int seed)
        {
            blockIdList = new String[height, width];
            heightDeviationList = new int[width];
            dirtDepthList = new double[width];
            int[] treeHeightDeviation = new int[width];
            PerlinNoiseGenerator generator = new PerlinNoiseGenerator(seed);

            double maxHeightDeviation = 6;
            int xWorldSpawn = width / 2;
            int yWorldSpawn = 0;

            List<int> heightDeviationCaveList = new List<int>();

            for (int x = 0; x < width; x++) {
                double noiseValue = generator.PerlinNoise(x * 0.005f + seed/100000000f);
                int heightDeviation = height / 2 + (int)Math.Round(noiseValue * maxHeightDeviation);
                heightDeviationList[x] = heightDeviation;
                heightDeviationCaveList.Add(heightDeviation);

                if (x == xWorldSpawn) yWorldSpawn = heightDeviation + 2;

                double dirtDepth = heightDeviation - 5 - Math.Abs(10 * generator.PerlinNoise(x * 0.005f));
                dirtDepthList[x] = dirtDepth;
                for (int y = 0; y < height; y++) {
                    String blockType;
                    if (y > heightDeviation) {
                        blockType = "Air";
                    } else if (y == heightDeviation) {
                        blockType = "GrassBlock";
                    } else if (y < heightDeviation && y >= dirtDepth) {
                        blockType = "Dirt";
                    } else {
                        blockType = "Stone";
                    }

                    blockIdList[y, x] = blockType;
                    if(blockType.Equals("GrassBlock"))
                        grassBlockList.Add(new Vector2Int(x,y));
                }

                int randomNumber1 = Random.Range(0, 5000) + 1;

                // 生成煤矿
                if (x == 0) {
                    for (int y = heightDeviation; y >= 0; y--) {
                        if (blockIdList[y, x] == "Stone" && randomNumber1 <= 100)
                            blockIdList[y, x] = "CoalOre";
                        randomNumber1 = Random.Range(0, 5000) + 1;
                    }
                } else {
                    for (int y = heightDeviation; y >= 1; y--) {
                        if (blockIdList[y, x] == "Stone") {
                            if ((blockIdList[y, x - 1] == "CoalOre" || blockIdList[y - 1, x - 1] == "CoalOre" ||
                                 blockIdList[y + 1, x - 1] == "CoalOre") && randomNumber1 <= 500) {
                                blockIdList[y, x] = "CoalOre";
                            } else if (blockIdList[y, x - 1] != "CoalOre" && randomNumber1 <= 100) {
                                blockIdList[y, x] = "CoalOre";
                            }
                        }

                        randomNumber1 = Random.Range(0, 5000) + 1;
                    }
                }

                // 生成铁矿
                int randomNumber2 = Random.Range(0, 5000) + 1;
                if (x == 0) {
                    for (int y = heightDeviation; y >= 0; y--) {
                        if (blockIdList[y, x] == "Stone" && randomNumber2 <= 75)
                            blockIdList[y, x] = "IronOre";
                        randomNumber2 = Random.Range(0, 5000) + 1;
                    }
                } else {
                    for (int y = heightDeviation; y >= 1; y--) {
                        if (blockIdList[y, x] == "Stone") {
                            if ((blockIdList[y, x - 1] == "IronOre" || blockIdList[y - 1, x - 1] == "IronOre" ||
                                 blockIdList[y + 1, x - 1] == "IronOre") && randomNumber2 <= 400) {
                                blockIdList[y, x] = "IronOre";
                            } else if (blockIdList[y, x - 1] != "IronOre" && randomNumber2 <= 75) {
                                blockIdList[y, x] = "IronOre";
                            }
                        }

                        randomNumber2 = Random.Range(0, 5000) + 1;
                    }
                }

                // 生成金矿
                int randomNumber3 = Random.Range(0, 5000) + 1;
                int goldLine = (int)(heightDeviation * 0.85f);
                if (goldLine >= height) goldLine = height - 2;

                if (x == 0) {
                    for (int y = goldLine; y >= 0; y--) {
                        if (blockIdList[y, x]== "Stone" && randomNumber3 <= 50)
                            blockIdList[y, x] = "GoldOre";
                        randomNumber3 = Random.Range(0, 5000) + 1;
                    }
                } else {
                    for (int y = goldLine; y >= 1; y--) {
                        if (blockIdList[y, x]== "Stone") {
                            if ((blockIdList[y, x - 1] == "GoldOre" || blockIdList[y - 1, x - 1] == "GoldOre" ||
                                 blockIdList[y + 1, x - 1] == "GoldOre") && randomNumber3 <= 300) {
                                blockIdList[y, x] = "GoldOre";
                            } else if (blockIdList[y, x - 1] != "GoldOre" && randomNumber3 <= 50) {
                                blockIdList[y, x] = "GoldOre";
                            }
                        }

                        randomNumber3 = Random.Range(0, 5000) + 1;
                    }
                }

                // 生成钻石矿
                int randomNumber4 = Random.Range(0, 5000) + 1;
                int diamondLine = (int)(heightDeviation * 0.7f);
                if (diamondLine >= height) diamondLine = height - 2;

                if (x == 0) {
                    for (int y = diamondLine; y >= 0; y--) {
                        if (blockIdList[y, x]== "Stone" && randomNumber4 <= 30)
                            blockIdList[y, x] = "DiamondOre";
                        randomNumber4 = Random.Range(0, 5000) + 1;
                    }
                } else {
                    for (int y = diamondLine; y >= 1; y--) {
                        if (blockIdList[y, x]== "Stone") {
                            if ((blockIdList[y, x - 1] == "DiamondOre" || blockIdList[y - 1, x - 1] == "DiamondOre" ||
                                 blockIdList[y + 1, x - 1] == "DiamondOre") && randomNumber4 <= 200) {
                                blockIdList[y, x] = "DiamondOre";
                            } else if (blockIdList[y, x - 1] != "DiamondOre" && randomNumber4 <= 30) {
                                blockIdList[y, x] = "DiamondOre";
                            }
                        }

                        randomNumber4 = Random.Range(0, 5000) + 1;
                    }
                }
            }

            int averageGrassLevel = (heightDeviationCaveList.Max() + heightDeviationCaveList.Min()) / 2;
            for (int k = 15; k < width - 16; k++) {
                int randomCanGenerateCave = Random.Range(0, 1000) + 1;
                
                if (randomCanGenerateCave < 20 && heightDeviationCaveList[k] != -1 &&
                    heightDeviationCaveList[k] >= averageGrassLevel) {
                    int x = k;
                    int y = heightDeviationCaveList[k] + 5;
                    heightDeviationCaveList[k] = -1;
                    heightDeviationCaveList[k - 1] = -1;
                    heightDeviationCaveList[k - 2] = -1;
                    heightDeviationCaveList[k - 3] = -1;
                    heightDeviationCaveList[k - 4] = -1;
                    heightDeviationCaveList[k - 5] = -1;
                    heightDeviationCaveList[k + 1] = -1;
                    heightDeviationCaveList[k + 2] = -1;
                    heightDeviationCaveList[k + 3] = -1;
                    heightDeviationCaveList[k + 4] = -1;
                    heightDeviationCaveList[k + 5] = -1;

                    int randomCaveDirection = Random.Range(0, 2);

                    while (y != -1) {
                        double noiseValue = generator.PerlinNoise(x * 0.005f);
                        int widthDelta = (int)(11 * (noiseValue - (int)noiseValue));

                        if (widthDelta < 7) widthDelta = 7;

                        for (int i = -widthDelta / 2; i < widthDelta - widthDelta / 2; i++) {
                            if (x + i < 0 || x + i > width - 1) continue;
                            blockIdList[y, x + i] = "Air";
                        }

                        int xDelta;

                        if (randomCaveDirection == 0) {
                            if (Random.Range(0, 100) + 1 <= 70)
                                xDelta = -1;
                            else xDelta = -2;
                        } else {
                            if (Random.Range(0, 100) + 1 <= 70)
                                xDelta = 1;
                            else xDelta = 2;
                        }

                        x += xDelta;
                        y--;
                        int heightLimit = height / 50;
                        if (heightLimit >= 1000) heightLimit = 1000;
                        if (Random.Range(0, 1000) < heightLimit) y = -1;
                    }
                }
                
                // 生成池塘
                int randomNumberPond = Random.Range(0, 12) + 4;
                if (randomCanGenerateCave > 100 && randomCanGenerateCave < 130 && heightDeviationCaveList[k] != -1 &&
                    heightDeviationCaveList[k] >= averageGrassLevel) {
                    for (int i = k-randomNumberPond; i <= k+randomNumberPond; i++) {
                        heightDeviationCaveList[i] = -1;
                    }
                    int randomNumberPond2 = Random.Range(0, 12) + 3;
                    if (randomNumberPond2 > randomNumberPond - 2) randomNumberPond2 = randomNumberPond - 2;
                    for (int x = k-randomNumberPond2; x <= k+randomNumberPond2; x++) {
                        double noiseValue = generator.PerlinNoise(x * 0.005f);
                        for (int y = heightDeviationList[x]; y > heightDeviationList[x] - noiseValue * 8 ; y--) {
                            blockIdList[y, x] = "Sand";
                        }
                    }
                    int randomNumberPond3 = Random.Range(0, 12) + 2;
                    if (randomNumberPond3 > randomNumberPond2 - 2) randomNumberPond3 = randomNumberPond2 - 2;
                    for (int x = k-randomNumberPond3; x <= k+randomNumberPond3; x++) {
                        double noiseValue = generator.PerlinNoise(x * 0.005f);
                        for (int y = heightDeviationList[x]; y > heightDeviationList[x] - noiseValue * 6; y--) {
                            blockIdList[y, x] = "WaterStill";
                        }
                    }
                }
            }
            
            for (int x = 0; x < width; x++) {
                double noiseValue = generator.PerlinNoise(x * 0.005f + seed/100000000f);
                int heightDeviation = height / 2 + (int)Math.Round(noiseValue * maxHeightDeviation);
                if (x >= 2 && x <= width - 3) {
                    if ((Math.Abs(noiseValue) >= 0 && Math.Abs(noiseValue) <= 0.3) ||
                        (Math.Abs(noiseValue) >= 0.5 && Math.Abs(noiseValue) <= 0.8)) {
                        bool canGenerateTree = true;
                        if (blockIdList[heightDeviation, x] == "Air") {
                            canGenerateTree = false;
                        }else if (blockIdList[heightDeviation, x] == "Sand") {
                            canGenerateTree = false;
                        }else if (blockIdList[heightDeviation, x] == "WaterStill") {
                            canGenerateTree = false;
                        }
                        for (int i = -2; i <= 2; i++) {
                            if (treeHeightDeviation[x + i] != 0) {
                                canGenerateTree = false;
                            }
                        }

                        if (canGenerateTree) {
                            treeHeightDeviation[x] = heightDeviation;
                        }
                    }
                }
            }

            for (int x = 0; x < width; x++) {
                if (treeHeightDeviation[x] != 0 && x >= 2 && x <= width - 3) {
                    for (int i = 1; i < 4; i++)
                        blockIdList[treeHeightDeviation[x] + i, x] = "LogOak";
                    blockIdList[treeHeightDeviation[x] + 4, x] = "Leaves";
                    blockIdList[treeHeightDeviation[x] + 5, x] = "Leaves";
                    blockIdList[treeHeightDeviation[x] + 4, x - 1] = "Leaves";
                    blockIdList[treeHeightDeviation[x] + 5, x - 1] = "Leaves";
                    blockIdList[treeHeightDeviation[x] + 4, x + 1] = "Leaves";
                    blockIdList[treeHeightDeviation[x] + 5, x + 1] = "Leaves";
                    blockIdList[treeHeightDeviation[x] + 4, x - 2] = "Leaves";
                    blockIdList[treeHeightDeviation[x] + 4, x + 2] = "Leaves";
                    heightDeviationCaveList[x] = -1;
                    if (x >= 1)
                        heightDeviationCaveList[x - 1] = -1;
                    if (x <= height - 2)
                        heightDeviationCaveList[x + 1] = -1;
                    if (x >= 2)
                        heightDeviationCaveList[x - 2] = -1;
                    if (x <= height - 3)
                        heightDeviationCaveList[x + 2] = -1;
                }
            }

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (x == 0 || x == width - 1) blockIdList[y, x] = "Bedrock";
                    else if (y == 0 || y == height - 1) blockIdList[y, x] = "Bedrock";
                }
            }
            String attributesPath = Application.persistentDataPath + "/Worlds/" + worldNameTMPText.text + "/attributes.csv";
            Directory.CreateDirectory(Application.persistentDataPath + "/Worlds/" + worldNameTMPText.text);
            using (StreamWriter sw = new StreamWriter(attributesPath)) {
                // 遍历二维数组，写入每个单元格的数据
                for (int i = 0; i <= 7; i++) {
                    if (i == 0) {
                        sw.Write("worldName," + worldNameTMPText.text + "\n");
                    } else if (i == 1) {
                        sw.Write("time," + 60000 + "\n");
                    } else if (i == 2) {
                        sw.Write("width," + width + "\n");
                    } 
                    else if (i == 3) {
                        sw.Write("height," + height + "\n");
                    }else if (i == 4) {
                        sw.Write("xWorldSpawn," + xWorldSpawn + "\n");
                    } 
                    else if (i == 5) {
                        sw.Write("yWorldSpawn," + yWorldSpawn + "\n");
                    }
                    else if (i == 6) {
                        sw.Write("seed," + seed + "\n");
                    }
                    else if (i == 7) {
                        sw.Write("heightDeviationMin," + heightDeviationList.Min() + "\n");
                    }
                }
                // 关闭 StreamWriter
                sw.Close();
            }
            String playerListPath = Application.persistentDataPath + "/Worlds/" + worldNameTMPText.text + "/playerList.csv"; 
            using (StreamWriter sw = new StreamWriter(playerListPath))
            {
                // 写入 CSV 文件的列名
                sw.WriteLine("Player" + "," + playerName + "," + xWorldSpawn +  "," + yWorldSpawn  +  ",20,20,null,0,null,0,null,0,null,0");
                sw.WriteLine("Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air,Air");
                sw.WriteLine("0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
                // 关闭 StreamWriter
                sw.Close();
            }
            return blockIdList;
        }
}
