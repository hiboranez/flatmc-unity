using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashHungerBar : MonoBehaviour
{
    private float flashTimer;
    
    public IEnumerator Flash() {
        flashTimer = 0.15f;
        while (flashTimer > 0)
        {
            // 等待一段时间
            yield return new WaitForSeconds(Time.deltaTime);
            // 逐步减小flashTimer的值
            flashTimer -= Time.deltaTime;
        }
        gameObject.SetActive(false);
    }
}
