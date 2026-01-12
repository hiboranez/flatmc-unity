using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class InventoryContent : MonoBehaviour {
    public PlayerThread playerThread;
    public bool freshed;
    private Transform[] _childTransforms1;
    
    void Awake() {
        freshed = true;
        _childTransforms1 = GetComponentsInRealChildren<Transform>(gameObject,true);
    }

    private void Update() {
        if (!freshed && gameObject.activeInHierarchy) {
            UpdateAll();
            freshed = true;
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
 
    public static T[] GetComponentsInRealChildren<T>(Transform go, bool includeInactive = false) where T : Component
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
    
    public void UpdateAll() {
        for (int i = 0; i < 36; i++) {
            Transform[] childTransforms2 = _childTransforms1[i].gameObject.GetComponentsInChildren<Transform>();
            Image imageIcon = childTransforms2[1].gameObject.GetComponent<Image>();
            TMP_Text textAmount = childTransforms2[2].gameObject.GetComponent<TMP_Text>();
            Sprite sprite = Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[i]);
            InventoryGrid inventoryGrid = _childTransforms1[i].gameObject.GetComponent<InventoryGrid>();
            if (sprite == null) {
                imageIcon.enabled = false;
                textAmount.text = "";
                inventoryGrid.amountBarBack.SetActive(false);
                inventoryGrid.amountBar.SetActive(false);
            }
            else {
                imageIcon.sprite = sprite;
                imageIcon.enabled = true;
                 if (IndexAll.nameToIsDurable(playerThread.InventoryName[i])) {
                    float length = 90 * ((float)playerThread.InventoryAmount[i] / IndexAll.nameToMaxAmount(playerThread.InventoryName[i]));
                    RectTransform amountBarRectTransform = inventoryGrid.amountBar.GetComponent<RectTransform>();
                    Image amountBarImage = inventoryGrid.amountBar.GetComponent<Image>();
                    if(length > 60 && length <= 90) amountBarImage.color = Color.green;
                    else if(length > 30 && length <= 60) amountBarImage.color = Color.yellow;
                    else if(length >= 0 && length <= 30) amountBarImage.color = Color.red;
                    amountBarRectTransform.sizeDelta = new Vector2(length, amountBarRectTransform.sizeDelta.y);
                    amountBarRectTransform.anchoredPosition = new Vector2(-(90 - length) / 2, amountBarRectTransform.anchoredPosition.y);
                    inventoryGrid.amountBarBack.SetActive(true);
                    inventoryGrid.amountBar.SetActive(true);
                }
                else {
                    textAmount.text = playerThread.InventoryAmount[i].ToString();
                    if (textAmount.text == "1") textAmount.text = "";
                    inventoryGrid.amountBarBack.SetActive(false);
                    inventoryGrid.amountBar.SetActive(false);
                }
            }
        }
    }
}
