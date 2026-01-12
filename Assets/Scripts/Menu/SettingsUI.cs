using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour {
    public TMP_InputField playerNameInputField;
    public Slider bgmVolumeSlider;
    public Slider soundVolumeSlider;
    public Toggle autoJumpToggle;
    public Toggle magnifierToggle;
    public Slider zoomScaleSlider;
    public String currentSettingsPath;

    private void Awake() {
        currentSettingsPath = Application.persistentDataPath + "/currentSettings.csv";
    }

    void OnEnable()
    {
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
                    if (cells[0].Equals("PlayerName")) {
                        playerNameInputField.text = cells[1];
                    }else if (cells[0].Equals("BgmVolume")) {
                        bgmVolumeSlider.value = float.Parse(cells[1]);
                    }else if (cells[0].Equals("SoundVolume")) {
                        soundVolumeSlider.value = float.Parse(cells[1]);
                    }else if (cells[0].Equals("AutoJump")) {
                        autoJumpToggle.isOn = bool.Parse(cells[1]);
                    }else if (cells[0].Equals("MagnifierOn")) {
                        magnifierToggle.isOn = bool.Parse(cells[1]);
                    }else if (cells[0].Equals("ZoomScale")) {
                        zoomScaleSlider.value = float.Parse(cells[1]);
                    }
                }
            }
        }
    }

    private void OnDisable() {
        string[] lines = File.ReadAllLines(currentSettingsPath);
        string[] newLines = lines;
        if (lines.Length < 8)
        {
            newLines = new string[8];
            for (int i = 0; i < lines.Length; i++)
            {
                newLines[i] = lines[i];
            }
        }
        
        for (int i = 0; i < 8; i++) {
            // 更新行的内容
            if (i == 1)
            {
                newLines[i] = "PlayerName," + playerNameInputField.text;
            }
            else if (i == 2) {
                newLines[i] = "BgmVolume," + bgmVolumeSlider.value;
            }
            else if (i == 3) {
                newLines[i] = "SoundVolume," + soundVolumeSlider.value;
            }
            else if (i == 4) {
                newLines[i] = "AutoJump," + autoJumpToggle.isOn;
            }
            else if (i == 6) {
                newLines[i] = "MagnifierOn," + magnifierToggle.isOn;
            }
            else if (i == 7) {
                newLines[i] = "ZoomScale," + zoomScaleSlider.value;
            }
        }
        File.WriteAllLines(currentSettingsPath, newLines);
    }
}
