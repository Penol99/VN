using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGame : MonoBehaviour
{
    public AudioSource hat;
    public GameObject obj;
    public GameObject sphere;
    public float numBeatsPerSegment = 1f;
    public float bpm = 60f;

    private AudioSource aS;     
    private double time;
    private double nextEventTime;

    public float Bpm
    {
        get
        {
            return bpm;
        }

        set
        {
            bpm = value;
        }
    }

    public AudioSource AS
    {
        get
        {
            return aS;
        }

        set
        {
            aS = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        AS = GetComponent<AudioSource>();
        //aS.Play();
        //nextEventTime = AudioSettings.dspTime + 2f;
        nextEventTime = AS.timeSamples / AS.clip.frequency + 1;
        StartCoroutine(StartAudio(1f));
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //time = AudioSettings.dspTime;
        Debug.Log(AS.timeSamples);
        AS.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
        time = AS.timeSamples / AS.clip.frequency;
        if (time == nextEventTime)
        {
            hat.Play();
            Instantiate(obj, sphere.transform.position, sphere.transform.rotation);
            //nextEventTime += 60f / Bpm * numBeatsPerSegment;
            nextEventTime += 1 * numBeatsPerSegment;

        }

    }
    IEnumerator StartAudio(float _time)
    {
        yield return new WaitForSeconds(_time);
        AS.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
        AS.Play();
    }

    

}

