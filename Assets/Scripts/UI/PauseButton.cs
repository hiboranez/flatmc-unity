using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public GameObject pauseUI;
    public AudioSource cameraAudioSource;
    public AudioClip clickAudioClip;
    public Slider bgmVolumeSlider;
    public Slider soundVolumeSlider;
    public Toggle autoJumpToggle;
    public String currentSettingsPath;
    
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
        currentSettingsPath = Application.persistentDataPath + "/currentSettings.csv";
        if (File.Exists(currentSettingsPath)) {
            // 使用 StreamReader 逐行读取文件内容
            using (StreamReader reader = new StreamReader(currentSettingsPath)) {
                // 逐行读取，直到文件末尾
                while (!reader.EndOfStream) {
                    // 读取一行数据
                    string line = reader.ReadLine();
                    // 分割行数据成单元格
                    string[] cells = line.Split(',');
                    // 遍历单元格
                    if (cells[0].Equals("BgmVolume")) {
                        bgmVolumeSlider.value = float.Parse(cells[1]);
                    } else if (cells[0].Equals("SoundVolume")) {
                        soundVolumeSlider.value = float.Parse(cells[1]);
                    } else if (cells[0].Equals("AutoJump")) {
                        autoJumpToggle.isOn = bool.Parse(cells[1]);
                    }
                }
            }
        }
        
    }

    private void OnClickCallBack() {
        pauseUI.SetActive(true);
        cameraAudioSource.PlayOneShot(clickAudioClip, 1f);
    }
}
