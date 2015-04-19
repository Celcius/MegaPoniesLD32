using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {


    [SerializeField]
    Slider _slider;

    [SerializeField]
    Slider _rounds;

    public void updatePlayers()
    {
        int slider = (int)_slider.value;
        if (_slider != null)
            ServiceLocator.instance._playerNum = slider;
        if (_rounds != null)
            ServiceLocator.instance._rounds = (int)_rounds.value;

    }

	public void LevelLoad(int level){
		Application.LoadLevel(level);

	}

}
