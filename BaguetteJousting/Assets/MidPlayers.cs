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

        float minX = int.MaxValue;
        float maxX = int.MinValue;
        float minZ = int.MaxValue;
        float maxZ = int.MinValue;

        int activePlayers = 0;
	    for(int i = 0; i < _players.Count; i++)
        {
            if(_players[i].gameObject.activeSelf)
            {
                minX = Mathf.Min(_players[i].transform.position.x, minX);
                maxX = Mathf.Max(_players[i].transform.position.x, maxX);
                minZ = Mathf.Min(_players[i].transform.position.z, minZ);
                maxZ = Mathf.Max(_players[i].transform.position.z, maxZ);
                activePlayers++;
            }
        }

        if(activePlayers > 0)
        {
            Vector3 destinyPos = new Vector3(Mathf.Clamp( (maxX + minX)/2, -10, 10), transform.position.y, Mathf.Clamp((maxZ + minZ)/2, -10, 10));

             transform.position = new Vector3(transform.position.x + (destinyPos.x - transform.position.x) * Time.deltaTime*2, destinyPos.y, transform.position.z + (destinyPos.z - transform.position.z-18) * Time.deltaTime*2);
        }
	}

    public void setPlayers(List<Pawn> players)
    {
        _players = players;

    }
}
