using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class SinglePlayerDeleteButton : MonoBehaviour {
    public GameObject world;
    public SinglePlayerWorldContent singlePlayerWorldContent;
    public SinglePlayerWorldThread singlePlayerWorldThread;
    public AudioClip clickAudioClip;
    public AudioSource audioSource;
        
    private void Awake() {
        singlePlayerWorldThread = world.GetComponent<SinglePlayerWorldThread>();
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }
        
    private void OnClickCallBack() {
        audioSource.PlayOneShot(clickAudioClip, 1f);
        singlePlayerWorldContent.worldList.Remove(singlePlayerWorldThread);
        // 指定文件夹路径
        string folderPath = Application.persistentDataPath + "/Worlds/" + singlePlayerWorldThread.tmpText.text;
        // 检查文件夹是否存在
        if (Directory.Exists(folderPath))
        {
            // 删除文件夹及其内部所有文件
            Directory.Delete(folderPath, true);
        }
        Destroy(world);
    }
}
