using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Carrier : MonoBehaviour {

    public delegate void OnPickupDelegate(Pickup pickup);
	
	Pickup pickup;
	float grabRange = 8.0f;
	[SerializeField]
	Transform _pickupSocket;
    OnPickupDelegate onPickupDelegateCallback;



	
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
        //onPickupDelegateCallback = new OnPickupDelegate(defaultPickUpCallback);
	}
	
	
	bool TryToPickup(Pickup somePickup){
		if(pickup != null){
			return false; // or else maybe swap?? //pickup drop + somepickup pickedup		
		}
		if (somePickup.PickedUp(this)){
			pickup = somePickup;
            pickup.carrier = this;
            onPickupDelegateCallback(somePickup);
			return true;
		}
		return false;
	}


    protected virtual void FixedUpdate()
    {
		if(!this.IsCarrying()){
			foreach (Pickup aPickup in Arena.instance.allPickups ){
				float distanceToPickup = Vector3.Distance(aPickup.gameObject.transform.position, this.gameObject.transform.position);
				if (distanceToPickup <= this.grabRange && this.TryToPickup(aPickup)) {
					break;
				}  
			}
		}
	}

    void defaultPickUpCallback(Pickup pickup)
    {

    }

    public void setOnPickUpCallBack(OnPickupDelegate del)
    {
        onPickupDelegateCallback = del;

    }

    
	
	
	
	
}
