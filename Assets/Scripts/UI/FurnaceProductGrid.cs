using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceProductGrid : MonoBehaviour
{
    public PlayerThread playerThread;
    public FurnaceContent furnaceContent;
    public NameTextThread nameTextThread;
    public Image iconImage;
    public TMP_Text amountText;
    public string productName;
    public int amount;

    private void Awake()
    {
        productName = "null";
        amount = 0;
    }

    public void UpdateProductGrid()
    {
        if (!productName.Equals("null"))
        {
            iconImage.sprite = Resources.Load<Sprite>("Icons/" + productName);
            amountText.text = amount.ToString();
            iconImage.enabled = true;
            amountText.enabled = true;
        }
        else
        {
            iconImage.sprite = null;
            amountText.text = "0";
            amountText.enabled = false;
            iconImage.enabled = false;
        }
        if (amountText.text == "1") amountText.text = "";
    }
    
    public void ClickOnCallBack()
    {
        if(playerThread.IfGetItemLeft(productName, amount, 36,false) <= amount){
            int left = playerThread.getItem(productName, amount, 36, true);
            amount = left;
            if(left <= 0) productName = "null";
            UpdateProductGrid();
            furnaceContent.UpdateAllFurnaceGrid();
            furnaceContent.furnaceThread.product = productName;
            furnaceContent.furnaceThread.amountProduct = amount;
        }else
        {
            nameTextThread.nameText.text = "背包已满";
            nameTextThread.timer = 1.5f;
        }
    }
}
