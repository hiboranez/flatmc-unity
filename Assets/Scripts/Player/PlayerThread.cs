using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;
using Random = UnityEngine.Random;

public class PlayerThread : MonoBehaviour {
    public InputAction attackAction;
    public InputAction moveAction;
    public JoyStick joyStick;
    public HeadThread head;
    public SpriteRenderer headSpriteRenderer;
    public Animator animator;
    public Rigidbody2D playerRigidbody2D;
    public AudioClip audioClipHurt;
    public AudioClip audioClipPop;
    public AudioSource audioSource;
    public WorldThread worldThread;
    public ItemBar itemBar;
    public NameTextThread nameTextThread;
    public bool canRun1;
    public bool canRun2;
    public bool canRun3;
    public String[] InventoryName;
    public int[] InventoryAmount;
    public int ItemBarChosen;
    public int health;
    public int hunger;
    public String gamemode;
    public FlashHealthBar flashHealthBar;
    public FlashHungerBar flashHungerBar;
    public String playerName;
    public TMP_Text playerNameText;
    public RectTransform rectTransformPlayerName;
    public String moveState;
    public bool onCraftingTable;
    public GameObject playerMoveLight;
    public Toggle toggle;
    public bool autoJump;
    public String currentSettingsPath;
    public RectTransform playerNameCanvasRectTransform;
    public bool dead;
    public GameObject modelRoot;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public GameObject deathUI;
    public bool onGround;
    public AudioClip diveAudioClip;
    public String armorHelmet;
    public String armorChest;
    public String armorLeggings;
    public String armorBoots;
    public int armorHelmetAmount;
    public int armorChestAmount;
    public int armorLeggingsAmount;
    public int armorBootsAmount;
    public int armorValue;
    public ArmorBar armorBar;
    public int breathValue;
    public bool underWater;
    public float underWaterTimer;
    public float breathTimer;
    public bool breathDamageFirst;
    public BreathBar breathBar;
    public AudioClip toolBreakAudioClip;
    public ArmorContent armorContent;
    public Image amountBarHelmet;
    public Image amountBarBackHelmet;
    public Image amountBarChest;
    public Image amountBarBackChest;
    public Image amountBarLeggings;
    public Image amountBarBackLeggings;
    public Image amountBarBoots;
    public Image amountBarBackBoots;
    public AudioClip eatAudioClip1;
    public AudioClip eatAudioClip2;
    public AudioClip eatAudioClip3;
    public AudioClip eatFinishAudioClip;
    public CameraThread mainCameraThread;
    public Vector2 vision;
    private Vector2 _movementLast;
    private int _healthLast;
    private float _reviveTimer;
    private float _hungerTimer;
    private float _hungerToHurtTimer;
    public float velocityYLast;
    private float _moveSpeed;
    public  float jumpSpeed;
    private float _joyTimer;
    private float _keyTimer;
    private float _lastXJoy;
    private bool _onGroundLast;
    private bool _reachLeftUpWall;
    private bool _reachRightUpWall;
    private bool _reachLeftDownWall;
    private bool _reachRightDownWall;
    private bool _reachLeftUpHalfWall;
    private bool _reachRightUpHalfWall;
    private bool _reachLeftDownHalfWall;
    private bool _reachRightDownHalfWall;
    private Camera _mainCamera;
    public bool inWater;
    private float _inWaterTimer;
    
    public void UpdateAmountBarAmount(){
        if (armorHelmet.Equals("null")) {
            amountBarHelmet.enabled = false;
            amountBarBackHelmet.enabled = false;
        }
        else {
            amountBarHelmet.enabled = true;
            amountBarBackHelmet.enabled = true;
        }
        
        if (armorChest.Equals("null")) {
            amountBarChest.enabled = false;
            amountBarBackChest.enabled = false;
        }
        else {
            amountBarChest.enabled = true;
            amountBarBackChest.enabled = true;
        }
        
        if (armorLeggings.Equals("null")) {
            amountBarLeggings.enabled = false;
            amountBarBackLeggings.enabled = false;
        }
        else {
            amountBarLeggings.enabled = true;
            amountBarBackLeggings.enabled = true;
        }
        
        if (armorBoots.Equals("null")) {
            amountBarBoots.enabled = false;
            amountBarBackBoots.enabled = false;
        }
        else {
            amountBarBoots.enabled = true;
            amountBarBackBoots.enabled = true;
        }
    }
    
