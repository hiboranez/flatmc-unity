using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;
using UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

public class ItemBarButton : MonoBehaviour {
    public RectTransform rectTransformSelectedItemBar;
    public PlayerThread playerThread;
    public float flashTimer;
    public int ItemBarSort;
    public int InventorySort;
    public Image iconImage;
    public Image whiteBackImage;
    public TMP_Text textMeshPro;
    public GameObject amountBarBack;
    public GameObject amountBar;
    public GameObject DropBarBack;
    public GameObject DropBar;
    public NameTextThread nameTextThread;
    public bool startPressed;
    public float timerPressed;
    public AudioSource audioSourceCamera;
    public AudioClip audioClipPop;
    public GameObject itemPrefab;
    public GameObject items;
    private RectTransform _rectTransform;
    private RectTransform _dropBarRectTransform;
    private Image _dropBarImage;
    
    void Start() {
        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        textMeshPro = childTransforms[3].gameObject.GetComponent<TMP_Text>();
        amountBarBack = childTransforms[4].gameObject;
        amountBar = childTransforms[5].gameObject;
        DropBarBack = childTransforms[6].gameObject;
        DropBar = childTransforms[7].gameObject;
        _dropBarRectTransform = DropBar.GetComponent<RectTransform>();
        _dropBarImage = DropBar.GetComponent<Image>();
        _dropBarImage.color = Color.green;
        amountBarBack.SetActive(false);
        amountBar.SetActive(false);
        DropBarBack.SetActive(false);
        DropBar.SetActive(false);
        InventorySort = ItemBarSort;
        String itemName = playerThread.InventoryName[InventorySort];
        if(itemName != null) {
            Sprite sprite = Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[InventorySort]);
            if (sprite == null) iconImage.enabled = false;
            else {
                iconImage.sprite = sprite;
                iconImage.enabled = true;
                if (IndexAll.nameToIsDurable(playerThread.InventoryName[InventorySort])) {
                    float length = 90 * ((float)playerThread.InventoryAmount[InventorySort] /
                                         IndexAll.nameToMaxAmount(playerThread.InventoryName[InventorySort]));
                    RectTransform amountBarRectTransform = amountBar.GetComponent<RectTransform>();
                    Image amountBarImage = amountBar.GetComponent<Image>();
                    if (length > 60 && length <= 90) amountBarImage.color = Color.green;
                    else if (length > 30 && length <= 60) amountBarImage.color = Color.yellow;
                    else if (length >= 0 && length <= 30) amountBarImage.color = Color.red;
                    amountBarRectTransform.sizeDelta = new Vector2(length, amountBarRectTransform.sizeDelta.y);
                    amountBarRectTransform.anchoredPosition =
                        new Vector2(-(90 - length) / 2, amountBarRectTransform.anchoredPosition.y);
                    amountBarBack.SetActive(true);
                    amountBar.SetActive(true);
                } else {
                    textMeshPro.text = playerThread.InventoryAmount[InventorySort].ToString();
                    if (textMeshPro.text == "1") textMeshPro.text = "";
                    amountBarBack.SetActive(false);
                    amountBar.SetActive(false);
                }
            }
        }
        _rectTransform = GetComponent<RectTransform>();
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }

    private void Update() {
        if (startPressed) {
            timerPressed += Time.deltaTime;
            if (timerPressed >= 0.75f) {
                float timeDrop = timerPressed - 0.75f;
                if (timeDrop >= 1) {
                    itemPrefab.SetActive(true);
                    GameObject item = Instantiate(itemPrefab,playerThread.transform.position,Quaternion.identity, items.transform);
                    ItemThread itemThread = item.gameObject.GetComponent<ItemThread>();
                    itemThread.itemInit(playerThread.InventoryName[InventorySort],playerThread.InventoryAmount[InventorySort],2);
                    if (IndexAll.BlockNameToIsLight(itemThread.nameItem)) {
                        item.GetComponentsInChildren<Transform>()[2].GetComponent<Light2D>().enabled = true;
                    }
                    itemPrefab.SetActive(false);
                    audioSourceCamera.PlayOneShot(audioClipPop,1f);
                    playerThread.InventoryName[InventorySort] = "Air";
                    playerThread.InventoryAmount[InventorySort] = 0;
                    textMeshPro.text = "";
                    iconImage.enabled = false;
                    startPressed = false;
                    timeDrop = 0;
                    amountBarBack.SetActive(false);
                    amountBar.SetActive(false);
                    DropBarBack.SetActive(false);
                    DropBar.SetActive(false);
                }
                float length = 90 * timeDrop;
                _dropBarRectTransform.sizeDelta = new Vector2(length, _dropBarRectTransform.sizeDelta.y);
                _dropBarRectTransform.anchoredPosition = new Vector2(-(90 - length) / 2, _dropBarRectTransform.anchoredPosition.y);
                DropBarBack.SetActive(true);
                DropBar.SetActive(true);
            }
        } else {
            DropBarBack.SetActive(false);
            DropBar.SetActive(false);
        }
    }

    private void OnClickCallBack() {
        if(timerPressed < 0.75f) {
            rectTransformSelectedItemBar.position = _rectTransform.position;
            if (playerThread.ItemBarChosen != ItemBarSort && playerThread.InventoryName[InventorySort] != "Air") {
                nameTextThread.nameText.text = IndexAll.nameToNameShow(playerThread.InventoryName[InventorySort]);
                nameTextThread.nameID = playerThread.InventoryName[InventorySort];
                nameTextThread.timer = 1f;
            }
            playerThread.ItemBarChosen = ItemBarSort;
        }
    }
    
    public IEnumerator Flash() {
        flashTimer = 0.15f;
        while (flashTimer > 0)
        {
            if (flashTimer > 0.12f && flashTimer <= 0.16f) {
                whiteBackImage.enabled = true;
            }else if (flashTimer > 0.08f && flashTimer <= 0.12f) {
                whiteBackImage.enabled = false;
            }else if (flashTimer > 0.04f && flashTimer <= 0.08f) {
                whiteBackImage.enabled = true;
            }else if (flashTimer > 0f && flashTimer <= 0.04f) {
                whiteBackImage.enabled = false;
            }
            // 等待一段时间，例如0.1秒
            yield return new WaitForSeconds(Time.deltaTime);
            // 逐步减小flashTimer的值
            flashTimer -= Time.deltaTime;
        }
    }
}
