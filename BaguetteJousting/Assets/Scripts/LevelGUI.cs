using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelGUI : MonoBehaviour {
	private static LevelGUI _instance;

	public static LevelGUI instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<LevelGUI> ();
			}
			return _instance;
		}
	}
	
	public GameObject mainMenuButton;
	
	void Start () {
		mainMenuButton.SetActive(false);
	}
	
	public void ShowEndGUI(){
		mainMenuButton.SetActive(true);
	}
	
	
	// Update is called once per frame
	void Update () {
	
	
	}
}
