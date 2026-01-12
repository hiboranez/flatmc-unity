using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class Bgm : MonoBehaviour {
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
    public float musicVolume;
    public float randomNum;
    public int state;
    public Slider slider;

    // Use this for initialization
    void Start () {
        slider.value = 0.5f;
        musicVolume = 0.5f;
        audioSource.volume = musicVolume;
        randomPlay();
    }
    
    
    // Update is called once per frame
    void Update () {
        if(state == 1 && !audioSource.isPlaying) randomPlay();
        if(state == 2 && !audioSource.isPlaying) randomPlay();
        if(state == 3 && !audioSource.isPlaying) randomPlay();
        if(state == 4 && !audioSource.isPlaying) randomPlay();
        if(state == 5 && !audioSource.isPlaying) randomPlay();
        if(state == 6 && !audioSource.isPlaying) randomPlay();
        if(state == 7 && !audioSource.isPlaying) randomPlay();
        if(state == 8 && !audioSource.isPlaying) randomPlay();
        if(state == 9 && !audioSource.isPlaying) randomPlay();
        if(state == 10 && !audioSource.isPlaying) randomPlay();
        if(state == 11 && !audioSource.isPlaying) randomPlay();
        if(state == 12 && !audioSource.isPlaying) randomPlay();
    }

    void randomPlay()
    {
        randomNum = Random.Range(1.0f, 13.0f);
        if (randomNum >= 1.0f && randomNum < 2.0f) { state = 1; audioSource.clip = audioClip1; audioSource.Play(); }
        else if (randomNum >= 2.0f && randomNum < 3.0f) { state = 2; audioSource.clip = audioClip2; audioSource.Play(); }
        else if (randomNum >= 3.0f && randomNum <= 4.0f) { state = 3; audioSource.clip = audioClip3; audioSource.Play(); }
        else if (randomNum >= 4.0f && randomNum <= 5.0f) { state = 4; audioSource.clip = audioClip4; audioSource.Play(); }
        else if (randomNum >= 5.0f && randomNum <= 6.0f) { state = 5; audioSource.clip = audioClip5; audioSource.Play(); }
        else if (randomNum >= 6.0f && randomNum <= 7.0f) { state = 6; audioSource.clip = audioClip6; audioSource.Play(); }
        else if (randomNum >= 7.0f && randomNum <= 8.0f) { state = 7; audioSource.clip = audioClip7; audioSource.Play(); }
        else if (randomNum >= 8.0f && randomNum <= 9.0f) { state = 8; audioSource.clip = audioClip8; audioSource.Play(); }
        else if (randomNum >= 9.0f && randomNum <= 10.0f) { state = 9; audioSource.clip = audioClip9; audioSource.Play(); }
        else if (randomNum >= 10.0f && randomNum <= 11.0f) { state = 10; audioSource.clip = audioClip10; audioSource.Play(); }
        else if (randomNum >= 11.0f && randomNum <= 12.0f) { state = 11; audioSource.clip = audioClip11; audioSource.Play(); }
        else if (randomNum >= 12.0f && randomNum <= 13.0f) { state = 12; audioSource.clip = audioClip12; audioSource.Play(); }
    }
}