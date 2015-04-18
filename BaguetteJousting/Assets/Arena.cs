using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arena : MonoBehaviour {
	List<Pickup> _allPickups = new List<Pickup>();
	private static Arena _instance;
    
	//This is the public reference that other classes will use
	public static Arena instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<Arena> ();
			}
			return _instance;
		}
	}
	
	public List<Pickup> allPickups{
		get {
			return _allPickups;
        }
    }
    
	void Awake(){
		Arena arena = Arena.instance;
		if(arena != null) Debug.Log("Arena opened!");
		else return;
	}
	
	// Use this for initialiList<Pickup> allPickups = new List<Pickup>();zation
	void Start () {
		_allPickups = new List<Pickup>(GameObject.FindObjectsOfType<Pickup>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