    void Awake() {
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
                    if (cells[0].Equals("PlayerName")) {
                        playerName = cells[1];
                    }
                }
            }
        }
        playerNameText.text = playerName;
        playerNameText.fontSize = 33f * playerNameCanvasRectTransform.localScale.x;
        _mainCamera = Camera.main;
        gamemode = "survival";
        moveState = "stand";
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        autoJump = true;
        _movementLast = new Vector2(0, 0);
        _moveSpeed = 3.6f;
        jumpSpeed = 12f;
        _joyTimer = 0;
        _keyTimer = 0;
        _lastXJoy = 0;
        canRun1 = false;
        canRun2 = false;
        InventoryName = new string[36];
        InventoryAmount = new int[36];
        ItemBarChosen = 0;
        health = 20;
        hunger = 20;
        onGround = false;
        _reachLeftUpWall = false;
        _reachRightUpWall = false;
        _reachLeftDownWall = false;
        _reachRightDownWall = false;
        breathValue = 20;
        underWater = false;
        underWaterTimer = 0;
        vision = new Vector2(100, 100);
        LoadInventoryNameList("playerList.csv");
    }

    public void UpdateArmorValue()
    {
        armorValue = IndexAll.nameToArmorValue(armorHelmet)
                     +IndexAll.nameToArmorValue(armorChest)
                     +IndexAll.nameToArmorValue(armorLeggings)
                     +IndexAll.nameToArmorValue(armorBoots);
        armorBar.UpdateArmorBar();
    }
    
    void LoadInventoryNameList(string fileName) {
        String inventoryPath = worldThread.worldPath + fileName;
        List<String> lineList = new List<string>();
        if (File.Exists(inventoryPath)) {
            // 使用 StreamReader 逐行读取文件内容
            using (StreamReader reader = new StreamReader(inventoryPath)) {
                // 逐行读取，直到文件末尾
                while (!reader.EndOfStream) {
                    // 读取一行数据
                    string line = reader.ReadLine();
                    lineList.Add(line);
                }

                for (int i = 0; i < lineList.Count; i++) {
                    // 分割行数据成单元格
                    string[] cells = lineList[i].Split(',');
                    if(cells[0].Equals("Player")) {
                        Vector2 playerPosition = new Vector2(float.Parse(cells[2]), float.Parse(cells[3]));
                        transform.position = playerPosition;
                        health = int.Parse(cells[4]);
                        hunger = int.Parse(cells[5]);
                        if (cells.Length > 6)
                        {
                            armorHelmet = cells[6]; 
                            armorHelmetAmount = int.Parse(cells[7]);
                            armorChest = cells[8];
                            armorChestAmount = int.Parse(cells[9]);
                            armorLeggings = cells[10];
                            armorLeggingsAmount = int.Parse(cells[11]);
                            armorBoots = cells[12];
                            armorBootsAmount = int.Parse(cells[13]);
                        }
                        else
                        {
                            armorHelmet = "null"; 
                            armorChest = "null";
                            armorLeggings = "null";
                            armorBoots = "null";
                            armorHelmetAmount = 0;
                            armorChestAmount = 0;
                            armorLeggingsAmount = 0;
                            armorBootsAmount = 0;
                        }
                        string[] cells1 = lineList[i+1].Split(',');
                        string[] cells2 = lineList[i+2].Split(',');
                        for (int j = 0; j < 36; j++) {
                            InventoryName[j] = cells1[j];
                            InventoryAmount[j] = int.Parse(cells2[j]);
                        }
                        Camera.main.transform.position = playerPosition;
                    }
                }
            }
        }
    }
    
    private void Update() {
        UpdateOnGround();
        UpdateAnimation();
        UpdateModel();
        UpdateHealth();
        UpdateHunger();
        UpdateNamePosition();
        UpdateMoveTorchLight();
        UpdateDeath();
        UpdateInBlock();
        UpdateInWater();
    }

    void UpdateInWater() {
        _inWaterTimer -= Time.deltaTime;
        if (_inWaterTimer < 0) _inWaterTimer = 0;
        Vector3Int blockPosition =
            worldThread.solidBlockTileMap.WorldToCell(transform.position);
        if (blockPosition.x >= 0 && blockPosition.x < worldThread.width && blockPosition.y >= 0 &&
            blockPosition.y < worldThread.height) {
            if(true){
                if (!worldThread.liquidBlockList[blockPosition.y, blockPosition.x].Equals("Air")) {
                    if (!inWater) {
                        _inWaterTimer = 0.5f;
                        audioSource.PlayOneShot(diveAudioClip,0.6f);
                    }
                    inWater = true;
                    if (playerRigidbody2D.velocity.y < -0.5f) {
                        playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, -0.5f);
                    }

                    if (_inWaterTimer == 0 && joyStick.yJoy > 0) {
                        playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, 3f);
                    }
                    else if (_inWaterTimer == 0 && joyStick.yJoy < 0) {
                        playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, -3f);
                    }
                } else {
                    if (inWater && joyStick.yJoy > 0) {
                        playerRigidbody2D.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
                    }
                    inWater = false;
                }
            }
        }
        Vector3Int headPosition =
            worldThread.solidBlockTileMap.WorldToCell(transform.position+new Vector3(0,1.8f,0));
        if (!worldThread.liquidBlockList[headPosition.y, headPosition.x].Equals("Air"))
        {
            underWater = true;
        }
        else
        {
            underWater = false;
        }

        if (underWater)
        {
            breathTimer = 0;
            underWaterTimer += Time.deltaTime;
            
            if (!breathDamageFirst && breathValue % 2 == 1)
            {
                breathValue--;
                underWaterTimer = 0;
            }
            else if (underWaterTimer > 2.5f && !breathDamageFirst)
            {
                if (breathValue >= 2)
                {
                    breathValue--;
                    breathDamageFirst = true;
                }
                else
                {
                    underWaterTimer = 0;
                    if (health >= 2)
                    {
                        Hurt(2,false);
                    }else if (health == 1)
                    {
                        Hurt(1,false);
                        health = 0;
                    }
                }
            }
            else if (underWaterTimer > 2.6f)
            {
                breathValue--;
                breathDamageFirst = false;
                underWaterTimer = 0;
            }
        }
        else
        {
            underWaterTimer = 0;
            breathDamageFirst = false;
            if (breathValue < 20)
            {
                breathTimer += Time.deltaTime;
                if (breathTimer > 0.2f)
                {
                    breathTimer = 0;
                    breathValue += 1;
                }
            }
        }
        breathBar.UpdateBreathBar();
    }

    void UpdateInBlock() {
        Vector3Int blockPosition =
            worldThread.solidBlockTileMap.WorldToCell(transform.position + new Vector3(0, 0.96f, 0));
        if (blockPosition.x >= 0 && blockPosition.x < worldThread.width && blockPosition.y >= 0 &&
            blockPosition.y < worldThread.height) {
            if(!worldThread.noReachBlockList[blockPosition.y, blockPosition.x]){
                if (!worldThread.solidBlockList[blockPosition.y, blockPosition.x].Equals("Air") && !worldThread.solidBlockList[blockPosition.y, blockPosition.x].Contains("Door")) {
                    playerRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                } else {
                    playerRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }
    }
    
    void UpdateDeath() {
        Vector3Int blockPosition = worldThread.solidBlockTileMap.WorldToCell(transform.position + new Vector3(0,0.96f,0));
        if ((!dead && health <= 0) || blockPosition.x < 0 || blockPosition.x >= worldThread.width || blockPosition.y < 0 || blockPosition.y >= worldThread.height) {
            dead = true;
            modelRoot.SetActive(false);
            skinnedMeshRenderer.enabled = false;
            headSpriteRenderer.enabled = false;
            playerNameText.enabled = false;
            playerRigidbody2D.bodyType = RigidbodyType2D.Static;
            deathUI.SetActive(true);
        }
    }
    
    private void UpdateMoveTorchLight() {
        if (IndexAll.BlockNameToIsLight(InventoryName[itemBar.itemBarButtonList[ItemBarChosen].InventorySort])) {
            playerMoveLight.SetActive(true);
        } else {
            playerMoveLight.SetActive(false);
        }
    }

    private void FixedUpdate() {
        UpdateMove();
    }

    public void OnEnable() {
        attackAction.Enable();
        moveAction.Enable();
    }

    public void OnDisable() {
        attackAction.Disable();
        moveAction.Disable();
    }

    void UpdateNamePosition() {
        Vector2 screenPoint = _mainCamera.WorldToScreenPoint(transform.position);
        float delta = Mathf.Pow(mainCameraThread.zoomRatio-1,2) * 0.1889f * Screen.height;
        rectTransformPlayerName.anchoredPosition = new Vector2(screenPoint.x, screenPoint.y + Screen.height * 0.1768f + delta);
    }

    public void Hurt(int damage, bool armorUsed)
    {
        _reviveTimer = 0;
        flashHealthBar.gameObject.SetActive(true);
        flashHealthBar.StartCoroutine(flashHealthBar.Flash());
        
        int damageFinal;
        if (armorUsed) damageFinal = damage - armorValue;
        else damageFinal = damage;
        
        if (damageFinal > 0)
        {
            if (damageFinal >= health) {
                health = 0; 
            }else {
                health -= damageFinal;
            }
        }
        audioSource.PlayOneShot(audioClipHurt, 1f);

        int armordamage = damage / 2;
        if (armordamage < 1) armordamage = 1;
        if (armorValue > 0 && armorUsed)
        {
            bool armorBroken = false;
            if (armorHelmet.Contains("Helmet"))
            {
                armorHelmetAmount -= armordamage;
                if (armorHelmetAmount <= 0)
                {
                    armorBroken = true;
                    armorHelmet = "null";
                }
            }
            if (armorChest.Contains("Chestplate"))
            {
                armorChestAmount -= armordamage;
                if (armorChestAmount <= 0)
                {
                    armorBroken = true;
                    armorChest = "null";
                }
            }
            if (armorLeggings.Contains("Leggings"))
            {
                armorLeggingsAmount -= armordamage;
                if (armorLeggingsAmount <= 0)
                {
                    armorBroken = true;
                    armorLeggings = "null";
                }
            }
            if (armorBoots.Contains("Boots"))
            {
                armorBootsAmount -= armordamage;
                if (armorBootsAmount <= 0)
                {
                    armorBroken = true;
                    armorBoots = "null";
                }
            }
            if (armorBroken)
            {
                armorContent.UpdateArmorModel();
                audioSource.PlayOneShot(toolBreakAudioClip, 1f);
                UpdateAmountBarAmount();
                UpdateArmorValue();
            }
        }
    }
    
    private void UpdateHealth() {
        if(!dead){
            if (_reviveTimer > 13f) {
                health += 1;
                if (health > 20) health = 20;
                flashHealthBar.gameObject.SetActive(true);
                flashHealthBar.StartCoroutine(flashHealthBar.Flash());
                _reviveTimer = 10f;
            }

            float velocity = playerRigidbody2D.velocity.y;
            if (velocityYLast < 0 && Math.Abs(velocity) < 0.01f) {
                if (health > 0 && velocityYLast < -20f)
                {
                    Hurt((int)-(velocityYLast + 20f), true);
                    // health += (int)(_velocityYLast + 20f);
                    if (health < 0) health = 0;
                }
            }

            velocityYLast = velocity;
            if (_healthLast >= 20 && health < 20f) {
                _reviveTimer = 0;
            }

            if (_healthLast < 20 && health >= 20f) {
                _reviveTimer = 0;
            }

            if (health < 20f) {
                _reviveTimer += Time.deltaTime;
            }

            _healthLast = health;
        }
    }
    
    private void UpdateHunger(){
        if(!dead){
            float deltaTime = Time.deltaTime;
            if (worldThread.difficulty.Equals("peaceful"))
            {
                if (hunger < 20)
                    _hungerTimer += deltaTime;
                else _hungerTimer = 0;
            } else if (worldThread.difficulty.Equals("easy")) {
                if (moveState.Equals("run")) deltaTime *= 2;
                _hungerTimer += deltaTime;
            } else if (worldThread.difficulty.Equals("normal")) {
                if (moveState.Equals("run")) deltaTime *= 2;
                _hungerTimer += deltaTime * 1.5f;
            } else if (worldThread.difficulty.Equals("hard")) {
                if (moveState.Equals("run")) deltaTime *= 2;
                _hungerTimer += deltaTime * 2;
            }

            if (worldThread.difficulty.Equals("peaceful"))
            {
                if(hunger < 20 && _hungerTimer > 0.5f)
                {
                    hunger++;
                    _hungerTimer = 0;
                }
            } 
            else if (hunger > 0 && _hungerTimer > 60) {
                _hungerTimer = 0;
                hunger--;
            }

            if (hunger < 0) hunger = 0;
            if (hunger <= 15) _reviveTimer = 0;
            if (hunger <= 0) {
                if (_hungerToHurtTimer > 4f) {
                    if (health > 0) {
                        health -= 2;
                        flashHealthBar.gameObject.SetActive(true);
                        flashHealthBar.StartCoroutine(flashHealthBar.Flash());
                        audioSource.PlayOneShot(audioClipHurt, 1f);
                        _hungerToHurtTimer = 0;
                    }

                    if (health < 0) health = 0;
                }

                _hungerToHurtTimer += Time.deltaTime;
            } else {
                _hungerToHurtTimer = 0;
            }
        }
    }
    
    private void UpdateOnGround()
    {
        // 使用Physics2D.OverlapBox进行2D碰撞检测
        Collider2D[] colliderList = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.24f, 0.1f), 0);
        Collider2D[] colliderLeftUpList = Physics2D.OverlapBoxAll(transform.position + new Vector3(-0.241f,1.44f,0), new Vector2(0.241f, 0.47f), 0);
        Collider2D[] colliderRightUpList = Physics2D.OverlapBoxAll(transform.position + new Vector3(0.241f,1.44f,0), new Vector2(0.241f, 0.47f), 0);
        Collider2D[] colliderLeftDownList = Physics2D.OverlapBoxAll(transform.position + new Vector3(-0.241f,0.48f,0), new Vector2(0.241f, 0.47f), 0);
        Collider2D[] colliderRightDownList = Physics2D.OverlapBoxAll(transform.position + new Vector3(0.241f,0.48f,0), new Vector2(0.241f, 0.47f), 0);
        Collider2D[] colliderLeftUpHalfList = Physics2D.OverlapBoxAll(transform.position + new Vector3(-0.25f,0.8f,0), new Vector2(0.25f, 0.2f), 0);
        Collider2D[] colliderRightUpHalfList = Physics2D.OverlapBoxAll(transform.position + new Vector3(0.25f,0.8f,0), new Vector2(0.25f, 0.2f), 0);
        Collider2D[] colliderLeftDownHalfList = Physics2D.OverlapBoxAll(transform.position + new Vector3(-0.25f,0.24f,0), new Vector2(0.25f, 0.2f), 0);
        Collider2D[] colliderRightDownHalfList = Physics2D.OverlapBoxAll(transform.position + new Vector3(0.25f,0.24f,0), new Vector2(0.25f, 0.2f), 0);
        // 判断是否在地面上
        onGround = colliderList.Length > 1;
        _reachLeftUpWall = false;
        _reachRightUpWall = false;
        _reachLeftDownWall = false;
        _reachRightDownWall = false;
        _reachLeftUpHalfWall = false;
        _reachRightUpHalfWall = false;
        _reachLeftDownHalfWall = false;
        _reachRightDownHalfWall = false;
        foreach (var collider2D in colliderLeftUpList) {
            if (collider2D.gameObject.tag.Equals("SolidBlocks")) {
                _reachLeftUpWall = true;
            }
        }
        foreach (var collider2D in colliderRightUpList) {
            if (collider2D.gameObject.tag.Equals("SolidBlocks")) {
                _reachRightUpWall = true;
            }
        }
        foreach (var collider2D in colliderLeftDownList) {
            if (collider2D.gameObject.tag.Equals("SolidBlocks")) {
                _reachLeftDownWall = true;
            }
        }
        foreach (var collider2D in colliderRightDownList) {
            if (collider2D.gameObject.tag.Equals("SolidBlocks")) {
                _reachRightDownWall = true;
            }
        }
        foreach (var collider2D in colliderLeftUpHalfList) {
            if (collider2D.gameObject.tag.Equals("SolidBlocks")) {
                _reachLeftUpHalfWall = true;
            }
        }
        foreach (var collider2D in colliderRightUpHalfList) {
            if (collider2D.gameObject.tag.Equals("SolidBlocks")) {
                _reachRightUpHalfWall = true;
            }
        }
        foreach (var collider2D in colliderLeftDownHalfList) {
            if (collider2D.gameObject.tag.Equals("SolidBlocks")) {
                _reachLeftDownHalfWall = true;
            }
        }
        foreach (var collider2D in colliderRightDownHalfList) {
            if (collider2D.gameObject.tag.Equals("SolidBlocks")) {
                _reachRightDownHalfWall = true;
            }
        }
        // _reachLeftUpWall = colliderLeftUpList.Length > 1;
        // _reachRightUpWall = colliderRightUpList.Length > 1;
        // _reachLeftDownWall = colliderLeftDownList.Length > 1;
        // _reachRightDownWall = colliderRightDownList.Length > 1;
    }

    private void OnDrawGizmos()
    {
        // 设置颜色为红色
        Gizmos.color = Color.red;
        // 绘制碰撞箱的边框
        Gizmos.DrawWireCube(transform.position, new Vector2(0.48f, 0.2f));
        // 设置颜色为緑色
        Gizmos.color = Color.green;
        // 绘制碰撞箱的边框
        Gizmos.DrawWireCube(transform.position + new Vector3(-0.241f,1.44f,0), new Vector2(0.482f, 0.94f));
        Gizmos.DrawWireCube(transform.position + new Vector3(0.241f,1.44f,0), new Vector2(0.482f, 0.94f));
        Gizmos.DrawWireCube(transform.position + new Vector3(-0.241f,0.48f,0), new Vector2(0.482f, 0.94f));
        Gizmos.DrawWireCube(transform.position + new Vector3(0.241f,0.48f,0), new Vector2(0.482f, 0.94f));
        // 设置颜色为黄色
        Gizmos.color = Color.yellow;
        // 绘制碰撞箱的边框
        Gizmos.DrawWireCube(transform.position + new Vector3(-0.25f,0.24f,0), new Vector2(0.5f, 0.4f));
        Gizmos.DrawWireCube(transform.position + new Vector3(0.25f,0.24f,0), new Vector2(0.5f, 0.4f));
        Gizmos.DrawWireCube(transform.position + new Vector3(-0.25f,0.8f,0), new Vector2(0.5f, 0.4f));
        Gizmos.DrawWireCube(transform.position + new Vector3(0.25f,0.8f,0), new Vector2(0.5f, 0.4f));
    }
    
    private void UpdateMove() {
        if(!dead){
            if (_joyTimer > 0) _joyTimer -= Time.deltaTime;
            if (_keyTimer > 0) _keyTimer -= Time.deltaTime;
            if (joyStick.xJoy * _lastXJoy <= 0) {
                canRun1 = false;
                canRun2 = false;
                _joyTimer = 0;
            }

            if (Math.Abs(joyStick.xJoy) < 0.2f) {
                playerRigidbody2D.velocity = new Vector2(0, playerRigidbody2D.velocity.y);
                canRun1 = false;
                canRun2 = false;
                moveState = "stand";
                _joyTimer = 0;
            } else {
                if (canRun3) {
                    canRun1 = true;
                    canRun2 = true;
                }

                if (canRun1 && canRun2) {
                    if(inWater) playerRigidbody2D.velocity =
                            new Vector2(Sign(joyStick.xJoy) * _moveSpeed, playerRigidbody2D.velocity.y);
                    else playerRigidbody2D.velocity =
                        new Vector2(Sign(joyStick.xJoy) * _moveSpeed * 2, playerRigidbody2D.velocity.y);
                    moveState = "run";
                } else if (canRun1) {
                    if (canRun1 && Math.Abs(joyStick.xJoy) - Math.Abs(_lastXJoy) >= 0.05f) canRun2 = true;
                } else if (Math.Abs(joyStick.xJoy) >= 0.2f) {
                    if(inWater) playerRigidbody2D.velocity =
                        new Vector2(Sign(joyStick.xJoy) * _moveSpeed * 0.5f, playerRigidbody2D.velocity.y);
                    else playerRigidbody2D.velocity =
                        new Vector2(Sign(joyStick.xJoy) * _moveSpeed, playerRigidbody2D.velocity.y);
                    if (Math.Abs(joyStick.xJoy) - Math.Abs(_lastXJoy) <= -0.05f) canRun1 = true;
                    _joyTimer = 0.5f;
                }
            }

            _lastXJoy = joyStick.xJoy;

            //transform.Translate(new Vector3(joyStick.xJoy * _moveSpeed * Time.deltaTime,0,0));

            Vector3 playerPosition = transform.position;
            int xPos = (int)playerPosition.x;
            int yPos = (int)playerPosition.y;
            if (xPos < 0) xPos = 0;
            if (xPos > worldThread.width - 1) xPos = worldThread.width - 1;
            if (yPos < 0) yPos = 0;
            if (yPos > worldThread.height - 1) yPos = worldThread.height - 1;
            if (joyStick.yJoy > 0.4) {
                if (worldThread.solidBlockList[yPos, xPos].Equals("Ladder")) {
                    playerRigidbody2D.gravityScale = 0;
                    playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, 5f);
                    //transform.Translate(new Vector3(0,5f * Time.deltaTime,0));
                }else if (onGround) {
                    if (playerRigidbody2D.velocity.y < 1) {
                        playerRigidbody2D.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                    }
                }
            }
            if (joyStick.yJoy < -0.4) {
                if (worldThread.solidBlockList[yPos, xPos].Equals("Ladder")) {
                    playerRigidbody2D.gravityScale = 0;
                    playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, -5f);
                }
            }
            if (!worldThread.solidBlockList[yPos, xPos].Equals("Ladder")) {
                playerRigidbody2D.gravityScale = 5;
            } else if (Math.Abs(joyStick.yJoy) < 0.2f) {
                playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, 0);
            }
            if (_reachLeftDownWall && !_reachLeftUpWall && onGround && joyStick.xJoy <= -0.2f) {
                if (_reachLeftDownHalfWall && !_reachLeftUpHalfWall) {
                    transform.Translate(-0.15f,0.5f,0);
                }
                else if (autoJump && playerRigidbody2D.velocity.y < 1)
                    playerRigidbody2D.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            } else if (_reachRightDownWall && !_reachRightUpWall && onGround && joyStick.xJoy >= 0.2f) {
                if (_reachRightDownHalfWall && !_reachRightUpHalfWall) {
                    transform.Translate(0.15f,0.5f,0);
                }
                else if (autoJump && playerRigidbody2D.velocity.y < 1)
                    playerRigidbody2D.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            }
        }
    }

    public void UpdateAutoJump() {
        autoJump = toggle.isOn;
    }
    
    private void UpdateAnimation() {
        if (Math.Abs(joyStick.xJoy) >= 0.2f) {
            if(canRun1 && canRun2) animator.SetFloat("xJoy", 1f);
            else animator.SetFloat("xJoy", 0.4f);
        }else animator.SetFloat("xJoy", 0f);
    }

    private void UpdateModel()
    {
        if (joyStick.xJoy <= -0.2f)
            modelRoot.transform.rotation = Quaternion.Euler(-90, 0, -90);
        if (joyStick.xJoy >= 0.2f) 
            modelRoot.transform.rotation = Quaternion.Euler(-90,0, 90);
        float rootRotationZ = modelRoot.transform.rotation.z;
        if (joyStick.xJoy <= -0.2f && rootRotationZ >= 85f)
        {
            modelRoot.transform.rotation = Quaternion.Euler(-90, 0, -90);
            float rotationX = head.transform.rotation.x;
            head.transform.rotation = Quaternion.Euler(-rotationX, 0, 0); //将欧拉角转换为四元数
            head.targetRotation = new Vector3(-rotationX, 0, 0);
        }
        if (joyStick.xJoy >= 0.2f && rootRotationZ <= -85f) {
            modelRoot.transform.rotation = Quaternion.Euler(-90,0, 90);
            float rotationX = head.transform.rotation.x;
            head.transform.rotation = Quaternion.Euler(-rotationX, 0, 0); //将欧拉角转换为四元数
            head.targetRotation = new Vector3(-rotationX, 0, 0);
        }
    }

    // 清除玩家物品，返回实际清除个数
    public int clearItem(String itemName, int amount) {
        int amountCleared = 0;
        for (int i = 0; i < 36; i++) {
            if (InventoryName[i].Equals(itemName)) {
                while (InventoryAmount[i] > 0) {
                    if (amountCleared >= amount) break;
                    amountCleared += 1;
                    InventoryAmount[i] -= 1;
                }
                if (InventoryAmount[i] == 0) {
                    InventoryName[i] = "Air";
                }
            }
            if (amountCleared >= amount) break;
        }

        return amountCleared;
    }
    
    // 玩家拾取掉落物，返回未被拾取的数量
    public int getItem(String name, int amount, int searchSize, bool soundOn) {
        ItemBarButton[] itemBarButtonList = new ItemBarButton[9];
        bool[] emptyList = new bool[9];
        for (int i = 0; i < 9; i++) {
            itemBarButtonList[i] = itemBar.childTransforms1[i].gameObject.GetComponent<ItemBarButton>();
            if (InventoryName[itemBarButtonList[i].InventorySort]=="Air") {
                emptyList[i] = true;
            }else emptyList[i] = false;
        }
        // 定义还未被捡完的掉落物数量
        int amountLeft = amount;
        // 如果不是工具
        if (!IndexAll.nameToIsDurable(name)) {
            // 搜索背包内是否已经存在该物品
            for (int i = 0; i < searchSize; i++)
                // 如果存在
                if (InventoryName[i] == name) {
                    // 如果物品数量小于最大堆叠数
                    if (InventoryAmount[i] < IndexAll.nameToMaxAmount(name)) {
                        // 如果物品数量加上全部物品多于最大堆叠数
                        if (InventoryAmount[i] + amountLeft > IndexAll.nameToMaxAmount(name)) {
                            // 掉落物剩余数量扣除已经捡走的数量
                            amountLeft -= (IndexAll.nameToMaxAmount(name) - InventoryAmount[i]);
                            // 该物品堆叠达到上限，设为最大堆叠数
                            InventoryAmount[i] = IndexAll.nameToMaxAmount(name);
                        } else {
                            // 否则该物品直接堆叠全部掉落物数量
                            InventoryAmount[i] += amountLeft;
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
                if (InventoryName[i] == "Air") {
                    // 如果物品剩余数量小于等于最大堆叠数
                    if (amountLeft <= IndexAll.nameToMaxAmount(name)) {
                        // 该物品栏直接堆叠剩余数量
                        InventoryAmount[i] += amountLeft;
                        // 设置此物品栏存在该物品
                        InventoryName[i] = name;
                        // 掉落物剩余数量扣除已经捡走的数量
                        amountLeft = 0;
                        // 退出循环
                        break;
                    } else {
                        // 否则堆叠达到上限，设为最大堆叠数
                        InventoryAmount[i] = IndexAll.nameToMaxAmount(name);
                        // 设置此物品栏存在该物品
                        InventoryName[i] = name;
                        // 掉落物剩余数量扣除最大堆叠数
                        amountLeft -= IndexAll.nameToMaxAmount(name);
                    }
                }
        // 如果得到了东西，播放pop音效
        if (amountLeft < amount && soundOn) {
            audioSource.PlayOneShot(audioClipPop,1f);
            itemBar.UpdateAll();
            if (InventoryName[itemBarButtonList[ItemBarChosen].InventorySort] != "Air" && emptyList[ItemBarChosen]) {
                nameTextThread.nameText.text = IndexAll.nameToNameShow(InventoryName[itemBarButtonList[ItemBarChosen].InventorySort]);
                nameTextThread.timer = 1.5f;
            }
        }
        if (InventoryAmount[ItemBarChosen] == 0)
            InventoryName[ItemBarChosen] = "Air";
        // 返回剩余数量
        return amountLeft;
    }
    
    // 玩家拾取掉落物，返回未被拾取的数量
    public int IfGetItemLeft(String name, int amount, int searchSize, bool soundOn) {
        // 定义还未被捡完的掉落物数量
        int amountLeft = amount;
        // 如果不是工具
        if (!IndexAll.nameToIsDurable(name)) {
            // 搜索背包内是否已经存在该物品
            for (int i = 0; i < searchSize; i++)
                // 如果存在
                if (InventoryName[i] == name) {
                    // 如果物品数量小于最大堆叠数
                    if (InventoryAmount[i] < IndexAll.nameToMaxAmount(name)) {
                        // 如果物品数量加上全部物品多于最大堆叠数
                        if (InventoryAmount[i] + amountLeft > IndexAll.nameToMaxAmount(name)) {
                            // 掉落物剩余数量扣除已经捡走的数量
                            amountLeft -= (IndexAll.nameToMaxAmount(name) - InventoryAmount[i]);
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
                if (InventoryName[i] == "Air") {
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
    
    // 清空玩家物品指定数量，返回实际清除数量
    public int ClearItem(String itemName, int amount) {
        int amountCleared = 0;
        for (int i = 0; i < 36; i++) {
            if (InventoryName[i].Equals(itemName)) {
                while (InventoryAmount[i] > 0) {
                    if (amountCleared >= amount) break;
                    amountCleared += 1;
                    InventoryAmount[i]--;
                    if (InventoryAmount[i] == 0) InventoryName[i] = "Air";
                }
            }
            if (amountCleared >= amount) break;
        }
        return amountCleared;
    }
    
    public void OnMoving(InputAction.CallbackContext value) {
        Vector2 movement = value.ReadValue<Vector2>();
        if(Math.Abs(movement.x) < 0.1f){    
            joyStick.xJoy = 0;
            moveState = "stand";
        }
        if(Math.Abs(Math.Abs(movement.x) - 1) < 0.1f){
            if (canRun3) {
                canRun1 = true;
                canRun2 = true;
            }
            if (_keyTimer > 0 && Math.Abs(_movementLast.x) <= 0.1f) {
                _lastXJoy = Sign(movement.x) * 0.5f;
                joyStick.xJoy = Sign(movement.x) * 0.5f;
                canRun1 = true;
                canRun2 = true;
                moveState = "run";
            }
            else if (Math.Abs(movement.x) >= 0.1f) {
                _keyTimer = 0.3f;
                joyStick.xJoy = Sign(movement.x) * 0.5f;
            }
        }
        if (Math.Abs(_movementLast.y - movement.y) > 0.2f) {
            joyStick.yJoy = movement.y;
            // playerRigidbody2D.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
        _movementLast = movement;
    }

    public void OnHitting(InputAction.CallbackContext value) {
        if (value.performed && Math.Abs(playerRigidbody2D.velocity.x) < 0.1f) animator.SetTrigger("hit");
    }
    
    public int Sign(float y)
    {
        return y>0 ? 1 : -1;
    }
    
    public void PlayEating()
    {
        float randomNum = Random.Range(1.0f, 4.0f);
        if (randomNum >= 1.0f && randomNum < 2.0f) {
            audioSource.PlayOneShot(eatAudioClip1, 1f);
        } else if (randomNum >= 2.0f && randomNum < 3.0f) {
            audioSource.PlayOneShot(eatAudioClip2, 1f);
        } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
            audioSource.PlayOneShot(eatAudioClip3, 1f);
        }
    }
    
    public void PlayEatFinish() {
        audioSource.PlayOneShot(eatFinishAudioClip, 1f);
    }
}
