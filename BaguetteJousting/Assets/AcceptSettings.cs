using UnityEngine;
using System.Collections;

public class AcceptSettings : MonoBehaviour {

    [SerializeField]
    int scene = 1;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)
    || Input.GetKeyDown(KeyCode.KeypadEnter)
    || Input.GetKeyDown(KeyCode.Joystick1Button0)
    || Input.GetKeyDown(KeyCode.Joystick2Button0)
    || Input.GetKeyDown(KeyCode.Joystick3Button0)
    || Input.GetKeyDown(KeyCode.Joystick4Button0))
        {
            GetComponent<LevelLoader>().LevelLoad(scene);
        }
	
	}
}
