using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServiceLocator 
{

    public int _playerNum = 2;
    public int _rounds = 3;

    private static ServiceLocator _instance;

    public int getPlayers() {  return _playerNum;}

    public int getRounds() { return _rounds; }

    public bool[] _bots = { false, false, false, false };

	//This is the public reference that other classes will use
    public static ServiceLocator instance
    {
		get {
			if (_instance == null) {
                _instance = new ServiceLocator();
			}
			return _instance;
		}
	}
}
