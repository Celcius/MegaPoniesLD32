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
    public GameObject roundLabel;
	
	void Start () {
		mainMenuButton.SetActive(false);
	}
	
	public void ShowEndGUI(){
		mainMenuButton.SetActive(true);
	}
	
	
	// Update is called once per frame
	void Update () {
        if(mainMenuButton.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Space)
                || Input.GetKeyDown(KeyCode.KeypadEnter)
                || Input.GetKeyDown(KeyCode.Joystick1Button0)
                || Input.GetKeyDown(KeyCode.Joystick2Button0)
                || Input.GetKeyDown(KeyCode.Joystick3Button0)
                || Input.GetKeyDown(KeyCode.Joystick4Button0))
            {
                GetComponent<LevelLoader>().LevelLoad(0);
            }
        }
	
	
	}
}
