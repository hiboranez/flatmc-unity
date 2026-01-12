using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

public class ItemBar : MonoBehaviour {
    public PlayerThread playerThread;
    public Transform[] childTransforms1;
    public ItemBarButton[] itemBarButtonList;
    void Start() {
        itemBarButtonList = new ItemBarButton[9];
        childTransforms1 = GetComponentsInRealChildren<Transform>(gameObject,true);
        for (int i = 0; i < 9; i++) {
            itemBarButtonList[i] = childTransforms1[i].GetComponent<ItemBarButton>();
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
        for (int i = 0; i < 9; i++) {
            UpdateSingle(i);
        }
    }
    
    public void UpdateSingle(int sort) {
        ItemBarButton itemBarButton = childTransforms1[sort].gameObject.GetComponent<ItemBarButton>();
        Transform[] childTransforms2 = childTransforms1[sort].gameObject.GetComponentsInChildren<Transform>();
        Image imageIcon = childTransforms2[2].gameObject.GetComponent<Image>();
        TMP_Text textAmount = childTransforms2[3].gameObject.GetComponent<TMP_Text>();
        Sprite sprite = Resources.Load<Sprite>("Icons/" + playerThread.InventoryName[itemBarButton.InventorySort]);
        if (sprite == null) {
            imageIcon.enabled = false;
            textAmount.text = "";
            itemBarButton.amountBarBack.SetActive(false);
            itemBarButton.amountBar.SetActive(false);
        }
        else { 
            imageIcon.sprite = sprite;
            imageIcon.enabled = true;
            if (IndexAll.nameToIsDurable(playerThread.InventoryName[itemBarButton.InventorySort])) {
                float length = 90 * ((float)playerThread.InventoryAmount[itemBarButton.InventorySort] / IndexAll.nameToMaxAmount(playerThread.InventoryName[itemBarButton.InventorySort]));
                RectTransform amountBarRectTransform = itemBarButton.amountBar.GetComponent<RectTransform>();
                Image amountBarImage = itemBarButton.amountBar.GetComponent<Image>();
                if(length > 60 && length <= 90) amountBarImage.color = Color.green;
                else if(length > 30 && length <= 60) amountBarImage.color = Color.yellow;
                else if(length >= 0 && length <= 30) amountBarImage.color = Color.red;
                amountBarRectTransform.sizeDelta = new Vector2(length, amountBarRectTransform.sizeDelta.y);
                amountBarRectTransform.anchoredPosition = new Vector2(-(90 - length) / 2, amountBarRectTransform.anchoredPosition.y);
                itemBarButton.amountBarBack.SetActive(true);
                itemBarButton.amountBar.SetActive(true);
            }
            else {
                itemBarButton.textMeshPro.text = playerThread.InventoryAmount[itemBarButton.InventorySort].ToString();
                if (itemBarButton.textMeshPro.text == "1") itemBarButton.textMeshPro.text = "";
                itemBarButton.amountBarBack.SetActive(false);
                itemBarButton.amountBar.SetActive(false);
            }
        }
    }
}
