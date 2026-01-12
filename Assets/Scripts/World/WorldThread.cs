using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq;
using Block;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Util;
using Random = UnityEngine.Random;
using Toggle = UnityEngine.UI.Toggle;

public class WorldThread : MonoBehaviour {
    public String worldName;
    public int width;
    public int height;
    public int seed;
    public float xWorldSpawn;
    public float yWorldSpawn;
    public float heightDeviationMin;
    public PlayerThread playerThread;
    public Transform backgroundTransform;
    public String[,] solidBlockList;
    public String[,] backBlockList;
    public String[,] liquidBlockList;
    public bool[,] noReachBlockList;
    public Tilemap solidBlockTileMap;
    public Tilemap noReachBlockTileMap;
    public Tilemap backBlockTileMap;
    public Tilemap liquidBlockTileMap;
    public Tilemap destroyTileMapFront;
    public Tilemap destroyTileMapBack;
    public String difficulty;
    public Dictionary<String, String[]> craftRecipeDictionary;
    public Dictionary<String, bool> craftRecipeNeedCraftingTableDictionary;
    public Dictionary<String, List<String>> craftInvolvedDictionary;
    public Dictionary<String, int> craftTargetAmount;
    public String currentSettingsPath;
    public String worldAttributesPath;
    public String worldPath;
    public TimeThread timeThread;
    public GameObject torchLight2DPrefab;
    public List<TorchLight2D> torchLight2DList;
    public GameObject lights;
    public List<FurnaceThread> furnaceList;
    public Transform furnacesTransform;
    public GameObject furnacePrefab;
    public List<Vector2Int> grassBlockList;
    public List<Vector2Int> saplingList;
    public List<ItemThread> itemList;
    public GameObject itemPrefab;
    public GameObject items;
    public List<ChestThread> chestList;
    public GameObject chestPrefab;
    public GameObject largeChestPrefab;
    public Transform chestsTransform;
    private bool _initiated;
    private float _timer;
    private int _countTime;
    void Awake()
    {
        _countTime = 0;
        _timer = 0;
        _initiated = false;
        itemList = new List<ItemThread>();
        grassBlockList = new List<Vector2Int>();
        furnaceList = new List<FurnaceThread>();
        chestList = new List<ChestThread>();
        torchLight2DList = new List<TorchLight2D>();
        currentSettingsPath = Application.persistentDataPath + "/currentSettings.csv";
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
                    if (cells[0].Equals("CurrentWorldName")) {
                        worldName = cells[1];
                        worldPath =  Application.persistentDataPath + "/Worlds/" + cells[1] + "/";
                    }
                }
            }
        }

        worldAttributesPath = worldPath + "attributes.csv";
        if (File.Exists(worldAttributesPath)) {
            // 使用 StreamReader 逐行读取文件内容
            using (StreamReader reader = new StreamReader(worldAttributesPath)) {
                // 逐行读取，直到文件末尾
                while (!reader.EndOfStream) {
                    // 读取一行数据
                    string line = reader.ReadLine();
                    // 分割行数据成单元格
                    string[] cells = line.Split(',');
                    // 遍历单元格
                    if (cells[0].Equals("worldName")) {
                        worldName = cells[1];
                    }else if (cells[0].Equals("time")) {
                        timeThread.timeNumber = float.Parse(cells[1]);
                    }else if (cells[0].Equals("width")) {
                        width = int.Parse(cells[1]);
                    }else if (cells[0].Equals("height")) {
                        height = int.Parse(cells[1]);
                    }else if (cells[0].Equals("xWorldSpawn")) {
                        xWorldSpawn = float.Parse(cells[1]);
                    }else if (cells[0].Equals("yWorldSpawn")) {
                        yWorldSpawn = float.Parse(cells[1]);
                    }else if (cells[0].Equals("seed")) {
                        seed = int.Parse(cells[1]);
                    }else if (cells[0].Equals("heightDeviationMin")) {
                        heightDeviationMin = float.Parse(cells[1]);
                    }
                }
            }
        }
        
        solidBlockList = LoadBlockList("solidBlockList.csv");
        backBlockList = LoadBlockList("backBlockList.csv");
        liquidBlockList = LoadBlockList("liquidBlockList.csv");
        noReachBlockList = LoadBoolBlockList("noReachBlockList.csv");
        
        String specialBlocksPath = Application.persistentDataPath + "/Worlds/" + worldName + "/specialBlocks.csv";
        if (File.Exists(specialBlocksPath))
        {
            string type = "null";
            // 使用 StreamReader 逐行读取文件内容
            using (StreamReader reader = new StreamReader(specialBlocksPath)) {
                // 逐行读取，直到文件末尾
                while (!reader.EndOfStream) {
                    // 读取一行数据
                    string line = reader.ReadLine();
                    // 分割行数据成单元格
                    string[] cells = line.Split(',');
                    if (cells[0].Equals("GrassBlock"))
                    {
                        type = "GrassBlock";
                        continue;
                    }else if (cells[0].Equals("Sapling"))
                    {
                        type = "Sapling";
                        continue;
                    }
                    // 遍历单元格
                    if (type.Equals("GrassBlock"))
                    {
                        grassBlockList.Add(new Vector2Int(int.Parse(cells[0]),int.Parse(cells[1])));
                    }else if (type.Equals("Sapling"))
                    {
                        saplingList.Add(new Vector2Int(int.Parse(cells[0]),int.Parse(cells[1])));
                    }
                }
            }
        }
        
        String entityListPath = Application.persistentDataPath + "/Worlds/" + worldName + "/entityList.csv";
        if (File.Exists(entityListPath))
        {
            string type = "null";
            // 使用 StreamReader 逐行读取文件内容
            using (StreamReader reader = new StreamReader(entityListPath)) {
                // 逐行读取，直到文件末尾
                while (!reader.EndOfStream) {
                    // 读取一行数据
                    string line = reader.ReadLine();
                    // 分割行数据成单元格
                    string[] cells = line.Split(',');
                    if (cells[0].Equals("Item"))
                    {
                        itemPrefab.SetActive(true);
                        Vector3 itemPosition = new Vector3(float.Parse(cells[3]), float.Parse(cells[4]), 0);
                        GameObject item = Instantiate(itemPrefab,itemPosition,Quaternion.identity, items.transform);
                        ItemThread itemThread = item.gameObject.GetComponent<ItemThread>();
                        itemThread.itemInit(cells[1],int.Parse(cells[2]),2);
                        itemPrefab.SetActive(false);
                    }
                }
            }
        }
        
        String containersPath = Application.persistentDataPath + "/Worlds/" + worldName + "/containers.csv";
        if (File.Exists(containersPath)) {
            // 使用 StreamReader 逐行读取文件内容
            using (StreamReader reader = new StreamReader(containersPath)) {
                // 逐行读取，直到文件末尾
                while (!reader.EndOfStream) {
                    // 读取一行数据
                    string line = reader.ReadLine();
                    // 分割行数据成单元格
                    string[] cells = line.Split(',');
                    // 遍历单元格
                    if (cells[0].Equals("Furnace"))
                    {
                        furnacePrefab.SetActive(true);
                        int xBlock = int.Parse(cells[2]);
                        int yBlock = int.Parse(cells[3]);
                        GameObject furnace = Instantiate(furnacePrefab, new Vector3(xBlock+0.5f,yBlock+0.5f,0), Quaternion.identity, furnacesTransform);
                        FurnaceThread furnaceThread = furnace.GetComponent<FurnaceThread>();
                        furnaceThread.onBurning = bool.Parse(cells[1]);
                        furnaceThread.xBlock = xBlock;
                        furnaceThread.yBlock = yBlock;
                        furnaceThread.material = cells[4];
                        furnaceThread.amountMaterial = int.Parse(cells[5]);
                        furnaceThread.fuel = cells[6];
                        furnaceThread.amountFuel = int.Parse(cells[7]);
                        furnaceThread.product = cells[8];
                        furnaceThread.amountProduct = int.Parse(cells[9]);
                        furnaceThread.timeTotal = float.Parse(cells[10]);
                        furnaceThread.timeLeft = float.Parse(cells[11]);
                        furnaceThread.progressTimer = float.Parse(cells[12]);
                        furnaceList.Add(furnaceThread);
                        furnacePrefab.SetActive(false);
                    }else if (cells[0].Equals("Chest"))
                    {
                        if (int.Parse(cells[1]) < 30)
                        {
                            Vector3 chestPosition = new Vector3(int.Parse(cells[2]) + 0.5f,
                                int.Parse(cells[3]), 3.5f);
                            chestPrefab.SetActive(true);
                            GameObject chest = Instantiate(chestPrefab, chestPosition, Quaternion.identity, chestsTransform);
                            ChestThread chestThread = chest.GetComponent<ChestThread>();
                            chestThread.blockPositionList.Add(new Vector2Int(int.Parse(cells[2]),int.Parse(cells[3])));
                            chestThread.InitChest(27, true);
                            for (int i = 0; i < 27; i++)
                            {
                                chestThread.nameList[i] = cells[6 + 2*i];
                                chestThread.amountList[i] = int.Parse(cells[7 + 2*i]);
                            }
                            noReachBlockList[int.Parse(cells[3]), int.Parse(cells[2])] = true;
                            solidBlockList[int.Parse(cells[3]), int.Parse(cells[2])] = "Chest";
                            chestPrefab.SetActive(false);
                            chestList.Add(chestThread);
                        }
                        else
                        {
                            Vector3 chestPosition = new Vector3((int.Parse(cells[2]) + int.Parse(cells[4]))/2.0f + 0.5f,
                                int.Parse(cells[3]), 3.5f);
                            largeChestPrefab.SetActive(true);
                            GameObject chest = Instantiate(largeChestPrefab, chestPosition,
                                Quaternion.identity, chestsTransform);
                            ChestThread chestThread = chest.GetComponent<ChestThread>();
                            chestThread.blockPositionList.Add(new Vector2Int(int.Parse(cells[2]),int.Parse(cells[3])));
                            chestThread.blockPositionList.Add(new Vector2Int(int.Parse(cells[4]),int.Parse(cells[5])));
                            chestThread.InitChest(54, true);
                            for (int i = 0; i < 27; i++)
                            {
                                chestThread.nameList[i] = cells[6 + 2*i];
                                chestThread.amountList[i] = int.Parse(cells[7 + 2*i]);
                            }
                            noReachBlockList[int.Parse(cells[3]), int.Parse(cells[2])] = true;
                            noReachBlockList[int.Parse(cells[5]), int.Parse(cells[4])] = true;
                            solidBlockList[int.Parse(cells[3]), int.Parse(cells[2])] = "Chest";
                            solidBlockList[int.Parse(cells[5]), int.Parse(cells[4])] = "Chest";
                            chestList.Add(chestThread);
                            largeChestPrefab.SetActive(false);
                        }
                    }
                }
            }
        }
        
        difficulty = "easy";
        craftRecipeNeedCraftingTableDictionary = new Dictionary<string, bool>();
        craftRecipeDictionary = new Dictionary<string, string[]>();
        craftInvolvedDictionary = new Dictionary<string, List<String>>();
        craftTargetAmount = new Dictionary<string, int>();
        TextAsset craftRecipesTextAsset = Resources.Load<TextAsset>("Recipes/craftRecipes");
        string[] rows = craftRecipesTextAsset.text.TrimEnd().Split("\n");
        for (int i = 0; i < rows.Length; i++) {
            string[] cols = rows[i].TrimEnd().Split(',');
            if (cols[0].Equals("Target")) {
                String targetName = cols[1];
                craftTargetAmount.Add(cols[1], int.Parse(cols[2]));
                String[] recipe = new String[9];
                {
                    int count = 0;
                    for (int j = 3; j >= 1; j--) {
                        string[] colsTmp = rows[i+j].TrimEnd().Split(',');
                        for (int k = 0; k < 3; k++) {
                            recipe[count] = colsTmp[k];
                            count++;
                        }
                    }
                }
                craftRecipeDictionary.Add(targetName,recipe);
                bool needCraftingTable = false;
                if (!recipe[2].Equals("Air")) needCraftingTable = true;
                if (!recipe[5].Equals("Air")) needCraftingTable = true;
                if (!recipe[6].Equals("Air")) needCraftingTable = true;
                if (!recipe[7].Equals("Air")) needCraftingTable = true;
                if (!recipe[8].Equals("Air")) needCraftingTable = true;
                craftRecipeNeedCraftingTableDictionary.Add(targetName, needCraftingTable);
                foreach (var ingredient in recipe) {
                    if(ingredient.Equals("Air")) continue;
                    if (craftInvolvedDictionary.ContainsKey(ingredient)) {
                        if (!craftInvolvedDictionary[ingredient].Contains(targetName)) {
                            craftInvolvedDictionary[ingredient].Add(targetName);
                        }
                    } else {
                        craftInvolvedDictionary.Add(ingredient, new List<string>());
                        craftInvolvedDictionary[ingredient].Add(targetName);
                    }
                }
            }
        }
        backgroundTransform.position = new Vector3(xWorldSpawn,
            heightDeviationMin + 13, backgroundTransform.transform.position.z);
        // Transform playerTransform = playerThread.transform;
        // playerTransform.position = new Vector3(xWorldSpawn, yWorldSpawn, playerThread.transform.position.z);
        // Camera.main.transform.position = new Vector3(xWorldSpawn, yWorldSpawn, playerThread.transform.position.z);
        // width = 1001;
        // height = 301;
        // seed = Random.Range(0, 100000000);
        // solidBlockList = GenerateWorld(width, height, seed);
        // backBlockList = new string[height, width];
        // for (int y = 0; y < height; y++) {
        //     for (int x = 0; x < width; x++) {
        //         if (solidBlockList[y, x].Equals("Air")) {
        //             backBlockList[y, x] = "Air";
        //         }
        //         if(y >= heightDeviationList[x]) continue;
        //         if (y >= dirtDepthList[x]) {
        //             if (solidBlockList[y, x].Equals("GrassBlock")) {
        //                 backBlockList[y, x] = "Dirt";
        //             }
        //             else if (solidBlockList[y, x].Equals("Sand")) {
        //                 backBlockList[y, x] = "Sand";
        //             }
        //             else if (solidBlockList[y, x].Equals("Dirt")) {
        //                 backBlockList[y, x] = "Dirt";
        //             }
        //             else if (solidBlockList[y, x].Equals("Air")) {
        //                 backBlockList[y, x] = "Dirt";
        //             }
        //         } else if (solidBlockList[y, x].Equals("Bedrock")) {
        //             backBlockList[y, x] = "Bedrock";
        //         } else {
        //             backBlockList[y, x] = "Stone";
        //         }
        //     }
        // }
        // liquidBlockList = new string[height, width];
        // for (int y = 0; y < height; y++) {
        //     for (int x = 0; x < width; x++) {
        //         liquidBlockList[y, x] = "Air";
        //     }
        // }
        // noReachBlockList = new bool[height, width];
        // for (int y = 0; y < height; y++) {
        //     for (int x = 0; x < width; x++) {
        //         if (solidBlockList[y, x].Equals("LogOak") || solidBlockList[y, x].Equals("Leaves")) {
        //             noReachBlockList[y, x] = true;
        //         }
        //     }
        // }
    }

    private void Start()
    {
        InitLightSource();
        InitContainer();
        InitItem();
        InitBlockTileMap();
        _initiated = true;
    }

    private void Update()
    {
        if(_initiated)
        {
            if(_timer >= 0.1f)
            {
                UpdateGrassSpreadVision();
                UpdateSaplingGrowVision();
                _timer = 0;
                _countTime++;
            }
            else
            {
                _timer += Time.deltaTime;
            }

            if (_countTime >= 10)
            {
                UpdateSaplingGrowWorld();
                UpdateGrassSpreadWorld();
                _countTime = 0;
            }
        }
    }

    void InitItem()
    {
        foreach (var item in itemList)
        {
            if (IndexAll.BlockNameToIsLight(item.nameItem)) {
                item.GetComponentsInChildren<Transform>()[2].GetComponent<Light2D>().enabled = true;
            }
        }
    }
    
    public void UpdateSaplingGrowWorld()
    {
        List<Vector2Int> saplingListTmp = new List<Vector2Int>(saplingList);
        foreach (var sapling in saplingListTmp)
        {
            int i = sapling.x;
            int j = sapling.y;
            if(i >= 4 && i <= width-4 && j <= height-5)
            {
                bool canGrow = true;
                for (int trav2 = j+1; trav2 <= j+2; trav2++)
                {
                    for (int trav1 = i-1; trav1 < i+1; trav1++)
                    {
                        if (!solidBlockList[trav2, trav1].Equals("Air"))
                            canGrow = false;
                    }
                }
                for (int trav1 = i-2; trav1 <= i+2; trav1++)
                {
                    if (!solidBlockList[j+3, trav1].Equals("Air") && !solidBlockList[j+3, trav1].Contains("Leaves"))
                        canGrow = false;
                    if (!solidBlockList[j+4, trav1].Equals("Air") && !solidBlockList[j+4, trav1].Contains("Leaves"))
                        canGrow = false;
                }
                if (!liquidBlockList[j, i].Equals("Air")) canGrow = false;
                if(canGrow){
                    float randomNumber = Random.Range(0, 100);
                    if (randomNumber < 0.0000001f)
                    {
                        noReachBlockList[j, i] = true;
                        noReachBlockList[j+1, i] = true;
                        noReachBlockList[j+2, i] = true;
                        noReachBlockList[j+3, i-2] = true;
                        noReachBlockList[j+3, i-1] = true;
                        noReachBlockList[j+3, i] = true;
                        noReachBlockList[j+3, i+1] = true;
                        noReachBlockList[j+3, i+2] = true;
                        noReachBlockList[j+4, i-1] = true;
                        noReachBlockList[j+4, i] = true;
                        noReachBlockList[j+4, i+1] = true;
                        SetBlock(solidBlockTileMap,i,j,"LogOak");
                        SetBlock(solidBlockTileMap,i,j+1,"LogOak");
                        SetBlock(solidBlockTileMap,i,j+2,"LogOak");
                        SetBlock(solidBlockTileMap,i-2,j+3,"Leaves");
                        SetBlock(solidBlockTileMap,i-1,j+3,"Leaves");
                        SetBlock(solidBlockTileMap,i,j+3,"Leaves");
                        SetBlock(solidBlockTileMap,i+1,j+3,"Leaves");
                        SetBlock(solidBlockTileMap,i+2,j+3,"Leaves");
                        SetBlock(solidBlockTileMap,i-1,j+4,"Leaves");
                        SetBlock(solidBlockTileMap,i,j+4,"Leaves");
                        SetBlock(solidBlockTileMap,i+1,j+4,"Leaves");
                        solidBlockList[j, i] = "LogOak";
                        solidBlockList[j+1, i] = "LogOak";
                        solidBlockList[j+2, i] = "LogOak";
                        solidBlockList[j+3, i-2] = "Leaves";
                        solidBlockList[j+3, i-1] = "Leaves";
                        solidBlockList[j+3, i] = "Leaves";
                        solidBlockList[j+3, i+1] = "Leaves";
                        solidBlockList[j+3, i+2] = "Leaves";
                        solidBlockList[j+4, i-1] = "Leaves";
                        solidBlockList[j+4, i] = "Leaves";
                        solidBlockList[j+4, i+1] = "Leaves";
                        saplingList.Remove(sapling);
                    }
                }
            }
        }
    }
    
    public void UpdateSaplingGrowVision()
    {
        Vector3 playerPosition = solidBlockTileMap.WorldToCell(playerThread.transform.position);
        int xLeftVision = (int)(playerPosition.x - playerThread.vision.x);
        int xRightVision = (int)(playerPosition.x + playerThread.vision.x);
        int yDownVision = (int)(playerPosition.y - playerThread.vision.y);
        int yUpVision = (int)(playerPosition.y + playerThread.vision.y);
        if (xLeftVision < 0)
            xLeftVision = 0;
        if (xRightVision >= width)
            xRightVision = width - 1;
        if (yDownVision < 0)
            yDownVision = 0;
        if (yUpVision >= height)
            yUpVision = height - 1;
        
        List<Vector2Int> saplingListTmp = new List<Vector2Int>(saplingList);
        foreach (var sapling in saplingListTmp)
        {
            int i = sapling.x;
            int j = sapling.y;
            if(i < xLeftVision || i > xRightVision || j < yDownVision || j > yUpVision)
                continue;
            if(i >= 4 && i <= width-4 && j <= height-5)
            {
                bool canGrow = true;
                for (int trav2 = j+1; trav2 <= j+2; trav2++)
                {
                    for (int trav1 = i-1; trav1 < i+1; trav1++)
                    {
                        if (!solidBlockList[trav2, trav1].Equals("Air"))
                            canGrow = false;
                    }
                }
                for (int trav1 = i-2; trav1 <= i+2; trav1++)
                {
                    if (!solidBlockList[j+3, trav1].Equals("Air") && !solidBlockList[j+3, trav1].Contains("Leaves"))
                        canGrow = false;
                    if (!solidBlockList[j+4, trav1].Equals("Air") && !solidBlockList[j+4, trav1].Contains("Leaves"))
                        canGrow = false;
                }
                if (!liquidBlockList[j, i].Equals("Air")) canGrow = false;
                if(canGrow){
                    float randomNumber = Random.Range(0, 100);
                    if (randomNumber < 0.0000001f)
                    {
                        noReachBlockList[j, i] = true;
                        noReachBlockList[j+1, i] = true;
                        noReachBlockList[j+2, i] = true;
                        noReachBlockList[j+3, i-2] = true;
                        noReachBlockList[j+3, i-1] = true;
                        noReachBlockList[j+3, i] = true;
                        noReachBlockList[j+3, i+1] = true;
                        noReachBlockList[j+3, i+2] = true;
                        noReachBlockList[j+4, i-1] = true;
                        noReachBlockList[j+4, i] = true;
                        noReachBlockList[j+4, i+1] = true;
                        SetBlock(solidBlockTileMap,i,j,"LogOak");
                        SetBlock(solidBlockTileMap,i,j+1,"LogOak");
                        SetBlock(solidBlockTileMap,i,j+2,"LogOak");
                        SetBlock(solidBlockTileMap,i-2,j+3,"Leaves");
                        SetBlock(solidBlockTileMap,i-1,j+3,"Leaves");
                        SetBlock(solidBlockTileMap,i,j+3,"Leaves");
                        SetBlock(solidBlockTileMap,i+1,j+3,"Leaves");
                        SetBlock(solidBlockTileMap,i+2,j+3,"Leaves");
                        SetBlock(solidBlockTileMap,i-1,j+4,"Leaves");
                        SetBlock(solidBlockTileMap,i,j+4,"Leaves");
                        SetBlock(solidBlockTileMap,i+1,j+4,"Leaves");
                        solidBlockList[j, i] = "LogOak";
                        solidBlockList[j+1, i] = "LogOak";
                        solidBlockList[j+2, i] = "LogOak";
                        solidBlockList[j+3, i-2] = "Leaves";
                        solidBlockList[j+3, i-1] = "Leaves";
                        solidBlockList[j+3, i] = "Leaves";
                        solidBlockList[j+3, i+1] = "Leaves";
                        solidBlockList[j+3, i+2] = "Leaves";
                        solidBlockList[j+4, i-1] = "Leaves";
                        solidBlockList[j+4, i] = "Leaves";
                        solidBlockList[j+4, i+1] = "Leaves";
                        saplingList.Remove(sapling);
                    }
                }
            }
        }
    }
    
    public void UpdateGrassSpreadWorld()
    {
        List<Vector2Int> grassBlockListTmp = new List<Vector2Int>(grassBlockList);
        foreach (var grassBlock in grassBlockListTmp)
        {
            int i = grassBlock.x;
            int j = grassBlock.y;
            bool canDisappear = false;
            if (j + 1 <= height - 1 && solidBlockList[j, i].Equals("GrassBlock") && !IndexAll.NameToIsTransparent(solidBlockList[j + 1, i])) {
                if (solidBlockList[j + 1, i].Contains("Oak"))
                {
                    if (!noReachBlockList[j + 1, i]) canDisappear = true;
                }    
                else canDisappear = true;
            }
            if (canDisappear) {
                float randomNumber = Random.Range(0, 100);
                if (randomNumber < 1)
                {
                    solidBlockList[j, i] = "Dirt";
                    SetBlock(solidBlockTileMap, i, j, "Dirt");
                    grassBlockList.Remove(grassBlock);
                }
            }
        }

        grassBlockListTmp = new List<Vector2Int>(grassBlockList);
        foreach (var grassBlock in grassBlockListTmp)
        {
            int i = grassBlock.x;
            int j = grassBlock.y;
            for (int k = -1; k <= 1; k++) {
                if (j + k < 0 || j + k > height - 1)
                    continue;
                if (i - 1 >= 0 && solidBlockList[j + k, i - 1].Equals("Dirt"))
                {
                    if ((j + k + 1 <= height-1 && IndexAll.NameToIsTransparent(solidBlockList[j + k + 1, i - 1])) || j + k == height-1)
                    {
                        float randomNumber = Random.Range(0, 100);
                        if (randomNumber < 1)
                        {
                            solidBlockList[j + k, i - 1] = "GrassBlock";
                            SetBlock(solidBlockTileMap, i - 1,j + k, "GrassBlock");
                            grassBlockList.Add(new Vector2Int(i - 1, j + k));
                        }
                    }
                } else if (i + 1 <= width-1 && solidBlockList[j + k, i + 1].Equals("Dirt"))
                {
                    if ((j + k + 1 <= height-1 && IndexAll.NameToIsTransparent(solidBlockList[j + k + 1, i + 1])) || j + k == height-1)
                    {
                        float randomNumber = Random.Range(0, 100);
                        if (randomNumber < 1)
                        {
                            solidBlockList[j + k, i + 1] = "GrassBlock";
                            SetBlock(solidBlockTileMap, i + 1,j + k, "GrassBlock");
                            grassBlockList.Add(new Vector2Int(i + 1, j + k));
                        }
                    }
                }
            }
        }
    }
    
    public void UpdateGrassSpreadVision()
    {
        Vector3 playerPosition = solidBlockTileMap.WorldToCell(playerThread.transform.position);
        int xLeftVision = (int)(playerPosition.x - playerThread.vision.x);
        int xRightVision = (int)(playerPosition.x + playerThread.vision.x);
        int yDownVision = (int)(playerPosition.y - playerThread.vision.y);
        int yUpVision = (int)(playerPosition.y + playerThread.vision.y);
        if (xLeftVision < 0)
            xLeftVision = 0;
        if (xRightVision >= width)
            xRightVision = width - 1;
        if (yDownVision < 0)
            yDownVision = 0;
        if (yUpVision >= height)
            yUpVision = height - 1;
        
        List<Vector2Int> grassBlockListTmp = new List<Vector2Int>(grassBlockList);
        foreach (var grassBlock in grassBlockListTmp)
        {
            int i = grassBlock.x;
            int j = grassBlock.y;
            if(i < xLeftVision || i > xRightVision || j < yDownVision || j > yUpVision)
                continue;
            bool canDisappear = false;
            if (j + 1 <= height - 1 && solidBlockList[j, i].Equals("GrassBlock") && !IndexAll.NameToIsTransparent(solidBlockList[j + 1, i])) {
                if (solidBlockList[j + 1, i].Contains("Oak"))
                {
                    if (!noReachBlockList[j + 1, i]) canDisappear = true;
                }    
                else canDisappear = true;
            }
            if (canDisappear) {
                float randomNumber = Random.Range(0, 100);
                if (randomNumber < 1)
                {
                    solidBlockList[j, i] = "Dirt";
                    SetBlock(solidBlockTileMap, i, j, "Dirt");
                    grassBlockList.Remove(grassBlock);
                }
            }
        }

        grassBlockListTmp = new List<Vector2Int>(grassBlockList);
        foreach (var grassBlock in grassBlockListTmp)
        {
            int i = grassBlock.x;
            int j = grassBlock.y;
            if (i < xLeftVision || i > xRightVision || j < yDownVision || j > yUpVision)
                continue;
            for (int k = -1; k <= 1; k++) {
                if (j + k < 0 || j + k > height - 1)
                    continue;
                if (i - 1 >= 0 && solidBlockList[j + k, i - 1].Equals("Dirt"))
                {
                    if ((j + k + 1 <= height-1 && IndexAll.NameToIsTransparent(solidBlockList[j + k + 1, i - 1])) || j + k == height-1)
                    {
                        float randomNumber = Random.Range(0, 100);
                        if (randomNumber < 1)
                        {
                            solidBlockList[j + k, i - 1] = "GrassBlock";
                            SetBlock(solidBlockTileMap, i - 1,j + k, "GrassBlock");
                            grassBlockList.Add(new Vector2Int(i - 1, j + k));
                        }
                    }
                } else if (i + 1 <= width-1 && solidBlockList[j + k, i + 1].Equals("Dirt"))
                {
                    if ((j + k + 1 <= height-1 && IndexAll.NameToIsTransparent(solidBlockList[j + k + 1, i + 1])) || j + k == height-1)
                    {
                        float randomNumber = Random.Range(0, 100);
                        if (randomNumber < 1)
                        {
                            solidBlockList[j + k, i + 1] = "GrassBlock";
                            SetBlock(solidBlockTileMap, i + 1,j + k, "GrassBlock");
                            grassBlockList.Add(new Vector2Int(i + 1, j + k));
                        }
                    }
                }
            }
        }
    }
    
    void InitContainer()
    {
        foreach (var furnace in furnaceList)
        {
            solidBlockList[furnace.yBlock, furnace.xBlock] = "FurnaceOff";
            if (furnace.onBurning)
            {
                solidBlockList[furnace.yBlock, furnace.xBlock] = "FurnaceOn";
                furnace.light.SetActive(true);
            }
        }
    }

    private void InitLightSource() {
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++) {
            Vector3 tmpPosition = new Vector3(x+0.5f, y+0.5f, 0);
            if (IndexAll.BlockNameToIsLight(solidBlockList[y,x])) {
                GameObject torchLight2DObject = Instantiate(torchLight2DPrefab, tmpPosition, Quaternion.identity, lights.transform);
                torchLight2DObject.SetActive(true);
                TorchLight2D torchLight2DTmp = torchLight2DObject.GetComponent<TorchLight2D>();
                torchLight2DTmp.x = x;
                torchLight2DTmp.y = y;
                torchLight2DList.Add(torchLight2DTmp);
            }
        }
    }
    
    bool[,] LoadBoolBlockList(string fileName)
    {
        bool[,] blockList = new bool[height, width];
        String filePath = worldPath + fileName;
        // 检查文件是否存在
        if (File.Exists(filePath)) {
            // 读取文件的所有行
            string[] rows = File.ReadAllLines(filePath);
            // 遍历每一行
            for (int y = 0; y < height; y++)
            {
                // 分割行数据
                string[] blocks = rows[y].TrimEnd().Split(',');
                // 遍历每个单元格
                for (int x = 0; x < width; x++)
                {
                    // 解析字符串为布尔值
                    if (bool.TryParse(blocks[x], out bool blockValue))
                    {
                        blockList[y, x] = blockValue;
                    }
                    else
                    {
                        Debug.LogError("无法解析布尔值: " + blocks[x]);
                    }
                }
            }

            return blockList;
        }
        else {
            Debug.LogError("文件不存在: " + filePath);
            return null;
        }
    }

    
    String[,] LoadBlockList(string fileName)
    {
        String[,] blockList = new String[height, width];
        String filePath = worldPath + fileName;
        // 检查文件是否存在
        if (File.Exists(filePath)) {
            // 读取文件的所有行
            string[] rows = File.ReadAllLines(filePath);
            // 遍历每一行
            for (int y = 0; y < height; y++)
            {
                // 分割行数据
                string[] blocks = rows[y].TrimEnd().Split(',');
                // 遍历每个单元格
                for (int x = 0; x < width; x++)
                {
                    blockList[y, x] = blocks[x];
                }
            }
            return blockList;
        }
        else {
            Debug.LogError("文件不存在: " + filePath);
            return null;
        }
    }
    
    void InitBlockTileMap() {
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++) {
            SetBlock(solidBlockTileMap, x, y, solidBlockList[y, x]);
            SetBlock(backBlockTileMap, x, y, backBlockList[y, x]);
            SetBlock(liquidBlockTileMap, x, y, liquidBlockList[y, x]);
        }
    }

    public void SetTile(Tilemap tilemap, int x, int y, String name) {
        Vector3Int tilePosition = new Vector3Int(x, y, 0);
        if (name != null && !name.Contains("Air")) {
            TileBase tile = Resources.Load<TileBase>("Tiles/" + name);
            if (tilemap == solidBlockTileMap) {
                if(noReachBlockList[y, x]) {
                    noReachBlockTileMap.SetTile(tilePosition, tile);
                }
                else {
                    solidBlockTileMap.SetTile(tilePosition, tile);
                }
            }
            else tilemap.SetTile(tilePosition, tile);
        } else {
            if (tilemap == solidBlockTileMap) {
                if(noReachBlockList[y, x]) noReachBlockTileMap.SetTile(tilePosition, null);
                else solidBlockTileMap.SetTile(tilePosition, null);
            }else tilemap.SetTile(tilePosition, null);
        }
    }

    public void SetBlock(Tilemap tilemap, int x, int y, String name) {
        SetTile(tilemap, x, y, "Blocks/" + name);
    }
    
    public void SetGUI(Tilemap tilemap, int x, int y, String name) {
        SetTile(tilemap, x, y, "GUI/" + name);
    }
}