using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

public class UseAudio : MonoBehaviour
{
    public Camera mainCamera;
    public AudioSource audioSource;
    public AudioClip audioClip1;
    public AudioClip audioClip2;
    public AudioClip audioClip3;
    public AudioClip audioClip4;
    public AudioClip audioClip5;
    public AudioClip audioClip6;
    public AudioClip audioClip7;
    public AudioClip audioClip8;
    public AudioClip audioClip9;
    public AudioClip audioClip10;
    public AudioClip audioClip11;
    public AudioClip audioClip12;
    public AudioClip audioClip13;
    public AudioClip audioClip14;
    public AudioClip audioClip15;
    public AudioClip audioClip16;
    public AudioClip audioClip17;
    public AudioClip audioClip18;
    public AudioClip audioClip19;
    public AudioClip audioClip20;
    public AudioClip audioClip21;
    public AudioClip audioClip22;
    public AudioClip audioClip23;
    public AudioClip audioClip24;
    public AudioClip audioClip25;
    public AudioClip audioClip26;
    public AudioClip audioClip27;
    public AudioClip audioClip28;
    public AudioClip audioClip29;
    public AudioClip audioClip30;
    public AudioClip audioClip31;
    public AudioClip doorOpenAudioClip;
    public AudioClip doorCloseAudioClip;
    public AudioClip waterFillAudioClip1;
    public AudioClip waterFillAudioClip2;
    public AudioClip waterFillAudioClip3;
    public AudioClip waterEmptyAudioClip1;
    public AudioClip waterEmptyAudioClip2;
    public AudioClip chestOpenAudioClip;
    public AudioClip chestCloseAudioClip;
    public float musicVolume;
    public float randomNum;
    private AudioClip _audioClip;
    
    void Start()
    {
        audioSource.pitch = 0.7f;
        musicVolume = 1f;
        audioSource.volume = musicVolume;
    }
    
    public void PlayUse(float x, float y, String type) {
        Vector3 position = new Vector3(x, y, mainCamera.transform.position.z);
        if (type.Equals("DoorOpen")) {
            AudioSource.PlayClipAtPoint(doorOpenAudioClip, position, 1f);
        }else if (type.Equals("DoorClose")) {
            AudioSource.PlayClipAtPoint(doorCloseAudioClip, position, 1f);
        }else if (type.Equals("WaterFill")) {
            PlayWater(x, y, "fill");
        }else if (type.Equals("WaterEmpty")) {
            PlayWater(x, y, "empty");
        }else if (type.Equals("ChestOpen")) {
            AudioSource.PlayClipAtPoint(chestOpenAudioClip, position, 1f);
        }else if (type.Equals("ChestClose")) {
            AudioSource.PlayClipAtPoint(chestCloseAudioClip, position, 1f);
        }
    }

    public void PlayWater(float x, float y, String type) {
        Vector3 position = new Vector3(x, y, mainCamera.transform.position.z);
        if (type.Equals("fill")) {
            randomNum = Random.Range(1.0f, 4.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                AudioSource.PlayClipAtPoint(waterFillAudioClip1, position, 0.8f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                AudioSource.PlayClipAtPoint(waterFillAudioClip2, position, 0.8f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                AudioSource.PlayClipAtPoint(waterFillAudioClip3, position, 0.8f);
            }
        }else if (type.Equals("empty")) {
            randomNum = Random.Range(1.0f, 3.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                AudioSource.PlayClipAtPoint(waterEmptyAudioClip1, position, 0.8f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                AudioSource.PlayClipAtPoint(waterEmptyAudioClip2, position, 0.8f);
            }
        }
    }

    public void PlayDestroy(float x, float y, String type) {
        Vector3 position = new Vector3(x, y, mainCamera.transform.position.z);
        if(type.Equals("grass")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip1;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip2;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip3;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip4;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("gravel")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip5;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip6;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip7;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip8;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("sand")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip9;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip10;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip11;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip12;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        } else if(type.Equals("wood")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip13;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip14;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip15;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip16;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("stone")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip17;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip18;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip19;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip20;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("snow")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip21;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip22;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip23;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip24;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("cloth")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip25;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip26;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip27;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip28;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("glass")){
            randomNum = Random.Range(1.0f, 4.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip29;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip30;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip31;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("ladder")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip13;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip14;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip15;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip16;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }
    }
    
    public void PlayDigging(float x, float y, String type) {
        audioSource.pitch = 0.7f;
        audioSource.transform.position = new Vector3(x, y, mainCamera.transform.position.z);
        if (type.Equals("grass")) {
            audioSource.clip = audioClip1;
            audioSource.PlayOneShot(audioSource.clip ,1f);
        } else if (type.Equals("gravel")) {
            audioSource.clip = audioClip5;
            audioSource.Play();
        } else if (type.Equals("sand")) {
            audioSource.clip = audioClip9;
            audioSource.Play();
        } else if (type.Equals("wood")) {
            audioSource.clip = audioClip13;
            audioSource.PlayOneShot(audioSource.clip ,1f);
        } else if (type.Equals("stone")) {
            audioSource.clip = audioClip17;
            audioSource.Play();
        } else if (type.Equals("snow")) {
            audioSource.clip = audioClip21;
            audioSource.Play();
        } else if (type.Equals("cloth")) {
            audioSource.clip = audioClip25;
            audioSource.Play();
        } else if (type.Equals("glass")) {
            audioSource.clip = audioClip17;
            audioSource.Play();
        } else if (type.Equals("ladder")) {
            audioSource.clip = audioClip13;
            audioSource.PlayOneShot(audioSource.clip ,1f);
        } 
    }
    
    public void PlayPlace(float x, float y, String type) {
        Vector3 position = new Vector3(x, y, mainCamera.transform.position.z);
        if(type.Equals("grass")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip1;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip2;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip3;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip4;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("gravel")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip5;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip6;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip7;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip8;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("sand")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip9;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip10;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip11;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip12;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        } else if(type.Equals("wood")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip13;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip14;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip15;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip16;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("stone")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip17;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip18;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip19;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip20;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("snow")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip21;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip22;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip23;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip24;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("cloth")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip25;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip26;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip27;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip28;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("glass")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip17;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip18;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip19;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip20;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }else if(type.Equals("ladder")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip13;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip14;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip15;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip16;
                AudioSource.PlayClipAtPoint(_audioClip, position, 1f);
            }
        }
    }
}
