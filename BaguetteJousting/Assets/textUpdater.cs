using UnityEngine;
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

    public void updateBotString()
    {
        if (_slider != null)
        {
            if(_slider.value ==1)
            {
                this.GetComponent<Text>().text = "Easy Bots";
                ServiceLocator.instance._hardBots = false;
            }
            else
            {
                this.GetComponent<Text>().text = "Hard Bots";
                ServiceLocator.instance._hardBots = true;
            }
        }

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
