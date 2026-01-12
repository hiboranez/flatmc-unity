using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SinglePlayerWorldContent : MonoBehaviour {
    public GameObject worldPrefab;
    public List<SinglePlayerWorldThread> worldList;
    public String worldFilePath;

    private void Awake() {
        worldList = new List<SinglePlayerWorldThread>();
        worldFilePath = Application.persistentDataPath + "/Worlds";
    }

    private void OnEnable() {
        UpdateWorldContent();
    }

    public void UpdateWorldContent() {
        foreach (var world in worldList) {
            Destroy(world.gameObject);
        }
        worldList.Clear();
        Vector3 FirstPosition = new Vector3(8, -100, 0);
        int count = 0;
        if (Directory.Exists(worldFilePath))
        {
            // 获取文件夹中的所有子文件夹
            string[] folders = Directory.GetDirectories(worldFilePath);
            // 遍历输出每个文件夹的名称
            worldPrefab.SetActive(true);
            foreach (string folder in folders)
            {
                // 使用 Path.GetFileName 获取文件夹名称
                string folderName = Path.GetFileName(folder);
                // string fileContent = File.ReadAllText(file);
                Vector3 tmpPosition = FirstPosition + new Vector3(0, -160 * count,0);
                GameObject worldGameObject = Instantiate(worldPrefab, transform);
                RectTransform rectTransform = worldGameObject.GetComponent<RectTransform>();
                SinglePlayerWorldThread singlePlayerWorldThread = worldGameObject.GetComponent<SinglePlayerWorldThread>();
                worldList.Add(singlePlayerWorldThread);
                singlePlayerWorldThread.tmpText.text = folderName;
                rectTransform.anchoredPosition = tmpPosition;
                count++;
            }
            worldPrefab.SetActive(false);
        }
    }
}
