using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class Initializer : MonoBehaviour
{
    public String type;
    public AudioSource bgmAudioSource;
    public AudioSource soundAudioSource;
    public String currentSettingsPath;
    public AudioSource cameraAudioSource;
    public AudioSource playerAudioSource;
    public AudioSource stepAudioSource;
    public AudioSource useAudioSource;
    public Material skinMenu;
    public Material skinGame;
    public TMP_InputField playerNameInputField;
    public Slider bgmVolumeSlider;
    public Slider soundVolumeSlider;
    public Toggle autoJumpToggle;
    public Toggle magnifierToggle;
    public Slider zoomScaleSlider;
    public CameraThread mainCameraThread;
    public ArmorContent armorContent;
    public PlayerThread playerThread;
    private bool _needContinue;
    
    void Start()
    {
        _needContinue = false;
        currentSettingsPath = Application.persistentDataPath + "/currentSettings.csv";
        if (type.Equals("menu"))
        {
            skinMenu.mainTexture = Resources.Load<Texture>("Textures/Skin/steve");
            skinGame.mainTexture = Resources.Load<Texture>("Textures/Skin/steve");
            bool skinPathExist = false;
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
                            bgmAudioSource.volume = float.Parse(cells[1]);
                        } 
                        else if (cells[0].Equals("SoundVolume")) {
                            soundAudioSource.volume = float.Parse(cells[1]);
                        }
                        else if (cells[0].Equals("AutoJump")) {
                            autoJumpToggle.isOn = bool.Parse(cells[1]);
                        }
                        else if (cells[0].Equals("SkinPath"))
                        {
                            if (!cells[1].Equals("null")) {
                                StartCoroutine(ReadFile("file://"+cells[1]));  
                            }
                        }
                        else if (cells[0].Equals("MagnifierOn")) {
                            magnifierToggle.isOn = bool.Parse(cells[1]);
                        }
                        else if (cells[0].Equals("ZoomScale")) {
                            zoomScaleSlider.value = float.Parse(cells[1]);
                        }
                    }
                }
            }
            else {
                using (StreamWriter sw = new StreamWriter(currentSettingsPath)) {
                    // 遍历二维数组，写入每个单元格的数据
                    for (int i = 0; i < 8; i++) {
                        if (i == 0) {
                            sw.Write("CurrentWorldName,null\n");
                        } else if (i == 1) {
                            sw.Write("PlayerName,Steve\n");
                            playerNameInputField.text = "Steve";
                        }
                        else if (i == 2) {
                            sw.Write("BgmVolume,0.5\n");
                            bgmVolumeSlider.value = 0.5f;
                        }
                        else if (i == 3) {
                            sw.Write("SoundVolume,0.5\n");
                            soundVolumeSlider.value = 0.5f;
                        }
                        else if (i == 4) {
                            sw.Write("AutoJump,True\n");
                            autoJumpToggle.isOn = true;
                        }
                        else if (i == 5) {
                            sw.Write("SkinPath,null\n");
                        }
                        else if (i == 6) {
                            sw.Write("MagnifierOn,True\n");
                            magnifierToggle.isOn = true;
                        }
                        else if (i == 7) {
                            sw.Write("ZoomScale,True\n");
                            zoomScaleSlider.value = 0.5f;
                        }
                    }
                    // 关闭 StreamWriter
                    sw.Close();
                }
            }
        }else if (type.Equals("game"))
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
                        if (cells[0].Equals("BgmVolume")) {
                            bgmAudioSource.volume = float.Parse(cells[1]);
                        } else if (cells[0].Equals("SoundVolume")) {
                            cameraAudioSource.volume = float.Parse(cells[1]);
                            playerAudioSource.volume = float.Parse(cells[1]);
                            stepAudioSource.volume = float.Parse(cells[1]);
                            useAudioSource.volume = float.Parse(cells[1]);
                        } else if (cells[0].Equals("AutoJump")) {
                            autoJumpToggle.isOn = bool.Parse(cells[1]);
                        } else if (cells[0].Equals("MagnifierOn")) {
                            magnifierToggle.isOn = bool.Parse(cells[1]);
                        } else if (cells[0].Equals("ZoomScale")) {
                            mainCameraThread.zoomRatio = float.Parse(cells[1]);
                        }
                    }
                }
            }
            mainCameraThread.UpdateVision();
            armorContent.UpdateArmorModel();
            playerThread.UpdateArmorValue();
        }
        if(!_needContinue) gameObject.SetActive(false);
    }
 
    IEnumerator ReadFile(string filePathValue)
    {
        _needContinue = true;
        WWW ReadFile1 = new WWW(filePathValue);
        yield return ReadFile1;
        if (ReadFile1.error == null)
        {
            var texture = ReadFile1.texture;
            texture.filterMode = FilterMode.Point;
            skinMenu.mainTexture = texture;
            skinGame.mainTexture = texture;
            gameObject.SetActive(false);
        }
    }
}