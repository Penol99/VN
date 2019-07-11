using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Options : MonoBehaviour {

    public Dropdown dDown;
    Resolution[] resolution;
    List<Resolution> fixedRes = new List<Resolution>();

	// Use this for initialization
	void Start () {
        resolution = Screen.resolutions;
        dDown.onValueChanged.AddListener(delegate { Screen.SetResolution(fixedRes[dDown.value].width, fixedRes[dDown.value].height, true); });
        for (int i = 0; i < resolution.Length; i++)
        {
            
            if ((resolution[i].height / 0.5625f) == resolution[i].width)
            {
                fixedRes.Add(resolution[i]);
            }
        }
        fixedRes.TrimExcess();
        for (int i = 0; i < fixedRes.Capacity; i++)
        {
            //Debug.Log(fixedRes[i].ToString());
            dDown.options[i].text = fixedRes[i].ToString();
        }
        


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
