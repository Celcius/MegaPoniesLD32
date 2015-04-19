using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bot : MonoBehaviour {
	public bool isBot = false;
	Arena arena;
	Pawn pawn;
	PlayerController movementController;

	Vector3 NO_TARGET = new Vector3(0.0f,-99999f,0.0f);
	GameObject target;

	Vector3 movementTarget;

	float _movementInput = 0.0f;
	float _rotationInput = 0.0f;


	public float movementInput {			
		get {
			return _movementInput;
		}
	}
	
	public float rotationInput {			
		get {
			return _rotationInput;
		}
	}


	bool chasing = true;


	void Start () {
		arena = Arena.instance;
		pawn = GetComponent<Pawn> ();
		if (pawn == null) {
			Debug.Log("ERROR: Bot failed to get pawn component.");
			return;
		}
		movementController = GetComponent<PlayerController> ();
	}


	float evaluateDistanceToObject(GameObject theObject){
		//TODO>
		return Vector3.Distance (theObject.transform.position,transform.position);
	}

	GameObject pickBaguette(){
		GameObject chosenBaguette = null;
		List<Pickup> allBaguettes = arena.allPickups;
		float closestDistanceToBaguette = 9999999999f;
		foreach(Pickup baguettePick in allBaguettes){
			float distanceToBaguette = evaluateDistanceToObject(baguettePick.gameObject);
			if(distanceToBaguette < closestDistanceToBaguette){
				closestDistanceToBaguette = distanceToBaguette;
				chosenBaguette = baguettePick.gameObject;
			}
		}
		return chosenBaguette;
	}

	bool ShouldChase(){
		// TODO:
		return true;
	}

	GameObject PickChaseTarget(){
		GameObject chosenTarget = null;
		foreach(Pawn possibleTarget in arena.allPlayers){
			if(possibleTarget == pawn || !possibleTarget.isAlive() ) continue;
			chosenTarget = possibleTarget.gameObject;
			Debug.Log("Chose to chase " + chosenTarget);
			break;
		}
		return chosenTarget;
	}

	void Update () {
		movementTarget = NO_TARGET;
		if(!isBot) return;
		if (pawn.baguette == null) {
			if(target == null || target.GetComponent<Baguette>() == null ){
				target = pickBaguette();
			}
			movementTarget = target.transform.position;
		}
		else {
			// chose between chasing or running away
			chasing = ShouldChase();
			if(chasing){
				// pick another alive pawn to chase
				target = PickChaseTarget();
				movementTarget = target.transform.position;
				// try to ram the target
				//TODO>
			}
			else {
				// pick a safe spot to move to
				//TODO>
			}
		}

		MoveTo(movementTarget);
	}


	bool MoveTo(Vector3 spot){
		_movementInput = 0.0f;
		_rotationInput = 0.0f;
		if(spot == NO_TARGET)
			return true;
		
		float distanceToSpot = Vector3.Distance(spot,transform.position);
		Vector3 directionToSpot = (spot - transform.position).normalized;
		Vector3 facingDirection = movementController.FacingDirection ();
		Vector3 projection = Vector3.Project (directionToSpot,facingDirection);
		Vector3 side = directionToSpot - projection;
		float dot = Vector3.Dot (directionToSpot, facingDirection);
		Debug.Log ("Dot > " + dot);
		Debug.Log ("DirectionToSpot " + directionToSpot);
		Debug.Log ("Facing Direction " + facingDirection);
		Debug.Log ("Projection > " + projection);
		Debug.Log ("Side > " + side);
		Debug.Log ("distanceToSpot " + distanceToSpot);
		Debug.Log ("velocity " + movementController.velocity);
		
		if (projection.x > 0.1f && distanceToSpot > movementController.velocity) {
			_movementInput = -1.0f;
		}
		if ( movementController.velocity > distanceToSpot) {
			_movementInput = 1.0f;
		}
		if (side.z > 0.15f) {
			_rotationInput = -1.0f;
		}
		if (side.z < -0.15f) {
			_rotationInput = 1.0f;
		}
		
		return true;
	}

}
