using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {

    [SerializeField]
    Text _text1;
    [SerializeField]
    Text _text2;

    [SerializeField]
    List<Color> _colors;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void setVictor(int player)
    {
        _text1.text = "Player " + player + " Wins!";
        _text2.text = "Player " + player + " Wins!";

        _text2.color = _colors[player - 1];
    }
}
