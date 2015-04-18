using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	Carrier carrier;

	
	public bool PickedUp (Carrier someCarrier) {
		if (carrier != null){
			return false;
		}
//		if(someCarrier.IsCarrying() ){
//			return false; // or maybe make the
//		}
//		
		transform.localPosition = new Vector3(0,0,0);
		transform.SetParent(carrier.pickupSocket);
		carrier = someCarrier;
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
