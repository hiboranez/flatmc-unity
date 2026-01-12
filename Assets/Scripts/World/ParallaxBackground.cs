using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallox : MonoBehaviour {
    [SerializeField] private Vector2 parallaxEffectMultiplier;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;

    private void Start() {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    private void LateUpdate() {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x,
            deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX) {
            float cameraTransformPositionX = cameraTransform.position.x;
            float offsetPositionX = (cameraTransformPositionX - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransformPositionX + offsetPositionX, transform.position.y,transform.position.z);
        }
    }
}
