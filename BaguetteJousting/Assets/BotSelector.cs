using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BotSelector : MonoBehaviour {

    [SerializeField]
    List<Toggle> _bots;

    [SerializeField]
    Slider _players;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void toggleBot(int i)
    {
        bool isBotOn = _bots[i].isOn;
        ServiceLocator.instance._bots[i] = isBotOn;

    }

    public void updatedPlayer()
    {
        if(_players.value == 2)
        {
            _bots[2].gameObject.SetActive(false);
            _bots[3].gameObject.SetActive(false);
        }
        else if(_players.value == 3)
        {
            _bots[2].gameObject.SetActive(true);
            _bots[3].gameObject.SetActive(false);
            toggleBot(2);
        }
        else
        {
            _bots[2].gameObject.SetActive(true);
            _bots[3].gameObject.SetActive(true);
            toggleBot(2);
            toggleBot(3);
        }


    }
}
