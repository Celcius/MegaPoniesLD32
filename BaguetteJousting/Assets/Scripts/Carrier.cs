using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Carrier : MonoBehaviour {
	
	Pickup pickup;
	float grabRange = 8.0f;
	[SerializeField]
	Transform _pickupSocket;


	
	public Transform pickupSocket{
		get {
			return _pickupSocket;
        }
    }
    
    
    public bool IsCarrying(){
//    Debug.Log(this + "is carrying" + ( (pickup != null)? "true": "false") );
		return pickup != null;
	}
	

	void Start () {
		
	}
	
	
	bool TryToPickup(Pickup somePickup){
		if(pickup != null){
			return false; // or else maybe swap?? //pickup drop + somepickup pickedup		
		}
		if (somePickup.PickedUp(this)){
			pickup = somePickup;
			return true;
		}
		return false;
	}
	
	
	void FixedUpdate () {
		if(!this.IsCarrying()){
			foreach (Pickup aPickup in Arena.instance.allPickups ){
				float distanceToPickup = Vector3.Distance(aPickup.gameObject.transform.position, this.gameObject.transform.position);
				if (distanceToPickup <= this.grabRange && this.TryToPickup(aPickup)) {
					break;
				}  
			}
		}
	}
	
	
	
	
}
