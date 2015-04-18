using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	Carrier carrier;

	
	public bool PickedUp (Carrier someCarrier) {
		
		if (someCarrier == null || someCarrier.pickupSocket == null){
			return false;
		}
//		if(someCarrier.IsCarrying() ){
//			return false; // or maybe make the
//		}
//		
		carrier = someCarrier;
		Debug.Log(this + " was picked up by " + someCarrier);
        transform.SetParent(someCarrier.pickupSocket);
		transform.localPosition = new Vector3(0.2f,0.3f,-0.5f);
		transform.rotation = someCarrier.transform.rotation;
//		rigidbody.velocity = Vector3.zero;
//		rigidbody.isKinematic = true;
		
//		rigidbody.detectCollisions = false;
		
		return true;
	}
	
	
	public void Dropped () {
		carrier = null;
	}
	
	
//	void FixedUpdate () {
//		this.gameObject.transform.position = carrier.pickupSocket.position;
//		this.gameObject.transform.rotation = carrier.pickupSocket.ro;
//	}	
	
}
