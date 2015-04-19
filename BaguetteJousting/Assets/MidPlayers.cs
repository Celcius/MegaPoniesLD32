using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MidPlayers : MonoBehaviour {


    List<Pawn> _players = new List<Pawn>();
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        float x = 0;
        float z = 0;

	    for(int i = 0; i < _players.Count; i++)
        {
            x += _players[i].transform.position.x;
            z += _players[i].transform.position.z;
        }

        Vector3 destinyPos = new Vector3(x/_players.Count,transform.position.y,z/_players.Count);

        transform.position = new Vector3(transform.position.x + (destinyPos.x - transform.position.x) * Time.deltaTime*2, destinyPos.y, transform.position.z + (destinyPos.z - transform.position.z-18) * Time.deltaTime*2);
	}

    public void setPlayers(List<Pawn> players)
    {
        _players = players;

    }
}
