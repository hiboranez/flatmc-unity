using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryExitButton : MonoBehaviour
{
    public GameObject InventoryUI;
    public AudioClip audioClip;
    public AudioSource audioSourcePlayer;
    public InventoryContent inventoryContent;
    public PlayerThread playerThread;
    void Start() {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }

    private void OnClickCallBack() {
        playerThread.onCraftingTable = false;
        audioSourcePlayer.PlayOneShot(audioClip, 1f);
        inventoryContent.freshed = false;
        InventoryUI.SetActive(false);
    }
}