using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMoreButton : MonoBehaviour
{
    public GameObject InventoryUI;
    public GameObject craftingUI;
    public GameObject inventoryUI;
    public GameObject armorUI;
    public GameObject furnaceUI;
    public GameObject chestUI;
    public TabButtonNew tabButtonNew1;
    public TabButtonNew tabButtonNew2;
    void Start() {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }

    private void OnClickCallBack() {
        furnaceUI.SetActive(false);
        chestUI.SetActive(false);
        InventoryUI.SetActive(true);
        if (craftingUI.activeInHierarchy) {
            tabButtonNew2.UpdateAllCraftGrid(true);
            tabButtonNew2.UpdateCraftGrid();
        }
        craftingUI.SetActive(false);
        armorUI.SetActive(false);
        inventoryUI.SetActive(true);
        tabButtonNew1.SwitchThisButton();
    }
}
