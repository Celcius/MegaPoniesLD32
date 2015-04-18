using UnityEngine;
using System.Collections;

public class ActivateRenderer : MonoBehaviour {

    TrailRenderer _renderer;
    bool activated = false;
    float decideTimer = 0.0f;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<TrailRenderer>();

        decideTimer = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {

	    
	}
}
