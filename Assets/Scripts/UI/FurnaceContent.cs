using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace UI
{
    public class FurnaceContent : MonoBehaviour
    {
        public string selection;
        public RectTransform selectRectTransform;
        public ItemBar itemBar;
        public FurnaceGrid[] furnaceGridList;
        public FurnaceThread furnaceThread;
        public FurnaceMaterialGrid furnaceMaterialGrid;
        public FurnaceFuelGrid furnaceFuelGrid;
        public FurnaceProductGrid furnaceProductGrid;
        public RectTransform fireRectTransform;
        public RectTransform arrowRectTransform;
        public WorldThread worldThread;
        public float timerPressed;
        public bool presssed;
        public int pressSort;
        
        private void Update()
        {
            if (presssed)
            {
                timerPressed += Time.deltaTime;
                if (timerPressed > 0.75f)
                {
                    furnaceGridList[pressSort].UpdatePressBar(timerPressed);
                }
            }
            else
            {
                timerPressed = 0;
            }
        }

        private void Awake()
        {
            furnaceGridList = IndexAll.GetComponentsInRealChildren<FurnaceGrid>(gameObject, true);
            selection = "material";
        }

        private void Start()
        {
            foreach (var furnaceGrid in furnaceGridList)
            {
                furnaceGrid.UpdatePressBar(0);
            }
        }

        private void OnEnable()
        {
            UpdateFurnaceUI();
        }

        private void OnDisable()
        {
            furnaceThread.connected = false;
            furnaceThread.furnaceContent = null;
            furnaceThread = null;
        }

        public void UpdateFurnaceUI()
        {
            furnaceMaterialGrid.materialName = furnaceThread.material;
            furnaceMaterialGrid.amount = furnaceThread.amountMaterial;
            furnaceFuelGrid.fuelName = furnaceThread.fuel;
            furnaceFuelGrid.amount = furnaceThread.amountFuel;
            furnaceProductGrid.productName = furnaceThread.product;
            furnaceProductGrid.amount = furnaceThread.amountProduct;
            furnaceMaterialGrid.UpdateMaterialGrid();
            furnaceMaterialGrid.UpdateAmountBar();
            furnaceFuelGrid.UpdateFuelGrid();
            furnaceFuelGrid.UpdateAmountBar();
            furnaceProductGrid.UpdateProductGrid();
            fireRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 260*(furnaceThread.timeLeft/(furnaceThread.timeTotal+0.01f)));
            arrowRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 320*(furnaceThread.progressTimer/10f));
        }

        public void UpdateAllFurnaceGrid()
        {
            foreach (var furnaceGrid in furnaceGridList)
            {
                furnaceGrid.UpdateGrid();
            }
        }
        
        public void UpdateItemBar()
        {
            itemBar.UpdateAll();
        }
        
        public void SelectToType(string type)
        {
            selection = type;
            if (type.Equals("material"))
            {
                selectRectTransform.anchoredPosition = new Vector2(250, 200);
            }
            else if (type.Equals("fuel"))
            {
                selectRectTransform.anchoredPosition = new Vector2(250, -200);
            }
        }
    }
}