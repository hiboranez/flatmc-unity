using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ArmorSlotButton : MonoBehaviour
{
    public string type;
    public Image iconImage;
    public Image armorIconImage;
    public PlayerThread playerThread;
    public NameTextThread nameTextThread;
    public Image amountBar;
    public Image amountBarBack;
    public TabButtonNew tabButton;
    public ArmorContent armorContent;
    
    public void ClickOnCallBack()
    {
        if (type.Equals("helmet") && !playerThread.armorHelmet.Equals("null"))
        {
            if (playerThread.IfGetItemLeft(playerThread.armorHelmet, playerThread.armorHelmetAmount,36,false) > 0) {
                nameTextThread.nameText.text = "背包已满";
                nameTextThread.timer = 1.5f;
            }
            else
            {
                armorIconImage.enabled = true;
                iconImage.enabled = true;
                playerThread.getItem(playerThread.armorHelmet, playerThread.armorHelmetAmount, 36, true);
                playerThread.armorHelmet = "null";
                playerThread.armorHelmetAmount = 0;
                armorIconImage.sprite = null;
                iconImage.sprite = Resources.Load<Sprite>("Icons/EmptySlotHelmet");
                armorIconImage.enabled = false;
                iconImage.enabled = true;
                tabButton.UpdateArmorGrid();
                tabButton.UpdateArmorSlot();
                armorContent.UpdateArmorModel();
                amountBar.enabled = false;
                amountBarBack.enabled = false;
            }
        } else if (type.Equals("chest") && !playerThread.armorChest.Equals("null"))
        {
            if (playerThread.IfGetItemLeft(playerThread.armorChest, playerThread.armorChestAmount,36,false) > 0) {
                nameTextThread.nameText.text = "背包已满";
                nameTextThread.timer = 1.5f;
            }
            else
            {
                armorIconImage.enabled = true;
                iconImage.enabled = true;
                playerThread.getItem(playerThread.armorChest, playerThread.armorChestAmount, 36, true);
                playerThread.armorChest = "null";
                playerThread.armorChestAmount = 0;
                armorIconImage.sprite = null;
                iconImage.sprite = Resources.Load<Sprite>("Icons/EmptySlotChestplate");
                armorIconImage.enabled = false;
                iconImage.enabled = true;
                tabButton.UpdateArmorGrid();
                tabButton.UpdateArmorSlot();
                armorContent.UpdateArmorModel();
                amountBar.enabled = false;
                amountBarBack.enabled = false;
            }
        } else if (type.Equals("leggings") && !playerThread.armorLeggings.Equals("null"))
        {
            if (playerThread.IfGetItemLeft(playerThread.armorLeggings, playerThread.armorLeggingsAmount,36,false) > 0) {
                nameTextThread.nameText.text = "背包已满";
                nameTextThread.timer = 1.5f;
            }
            else
            {
                armorIconImage.enabled = true;
                iconImage.enabled = true;
                playerThread.getItem(playerThread.armorLeggings, playerThread.armorLeggingsAmount, 36, true);
                playerThread.armorLeggings = "null";
                playerThread.armorLeggingsAmount = 0;
                armorIconImage.sprite = null;
                iconImage.sprite = Resources.Load<Sprite>("Icons/EmptySlotLeggings");
                armorIconImage.enabled = false;
                iconImage.enabled = true;
                tabButton.UpdateArmorGrid();
                tabButton.UpdateArmorSlot();
                armorContent.UpdateArmorModel();
                amountBar.enabled = false;
                amountBarBack.enabled = false;
            }
        } else if (type.Equals("boots") && !playerThread.armorBoots.Equals("null"))
        {
            if (playerThread.IfGetItemLeft(playerThread.armorBoots, playerThread.armorBootsAmount,36,false) > 0) {
                nameTextThread.nameText.text = "背包已满";
                nameTextThread.timer = 1.5f;
            }
            else
            {
                armorIconImage.enabled = true;
                iconImage.enabled = true;
                playerThread.getItem(playerThread.armorBoots, playerThread.armorBootsAmount, 36, true);
                playerThread.armorBoots = "null";
                playerThread.armorBootsAmount = 0;
                armorIconImage.sprite = null;
                iconImage.sprite = Resources.Load<Sprite>("Icons/EmptySlotBoots");
                armorIconImage.enabled = false;
                iconImage.enabled = true;
                tabButton.UpdateArmorGrid();
                tabButton.UpdateArmorSlot();
                armorContent.UpdateArmorModel();
                amountBar.enabled = false;
                amountBarBack.enabled = false;
            }
        }
        playerThread.UpdateArmorValue();
    }
}
