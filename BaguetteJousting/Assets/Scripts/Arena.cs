using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arena : MonoBehaviour {
	List<Pickup> _allPickups = new List<Pickup>();
	List<Pawn> _allPlayers = new List<Pawn>();
	
	private static Arena _instance;

    [SerializeField]
    List<Transform> _spawners;

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
    
	public List<Pawn> allPlayers{
		get {
			return _allPlayers;
		}
	}
	
	
	void Awake(){
		Arena arena = Arena.instance;
		if(arena != null) Debug.Log("Arena opened!");
        else return;

        int players = ServiceLocator.instance.getPlayers();

        
        for(int i = 0; i <players; i++)
        {
            if(i < _spawners.Count)
            {
                GameObject controller = (GameObject)Instantiate(Resources.Load("Prefabs/Player"));

                controller.transform.position = _spawners[i].position;
                controller.transform.rotation = _spawners[i].rotation;

                controller.GetComponent<PlayerController>().setPlayerNum(i);

                controller.name = "PLayer" + i;
            }
        }


	}
	
	// Use this for initialiList<Pickup> allPickups = new List<Pickup>();zation
	void Start () {
		_allPickups = new List<Pickup>(GameObject.FindObjectsOfType<Pickup>());
		_allPlayers = new List<Pawn>(GameObject.FindObjectsOfType<Pawn>());
	}
	
	
	public void PlayerDied(Pawn player){
		int playersAlive = 0;
		foreach (Pawn p in allPlayers){
			if(p.isAlive()){
				playersAlive += 1;
				if (playersAlive > 1)
					break;
			}
		}
		if (playersAlive < 2){
			Debug.Log("Only one player left.");
			MatchOver();
		}
	}
	
	
	void MatchOver(){
		LevelGUI.instance.ShowEndGUI();
	}
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
