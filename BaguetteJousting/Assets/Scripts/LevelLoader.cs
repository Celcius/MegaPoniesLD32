using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {


    [SerializeField]
    Slider _slider;

    public void updatePlayers()
    {
        int slider = (int)_slider.value;
        if (_slider != null)
            ServiceLocator.instance._playerNum = slider;

    }

	public void LevelLoad(int level){
		Application.LoadLevel(level);

	}

}
