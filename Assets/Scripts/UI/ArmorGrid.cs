using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class ArmorGrid : MonoBehaviour
    {
        public PlayerThread playerThread;
        public Image amountBarImage;
        public RectTransform amountBarRectTransform;
        public Image iconImage;
        public string armorName;
        public int inventorySort;
        
        public void GridInit(int sort)
        {
            inventorySort = sort;
            float length = 105 * ((float)playerThread.InventoryAmount[inventorySort] / IndexAll.nameToMaxAmount(playerThread.InventoryName[inventorySort]));
            if(length > 70 && length <= 105) amountBarImage.color = Color.green;
            else if(length > 35 && length <= 70) amountBarImage.color = Color.yellow;
            else if(length >= 0 && length <= 35) amountBarImage.color = Color.red;
            amountBarRectTransform.sizeDelta = new Vector2(length, amountBarRectTransform.sizeDelta.y);
            amountBarRectTransform.anchoredPosition = new Vector2(-(105 - length) / 2, amountBarRectTransform.anchoredPosition.y);
            armorName = playerThread.InventoryName[inventorySort];
            iconImage.sprite = Resources.Load<Sprite>("Icons/" + armorName);
            iconImage.enabled = true;
        }
    }
}