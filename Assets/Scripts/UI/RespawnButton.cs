using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnButton : MonoBehaviour {
    public GameObject deathUI;
    public AudioSource cameraAudioSource;
    public AudioClip clickAudioClip;
    public WorldThread worldThread;
    public PlayerThread playerThread;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }

    private void OnClickCallBack() {
        cameraAudioSource.PlayOneShot(clickAudioClip);
        playerThread.transform.position =
            new Vector3(worldThread.xWorldSpawn, worldThread.yWorldSpawn, playerThread.transform.position.z);
        playerThread.health = 20;
        playerThread.hunger = 20;
        playerThread.playerRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        playerThread.modelRoot.SetActive(true);
        playerThread.skinnedMeshRenderer.enabled = true;
        playerThread.headSpriteRenderer.enabled = true;
        playerThread.playerNameText.enabled = true;
        playerThread.dead = false;
        playerThread.breathValue = 20;
        playerThread.velocityYLast = 0;
        playerThread.playerRigidbody2D.velocity = new Vector2(0,0);
        deathUI.SetActive(false);
    }
}
