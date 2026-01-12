using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterWorldButton : MonoBehaviour {
    public AudioClip clickAudioClip;
    public AudioSource audioSource;
    public TMP_Text worldName;
    public TMP_InputField playerName;
    public TMP_Text playerNameNullWarningText;
    public float PlayerNameNullWarningShowTimer;
    public GameObject loadingUI;
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClickCallBack);
    }
        
    private void OnEnable() {
        playerNameNullWarningText.enabled = false;
    }

    public IEnumerator PlayerNameNullWarning() {
        PlayerNameNullWarningShowTimer = 1.5f;
        playerNameNullWarningText.enabled = true;
        while (PlayerNameNullWarningShowTimer > 0)
        {
            // 等待一段时间，例如0.1秒
            yield return new WaitForSeconds(Time.deltaTime);
            // 逐步减小flashTimer的值
            PlayerNameNullWarningShowTimer -= Time.deltaTime;
        }
        playerNameNullWarningText.enabled = false;
    }
    
    private void OnClickCallBack() {
        audioSource.PlayOneShot(clickAudioClip, 1f);
        if (playerName.text == "") {
            StartCoroutine(PlayerNameNullWarning());
        }
        else {
            loadingUI.SetActive(true);
            // 指定 CSV 文件路径
            string currentSettingsPath = Application.persistentDataPath + "/currentSettings.csv";
            // 读取整个文件内容
            string[] lines = File.ReadAllLines(currentSettingsPath);
            // 要修改的行号（假设修改第二行，索引从0开始）
            int rowIndexToModify = 0;
            // 检查行号是否在有效范围内
            if (rowIndexToModify >= 0 && rowIndexToModify < lines.Length) {
                // 更新行的内容
                lines[rowIndexToModify] = "CurrentWorldName," + worldName.text; // 替换成你想要的新数据
            } else {
                Debug.LogError("无效的行号");
                return;
            }
            // 写入更新后的内容回到文件
            File.WriteAllLines(currentSettingsPath, lines);
            SceneManager.LoadScene("GameScene");
        }
    }
}
