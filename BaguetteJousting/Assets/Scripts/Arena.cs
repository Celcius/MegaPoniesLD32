﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Arena : MonoBehaviour {
	List<Pickup> _allPickups = new List<Pickup>();
	List<Pawn> _allPlayers = new List<Pawn>();
	
	private static Arena _instance;

    public int _rounds = 3;
    public int _currentRound = 1;
    public bool roundStarted = false;
    public int[] _scores = {0,0,0,0};
    public int[] _victors = { 0, 0, 0, 0 };

	List<Text> playersText = new List<Text>();

    bool _playing = false;
    [SerializeField]
    RoundL roundController;

    [SerializeField]
    List<Transform> _spawners;
    [SerializeField]
    List<BaguetteSpawner> _baguetteSpawners;
    
    [SerializeField]
    WinScreen _win;


    [SerializeField]
    MidPlayers _mainCam;

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

        _rounds = ServiceLocator.instance.getRounds();
        _currentRound = 0;
	
        spawnRound();

	}
	

    void spawnRound()
    {
        roundController.roundStart();
        roundStarted = false;

        for (int i = 0; i < _allPlayers.Count; i++)
        {
            Pawn pawn = _allPlayers[i];
            pawn.Kill();
        }
        _allPlayers.Clear();

        // spawn for tiebreaker
        bool _spawned = false;
        for (int i = 0; i < _victors.Length;i++ )
        {
            if(_victors[i] == 1)
            {
                _spawned = true;
                spawnPlayer(i);
            }
        }
        int players = ServiceLocator.instance.getPlayers();
        if(!_spawned)
        { 
           
            for (int i = 0; i < players; i++)
            {
                if (i < _spawners.Count)
                {
                    spawnPlayer(i);
                }
            }
        }

        updateText();

        for (int i = 0; i < _allPickups.Count; i++)
        {
            Pickup pickup = _allPickups[i];
            pickup.GetComponent<Baguette>().destroyBaguette();
            Destroy(pickup.gameObject);
        }
        _allPickups.Clear();

        Baguette[] missingB =  FindObjectsOfType<Baguette>();
        for (int i = 0; i < missingB.Length; i ++ )
        {
            Baguette b = missingB[i];
            b.destroyBaguette();
            Destroy(b.gameObject);
        }

        for (int i = 0; i < _baguetteSpawners.Count; i++)
        {
            if((players ==2 && i <2)
               ||  (players == 3 && (i== 0 || i >1))
               ||  (players == 4))
             ((BaguetteSpawner)_baguetteSpawners[i]).spawnBaguette();
  
        }

        _mainCam.setPlayers(_allPlayers);
        _playing = true;
    }

    void updateText()
    {
        playersText.Clear();
        for (int i = 0; i < _allPlayers.Count; i++)
        {
            playersText.Add(GameObject.Find("ScoreP" + (i + 1)).GetComponent<Text>());
            Debug.Log("playersText; " + playersText);
        }

        for (int i = 0; i < _allPlayers.Count; i++)
        {
            string text = "";
            int j = 0;
            while (j < _scores[i])
            {
                text += "★";
                j++;
            }
            while (j < _rounds)
            {
                text += "☆";
                j++;
            }

            playersText[i].text = text;
        }

    }

    void spawnPlayer(int i)
    {
        GameObject controller = (GameObject)Instantiate(Resources.Load("Prefabs/Player"));

        controller.transform.position = _spawners[i].position;
        controller.transform.rotation = _spawners[i].rotation;

        controller.GetComponent<PlayerController>().setPlayerNum(i);

        controller.name = "PLayer" + i;

        _allPlayers.Add(controller.GetComponent<Pawn>());
    }
    public void startRound()
    {
        roundStarted = true;
    }

	// Use this for initialiList<Pickup> allPickups = new List<Pickup>();zation
	void Start () {
		_allPickups = new List<Pickup>(GameObject.FindObjectsOfType<Pickup>());
		_allPlayers = new List<Pawn>(GameObject.FindObjectsOfType<Pawn>());
	}
	
	
	public void PlayerDied(Pawn player){
        _allPlayers.Remove(player);
        _mainCam.setPlayers(_allPlayers);

        if (!_playing)
            return;

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
            _scores[_allPlayers[0].GetComponent<PlayerController>().getPlayerIndex()]++;
			RoundOver();
		}

        

	}
	
	
    void RoundOver()
    {
            
        _currentRound++;

        bool finished = false;
        for (int i = 0; i < _scores.Length; i ++ )
        {
            finished |= _scores[i] >=_rounds;
        }

        _playing = false;
        if(!finished)
        {
            spawnRound();
        }
        else
        {
            MatchOver();
            updateText();
        }
         
    }
	void MatchOver(){

   		LevelGUI.instance.ShowEndGUI();
        int victor = 0;
        int currentScore = 0;
        for(int i = 0; i < _scores.Length;i++)
        {
            if(_scores[i] > currentScore)
            {
                victor = i+1;
                currentScore = _scores[i];
            }
        }
        _win.setVictor(victor);

        //Application.LoadLevel("MainMenu");

	}
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
