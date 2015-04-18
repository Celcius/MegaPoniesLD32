using UnityEngine;
using System.Collections;

public class Pawn : MonoBehaviour {
	string pawnName = "Joe";
	bool alive = true;
	
	float killZ = -50.0f;

	public bool isAlive(){
		return alive;
	}
	
	void Kill(){
		Debug.Log(this + " died");
		this.alive = false;
		Arena.instance.PlayerDied(this);
		
	}
	
	
	
	void Update () {
		if(gameObject.transform.position.y < killZ){
			this.Kill();
		}
	}
}
