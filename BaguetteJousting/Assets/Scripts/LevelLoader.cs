using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	public void LevelLoad(int level){
		Application.LoadLevel(level);
	}

}
