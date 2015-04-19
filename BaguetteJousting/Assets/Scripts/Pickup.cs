using UnityEngine;
using System.Collections;

public enum PickupTypes {
    PickupBaguette,
    PickupNone
}


public class Pickup : MonoBehaviour {

    public delegate void pickedPickup(Pickup pickup);

    protected const float SECONDS_BEFORE_BAGUETTE_IS_PICKABLE_AFTER_THROW = 4;

    protected Carrier thrower = null;

    protected PickupTypes pickUpType = PickupTypes.PickupNone;


    public PickupTypes PickUpType
    {
        get
        {
            return pickUpType;
        }
    }
	public Carrier carrier;
    protected bool isAvaiableForPickup = true;

	public bool PickedUp (Carrier someCarrier) {
        if (carrier != null || !isAvaiableForPickup)
			return false;
		
		if (someCarrier == null || someCarrier.pickupSocket == null){
			return false;
		}
//		if(someCarrier.IsCarrying() ){
//			return false; // or maybe make the
//		}

        Rigidbody rBody = GetComponent<Rigidbody>();

        if (rBody)
            Destroy(rBody);
//		
		carrier = someCarrier;
  
		Debug.Log(this.name  + " was picked up by " + someCarrier);
        
        gameObject.transform.SetParent(carrier.pickupSocket);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.Euler(new Vector3(90, 90, 0));

		
//		rigidbody.detectCollisions = false;
		
		return true;
	}

    public virtual void throwPickup()
    {
        StartCoroutine("pickupThrowCoroutine");
    }

    public void pickupThrow()
    {
        

    }

 

    private IEnumerator pickupThrowCoroutine()
    {
        thrower = carrier;
        isAvaiableForPickup = false;
        dropped();
        Vector3 throwDir = carrier.transform.right;
        carrier = null;
        Rigidbody rigBody = gameObject.GetComponent<Rigidbody>();
        if (!rigidbody)
            rigBody = gameObject.AddComponent<Rigidbody>();

        rigBody.AddForce(throwDir * 100, ForceMode.Impulse);

        yield return new WaitForSeconds(SECONDS_BEFORE_BAGUETTE_IS_PICKABLE_AFTER_THROW);

        isAvaiableForPickup = true;
        thrower = null;

  
    }

	
	
	public virtual void dropped () {
        transform.SetParent(null);
	}

    public bool isPickedUp()
    {
        return carrier != null;
    }
	
	
//	void FixedUpdate () {
//		this.gameObject.transform.position = carrier.pickupSocket.position;
//		this.gameObject.transform.rotation = carrier.pickupSocket.ro;
//	}	
	
}
