using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Block;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Util;
using Random = UnityEngine.Random;

public class ScreenThread : MonoBehaviour {
    public WorldThread worldThread;
    public JoyStick joyStick;
    public HeadThread playerHead;
    public PlayerThread playerThread;
    public UseAudio useAudio;
    public GameObject magnifier;
    public RectTransform magnifierTransform;
    public Transform magnifierCameraTransform;
    public ItemBar itemBar;
    public AudioClip toolBreakAudioClip;
    public GameObject itemPrefab;
    public TabButtonNew tabButtonNew2;
    public GameObject torchLight2DPrefab;
    public GameObject lights;
    public GameObject items;
    public GameObject furnaceUI;
    public Transform furnacesTransform;
    public GameObject furnacePrefab;
    public FurnaceContent furnaceContent;
    public bool magnifierOn;
    public Toggle magnifierToggle;
    public int furnacePressTouchID;
    public int chestPressTouchID;
    public GameObject chestPrefab;
    public GameObject largeChestPrefab;
    public Transform chestsTransform;
    public GameObject chestUI;
    public ChestContent chestContent;
    public TMP_Text chestTitle;
    public JumpButton jumpButton;
    private ItemBarButton[] _itemBarButtonList;
    private Camera _mainCamera;
    private Collider2D _playerCollider2D;
    private int _destroyTouchFingerId;
    private float _destroyStartTime;
    private List<int> _airTouchedFingerIdList;
    private Dictionary<int, float> _touchDuration;
    private Vector2Int _selectBlockPosition;
    private int[] _dropPressedFingerIdList;
    private int _playerLastChosen;
    private String _playerLastDestroyBlock;
    private float _eatStartTime;
    private bool eating;

