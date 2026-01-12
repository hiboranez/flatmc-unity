using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class CameraThread : MonoBehaviour {
    public PlayerThread _playerThread;
    public TMP_Text playerNameText;
    public Slider zoomScaleSlider;
    public float zoomRatio = 0f;
    public Camera mainCamera;
    public Transform stars;
    public Transform sky;
    public Transform clouds;
    private Vector3 speed = Vector3.zero;
    private float _lastZoomRatio = 0f;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Start() {
        Application.targetFrameRate = 60;
    }

    void Update() {
        // 更新镜头位移
        transform.position = Vector3.SmoothDamp(this.transform.position, _playerThread.transform.position- new Vector3(0,-0.2f,10), ref speed, 0.25f);
        if (zoomRatio != _lastZoomRatio)
        {
            UpdateVision();
        }
    }

    public void UpdateVision()
    {
        mainCamera.orthographicSize = 3 + zoomRatio * 4; 
        _lastZoomRatio = zoomRatio;
        Vector3 starPosition = stars.localPosition;
        starPosition.y = 1.4f + 4.1f * (zoomRatio - 0.5f);
        stars.localPosition = starPosition;
        Vector3 cloudScale = clouds.localScale;
        cloudScale.y = 0.95f + 0.9f * (0.5f - zoomRatio);
        cloudScale.x = 0.95f + 0.9f * (0.5f - zoomRatio);
        clouds.localScale = cloudScale;
        Vector3 cloudPosition = clouds.localPosition;
        cloudPosition.y = 0.5f + 5f * (zoomRatio - 0.5f);
        clouds.localPosition = cloudPosition;
        Vector3 skyScale = sky.localScale;
        skyScale.y = 1.3f + 0.8f * (zoomRatio - 0.5f);
        sky.localScale = skyScale;
    }
    
    public void UpdateZoomSize()
    {
        zoomRatio = zoomScaleSlider.value;
    }
   
    
}
