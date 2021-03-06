﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Carrier : MonoBehaviour {

    public delegate void OnPickupDelegate(Pickup pickup);
	
	public Pickup pickup;
	float grabRange = 14.0f;
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
		if(!this.IsCarrying())
        {
//            Debug.Log(Arena.instance.allPickups.Count);
			foreach (Pickup aPickup in Arena.instance.allPickups ){
                if (aPickup == null)
                    continue;
				float distanceToPickup = Vector3.Distance(aPickup.gameObject.transform.position, this.gameObject.transform.position);
				if (distanceToPickup <= this.grabRange && this.TryToPickup(aPickup)) {
					break;
				}  
			}
		}
	}

    public void throwPickup()
    {
        if (pickup == null)
            return;
        Debug.Log("throwpickup");
        pickup.throwPickup();
        pickup = null;
    }

    void defaultPickUpCallback(Pickup pickup)
    {

    }

    public void setOnPickUpCallBack(OnPickupDelegate del)
    {
        onPickupDelegateCallback = del;

    }

    public void destroyedPickUp()
    {
        pickup = null;
    }

    
	
	
	
	
}