    private void Awake()
    {
        chestPressTouchID = -1;
        furnacePressTouchID = -1;
        magnifierOn = true;
        _airTouchedFingerIdList = new List<int>();
        _mainCamera = Camera.main;
        _playerCollider2D = playerThread.GetComponent<Collider2D>();
        _destroyTouchFingerId = -1;
        _destroyStartTime = 0;
        _touchDuration = new Dictionary<int, float>();
        _selectBlockPosition = new Vector2Int(0, 0);
        _itemBarButtonList = new ItemBarButton[9];
        Transform[] childTransforms = GetComponentsInRealChildren<Transform>(itemBar.gameObject,true);
        _dropPressedFingerIdList = new int[9];
        for (int i = 0; i < 9; i++) {
            _itemBarButtonList[i] = childTransforms[i].gameObject.GetComponent<ItemBarButton>();
            _dropPressedFingerIdList[i] = -1;
        }
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
    void Update() {
        // 处理触摸输入
        UpdateTouch();
        UpdateTouchDuration();
    }

    Vector3Int TouchToBlockPosition(Touch touch) {
        // 获取触摸世界位置
        Vector3 touchWorldPosition = _mainCamera.ScreenToWorldPoint(touch.position);
        // 将触摸世界位置转换为瓦片坐标
        Vector3Int blockPosition = worldThread.solidBlockTileMap.WorldToCell(touchWorldPosition);
        blockPosition = new Vector3Int(blockPosition.x, blockPosition.y, 0);
        return blockPosition;
    }
    
    void UpdateTouchDuration() {
        var touchList = new List<int>(_touchDuration.Keys);

        foreach (var touch in touchList)
        {
            _touchDuration[touch] += Time.deltaTime;
        }
    }
    
    // 处理触摸输入的方法
    void UpdateTouch()
    {
        if (Input.touchCount > 0) {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                    switch (touch.phase) {
                        case TouchPhase.Began:
                            HandleTouchBegan(touch);
                            break;

                        case TouchPhase.Moved:
                            HandleTouchMoved(touch);
                            break;

                        case TouchPhase.Ended:
                            HandleTouchEnded(touch);
                            break;
                    }
                    
                    String nameInHand = playerThread.InventoryName[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort];
                    if (IndexAll.nameToFoodValue(nameInHand) > 0 && playerThread.hunger < 20)
                    {
                        if(_touchDuration.Keys.Contains(touch.fingerId) && _touchDuration[touch.fingerId] >= 0.26f && !eating){
                            _eatStartTime = _touchDuration[touch.fingerId];
                            eating = true;
                        }
                        float timeEat = 0;
                        if(_touchDuration.Keys.Contains(touch.fingerId))
                            timeEat = _touchDuration[touch.fingerId] - _eatStartTime;
                        if (timeEat % 0.205f <= 0.0181f && timeEat > 0 && eating)
                        {
                            playerThread.PlayEating();
                            if (timeEat > 2f)
                            {
                                playerThread.InventoryAmount[
                                    _itemBarButtonList[playerThread.ItemBarChosen].InventorySort]--;
                                if (playerThread.InventoryAmount[
                                        _itemBarButtonList[playerThread.ItemBarChosen].InventorySort] <= 0)
                                {
                                    playerThread.InventoryName[
                                        _itemBarButtonList[playerThread.ItemBarChosen].InventorySort] = "Air";
                                }
                                if (playerThread.hunger + IndexAll.nameToFoodValue(nameInHand) > 20) {
                                    playerThread.hunger = 20;
                                }else {
                                    playerThread.hunger += IndexAll.nameToFoodValue(nameInHand);
                                }
                                itemBar.UpdateAll();
                                playerThread.flashHungerBar.gameObject.SetActive(true);
                                playerThread.flashHungerBar.StartCoroutine(playerThread.flashHungerBar.Flash());
                                playerThread.PlayEatFinish();
                                _eatStartTime = _touchDuration[touch.fingerId];
                            }

                            if (_playerLastChosen != playerThread.ItemBarChosen)
                            {
                                eating = false;
                                _eatStartTime = _touchDuration[touch.fingerId];
                            }
                            _playerLastChosen = playerThread.ItemBarChosen;
                        }
                    }
                    else if (_destroyTouchFingerId == touch.fingerId && _touchDuration.Keys.Contains(touch.fingerId) && _touchDuration[touch.fingerId] >= 0.26f) {
                        Vector3Int blockPosition = TouchToBlockPosition(touch);
                        float distance = Vector3.Distance(new Vector3(blockPosition.x, blockPosition.y, 0),
                            worldThread.solidBlockTileMap.WorldToCell(playerThread.transform.position));
                        if(playerThread.gamemode.Equals("creative") || distance <= 5){
                            if (_selectBlockPosition.x != blockPosition.x ||
                                _selectBlockPosition.y != blockPosition.y || _playerLastChosen != playerThread.ItemBarChosen) {
                                worldThread.SetGUI(worldThread.destroyTileMapFront, _selectBlockPosition.x,
                                    _selectBlockPosition.y, "Air");
                                worldThread.SetGUI(worldThread.destroyTileMapBack, _selectBlockPosition.x,
                                    _selectBlockPosition.y, "Air");
                                _destroyStartTime = _touchDuration[touch.fingerId];
                                _selectBlockPosition.x = blockPosition.x;
                                _selectBlockPosition.y = blockPosition.y;
                                _playerLastChosen = playerThread.ItemBarChosen;
                            }
                            if (blockPosition.x >= 0 && blockPosition.x < worldThread.width && blockPosition.y >= 0 &&
                                blockPosition.y < worldThread.height) {
                                String blockOriginName = worldThread.solidBlockList[blockPosition.y, blockPosition.x];
                                if (blockOriginName.Equals("Air")) {
                                    blockOriginName = worldThread.backBlockList[blockPosition.y, blockPosition.x];
                                }
                                if (_playerLastDestroyBlock != blockOriginName) {
                                    worldThread.SetGUI(worldThread.destroyTileMapFront, _selectBlockPosition.x,
                                        _selectBlockPosition.y, "Air");
                                    worldThread.SetGUI(worldThread.destroyTileMapBack, _selectBlockPosition.x,
                                        _selectBlockPosition.y, "Air");
                                    _destroyStartTime = _touchDuration[touch.fingerId];
                                    _selectBlockPosition.x = blockPosition.x;
                                    _selectBlockPosition.y = blockPosition.y;
                                    _playerLastDestroyBlock = blockOriginName;
                                }
                                if (blockOriginName != "Air" &&
                                    !_airTouchedFingerIdList.Contains(touch.fingerId)) {
                                    if (IndexAll.blockToType(blockOriginName).Equals("wall")) {
                                        if(!nameInHand.Contains("Hammer")) {
                                            _destroyStartTime = _touchDuration[touch.fingerId];
                                        }
                                    }
                                    float timeDestroy = _touchDuration[touch.fingerId] - _destroyStartTime;
                                    float timeAll = IndexAll.nameToDestroyTime(blockOriginName,nameInHand);
                                    if (playerThread.underWater) timeAll *= 3f;
                                    if (blockOriginName.Contains("Ore")) timeAll *= 2f;
                                    int crackSort = (int)(timeDestroy / (timeAll / 8));
                                    if (playerThread.joyStick.xJoy == 0 && timeDestroy % 0.26 <= 0.0185f &&
                                        timeDestroy > 0) {
                                        playerThread.animator.SetTrigger("hit");
                                        Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(touch.position);
                                        Vector3Int cellPoint = worldThread.solidBlockTileMap.WorldToCell(worldPoint);
                                        worldPoint = worldThread.solidBlockTileMap.CellToWorld(cellPoint);
                                        worldPoint += new Vector3(0.5f, 0.5f, 0f);
                                        useAudio.PlayDigging(worldPoint.x, worldPoint.y, IndexAll.blockToAudioType(
                                            blockOriginName));
                                    }
                                    if (crackSort <= 8) {
                                        // 放置触摸位置的裂痕
                                        if(worldThread.noReachBlockList[blockPosition.y, blockPosition.x]) {
                                            worldThread.SetGUI(worldThread.destroyTileMapBack, blockPosition.x, blockPosition.y, "Destroy" + crackSort);
                                        }
                                        else
                                        {
                                            worldThread.SetGUI(worldThread.destroyTileMapFront, blockPosition.x, blockPosition.y, "Destroy" + crackSort);
                                        }
                                    } else if (crackSort > 8) {
                                        crackSort = 0;
                                        // 清除触摸位置的瓦片
                                        Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(touch.position);
                                        Vector3Int cellPoint = worldThread.solidBlockTileMap.WorldToCell(worldPoint);
                                        worldPoint = worldThread.solidBlockTileMap.CellToWorld(cellPoint);
                                        worldPoint += new Vector3(0.5f, 0.5f, 0f);
                                        useAudio.PlayDestroy(worldPoint.x, worldPoint.y, IndexAll.blockToAudioType(
                                            blockOriginName));
                                        String itemName = IndexAll.blockNameToItemName(blockOriginName,nameInHand);
                                        if (blockOriginName.Contains("Furnace"))
                                        {
                                            foreach (var furnace in worldThread.furnaceList)
                                            {
                                                if (furnace.xBlock == cellPoint.x && furnace.yBlock == cellPoint.y)
                                                {
                                                    furnace.DestroyFurnace();
                                                    break;
                                                }
                                            }
                                        }

                                        List<Vector2Int> saplingListTmp = new List<Vector2Int>(worldThread.saplingList);
                                        if (blockOriginName.Contains("Sapling"))
                                        {
                                            foreach (var sapling in saplingListTmp)
                                            {
                                                if (sapling.x == cellPoint.x && sapling.y == cellPoint.y)
                                                {
                                                    worldThread.saplingList.Remove(sapling);
                                                    break;
                                                }
                                            }
                                        }
                                        
                                        if (blockOriginName.Contains("Chest"))
                                        {
                                            bool stopSearching = false;
                                            foreach (var chest in worldThread.chestList)
                                            {
                                                foreach (var position in chest.blockPositionList)
                                                {
                                                    if(position.x == cellPoint.x && position.y == cellPoint.y)
                                                    {
                                                        chest.DestroyChest(cellPoint.x, cellPoint.y);
                                                        stopSearching = true;
                                                        break;
                                                    }
                                                }

                                                if (stopSearching) break;
                                            }
                                        }

                                        if (blockOriginName.Contains("Leaves")){
                                            float randomNum = Random.Range(0f, 3f);
                                            if (randomNum >= 0f && randomNum < 1.0f)
                                            {
                                                itemPrefab.SetActive(true);
                                                GameObject item = Instantiate(itemPrefab,
                                                    new Vector3(blockPosition.x + 0.5f, blockPosition.y + 0.5f, 0),
                                                    Quaternion.identity, items.transform);
                                                ItemThread itemThread = item.gameObject.GetComponent<ItemThread>();
                                                itemThread.itemInit("Apple", 1, 0);
                                                itemPrefab.SetActive(false);
                                            }else if (randomNum >= 1f && randomNum < 2.0f)
                                            {
                                                itemPrefab.SetActive(true);
                                                GameObject item = Instantiate(itemPrefab,
                                                    new Vector3(blockPosition.x + 0.5f, blockPosition.y + 0.5f, 0),
                                                    Quaternion.identity, items.transform);
                                                ItemThread itemThread = item.gameObject.GetComponent<ItemThread>();
                                                itemThread.itemInit("SaplingOak", 1, 0);
                                                itemPrefab.SetActive(false);
                                            }
                                        }
                                        if(itemName != "Air"){
                                            itemPrefab.SetActive(true);
                                            GameObject item = Instantiate(itemPrefab,
                                                new Vector3(blockPosition.x + 0.5f, blockPosition.y + 0.5f, 0),
                                                Quaternion.identity, items.transform);
                                            ItemThread itemThread = item.gameObject.GetComponent<ItemThread>();
                                            itemThread.itemInit(itemName, 1, 0);
                                            if (IndexAll.BlockNameToIsLight(itemThread.nameItem)) {
                                                item.GetComponentsInChildren<Transform>()[2].GetComponent<Light2D>().enabled = true;
                                            }
                                            itemPrefab.SetActive(false);
                                        }

                                        if (blockOriginName.Contains("Wall")) {
                                            worldThread.SetGUI(worldThread.destroyTileMapFront, blockPosition.x, blockPosition.y,
                                                "Air");
                                            worldThread.SetGUI(worldThread.destroyTileMapBack, blockPosition.x, blockPosition.y,
                                                "Air");
                                            worldThread.SetBlock(worldThread.backBlockTileMap, blockPosition.x,
                                                blockPosition.y, "Air");
                                            worldThread.backBlockList[blockPosition.y, blockPosition.x] = "Air";
                                        } else {
                                            worldThread.SetGUI(worldThread.destroyTileMapFront, blockPosition.x, blockPosition.y,
                                                "Air");
                                            worldThread.SetGUI(worldThread.destroyTileMapBack, blockPosition.x, blockPosition.y,
                                                "Air");
                                            worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x,
                                                blockPosition.y, "Air");
                                            worldThread.solidBlockList[blockPosition.y, blockPosition.x] = "Air";
                                            worldThread.noReachBlockList[blockPosition.y, blockPosition.x] = false;
                                        }
                                       _destroyStartTime = _touchDuration[touch.fingerId];
                                        if (IndexAll.nameToIsTool(nameInHand)) {
                                            itemBar.UpdateSingle(playerThread.ItemBarChosen);
                                            playerThread.InventoryAmount[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort]--;
                                            if (playerThread.InventoryAmount[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort] <= 0) {
                                                playerThread.InventoryName[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort] = "Air";
                                                ItemBarButton itemBarButtonTmp = _itemBarButtonList[playerThread.ItemBarChosen];
                                                itemBarButtonTmp.iconImage.enabled = false;
                                                itemBarButtonTmp.textMeshPro.text = "";
                                                itemBarButtonTmp.amountBarBack.SetActive(false);
                                                itemBarButtonTmp.amountBar.SetActive(false);
                                                Vector3 playerPosition = playerThread.transform.position;
                                                AudioSource.PlayClipAtPoint(toolBreakAudioClip,new Vector3(playerPosition.x + 0.5f,playerPosition.y + 0.5f,Camera.main.transform.position.z),1f);
                                            }
                                        }
                                        foreach (var torchLight2D in worldThread.torchLight2DList) {
                                            if (torchLight2D.x.Equals(blockPosition.x) &&
                                                torchLight2D.y.Equals(blockPosition.y)) {
                                                worldThread.torchLight2DList.Remove(torchLight2D);
                                                Destroy(torchLight2D.gameObject);
                                                break;
                                            }
                                        }
                                        UpdateNearbyBlock(blockPosition.x, blockPosition.y);
                                    }
                                } else {
                                    _destroyStartTime = _touchDuration[touch.fingerId];
                                }
                            }
                        } else {
                            _destroyStartTime = _touchDuration[touch.fingerId];
                        }
                    }
                    
                    if (_destroyTouchFingerId == -1 && _touchDuration.Keys.Contains(touch.fingerId) &&
                        _touchDuration[touch.fingerId] >= 0.26f)
                    {
                        if (!_airTouchedFingerIdList.Contains(touch.fingerId))
                        {
                            if (IndexAll.nameToFoodValue(nameInHand) <= 0)
                            {
                                _destroyTouchFingerId = touch.fingerId;
                                if (magnifierOn){
                                    magnifier.SetActive(true);
                                    Vector3 touchWorldPosition = _mainCamera.ScreenToWorldPoint(touch.position);
                                    magnifierCameraTransform.position =
                                        new Vector3(touchWorldPosition.x, touchWorldPosition.y, -10);
                                    magnifierTransform.position = touch.position - new Vector2(300, 0);
                                }
                            }
                        }
                    }

                    if (_destroyTouchFingerId == touch.fingerId && (_mainCamera.velocity.x != 0 ||
                                                                    _mainCamera.velocity.y != 0))
                    {
                        if (IndexAll.nameToFoodValue(nameInHand) <= 0)
                        {
                            if (magnifierOn){
                                Vector3 touchWorldPosition = _mainCamera.ScreenToWorldPoint(touch.position);
                                magnifierCameraTransform.position =
                                    new Vector3(touchWorldPosition.x, touchWorldPosition.y, -10);
                                magnifierTransform.position = touch.position - new Vector2(300, 0);
                            }
                        }
                    }
            }
        }
    }

    void HandleTouchBegan(Touch touch)
    {
        EventSystem eventSystem = EventSystem.current;
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position =  touch.position;
        //射线检测ui
        List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, uiRaycastResultCache);
        if (uiRaycastResultCache.Count == 0) {
            Vector3 touchWorldPosition = _mainCamera.ScreenToWorldPoint(touch.position);
            Vector3Int blockPosition = worldThread.solidBlockTileMap.WorldToCell(touchWorldPosition);
            if (!_airTouchedFingerIdList.Contains(touch.fingerId)) {
                if (blockPosition.x >= 0 && blockPosition.x < worldThread.width && blockPosition.y >= 0 &&
                    blockPosition.y < worldThread.height) {
                    String nameInHand = playerThread.InventoryName[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort];
                    String blockName = worldThread.solidBlockList[blockPosition.y, blockPosition.x];
                    if (blockName.Equals("Air")) {
                        blockName = worldThread.backBlockList[blockPosition.y, blockPosition.x];
                    }
                    if (IndexAll.blockToType(blockName).Equals("wall")) {
                        if(!nameInHand.Contains("Hammer")) {
                            _airTouchedFingerIdList.Add(touch.fingerId);
                        }
                    }
                    else if (worldThread.solidBlockList[blockPosition.y, blockPosition.x] == "Air")
                        _airTouchedFingerIdList.Add(touch.fingerId);
                }
            }
            if (!_touchDuration.Keys.Contains(touch.fingerId)) {
                if (blockPosition.x >= 0 && blockPosition.x < worldThread.width && blockPosition.y >= 0 &&
                    blockPosition.y < worldThread.height) {
                    float distance = Vector3.Distance(new Vector3(blockPosition.x, blockPosition.y, 0),
                        worldThread.solidBlockTileMap.WorldToCell(playerThread.transform.position));
                    String nameInHand = playerThread.InventoryName[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort];
                    if(IndexAll.nameToFoodValue(nameInHand) > 0 || playerThread.gamemode.Equals("creative") || distance <= 5) {
                        _touchDuration.Add(touch.fingerId, 0f);
                    }
                }
            }
            // float distance = Vector2.Distance(joyStick.transform.position + new Vector3(joyStick.mRadius, joyStick.mRadius, 0), touch.position);
            playerHead.SpriteFaceTo(new Vector2(touchWorldPosition.x, touchWorldPosition.y));
        } else {
            String[] nameTmpList = uiRaycastResultCache[0].gameObject.name.Split("_");
            if (nameTmpList.Length >= 2 && nameTmpList[0].Equals("ItemBar")) {
                _itemBarButtonList[int.Parse(nameTmpList[1])-1].startPressed = true;
                _dropPressedFingerIdList[int.Parse(nameTmpList[1]) - 1] = touch.fingerId;
            }else if (nameTmpList.Length >= 1 && nameTmpList[0].Equals("SelectedBar")) {
                _itemBarButtonList[playerThread.ItemBarChosen].startPressed = true;
                _dropPressedFingerIdList[playerThread.ItemBarChosen] = touch.fingerId;
            }else if (nameTmpList.Length >= 1 && nameTmpList[0].Equals("JumpButton"))
            {
                jumpButton.startPressed = true;
            }

            if(furnaceUI.activeInHierarchy){
                bool furnaceDetected = false;
                int furnaceSort = -1;
                foreach (var result in uiRaycastResultCache)
                {
                    if (result.gameObject.name.Contains("FurnaceGrid"))
                    {
                        furnaceDetected = true;
                        furnaceSort = int.Parse(result.gameObject.name.Split("_")[1]) - 1;
                        break;
                    }
                }

                if (furnaceDetected)
                {
                    furnaceContent.timerPressed = 0;
                    furnaceContent.furnaceGridList[furnaceSort].functioned = false;
                    furnaceContent.furnaceGridList[furnaceSort].UpdatePressBar(0);
                    furnacePressTouchID = touch.fingerId;
                    furnaceContent.presssed = true;
                    furnaceContent.pressSort = furnaceSort;
                }
            }

            if(chestUI.activeInHierarchy){
                bool chestDetected = false;
                bool inventoryDetected = false;
                int chestSort = -1;
                foreach (var result in uiRaycastResultCache)
                {
                    if (result.gameObject.name.Contains("ChestGrid"))
                    {

                        if (result.gameObject.name.Contains("Inventory"))
                        {
                            inventoryDetected = true;
                            chestContent.chestPressType = "inventory";
                            chestSort = result.gameObject.GetComponent<InventoryChestGrid>().inventoryGridSort;
                        }
                        else
                        {
                            chestDetected = true;
                            chestContent.chestPressType = "chest";
                            chestSort = result.gameObject.GetComponent<ChestGrid>().gridSort;
                        }

                        break;
                    }
                }
                
                if (inventoryDetected)
                {
                    chestContent.timerPressed = 0;
                    chestPressTouchID = touch.fingerId;
                    chestContent.presssed = true;
                    chestContent.pressSort = chestSort;
                    chestContent.inventoryChestGridList[chestSort].functioned = false;
                    chestContent.inventoryChestGridList[chestSort].UpdatePressBar(0);
                }
                
                if (chestDetected)
                {
                    chestContent.timerPressed = 0;
                    chestPressTouchID = touch.fingerId;
                    chestContent.presssed = true;
                    chestContent.pressSort = chestSort;
                    chestContent.chestGridList[chestSort].functioned = false;
                    chestContent.chestGridList[chestSort].UpdatePressBar(0);
                }
            }
        }
    }

    void HandleTouchMoved(Touch touch)
    {
        if(_touchDuration.Keys.Contains(touch.fingerId)){
            Vector3 touchWorldPosition = _mainCamera.ScreenToWorldPoint(touch.position);
            playerHead.SpriteFaceTo(new Vector2(touchWorldPosition.x, touchWorldPosition.y));
            if (touch.fingerId == _destroyTouchFingerId) {
                magnifierCameraTransform.position =
                    new Vector3(touchWorldPosition.x, touchWorldPosition.y, -10);
                magnifierTransform.position = touch.position - new Vector2(300, 0);
            }
        }
    }

    void HandleTouchEnded(Touch touch)
    {
        eating = false;
        if(_touchDuration.Keys.Contains(touch.fingerId)){
            if (_destroyTouchFingerId == touch.fingerId) _destroyTouchFingerId = -1;
            // 处理短按
            if (_touchDuration[touch.fingerId] < 0.26f) {
                Vector3Int blockPosition = TouchToBlockPosition(touch);
                if (blockPosition.x >= 0 && blockPosition.x < worldThread.width && blockPosition.y >= 0 &&
                    blockPosition.y < worldThread.height) {
                    String blockName = playerThread.InventoryName[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort];
                    String blockOriginalName = "Air";
                    if (blockName.Contains("Wall")) {
                        blockOriginalName = worldThread.backBlockList[blockPosition.y, blockPosition.x];
                    } else {
                        blockOriginalName = worldThread.solidBlockList[blockPosition.y, blockPosition.x];
                    }
                    if (blockOriginalName.Equals("Air")) {
                        Vector2 center = new Vector2(_playerCollider2D.bounds.center.x,
                            _playerCollider2D.bounds.center.y);
                        Vector2 extends = new Vector2(_playerCollider2D.bounds.extents.x,
                            _playerCollider2D.bounds.extents.y);
                        Vector3 leftUp =
                            worldThread.solidBlockTileMap.WorldToCell(new Vector3(center.x - extends.x,
                                center.y + extends.y, 0));
                        Vector3 rightDown =
                            worldThread.solidBlockTileMap.WorldToCell(new Vector3(center.x + extends.x,
                                center.y - extends.y, 0));
                        if (blockName.Contains("Bucket")||blockName.Contains("Wall")||IndexAll.BlockNameToUntouchable(blockName)||(!(blockPosition.x >= leftUp.x && blockPosition.x <= rightDown.x &&
                               blockPosition.y >= rightDown.y && blockPosition.y <= leftUp.y))) {
                            bool bucketChanged = false;
                            if (!worldThread.liquidBlockList[blockPosition.y, blockPosition.x].Equals("Air")) {
                                if (playerThread.InventoryName[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort].Equals("BucketEmpty")) {
                                    if (playerThread.InventoryAmount[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort] == 1) {
                                        playerThread.InventoryName[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort] = "BucketWater";
                                        worldThread.liquidBlockList[blockPosition.y, blockPosition.x] = "Air";
                                        worldThread.SetBlock(worldThread.liquidBlockTileMap,blockPosition.x,blockPosition.y,"Air");
                                        useAudio.PlayUse(blockPosition.x,blockPosition.y,"WaterFill");
                                        itemBar.UpdateAll();
                                        bucketChanged = true;
                                    } else {
                                        int amountLeft = playerThread.getItem("BucketWater", 1, 36, false);
                                        if (amountLeft == 0) {
                                            playerThread.clearItem("BucketEmpty", 1);
                                            worldThread.liquidBlockList[blockPosition.y, blockPosition.x] = "Air";
                                            worldThread.SetBlock(worldThread.liquidBlockTileMap,blockPosition.x,blockPosition.y,"Air");
                                            useAudio.PlayUse(blockPosition.x,blockPosition.y,"WaterFill");
                                            itemBar.UpdateAll();
                                            bucketChanged = true;
                                        }
                                    }
                                }
                            } 
                            if (blockName != "Air"){
                                if (IndexAll.NameToIsBlock(blockName)) {
                                    bool canPlace = false;
                                    if (IndexAll.BlockNameToIsAttachable(blockName)) {
                                        canPlace = CanPlaceAttachableBlock(blockPosition.x, blockPosition.y);
                                    } else if (blockName.Contains("Door")) {
                                        if (blockPosition.y < worldThread.height - 1 && blockPosition.y > 0) {
                                            if (worldThread.solidBlockList[blockPosition.y+1, blockPosition.x].Equals("Air") && !worldThread.solidBlockList[blockPosition.y-1, blockPosition.x].Equals("Air")) {
                                                canPlace = true;
                                            }
                                        }
                                    } else if (blockName.Contains("Sapling")) {
                                        if (blockPosition.y > 0) {
                                            if (worldThread.solidBlockList[blockPosition.y-1, blockPosition.x].Equals("Dirt") || worldThread.solidBlockList[blockPosition.y-1, blockPosition.x].Equals("GrassBlock")) {
                                                canPlace = true;
                                                worldThread.saplingList.Add(new Vector2Int(blockPosition.x, blockPosition.y));
                                            }
                                        }
                                    }else canPlace = true;
                                    if(canPlace){
                                        if (IndexAll.BlockNameToHasDirection(blockName)) {
                                            if (playerThread.modelRoot.transform.rotation.z < 0) {
                                                blockName += "Left";
                                            } else {
                                                blockName += "Right";
                                            }
                                        }
                                        // 放置触摸位置的瓦片
                                        if (blockName.Contains("Wall")) {
                                            worldThread.backBlockList[blockPosition.y, blockPosition.x] = blockName;
                                            worldThread.SetBlock(worldThread.backBlockTileMap, blockPosition.x, blockPosition.y, blockName);
                                        } else if (blockName.Contains("Door")) {
                                            worldThread.solidBlockList[blockPosition.y+1, blockPosition.x] = blockName + "SideUpper";
                                            worldThread.solidBlockList[blockPosition.y, blockPosition.x] = blockName + "SideLower";
                                            worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y+1, blockName + "SideUpper");
                                            worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y, blockName + "SideLower");
                                        }else if (blockName.Contains("Furnace")) {
                                            worldThread.solidBlockList[blockPosition.y, blockPosition.x] = blockName + "Off";
                                            worldThread.noReachBlockList[blockPosition.y, blockPosition.x] = true;
                                            worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y, blockName + "Off");
                                            Vector3 furnacePosition = new Vector3(blockPosition.x + 0.5f,
                                                blockPosition.y + 0.5f, playerThread.transform.position.z);
                                            furnacePrefab.SetActive(true);
                                            GameObject furnace = Instantiate(furnacePrefab, furnacePosition, Quaternion.identity, furnacesTransform);
                                            FurnaceThread furnaceThread = furnace.GetComponent<FurnaceThread>();
                                            furnaceThread.xBlock = blockPosition.x;
                                            furnaceThread.yBlock = blockPosition.y;
                                            worldThread.furnaceList.Add(furnaceThread);
                                            furnacePrefab.SetActive(false);
                                        }else if (blockName.Contains("Chest")) {
                                            worldThread.solidBlockList[blockPosition.y, blockPosition.x] = blockName;
                                            worldThread.noReachBlockList[blockPosition.y, blockPosition.x] = true;
                                            if (blockName.Contains("Large")) {
                                                if(blockPosition.x+1<=worldThread.width && worldThread.solidBlockList[blockPosition.y,blockPosition.x+1].Equals("Air")){
                                                    Vector3 chestPosition = new Vector3(blockPosition.x,
                                                        blockPosition.y, 3.5f);
                                                    largeChestPrefab.SetActive(true);
                                                    GameObject chest = Instantiate(largeChestPrefab, chestPosition,
                                                        Quaternion.identity, chestsTransform);
                                                    ChestThread chestThread = chest.GetComponent<ChestThread>();
                                                    chestThread.blockPositionList.Add(new Vector2Int(blockPosition.x,blockPosition.y));
                                                    chestThread.blockPositionList.Add(new Vector2Int(blockPosition.x+1,blockPosition.y));
                                                    chestThread.InitChest(54, true);
                                                    worldThread.chestList.Add(chestThread);
                                                    largeChestPrefab.SetActive(false);
                                                }
                                            }else {
                                                Vector3 chestPosition = new Vector3(blockPosition.x + 0.5f,
                                                    blockPosition.y, 3.5f);
                                                chestPrefab.SetActive(true);
                                                GameObject chest = Instantiate(chestPrefab, chestPosition, Quaternion.identity, chestsTransform);
                                                ChestThread chestThread = chest.GetComponent<ChestThread>();
                                                chestThread.blockPositionList.Add(new Vector2Int(blockPosition.x,blockPosition.y));
                                                chestThread.InitChest(27, true);
                                                bool assembled = chestThread.AssembleNearbyChest();
                                                if(!assembled) worldThread.chestList.Add(chestThread);
                                                else Destroy(chestThread.gameObject);
                                                chestPrefab.SetActive(false);
                                            }
                                        }else {
                                            worldThread.solidBlockList[blockPosition.y, blockPosition.x] = blockName;
                                            if (IndexAll.BlockNameToUntouchable(blockName))
                                                worldThread.noReachBlockList[blockPosition.y, blockPosition.x] = true;
                                            worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y, blockName);
                                        }
                                        playerThread.InventoryAmount[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort]--;
                                        if (playerThread.InventoryAmount[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort] <= 0) {
                                            playerThread.InventoryAmount[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort] = 0;
                                            playerThread.InventoryName[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort] = "Air";
                                        }

                                        itemBar.UpdateSingle(playerThread.ItemBarChosen);
                                        if (playerThread.joyStick.xJoy == 0) playerThread.animator.SetTrigger("hit");
                                        {
                                            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(touch.position);
                                            Vector3Int cellPoint =
                                                worldThread.solidBlockTileMap.WorldToCell(worldPoint);
                                            worldPoint = worldThread.solidBlockTileMap.CellToWorld(cellPoint);
                                            worldPoint += new Vector3(0.5f, 0.5f, 0f);
                                            useAudio.PlayPlace(worldPoint.x, worldPoint.y,
                                                IndexAll.blockToAudioType(blockName));
                                        }
                                        if (IndexAll.BlockNameToIsLight(blockName)) {
                                            Vector3 tmpPosition = new Vector3(blockPosition.x + 0.5f,
                                                blockPosition.y + 0.5f, 0);
                                            GameObject torchLight2DObject = Instantiate(torchLight2DPrefab, tmpPosition,
                                                Quaternion.identity,
                                                lights.transform);
                                            torchLight2DObject.SetActive(true);
                                            TorchLight2D torchLight2DTmp =
                                                torchLight2DObject.GetComponent<TorchLight2D>();
                                            torchLight2DTmp.x = blockPosition.x;
                                            torchLight2DTmp.y = blockPosition.y;
                                            worldThread.torchLight2DList.Add(torchLight2DTmp);
                                        }
                                    }
                                } else if (!bucketChanged && playerThread.InventoryName[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort].Equals("BucketWater")) {
                                    playerThread.InventoryName[_itemBarButtonList[playerThread.ItemBarChosen].InventorySort] = "BucketEmpty";
                                    worldThread.liquidBlockList[blockPosition.y, blockPosition.x] = "WaterStill";
                                    worldThread.SetBlock(worldThread.liquidBlockTileMap,blockPosition.x, blockPosition.y,"WaterStill");
                                    itemBar.UpdateAll();
                                    useAudio.PlayUse(blockPosition.x,blockPosition.y,"WaterEmpty");
                                }
                            }
                        }
                    } else if (worldThread.solidBlockList[blockPosition.y, blockPosition.x] == "CraftingTable") {
                        playerThread.onCraftingTable = true;
                        furnaceUI.SetActive(false);
                        chestUI.SetActive(false);
                        tabButtonNew2.OpenCraftingTable();
                        if(tabButtonNew2.CurrentTargetCraftGridList.Count > 0) {
                            tabButtonNew2.CurrentTargetCraftGridList[0].SelectInit();
                        }
                    } else if (worldThread.solidBlockList[blockPosition.y, blockPosition.x].Contains("Furnace")){
                        OpenFurnaceUI(blockPosition.x, blockPosition.y);
                    }else if (worldThread.solidBlockList[blockPosition.y, blockPosition.x].Contains("Chest")){
                        OpenChestUI(blockPosition.x, blockPosition.y);
                    }else if (worldThread.solidBlockList[blockPosition.y, blockPosition.x].Contains("Door")) {
                        String blockNameHere = worldThread.solidBlockList[blockPosition.y, blockPosition.x];
                        if (blockNameHere.Contains("Upper")) {
                            if (blockNameHere.Contains("Side")) {
                                useAudio.PlayUse(blockPosition.x, blockPosition.y, "DoorOpen");
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y-1, "Air");
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y, "Air");
                                worldThread.noReachBlockList[blockPosition.y, blockPosition.x] = true;
                                worldThread.noReachBlockList[blockPosition.y-1, blockPosition.x] = true;
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y, blockNameHere.Replace("Side", ""));
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y-1, worldThread.solidBlockList[blockPosition.y-1, blockPosition.x].Replace("Side", ""));
                                worldThread.solidBlockList[blockPosition.y, blockPosition.x] = blockNameHere.Replace("Side", "");
                                worldThread.solidBlockList[blockPosition.y-1, blockPosition.x] = worldThread.solidBlockList[blockPosition.y-1, blockPosition.x].Replace("Side", "");
                            }else {
                                useAudio.PlayUse(blockPosition.x, blockPosition.y, "DoorClose");
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y-1, "Air");
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y, "Air");
                                worldThread.noReachBlockList[blockPosition.y, blockPosition.x] = false;
                                worldThread.noReachBlockList[blockPosition.y-1, blockPosition.x] = false;
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y, blockNameHere.Replace("Upper", "SideUpper"));
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y-1, worldThread.solidBlockList[blockPosition.y-1, blockPosition.x].Replace("Lower", "SideLower"));
                                worldThread.solidBlockList[blockPosition.y, blockPosition.x] = blockNameHere.Replace("Upper", "SideUpper");
                                worldThread.solidBlockList[blockPosition.y-1, blockPosition.x] = worldThread.solidBlockList[blockPosition.y-1, blockPosition.x].Replace("Lower", "SideLower");
                            }
                        }else if (blockNameHere.Contains("Lower")) {
                            if (blockNameHere.Contains("Side")) {
                                useAudio.PlayUse(blockPosition.x, blockPosition.y, "DoorOpen");
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y+1, "Air");
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y, "Air");
                                worldThread.noReachBlockList[blockPosition.y+1, blockPosition.x] = true;
                                worldThread.noReachBlockList[blockPosition.y, blockPosition.x] = true;
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y+1, worldThread.solidBlockList[blockPosition.y+1, blockPosition.x].Replace("Side", ""));
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y, blockNameHere.Replace("Side", ""));
                                worldThread.solidBlockList[blockPosition.y+1, blockPosition.x] = worldThread.solidBlockList[blockPosition.y+1, blockPosition.x].Replace("Side", "");
                                worldThread.solidBlockList[blockPosition.y, blockPosition.x] = blockNameHere.Replace("Side", "");
                            }else {
                                useAudio.PlayUse(blockPosition.x, blockPosition.y, "DoorClose");
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y+1, "Air");
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y, "Air");
                                worldThread.noReachBlockList[blockPosition.y+1, blockPosition.x] = false;
                                worldThread.noReachBlockList[blockPosition.y, blockPosition.x] = false;
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y+1, worldThread.solidBlockList[blockPosition.y+1, blockPosition.x].Replace("Upper", "SideUpper"));
                                worldThread.SetBlock(worldThread.solidBlockTileMap, blockPosition.x, blockPosition.y, blockNameHere.Replace("Lower", "SideLower"));
                                worldThread.solidBlockList[blockPosition.y+1, blockPosition.x] = worldThread.solidBlockList[blockPosition.y+1, blockPosition.x].Replace("Upper", "SideUpper");
                                worldThread.solidBlockList[blockPosition.y, blockPosition.x] = blockNameHere.Replace("Lower", "SideLower");
                            }
                        }
                    }
                }
            }
            // 处理长按
            else if (_touchDuration[touch.fingerId] >= 0.26f) {
                Vector3Int blockPosition = TouchToBlockPosition(touch);
                worldThread.SetGUI(worldThread.destroyTileMapFront, blockPosition.x, blockPosition.y, "Air");
                worldThread.SetGUI(worldThread.destroyTileMapBack, blockPosition.x, blockPosition.y, "Air");
                magnifier.SetActive(false);
            }

            if (_touchDuration.ContainsKey(touch.fingerId))
                _touchDuration.Remove(touch.fingerId);
            if (_airTouchedFingerIdList.Contains(touch.fingerId))
                _airTouchedFingerIdList.Remove(touch.fingerId);
        }
        for (int i = 0; i < 9; i++) {
            if (_dropPressedFingerIdList[i] != -1) {
                _itemBarButtonList[i].startPressed = false;
                _itemBarButtonList[i].timerPressed = 0;
                _dropPressedFingerIdList[i] = -1;
            }
        }

        if (furnacePressTouchID.Equals(touch.fingerId))
        {
            furnaceContent.furnaceGridList[furnaceContent.pressSort].UpdatePressBar(0);
            furnacePressTouchID = -1;
            furnaceContent.presssed = false;
        }
        
        if (chestPressTouchID.Equals(touch.fingerId))
        {
            if (chestContent.chestPressType.Equals("inventory")) {
                chestContent.inventoryChestGridList[chestContent.pressSort].UpdatePressBar(0);
            }else if (chestContent.chestPressType.Equals("chest")) {
                chestContent.chestGridList[chestContent.pressSort].UpdatePressBar(0);
            }
            chestPressTouchID = -1;
            chestContent.presssed = false;
        }
    }

    public bool CanPlaceAttachableBlock(int x, int y) {
        bool canPlace = false;
        // if(x >= 1 && !worldThread.solidBlockList[y,x-1].Equals("Air") && !IndexAll.BlockNameToUntouchable(worldThread.solidBlockList[y,x-1]))
        //     canPlace = true;
        // else if(x < worldThread.width-1 && !worldThread.solidBlockList[y,x+1].Equals("Air") && !IndexAll.BlockNameToUntouchable(worldThread.solidBlockList[y,x+1]))
        //     canPlace = true;
        if(y >= 1 && !worldThread.solidBlockList[y-1,x].Equals("Air") && !IndexAll.BlockNameToUntouchable(worldThread.solidBlockList[y-1,x]))
            canPlace = true;
        // else if(y < worldThread.height-1 && !worldThread.solidBlockList[y+1,x].Equals("Air") && !IndexAll.BlockNameToUntouchable(worldThread.solidBlockList[y+1,x]))
        //     canPlace = true;
        else if(!worldThread.backBlockList[y,x].Equals("Air"))
            canPlace = true;
        return canPlace;
    }

    public void UpdateNearbyBlock(int x, int y) {
        if(x >= 1 && IndexAll.BlockNameToIsAttachable(worldThread.solidBlockList[y,x-1])) {
            if (!CanPlaceAttachableBlock(x-1, y)) {
                SummonItemFallen(x-1, y);
                worldThread.SetBlock(worldThread.solidBlockTileMap, x-1,
                    y, "Air");
                worldThread.solidBlockList[y, x-1] = "Air";
                worldThread.noReachBlockList[y, x-1] = false;
                DeleteLightSource(x-1, y);
            }
        }
        else if(x < worldThread.width-1 && IndexAll.BlockNameToIsAttachable(worldThread.solidBlockList[y,x+1])) {
            if (!CanPlaceAttachableBlock(x+1, y)) {
                SummonItemFallen(x+1, y);
                worldThread.SetBlock(worldThread.solidBlockTileMap, x+1,
                    y, "Air");
                worldThread.solidBlockList[y, x+1] = "Air";
                worldThread.noReachBlockList[y, x+1] = false;
                DeleteLightSource(x+1, y);
            }
        }
        else if(y >= 1 && IndexAll.BlockNameToIsAttachable(worldThread.solidBlockList[y-1,x])) {
            if (!CanPlaceAttachableBlock(x , y-1)) {
                SummonItemFallen(x, y-1);
                worldThread.SetBlock(worldThread.solidBlockTileMap, x,
                    y-1, "Air");
                worldThread.solidBlockList[y-1, x] = "Air";
                worldThread.noReachBlockList[y-1, x] = false;
                DeleteLightSource(x, y-1);
            }
        }
        else if(y < worldThread.height-1 && IndexAll.BlockNameToIsAttachable(worldThread.solidBlockList[y+1,x])) {
            if (!CanPlaceAttachableBlock(x , y+1)) {
                SummonItemFallen(x, y+1);
                worldThread.SetBlock(worldThread.solidBlockTileMap, x,
                    y+1, "Air");
                worldThread.solidBlockList[y+1, x] = "Air";
                worldThread.noReachBlockList[y+1, x] = false;
                DeleteLightSource(x, y+1);
            }
        }
        if (y >= 1 && worldThread.solidBlockList[y-1,x].Contains("Lower")) {
            if(worldThread.solidBlockList[y,x].Equals("Air")) {
                worldThread.SetBlock(worldThread.solidBlockTileMap, x,
                    y-1, "Air");
                worldThread.solidBlockList[y-1, x] = "Air";
                worldThread.noReachBlockList[y-1, x] = false;
            }
        } 
        else if (y < worldThread.height - 1 && worldThread.solidBlockList[y+1,x].Contains("Upper")) {
            if (worldThread.solidBlockList[y,x].Equals("Air")) {
                worldThread.SetBlock(worldThread.solidBlockTileMap, x,
                    y+1, "Air");
                worldThread.solidBlockList[y+1, x] = "Air";
                worldThread.noReachBlockList[y+1, x] = false;
            }
        }
        else if (y < worldThread.height - 1 && worldThread.solidBlockList[y+1,x].Contains("Lower")) {
            if (worldThread.solidBlockList[y,x].Equals("Air")) {
                SummonItemFallen(x, y+1);
                worldThread.SetBlock(worldThread.solidBlockTileMap, x,
                    y+1, "Air");
                worldThread.solidBlockList[y+1, x] = "Air";
                worldThread.noReachBlockList[y+1, x] = false;
                
                worldThread.SetBlock(worldThread.solidBlockTileMap, x,
                    y+2, "Air");
                worldThread.solidBlockList[y+2, x] = "Air";
                worldThread.noReachBlockList[y+2, x] = false;
            }
        }

        if (y < worldThread.height - 1 && worldThread.solidBlockList[y+1,x].Contains("Sapling")) {
            if (worldThread.solidBlockList[y,x].Equals("Air")) {
                SummonItemFallen(x, y+1);
                worldThread.SetBlock(worldThread.solidBlockTileMap, x,
                    y+1, "Air");
                worldThread.solidBlockList[y+1, x] = "Air";
                worldThread.noReachBlockList[y+1, x] = false;
            }
        }
    }

    public void SummonItemFallen(int x, int y) {
        String itemName = IndexAll.blockNameToItemName(worldThread.solidBlockList[y, x], "null");
        if(itemName != "Air"){
            itemPrefab.SetActive(true);
            GameObject item = Instantiate(itemPrefab,
                new Vector3(x + 0.5f, y + 0.5f, 0),
                Quaternion.identity, items.transform);
            ItemThread itemThread = item.gameObject.GetComponent<ItemThread>();
            itemThread.itemInit(itemName, 1, 0);
            if (IndexAll.BlockNameToIsLight(itemThread.nameItem)) {
                item.GetComponentsInChildren<Transform>()[2].GetComponent<Light2D>().enabled = true;
            }
            itemPrefab.SetActive(false);
        }
    }
    
    public void DeleteLightSource(int x, int y) {
        foreach (var torchLight2D in worldThread.torchLight2DList) {
            if (torchLight2D.x.Equals(x) &&
                torchLight2D.y.Equals(y)) {
                worldThread.torchLight2DList.Remove(torchLight2D);
                Destroy(torchLight2D.gameObject);
                break;
            }
        }
    }

    public void OpenFurnaceUI(int xBlock, int yBlock)
    {
        foreach (var furnace in worldThread.furnaceList)
        {
            if (furnace.xBlock == xBlock && furnace.yBlock == yBlock)
            {
                furnace.furnaceContent = furnaceContent;
                furnaceContent.furnaceThread = furnace;
                furnace.connected = true;
                break;
            }   
        }
        furnaceUI.SetActive(true);
        chestUI.SetActive(false);
        furnaceContent.UpdateAllFurnaceGrid();
    }
    
    public void OpenChestUI(int xBlock, int yBlock)
    {
        bool stopSearching = false;
        foreach (var chest in worldThread.chestList)
        {
            foreach (var position in chest.blockPositionList)
            {
                if(position.x == xBlock && position.y == yBlock)
                {
                    // chest.chestContent = chestContent;
                    // chest.connected = true;
                    chestContent.chestThread = chest;
                    chest.OpenChest();
                    stopSearching = true;
                    break;
                }
            }
            if (stopSearching) break;
        }

        if (chestContent.chestThread.volume < 30)
        { 
            chestTitle.text = "小型箱子";
        }
        else
        {
            chestTitle.text = "大型箱子";
        }
        chestUI.SetActive(true);
        furnaceUI.SetActive(false);
        // chestContent.UpdateAllFurnaceGrid();
    }
    
    public void UpdateMagnifierOn() {
        magnifierOn = magnifierToggle.isOn;
    }
}