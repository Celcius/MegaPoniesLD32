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

	float topBorder = float.MaxValue;
	float bottomBorder = - float.MaxValue;
	float rightBorder = float.MaxValue;
	float leftBorder = - float.MaxValue ;

	public float dumbness = 0.0f;
	float timeUntillNextThought;


	List<Vector3> pathNodes = new List<Vector3>();
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
		timeUntillNextThought = Random.Range (0.0f, dumbness);
		arena = Arena.instance;
		pawn = GetComponent<Pawn> ();
		if (pawn == null) {
			Debug.Log("ERROR: Bot failed to get pawn component.");
			return;
		}
		movementController = GetComponent<PlayerController> ();
		GameObject topLeft = GameObject.Find ("TopLeft");
		if (topLeft != null) {
			topBorder = topLeft.transform.position.z;
			leftBorder = topLeft.transform.position.x;
		}
		GameObject botRight = GameObject.Find ("BotRight");
		if (botRight != null) {
			bottomBorder = botRight.transform.position.z;
			rightBorder = botRight.transform.position.x;
		}
		GameObject pathMarker = GameObject.Find ("PathNodes");
		if (pathMarker == null) {
			Debug.Log("Could not find path marker");
			return;
				}
		foreach (Transform node in pathMarker.transform) {
			pathNodes.Add(node.position);
		}
	}

	bool FrontIsClear(float maxDistance){
		bool hit = Physics.Raycast (transform.position, movementController.FacingDirection (), maxDistance);
		Color color = hit ? Color.green : Color.red;
		Debug.DrawRay(transform.position, movementController.FacingDirection () * maxDistance, color);
	
		return !hit;
	}

	bool PathIsClear(Vector3 position){
		float distance = Vector3.Distance (transform.position, position);
		bool hit = Physics.Raycast (transform.position, position - transform.position, distance);
		Color color = (hit)? Color.green : Color.red;
		Debug.DrawRay(transform.position, position - transform.position * distance, color);
		return !hit;
	}

	float evaluateDistanceToObject(GameObject theObject){
		float checkDistance = 35.0f;
		float distance = Vector3.Distance (theObject.transform.position,transform.position);
		//TODO>
		//Debug.Log ("checking ray");
		if (!FrontIsClear(checkDistance)) {
			//Debug.Log("Object in the way");
			distance += checkDistance;
		}
		return distance;
	}

	GameObject pickBaguette(){
		GameObject chosenBaguette = null;
		List<Pickup> allBaguettes = arena.allPickups;
		//Debug.Log ("allBaguettes" + allBaguettes);
		float closestDistanceToBaguette = float.MaxValue;
		foreach(Pickup baguettePick in allBaguettes){
			if(baguettePick.carrier != null) continue;
			float distanceToBaguette = evaluateDistanceToObject(baguettePick.gameObject);
			if(distanceToBaguette < closestDistanceToBaguette){
				closestDistanceToBaguette = distanceToBaguette;
				//Debug.Log("closestDistanceToBaguette" + closestDistanceToBaguette);
				chosenBaguette = baguettePick.gameObject;
			}
		}
		Debug.Log ("Chose baguete: " + chosenBaguette);
		return chosenBaguette;
	}

	bool ShouldChase(){
		// TODO:
		//return pawn.baguette != null;
		return true;
	}

	GameObject LastManStanding(){
		GameObject lastManStanding = null;
		foreach (Pawn possibleTarget in arena.allPlayers) {
			if(possibleTarget.isAlive()){
				if(lastManStanding != null){
					return null;
				}
				lastManStanding = possibleTarget.gameObject;
			}
		}
		Debug.Log ("LAST MAN! " + lastManStanding);
		return lastManStanding;
	}

	GameObject PickChaseTarget(){
		GameObject chosenTarget = null; //LastManStanding();
		if (chosenTarget != null)
			return chosenTarget;

		GameObject possibleTargetObject = null;
		float shortestDistance = float.MaxValue;
//		float secondShortestDistance = -1.0f;
		foreach(Pawn possibleTarget in arena.allPlayers){
			if(possibleTarget == pawn || !possibleTarget.isAlive() ) continue;
			possibleTargetObject = possibleTarget.gameObject;
			if(possibleTargetObject.transform.position.x > rightBorder || possibleTargetObject.transform.position.x < leftBorder 
			   || possibleTargetObject.transform.position.z > topBorder || possibleTargetObject.transform.position.z < bottomBorder )
				continue;
			float distance = evaluateDistanceToObject(possibleTargetObject);
			if(distance  < shortestDistance){
				chosenTarget = possibleTarget.gameObject;
				shortestDistance = distance;
			}
		}
		chosenTarget = (chosenTarget != null) ? chosenTarget : possibleTargetObject;
		//Debug.Log("Chose to chase " + chosenTarget);
		return chosenTarget;
	}



	
	Vector3 GetClosestReachableSpot(){
		float closestDistance = float.MaxValue;
		Vector3 bestSpot = NO_TARGET;
		foreach(Vector3 node in pathNodes){
			float distance = Vector3.Distance(node, transform.position);
			bool hit = Physics.Raycast (transform.position,node - transform.position, distance);
			if (hit) continue;
			if(distance < closestDistance){
				closestDistance = distance;
				bestSpot = node;
			}
		}
		return bestSpot;
	}


	
	void EvaluateMovementTarget(){
		if (movementTarget == NO_TARGET) {
			Debug.Log("no target");
			return;
		}
			



		if (! FrontIsClear (25.0f)) 
		{
			timeUntillNextThought += 0.4f;
			Vector3 bestSpot = GetClosestReachableSpot ();
			if(bestSpot != NO_TARGET){
				movementTarget = bestSpot;
			}
			else movementTarget = movementTarget + new Vector3(Random.Range(-5.0f, 5.0f), 0.0f,Random.Range(-5.0f,5.0f));

			movementTarget = new Vector3 (Mathf.Clamp(movementTarget.x,leftBorder,rightBorder), movementTarget.y, Mathf.Clamp(movementTarget.z, bottomBorder,topBorder));
		}
		//else
	//		Debug.Log ("all clear");
	}

	void Think(){
		if(!isBot) return;
		//movementTarget = NO_TARGET;
		if (pawn.baguette == null && arena.allPickups.Count > 0) {
			//if(target == null || target.GetComponent<Baguette>() == null ){
			target = pickBaguette();
			movementTarget = (target != null)? target.transform.position : NO_TARGET;
			EvaluateMovementTarget();
		}
		else {
			// chose between chasing or running away
			chasing = ShouldChase();
			if(chasing){
				// pick another alive pawn to chase
				target = PickChaseTarget();
				movementTarget = (target != null)? target.transform.position : NO_TARGET;
				EvaluateMovementTarget();
				// try to ram the target
				//TODO>
			}
			else {
				// pick a safe spot to move to
				//TODO>
				Debug.Log(gameObject + " is braindead");
			}
		}

	}
	void Update () {
		timeUntillNextThought -= Time.deltaTime;
		if (timeUntillNextThought <= 0.0f) {
			timeUntillNextThought = dumbness;
			Think ();
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
		//Vector3 projection = Vector3.Project (directionToSpot,facingDirection);
		//Vector3 side = directionToSpot - projection;
		float dot = Vector3.Dot (directionToSpot, facingDirection);
		Vector3 horizontal = Vector3.Cross (facingDirection, Vector3.up);
		float dotHorizontal = Vector3.Dot (directionToSpot, horizontal);
		//Debug.Log ("Dot > " + dot);
		//Debug.Log ("Dot horizontal > " + dotHorizontal);
		//Debug.Log ("DirectionToSpot " + directionToSpot);
		//Debug.Log ("Facing Direction " + facingDirection);
		//Debug.Log ("Projection > " + projection);
		//Debug.Log ("Side > " + side);
		//Debug.Log ("distanceToSpot " + distanceToSpot);
		//Debug.Log ("velocity " + movementController.velocity);
		
		if (dot > 0.1f && distanceToSpot > movementController.velocity * 5) {
			_movementInput = -1.0f;
		}
		if ( movementController.velocity > distanceToSpot) {
			_movementInput = 1.0f;
		}
		if (dotHorizontal > 0.15f) {
			_rotationInput = -1.0f;
		}
		if (dotHorizontal < -0.15f) {
			_rotationInput = 1.0f;
		}
		
		return true;
	}

}
