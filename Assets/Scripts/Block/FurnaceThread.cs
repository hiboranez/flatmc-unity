using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Util;

public class FurnaceThread : MonoBehaviour
{
    public int xBlock;
    public int yBlock;
    public string material;
    public string fuel;
    public string product;
    public int amountMaterial;
    public int amountFuel;
    public int amountProduct;
    public float timeTotal;
    public float timeLeft;
    public float progressTimer;
    public bool onBurning;
    public bool connected;
    public FurnaceContent furnaceContent;
    public WorldThread worldThread;
    public GameObject light;
    public GameObject items;
    public GameObject itemPrefab;
    private string _lastMaterial;
    private bool _updateLastProgressState;
    
    void Awake()
    {
        material = "null";
        fuel = "null";
        product = "null";
        amountMaterial = 0;
        amountFuel = 0;
        amountProduct = 0;
        timeTotal = 0;
        timeLeft = 0;
        progressTimer = 0;
        onBurning = false;
        _updateLastProgressState = false;
        light.SetActive(false);
    }
    
    void Update()
    {
        CheckState();
        if (onBurning)
        {
            timeLeft -= Time.deltaTime;
            progressTimer += Time.deltaTime;
            if (progressTimer >= 10f)
            {
                progressTimer = 0;
                if (product.Equals("null"))
                {
                    product = IndexAll.nameToBurnProduct(material);
                    if(connected) furnaceContent.furnaceProductGrid.productName = product;
                }
                amountProduct++;
                amountMaterial--;
                if (amountMaterial <= 0) material = "null";
                if(connected){
                    furnaceContent.furnaceProductGrid.amount = amountProduct;
                    furnaceContent.furnaceMaterialGrid.amount = amountMaterial;
                    furnaceContent.UpdateFurnaceUI();
                }
            }
            UpdateFurnaceProgressUI();
        }else {
            timeTotal = 0;
            timeLeft = 0;
            progressTimer = 0;
            if(!_updateLastProgressState)
            {
                light.SetActive(false);
                worldThread.SetBlock(worldThread.solidBlockTileMap, xBlock, yBlock, "FurnaceOff");
                worldThread.solidBlockList[yBlock, xBlock] = "FurnaceOff";
                if(connected) {
                    UpdateFurnaceProgressUI();
                    _updateLastProgressState = true;
                }
            }
        }
    }

    public void DestroyFurnace()
    {
        if (!material.Equals("null"))
        {
            itemPrefab.SetActive(true);
            GameObject item1 = Instantiate(itemPrefab,
                new Vector3(xBlock + 0.5f, yBlock + 0.5f, 0),
                Quaternion.identity, items.transform);
            ItemThread itemThread1 = item1.gameObject.GetComponent<ItemThread>();
            itemThread1.itemInit(material, amountMaterial, 0);
            if (IndexAll.BlockNameToIsLight(itemThread1.nameItem)) {
                item1.GetComponentsInChildren<Transform>()[2].GetComponent<Light2D>().enabled = true;
            } 
            itemPrefab.SetActive(false);
        }
        
        if (!fuel.Equals("null"))
        {
            itemPrefab.SetActive(true);
            GameObject item2 = Instantiate(itemPrefab,
                new Vector3(xBlock + 0.5f, yBlock + 0.5f, 0),
                Quaternion.identity, items.transform);
            ItemThread itemThread2 = item2.gameObject.GetComponent<ItemThread>();
            itemThread2.itemInit(fuel, amountFuel, 0);
            if (IndexAll.BlockNameToIsLight(itemThread2.nameItem))
            {
                item2.GetComponentsInChildren<Transform>()[2].GetComponent<Light2D>().enabled = true;
            }
            itemPrefab.SetActive(false);
        }
        
        if (!product.Equals("null"))
        {
            itemPrefab.SetActive(true);
            GameObject item3 = Instantiate(itemPrefab,
                new Vector3(xBlock + 0.5f, yBlock + 0.5f, 0),
                Quaternion.identity, items.transform);
            ItemThread itemThread3 = item3.gameObject.GetComponent<ItemThread>();
            itemThread3.itemInit(product, amountProduct, 0);
            if (IndexAll.BlockNameToIsLight(itemThread3.nameItem))
            {
                item3.GetComponentsInChildren<Transform>()[2].GetComponent<Light2D>().enabled = true;
            }
            itemPrefab.SetActive(false);
        }

        worldThread.furnaceList.Remove(this);
        Destroy(gameObject);
    }
    
    public void UpdateFurnaceProgressUI()
    {
        if(connected)
        {
            furnaceContent.fireRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                260 * (timeLeft / (timeTotal+0.01f)));
            furnaceContent.arrowRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                320 * (progressTimer / 10f));
        } 
    }
    
    public void CheckState()
    {
        if (timeLeft <= 0 && !material.Equals("null") && amountMaterial > 0 && !fuel.Equals("null") && amountFuel > 0)
        {
            if(!IndexAll.nameToBurnProduct(material).Equals("null") && IndexAll.nameToBurnTime(fuel) > 0)
            {
                bool canBurn = false;
                if (!product.Equals("null")) {
                    if (IndexAll.nameToBurnProduct(material).Equals(product)) {
                        canBurn = true;
                    }
                } else {
                    canBurn = true;
                }

                if (amountProduct >= IndexAll.nameToMaxAmount(product)) canBurn = false;
                
                if(canBurn){
                    timeTotal = IndexAll.nameToBurnTime(fuel);
                    timeLeft = timeTotal;
                    onBurning = true;
                    _updateLastProgressState = false;
                    if (IndexAll.nameToIsDurable(fuel))
                    {
                        amountFuel = 0;
                        fuel = "null";
                    }
                    else
                    {
                        amountFuel--;
                        if (amountFuel <= 0) fuel = "null";
                    }
                    light.SetActive(true);

                    if (connected) {
                        furnaceContent.furnaceFuelGrid.fuelName = fuel;
                        furnaceContent.furnaceFuelGrid.amount = amountFuel;
                        furnaceContent.furnaceFuelGrid.UpdateFuelGrid();
                        furnaceContent.furnaceFuelGrid.UpdateAmountBar();
                        furnaceContent.worldThread.SetBlock(furnaceContent.worldThread.solidBlockTileMap, xBlock, yBlock, "FurnaceOn");
                        furnaceContent.worldThread.solidBlockList[yBlock, xBlock] = "FurnaceOn";
                    }
                }
            }
        } else if (timeLeft <= 0)
        {
            timeTotal = 0;
            progressTimer = 0;
            onBurning = false;
            UpdateFurnaceProgressUI();
        }

        if (!product.Equals("null")) {
            if (!IndexAll.nameToBurnProduct(material).Equals(product)) {
                progressTimer = 0;
                UpdateFurnaceProgressUI();
            }
        }
        
        if (material.Equals("null") || amountMaterial <= 0) {
            progressTimer = 0;
            UpdateFurnaceProgressUI();
        }

        if (!material.Equals(_lastMaterial) || IndexAll.nameToBurnProduct(material).Equals("null"))
        {
            progressTimer = 0;
            UpdateFurnaceProgressUI();
        }
        
        if(!product.Equals("null") && amountProduct >= IndexAll.nameToMaxAmount(product))
        {
            progressTimer = 0;
            UpdateFurnaceProgressUI();
        }
        
        _lastMaterial = material;
    }
}
