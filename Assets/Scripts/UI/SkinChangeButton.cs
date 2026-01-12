using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Networking;
using Application = UnityEngine.Application;


public class SkinChangeButton : MonoBehaviour
{
    public String type;
    public Material skinGame;
    public Material skinMenu;
    private String _path;
    
    public void OnClickCallBack()
    {
        if(type.Equals("select"))
            SelectImage();
        else if(type.Equals("reset"))
            ResetSkin();
    }

    public void ResetSkin()
    {
        String currentSettingsPath = Application.persistentDataPath + "/currentSettings.csv";
        skinMenu.mainTexture = Resources.Load<Texture>("Textures/Skin/steve");
        skinGame.mainTexture = Resources.Load<Texture>("Textures/Skin/steve");
        string[] lines = File.ReadAllLines(currentSettingsPath);
        for (int i = 0; i < lines.Length; i++) {
            // 更新行的内容
            if (i == 5) {
                lines[i] = "SkinPath,null";
            }
        }
        File.WriteAllLines(currentSettingsPath, lines);
    }
    
    public void SelectImage(){
        if (Application.platform == RuntimePlatform.WindowsEditor){
            OpenFileDialog od = new OpenFileDialog();
            od.Title = "请选择皮肤图片";
            od.Multiselect = false;
            od.Filter = "图片文件(*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp";
            if (od.ShowDialog() == DialogResult.OK)
            {
                _path = od.FileName;
                StartCoroutine(ReadFile("file://" + od.FileName));
            }
        }else if (Application.platform == RuntimePlatform.Android)
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery(PhotoAlbum, "选择一张图片", "image/png");
        }
    }
    
    private void PhotoAlbum(string path)
    {
        String currentSettingsPath = Application.persistentDataPath + "/currentSettings.csv";
        _path = path;
        var texture = NativeGallery.LoadImageAtPath(path, 2048, false);
        texture.filterMode = FilterMode.Point;
        skinMenu.mainTexture = texture;
        skinGame.mainTexture = texture;
        string[] lines = File.ReadAllLines(currentSettingsPath);
        for (int i = 0; i < lines.Length; i++) {
            // 更新行的内容
            if (i == 5) {
                lines[i] = "SkinPath,"+ _path;
            }
        }
        File.WriteAllLines(currentSettingsPath, lines);
    }
 
    IEnumerator ReadFile(string filePathValue)
    {
        String currentSettingsPath = Application.persistentDataPath + "/currentSettings.csv";
        WWW ReadFile1 = new WWW(filePathValue);
        yield return ReadFile1;
        if (ReadFile1.error == null)
        {
            var texture = ReadFile1.texture;
            texture.filterMode = FilterMode.Point;
            skinMenu.mainTexture = texture;
            skinGame.mainTexture = texture;
            string[] lines = File.ReadAllLines(currentSettingsPath);
            for (int i = 0; i < lines.Length; i++) {
                // 更新行的内容
                if (i == 5) {
                    lines[i] = "SkinPath,"+ _path;
                }
            }
            File.WriteAllLines(currentSettingsPath, lines);
        }
    }
}
