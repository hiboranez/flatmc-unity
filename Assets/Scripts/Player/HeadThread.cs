using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadThread : MonoBehaviour {
    public JoyStick joyStick;
    public GameObject modelRoot;
    public Vector3 targetRotation;
    private Vector3 _headMoveSpeed;
    // Start is called before the first frame update
    void Awake()
    {
        targetRotation = new Vector3(0, 0, 0);
        _headMoveSpeed = Vector3.zero;
    }

    private void Update() {
        Vector3 current = transform.rotation.eulerAngles;
        // float angleZ = Mathf.Lerp(current.z, targetRotation.z, 0.3f);
        float angleZ = Mathf.SmoothDampAngle(current.x, targetRotation.z, ref _headMoveSpeed.z, 0.07f);
        transform.rotation = Quaternion.Euler(angleZ, current.y, current.z);
    }
    
    public void SpriteFaceTo(Vector2 target) {
        Vector3 dir = new Vector3(target.x,target.y,0) - transform.position;
        float angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
        if(!(modelRoot.transform.rotation.z < 0)){
            if (!((angle > 90 && angle <= 180) || (angle < -90 && angle >= -180))) {
                targetRotation = new Vector3(0, 0, angle);
            } else {
                if(joyStick.xJoy == 0) {
                    modelRoot.transform.rotation = Quaternion.Euler(new Vector3(-90,0, -90));
                    targetRotation = new Vector3(0, 0, angle + 180f);
                }
            }
        } else {
            if (!(angle > -90 && angle < 90)){
                targetRotation = new Vector3(0, 0, angle + 180f);
            } else {
                if(joyStick.xJoy == 0) {
                    modelRoot.transform.rotation = Quaternion.Euler(new Vector3(-90,0, 90));
                    targetRotation = new Vector3(0, 0, angle);
                }
            }
        }
        if (modelRoot.transform.rotation.z > 0)
        {
            targetRotation = new Vector3(0, 0, 360-angle);
        }
        
        Vector3 currentRotation = transform.rotation.eulerAngles;
        if (Math.Abs(currentRotation.z) > 0.1f)
        {
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y-180, 0);
        }
    }
}
