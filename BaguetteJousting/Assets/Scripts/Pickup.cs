﻿using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	Carrier carrier;

	
	public bool PickedUp (Carrier someCarrier) {
		if(carrier != null)
			return false;
		
		if (someCarrier == null || someCarrier.pickupSocket == null){
			return false;
		}
//		if(someCarrier.IsCarrying() ){
//			return false; // or maybe make the
//		}
//		
		carrier = someCarrier;
  
		Debug.Log(this + " was picked up by " + someCarrier);
        
        transform.SetParent(carrier.pickupSocket);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(new Vector3(90,90,0));

		
//		rigidbody.detectCollisions = false;
		
		return true;
	}
	
	
	public void dropped () {
		carrier = null;
	}
	
	
//	void FixedUpdate () {
//		this.gameObject.transform.position = carrier.pickupSocket.position;
//		this.gameObject.transform.rotation = carrier.pickupSocket.ro;
//	}	
	
}