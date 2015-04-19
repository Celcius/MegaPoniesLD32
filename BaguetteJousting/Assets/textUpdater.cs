﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class textUpdater : MonoBehaviour {

    [SerializeField]
    Slider _slider;

    public void updateString(string str)
    {
        if (_slider != null)
            this.GetComponent<Text>().text = _slider.value + str;
        
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
