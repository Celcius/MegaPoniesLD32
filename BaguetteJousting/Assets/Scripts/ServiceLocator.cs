using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServiceLocator 
{

    public int _playerNum = 2;

    private static ServiceLocator _instance;

    public int getPlayers() {  return _playerNum;}

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
