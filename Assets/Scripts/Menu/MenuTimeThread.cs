using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuTimer : MonoBehaviour
{
    public SpriteRenderer skySpriteRenderer;
    public SpriteRenderer cloudSpriteRenderer;
    public SpriteRenderer starSpriteRenderer;
    public float timeNumber;
    public float nightRatio;
    
    void Update() {
        timeNumber += Time.deltaTime * 500;
        if (timeNumber >= 120000) timeNumber = 0;
        if (timeNumber < 0) timeNumber = 0;
        UpdateBackgroundColor();
    }
    
    // 更新背景颜色
    public void UpdateBackgroundColor() {
        float cloudDarkRatio = 0;
        if (timeNumber >= 0 && timeNumber < 30000)
            cloudDarkRatio = 1;
        else if (timeNumber >= 30000 && timeNumber < 45000)
            cloudDarkRatio = 1 - ((timeNumber - 30000) / 15000.0f);
        else if (timeNumber >= 45000 && timeNumber < 85000)
            cloudDarkRatio = 0;
        else if (timeNumber >= 85000 && timeNumber <= 102500)
            cloudDarkRatio = 1 - ((102500 - timeNumber) / 17500.0f);
        else if (timeNumber >= 102500 && timeNumber <= 120000)
            cloudDarkRatio = 1;

        if (timeNumber >= 0 && timeNumber < 25000)
            timeNumber += Time.deltaTime * 10000;
        else if (timeNumber >= 25000 && timeNumber < 40000)
            nightRatio = 1 - ((timeNumber - 25000) / 15000.0f);
        else if (timeNumber >= 40000 && timeNumber < 90000)
            timeNumber += Time.deltaTime * 10000;
        else if (timeNumber >= 90000 && timeNumber <= 105000)
            nightRatio = 1 - ((105000 - timeNumber) / 15000.0f);
        else if (timeNumber >= 105000 && timeNumber <= 120000)
            timeNumber += Time.deltaTime * 10000;
        skySpriteRenderer.color = new Color(1, 1, 1, 1 - nightRatio);
        cloudSpriteRenderer.color = new Color(1, 1, 1, 1 - cloudDarkRatio);
        starSpriteRenderer.color = new Color(1, 1, 1, nightRatio);
    }
}
