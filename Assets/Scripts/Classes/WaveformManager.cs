using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveformManager : MonoBehaviour {

    public GameObject mCamera;
    public float amplitude = 2f;    
    public float horMovSpeed = 2;
    public MusicGame mGame;

    private float frequency;
    private float _frequency;
    private float phase = 0f;

    // Use this for initialization
    void Start () {
        frequency = (Mathf.PI/30)*120;
        _frequency = frequency;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        // Make the camera follow the wave objects x position
        //Vector3 mCamPos = mCamera.transform.position;
        //mCamera.transform.position = new Vector3 (transform.position.x, mCamPos.y, mCamPos.z);
        //horMovSpeed = Mathf.Cos(Mathf.PI);
        if (frequency != _frequency)
        {
            CalculateNewFrequency();
        }
        Vector3 v3 = transform.position;
        v3.y = Mathf.Abs(Mathf.Sin(Mathf.Sin(Time.time) * _frequency + phase) * amplitude);
        v3.x = Mathf.Abs(Mathf.Cos(Mathf.Cos(Time.time) * _frequency + phase) * amplitude + horMovSpeed);
        transform.position = v3;
    }
    
    private void CalculateNewFrequency()
    {
        float curr = (Time.time * _frequency + phase) % (2.0f * Mathf.PI);
        float next = (Time.time * frequency) % (2.0f * Mathf.PI);
        phase = curr - next;
        _frequency = frequency;
        
    }
}
