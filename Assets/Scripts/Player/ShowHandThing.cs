using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Util;

public class ShowHandThing : MonoBehaviour
{
    public PlayerThread playerThread;
    public ItemBar itemBar;
    public String type;
    public GameObject handItem;
    public GameObject handTool;
    private SpriteRenderer _spriteRenderer;
    private String _lastName;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lastName = "null";
    }

    void Update()
    {
        String nameInHand = playerThread.InventoryName[itemBar.itemBarButtonList[playerThread.ItemBarChosen].InventorySort];
        if (nameInHand.Equals("Air"))
        {
            _lastName = nameInHand;
            _spriteRenderer.sprite = null;
        }
        if (!nameInHand.Equals("Air") && !nameInHand.Equals(_lastName))
        {
            _lastName = nameInHand;
            if (IndexAll.nameToIsTool(nameInHand) && type.Equals("item"))
            {
                _spriteRenderer.sprite = null;
                handItem.SetActive(false);
                handTool.SetActive(true); 
            }else if(!IndexAll.nameToIsTool(nameInHand) && type.Equals("tool"))
            {
                _spriteRenderer.sprite = null;
                handTool.SetActive(false);
                handItem.SetActive(true);
            }
            else
            {
                _spriteRenderer.sprite = Resources.Load<Sprite>("Icons/" + nameInHand);
            }
        }
    }
}
