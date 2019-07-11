using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    public List<AudioClip> musicList = new List<AudioClip>();

    public Slider volumeSlider;
    public AudioSource audioSource;
    public static int currentTrack = 69;

	// Use this for initialization
	void Start () {
        volumeSlider.minValue = 0;
        volumeSlider.maxValue = 1;
        volumeSlider.value = .5f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayTrack(int trackIndex)
    {
        audioSource = GetComponent<AudioSource>();
        currentTrack = trackIndex;
        if (currentTrack == 69)
        {
            audioSource.Stop();
        } else
        {
            StartCoroutine(Fade(audioSource.clip == null,trackIndex,audioSource));
            
            
        }
        
        
    }
    
    IEnumerator Fade(bool isNull, int _trackIndex, AudioSource _audioSource)
    {
        bool fadeDone = false;
        int timeVelocity = 2;

        if (isNull)
        {
            for (float i = 0; i < timeVelocity; i += timeVelocity * Time.deltaTime)
            {
                _audioSource.volume += i;
                Debug.Log(_audioSource.volume);
            }
            fadeDone = true;
        }
        _audioSource.clip = musicList[_trackIndex];
        _audioSource.Play();
        if (!isNull)
        {
            for (float i = timeVelocity; i > 0; i -= timeVelocity * Time.deltaTime)
            {
                _audioSource.volume -= i;
                StartCoroutine(Fade(true,_trackIndex,_audioSource));
            }
        }       
        
        yield return new WaitUntil(() => fadeDone);
    }

    public void OnValueChanged()
    {
        audioSource.volume = volumeSlider.value;
    }

}
