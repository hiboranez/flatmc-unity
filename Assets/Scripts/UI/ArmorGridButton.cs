using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ArmorGridButton : MonoBehaviour
{
    public Image whiteBackImageHelmet;
    public Image whiteBackImageChest;
    public Image whiteBackImageLeggings;
    public Image whiteBackImageBoots;
    public ArmorGrid armorGrid;
    public AudioSource cameraAudioSource;
    public AudioClip selectAudioClip;
    public PlayerThread playerThread;
    public Image iconHelmet;
    public Image iconChest;
    public Image iconLeggings;
    public Image iconBoots;
    public Image iconArmorHelmet;
    public Image iconArmorChest;
    public Image iconArmorLeggings;
    public Image iconArmorBoots;
    public ArmorContent armorContent;
    public TabButtonNew tabButton;
    public Image amountBarHelmet;
    public Image amountBarBackHelmet;
    public Image amountBarChest;
    public Image amountBarBackChest;
    public Image amountBarLeggings;
    public Image amountBarBackLeggings;
    public Image amountBarBoots;
    public Image amountBarBackBoots;
    
    public void OnClickCallBack()
    {
        string gridArmorName = playerThread.InventoryName[armorGrid.inventorySort];
        int gridArmorAmount = playerThread.InventoryAmount[armorGrid.inventorySort];
        if (armorGrid.armorName.Contains("Helmet"))
        {
            if (playerThread.armorHelmet.Equals("null"))
            {
                playerThread.InventoryName[armorGrid.inventorySort] = "Air";
                playerThread.InventoryAmount[armorGrid.inventorySort] = 0;
            }
            else {
                playerThread.InventoryName[armorGrid.inventorySort] = playerThread.armorHelmet;
                playerThread.InventoryAmount[armorGrid.inventorySort] = playerThread.armorHelmetAmount;
            }
            playerThread.armorHelmet = gridArmorName;
            playerThread.armorHelmetAmount = gridArmorAmount;
            iconArmorHelmet.sprite = Resources.Load<Sprite>("Icons/" + armorGrid.armorName);
            iconArmorHelmet.enabled = true;
            iconHelmet.enabled = false;
            amountBarHelmet.enabled = true;
            amountBarBackHelmet.enabled = true;
            armorContent.StartFlash(whiteBackImageHelmet);
        }else if (armorGrid.armorName.Contains("Chest"))
        {
            if (playerThread.armorChest.Equals("null"))
            {
                playerThread.InventoryName[armorGrid.inventorySort] = "Air";
                playerThread.InventoryAmount[armorGrid.inventorySort] = 0;
            }
            else {
                playerThread.InventoryName[armorGrid.inventorySort] = playerThread.armorChest;
                playerThread.InventoryAmount[armorGrid.inventorySort] = playerThread.armorChestAmount;
            }
            playerThread.armorChest = gridArmorName;
            playerThread.armorChestAmount = gridArmorAmount;
            iconArmorChest.sprite = Resources.Load<Sprite>("Icons/" + armorGrid.armorName);
            iconArmorChest.enabled = true;
            iconChest.enabled = false;
            amountBarChest.enabled = true;
            amountBarBackChest.enabled = true;
            armorContent.StartFlash(whiteBackImageChest);
        }else if (armorGrid.armorName.Contains("Leggings"))
        {
            if (playerThread.armorLeggings.Equals("null"))
            {
                playerThread.InventoryName[armorGrid.inventorySort] = "Air";
                playerThread.InventoryAmount[armorGrid.inventorySort] = 0;
            }
            else {
                playerThread.InventoryName[armorGrid.inventorySort] = playerThread.armorLeggings;
                playerThread.InventoryAmount[armorGrid.inventorySort] = playerThread.armorLeggingsAmount;
            }
            playerThread.armorLeggings = gridArmorName;
            playerThread.armorLeggingsAmount = gridArmorAmount;
            iconArmorLeggings.sprite = Resources.Load<Sprite>("Icons/" + armorGrid.armorName);
            iconArmorLeggings.enabled = true;
            iconLeggings.enabled = false;
            amountBarLeggings.enabled = true;
            amountBarBackLeggings.enabled = true;
            armorContent.StartFlash(whiteBackImageLeggings);
        }else if (armorGrid.armorName.Contains("Boots"))
        {
            if (playerThread.armorBoots.Equals("null"))
            {
                playerThread.InventoryName[armorGrid.inventorySort] = "Air";
                playerThread.InventoryAmount[armorGrid.inventorySort] = 0;
            }
            else {
                playerThread.InventoryName[armorGrid.inventorySort] = playerThread.armorBoots;
                playerThread.InventoryAmount[armorGrid.inventorySort] = playerThread.armorBootsAmount;
            }
            playerThread.armorBoots = gridArmorName;
            playerThread.armorBootsAmount = gridArmorAmount;
            iconArmorBoots.sprite = Resources.Load<Sprite>("Icons/" + armorGrid.armorName);
            iconArmorBoots.enabled = true;
            iconBoots.enabled = false;
            amountBarBoots.enabled = true;
            amountBarBackBoots.enabled = true;
            armorContent.StartFlash(whiteBackImageBoots);
        }
        cameraAudioSource.PlayOneShot(selectAudioClip);
        tabButton.UpdateArmorGrid();
        armorContent.UpdateArmorModel();
        tabButton.UpdateArmorSlotAmount();
        playerThread.UpdateArmorValue();
    }
}
