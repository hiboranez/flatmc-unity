using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

public class StepAudio : MonoBehaviour
{
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
    public AudioClip audioClip32;
    public AudioClip audioClip33;
    public AudioClip audioClip34;
    public AudioClip audioClip35;
    public AudioClip audioClip36;
    public AudioClip audioClip37;
    public AudioClip audioClip38;
    public AudioClip audioClip39;
    public AudioClip audioClip40;
    public PlayerThread playerThread;
    public WorldThread worldThread;
    public float musicVolume;
    public float randomNum;
    public float timer;
    public int state;
    private AudioSource _audioSource;
    private AudioClip _audioClip;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (timer > 0) timer -= Time.deltaTime;
        if (timer <= 0) {
            float xJoy = playerThread.joyStick.xJoy;
            Vector3 position = playerThread.transform.position;
            position += new Vector3(0, -0.1f, 0);
            Vector3Int blockPosition = worldThread.solidBlockTileMap.WorldToCell(position);
            if(blockPosition.x >= 0 && blockPosition.x < worldThread.width && blockPosition.y >= 0 &&
               blockPosition.y < worldThread.height){
                Vector3 playerPosition = transform.position;
                int xPos = (int)playerPosition.x;
                int yPos = (int)playerPosition.y;
                if (Math.Abs(playerThread.joyStick.yJoy) > 0.2f) {
                    if (worldThread.solidBlockList[yPos, xPos].Equals("Ladder")) {
                        timer = 0.5f;
                        randomPlay("ladder");
                    }
                }
                String blockName = worldThread.solidBlockList[blockPosition.y, blockPosition.x];
                if (xJoy != 0 && !worldThread.noReachBlockList[blockPosition.y, blockPosition.x]) {
                    String type = IndexAll.blockToAudioType(blockName);
                    if (!type.Equals("null")) {
                        if (playerThread.canRun2) {
                            timer = 0.28f;
                            randomPlay(type);
                        } else {
                            timer = 0.5f;
                            randomPlay(type);
                        }
                    }
                }
            }
        }
    }
    
    void randomPlay(String type)
    {
        if(type.Equals("grass")){
            randomNum = Random.Range(1.0f, 7.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip1;
                _audioSource.PlayOneShot(_audioClip, 1f);
             } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip2;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip3;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip4;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 5.0f && randomNum <= 6.0f) {
                _audioClip = audioClip5;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 6.0f && randomNum <= 7.0f) {
                _audioClip = audioClip6;
                _audioSource.PlayOneShot(_audioClip, 1f);
            }
        }else if(type.Equals("gravel")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip7;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip8;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip9;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip10;
                _audioSource.PlayOneShot(_audioClip, 1f);
            }
        }else if(type.Equals("sand")){
            randomNum = Random.Range(1.0f, 6.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip11;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip12;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip13;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip14;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 5.0f && randomNum <= 6.0f) {
                _audioClip = audioClip15;
                _audioSource.PlayOneShot(_audioClip, 1f);
            }
        } else if(type.Equals("wood")){
            randomNum = Random.Range(1.0f, 7.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip16;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip17;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip18;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip19;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 5.0f && randomNum <= 6.0f) {
                _audioClip = audioClip20;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 6.0f && randomNum <= 7.0f) {
                _audioClip = audioClip21;
                _audioSource.PlayOneShot(_audioClip, 1f);
            }
        }else if(type.Equals("stone")){
            randomNum = Random.Range(1.0f, 7.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip22;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip23;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip24;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip25;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 5.0f && randomNum <= 6.0f) {
                _audioClip = audioClip26;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 6.0f && randomNum <= 7.0f) {
                _audioClip = audioClip27;
                _audioSource.PlayOneShot(_audioClip, 1f);
            }
        }else if(type.Equals("snow")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip28;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip29;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip30;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip31;
                _audioSource.PlayOneShot(_audioClip, 1f);
            }
        }else if(type.Equals("cloth")){
            randomNum = Random.Range(1.0f, 5.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip32;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip33;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip34;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip35;
                _audioSource.PlayOneShot(_audioClip, 1f);
            }
        }else if(type.Equals("glass")){
            randomNum = Random.Range(1.0f, 7.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip22;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip23;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip24;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip25;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 5.0f && randomNum <= 6.0f) {
                _audioClip = audioClip26;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 6.0f && randomNum <= 7.0f) {
                _audioClip = audioClip27;
                _audioSource.PlayOneShot(_audioClip, 1f);
            }
        } else if(type.Equals("ladder")){
            randomNum = Random.Range(1.0f, 6.0f);
            if (randomNum >= 1.0f && randomNum < 2.0f) {
                _audioClip = audioClip36;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 2.0f && randomNum < 3.0f) {
                _audioClip = audioClip37;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 3.0f && randomNum <= 4.0f) {
                _audioClip = audioClip38;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 4.0f && randomNum <= 5.0f) {
                _audioClip = audioClip39;
                _audioSource.PlayOneShot(_audioClip, 1f);
            } else if (randomNum >= 5.0f && randomNum <= 6.0f) {
                _audioClip = audioClip40;
                _audioSource.PlayOneShot(_audioClip, 1f);
            }
        }
    }
}
